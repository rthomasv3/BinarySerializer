using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace CodeCompendium.BinarySerialization.Version1
{
   /// <summary>
   /// Version 1 of the class used to serialize and deserialize objects.
   /// </summary>
   internal sealed class Serializer : Converter, ISerializer
   {
      #region Public Methods

      /// <inheritdoc />
      public byte[] GetObjectBytes(object obj, Endianness endianness, bool ignoreReadOnlyProps)
      {
         if (obj == null)
         {
            throw new ArgumentNullException(nameof(obj));
         }

         List<byte> bytes = new List<byte>();

         List<PropertyInfo> properties = new List<PropertyInfo>();
         foreach (PropertyInfo propertyInfo in obj.GetType().GetProperties().Where(x => x.CanRead))
         {
            if ((propertyInfo.CanWrite || !ignoreReadOnlyProps) && propertyInfo.GetValue(obj) != null)
            {
               properties.Add(propertyInfo);
            }
         }

         bytes.AddRange(GetBytes(GetPropertiesHeader(properties), endianness));

         foreach (PropertyInfo propertyInfo in properties)
         {
            bytes.AddRange(GetPropertyBytes(propertyInfo.PropertyType, propertyInfo.GetValue(obj), endianness, ignoreReadOnlyProps));
         }

         return bytes.ToArray();
      }

      /// <inheritdoc />
      public object GetObject(Type type, Stream stream, Endianness endianness, bool propertyNameCaseInsensitive)
      {
         if (type == null)
         {
            throw new ArgumentNullException(nameof(type));
         }

         if (stream == null)
         {
            throw new ArgumentNullException(nameof(stream));
         }

         object obj = default;
         Dictionary<string, object> parameters = new Dictionary<string, object>();
         if (type.GetConstructor(Type.EmptyTypes) != null)
         {
            obj = Activator.CreateInstance(type);
         }

         IEnumerable<PropertyInfo> typeProperties = type.GetProperties().Where(x => x.CanWrite);

         BinaryReader reader = new BinaryReader(stream);

         string propertyHeader = GetNextString(reader, endianness);
         string[] properties = propertyHeader.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
         foreach (string property in properties)
         {
            string[] nameType = property.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
            if (nameType.Length == 2)
            {
               string propertyName = nameType[0];
               string propertyTypeName = nameType[1];
               Type propertyType = Type.GetType(propertyTypeName);

               object propertyValue = GetProperty(reader, propertyType, endianness, propertyNameCaseInsensitive);

               if (obj != null)
               {
                  PropertyInfo propertyInfo = typeProperties.FirstOrDefault(x => x.Name.Equals(propertyName,
                        propertyNameCaseInsensitive ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal));

                  if (propertyInfo != null)
                  {
                     propertyInfo.SetValue(obj, propertyValue);
                  }
               }
               else
               {
                  parameters[propertyName] = propertyValue;
               }
            }
         }

         if (obj == null)
         {
            List<object> parameterValues = new List<object>();
            ConstructorInfo constructorInfo = type.GetConstructors().FirstOrDefault(x => x.IsPublic && x.GetParameters().Any());
            if (constructorInfo != null)
            {
               foreach (ParameterInfo parameter in constructorInfo.GetParameters())
               {
                  KeyValuePair<string, object> parameterPair = parameters.FirstOrDefault(x => x.Key.Equals(parameter.Name, 
                     propertyNameCaseInsensitive ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal));

                  if (!parameterPair.Equals(default(KeyValuePair<string, object>)))
                  {
                     parameterValues.Add(parameterPair.Value);
                  }
                  else if (parameter.ParameterType.IsValueType)
                  {
                     parameterValues.Add(Activator.CreateInstance(parameter.ParameterType));
                  }
                  else
                  {
                     parameterValues.Add(null);
                  }
               }

               obj = Activator.CreateInstance(type, parameterValues.ToArray());
            }
         }

         return obj;
      }

      #endregion

      #region Private Methods

      private string GetPropertiesHeader(IEnumerable<PropertyInfo> properties)
      {
         return String.Join(";", properties.Select(x => $"{x.Name}:{x.PropertyType.AssemblyQualifiedName}"));
      }

      private IEnumerable<byte> GetPropertyBytes(Type type, object obj, Endianness endianness, bool ignoreReadOnlyProps)
      {
         List<byte> bytes = new List<byte>();

         if (IsValueType(type))
         {
            bytes.AddRange(GetBytes(obj, endianness));
         }
         else if (type.GetInterface(nameof(IDictionary)) != null)
         {
            bytes.AddRange(GetDictionaryBytes(obj as IDictionary, endianness, ignoreReadOnlyProps));
         }
         else if (type.GetInterface(nameof(IEnumerable)) != null)
         {
            bytes.AddRange(GetCollectionBytes(obj as IEnumerable, endianness, ignoreReadOnlyProps));
         }
         else
         {
            bytes.AddRange(GetObjectBytes(obj, endianness, ignoreReadOnlyProps));
         }

         return bytes;
      }

      private object GetProperty(BinaryReader reader, Type propertyType, Endianness endianness, bool propertyNameCaseInsensitive)
      {
         object value = null;

         if (IsStringType(propertyType))
         {
            value = GetNextString(reader, endianness);
         }
         else if (IsValueType(propertyType))
         {
            value = GetValue(propertyType, reader.ReadBytes(TypeSize(propertyType)), endianness);
         }
         else if (propertyType.GetInterface(nameof(IDictionary)) != null)
         {
            value = GetDictionary(reader, propertyType, endianness, propertyNameCaseInsensitive);
         }
         else if (propertyType.GetInterface(nameof(IEnumerable)) != null)
         {
            value = GetCollection(reader, propertyType, endianness, propertyNameCaseInsensitive);
         }
         else
         {
            value = GetObject(propertyType, Stream.Synchronized(reader.BaseStream), endianness, propertyNameCaseInsensitive);
         }

         return value;
      }

      private IEnumerable<byte> GetCollectionBytes(IEnumerable collection, Endianness endianness, bool ignoreReadOnlyProps)
      {
         List<byte> collectionBytes = new List<byte>();

         int count = 0;
         foreach (object element in collection)
         {
            collectionBytes.AddRange(GetPropertyBytes(element.GetType(), element, endianness, ignoreReadOnlyProps));
            count++;
         }

         collectionBytes.InsertRange(0, GetBytes(count, endianness));

         return collectionBytes;
      }

      private IEnumerable<byte> GetDictionaryBytes(IDictionary dictionary, Endianness endianness, bool ignoreReadOnlyProps)
      {
         List<byte> dictionaryBytes = new List<byte>();

         if (dictionary.Count > 0)
         {
            Type keyType = null;
            Type valueType = null;
            Type dictionaryType = dictionary.GetType();

            if (dictionaryType.GenericTypeArguments.Length == 2)
            {
               keyType = dictionaryType.GenericTypeArguments[0];
               valueType = dictionaryType.GenericTypeArguments[1];

               dictionaryBytes.AddRange(GetBytes(dictionary.Count, endianness));

               foreach (object key in dictionary.Keys)
               {
                  dictionaryBytes.AddRange(GetPropertyBytes(keyType, key, endianness, ignoreReadOnlyProps));
                  dictionaryBytes.AddRange(GetPropertyBytes(valueType, dictionary[key], endianness, ignoreReadOnlyProps));
               }
            }
         }

         return dictionaryBytes;
      }

      private IList GetCollection(BinaryReader reader, Type propertyType, Endianness endianness, bool propertyNameCaseInsensitive)
      {
         IList collection = default;

         bool isArray = false;
         Type elementType = null;
         int count = GetValue<int>(reader.ReadBytes(sizeof(int)), endianness);

         if (propertyType.GenericTypeArguments.Any())
         {
            elementType = propertyType.GenericTypeArguments[0];
            collection = Activator.CreateInstance(propertyType) as IList;
         }
         else
         {
            elementType = propertyType.GetElementType();
            collection = Array.CreateInstance(elementType, count);
            isArray = true;
         }

         for (int i = 0; i < count; ++i)
         {
            object element = GetProperty(reader, elementType, endianness, propertyNameCaseInsensitive);

            if (isArray)
            {
               collection[i] = element;
            }
            else
            {
               collection.Add(element);
            }
         }

         return collection;
      }

      private IDictionary GetDictionary(BinaryReader reader, Type propertyType, Endianness endianness, bool propertyNameCaseInsensitive)
      {
         IDictionary dictionary = default;

         if (propertyType.GenericTypeArguments.Length == 2)
         {
            dictionary = Activator.CreateInstance(propertyType) as IDictionary;

            Type keyType = propertyType.GenericTypeArguments[0];
            Type valueType = propertyType.GenericTypeArguments[1];

            int count = GetValue<int>(reader.ReadBytes(sizeof(int)), endianness);

            for (int i = 0; i < count; ++i)
            {
               object key = GetProperty(reader, keyType, endianness, propertyNameCaseInsensitive);
               dictionary[key] = GetProperty(reader, valueType, endianness, propertyNameCaseInsensitive);
            }
         }

         return dictionary;
      }

      #endregion
   }
}
