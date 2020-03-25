using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CodeCompendium.BinarySerialization
{
   /// <summary>
   /// Converts objects to/from byte arrays.
   /// </summary>
   internal class Converter
   {
      #region Fields

      private const string _typeNotSupported = "Serializing/Deserializing properties of type {0} is not supported at this time.";
      private static readonly Type _stringType = typeof(string);
      private readonly Endianness _systemEndianness = BitConverter.IsLittleEndian ? Endianness.LittleEndian : Endianness.BigEndian;

      #endregion

      #region Protected Methods

      /// <summary>
      /// Gets a byte array representing the value.
      /// </summary>
      protected byte[] GetBytes<T>(T value, Endianness endianness)
      {
         byte[] bytes = null;

         bool reverseIfNeeded = true;

         if (value is bool boolValue)
         {
            bytes = BitConverter.GetBytes(boolValue);
         }
         else if (value is byte byteValue)
         {
            bytes = new byte[] { byteValue };
         }
         else if (value is char charValue)
         {
            bytes = BitConverter.GetBytes(charValue);
         }
         else if (value is decimal decimalValue)
         {
            reverseIfNeeded = false;

            List<byte> decimalBytes = new List<byte>();
            foreach (int bit in Decimal.GetBits(decimalValue))
            {
               decimalBytes.AddRange(GetBytes(bit, endianness));
            }
            bytes = decimalBytes.ToArray();
         }
         else if (value is double doubleValue)
         {
            bytes = BitConverter.GetBytes(doubleValue);
         }
         else if (value is float floatValue)
         {
            bytes = BitConverter.GetBytes(floatValue);
         }
         else if (value is int intValue)
         {
            bytes = BitConverter.GetBytes(intValue);
         }
         else if (value is uint uintValue)
         {
            bytes = BitConverter.GetBytes(uintValue);
         }
         else if (value is long longValue)
         {
            bytes = BitConverter.GetBytes(longValue);
         }
         else if (value is ulong ulongValue)
         {
            bytes = BitConverter.GetBytes(ulongValue);
         }
         else if (value is short shortValue)
         {
            bytes = BitConverter.GetBytes(shortValue);
         }
         else if (value is ushort ushortValue)
         {
            bytes = BitConverter.GetBytes(ushortValue);
         }
         else if (value is string stringValue)
         {
            reverseIfNeeded = false;

            byte[] stringBytes = Encoding.Unicode.GetBytes(stringValue).ToArray();
            byte[] lengthBytes = GetBytes(stringBytes.Length, endianness);

            if (endianness != _systemEndianness)
            {
               stringBytes = stringBytes.Reverse().ToArray();
            }

            List<byte> combined = new List<byte>(lengthBytes);
            combined.AddRange(stringBytes);

            bytes = combined.ToArray();
         }
         else if (value is Guid guid)
         {
            bytes = guid.ToByteArray();
         }
         else if (value is DateTime dateTime)
         {
            bytes = GetBytes(dateTime.Ticks, endianness);
         }
         else
         {
            throw new NotSupportedException(String.Format(_typeNotSupported, typeof(T).Name));
         }

         if (reverseIfNeeded)
         {
            if (_systemEndianness != endianness)
            {
               bytes = bytes.Reverse().ToArray();
            }
         }

         return bytes;
      }

      /// <summary>
      /// Gets a value of the selected type from the byte array.
      /// </summary>
      protected T GetValue<T>(byte[] bytes, Endianness endianness)
      {
         return (T)GetValue(typeof(T), bytes, endianness);
      }

      /// <summary>
      /// Gets a value of the selected type from the byte array.
      /// </summary>
      protected object GetValue(Type type, byte[] bytes, Endianness endianness)
      {
         object value = default;

         if (type == typeof(decimal))
         {
            List<int> bits = new List<int>();
            for (int i = 0; i < 4; ++i)
            {
               bits.Add(GetValue<int>(bytes.Skip(i * 4).Take(4).ToArray(), endianness));
            }
            value = new Decimal(bits.ToArray());
         }
         else if (type == typeof(string))
         {
            using (MemoryStream memoryStream = new MemoryStream(bytes))
            {
               using (BinaryReader reader = new BinaryReader(memoryStream))
               {
                  value = GetNextString(reader, endianness);
               }
            }
         }
         else
         {
            if (_systemEndianness != endianness)
            {
               bytes = bytes.Reverse().ToArray();
            }

            if (type == typeof(bool))
            {
               value = BitConverter.ToBoolean(bytes, 0);
            }
            else if (type == typeof(byte))
            {
               value = bytes[0];
            }
            else if (type == typeof(char))
            {
               value = BitConverter.ToChar(bytes, 0);
            }
            else if (type == typeof(double))
            {
               value = BitConverter.ToDouble(bytes, 0);
            }
            else if (type == typeof(float))
            {
               value = BitConverter.ToSingle(bytes, 0);
            }
            else if (type == typeof(int))
            {
               value = BitConverter.ToInt32(bytes, 0);
            }
            else if (type == typeof(uint))
            {
               value = BitConverter.ToUInt32(bytes, 0);
            }
            else if (type == typeof(long))
            {
               value = BitConverter.ToInt64(bytes, 0);
            }
            else if (type == typeof(ulong))
            {
               value = BitConverter.ToUInt64(bytes, 0);
            }
            else if (type == typeof(short))
            {
               value = BitConverter.ToInt16(bytes, 0);
            }
            else if (type == typeof(ushort))
            {
               value = BitConverter.ToUInt16(bytes, 0);
            }
            else if (type == typeof(Guid))
            {
               value = new Guid(bytes);
            }
            else if (type == typeof(DateTime))
            {
               value = new DateTime(BitConverter.ToInt64(bytes, 0));
            }
            else
            {
               throw new NotSupportedException(String.Format(_typeNotSupported, type.Name));
            }
         }

         return value;
      }

      /// <summary>
      /// Gets the next string from the reader's stream.
      /// </summary>
      protected string GetNextString(BinaryReader reader, Endianness endianness)
      {
         string value = null;

         int length = GetValue<int>(reader.ReadBytes(4), endianness);
         byte[] stringBytes = reader.ReadBytes(length);

         if (_systemEndianness != endianness)
         {
            stringBytes = stringBytes.Reverse().ToArray();
         }

         value = Encoding.Unicode.GetString(stringBytes);

         return value;
      }

      /// <summary>
      /// Returns true if the type is a primitive or value type supported by this converted.
      /// </summary>
      protected bool IsValueType(Type type)
      {
         return type != null &&
                (type.IsPrimitive ||
                 type == typeof(decimal) ||
                 type == typeof(string) ||
                 type == typeof(Guid) ||
                 type == typeof(DateTime));
      }

      /// <summary>
      /// Returns true if the type name is a primitive or value type supported by this converted.
      /// </summary>
      protected bool IsValueType(string typeName)
      {
         return IsValueType(Type.GetType(typeName));
      }

      /// <summary>
      /// Returns true if the type name matches the type name for string.
      /// </summary>
      protected bool IsStringType(Type type)
      {
         return type == _stringType;
      }

      /// <summary>
      /// Gets the size of the type.
      /// </summary>
      protected int TypeSize(Type type)
      {
         int size = 0;

         if (type == typeof(bool))
         {
            size = sizeof(bool);
         }
         else if (type == typeof(byte))
         {
            size = sizeof(byte);
         }
         else if (type == typeof(char))
         {
            size = sizeof(char);
         }
         else if (type == typeof(decimal))
         {
            size = sizeof(decimal);
         }
         else if (type == typeof(double))
         {
            size = sizeof(double);
         }
         else if (type == typeof(float))
         {
            size = sizeof(float);
         }
         else if (type == typeof(int))
         {
            size = sizeof(int);
         }
         else if (type == typeof(uint))
         {
            size = sizeof(uint);
         }
         else if (type == typeof(long))
         {
            size = sizeof(long);
         }
         else if (type == typeof(ulong))
         {
            size = sizeof(ulong);
         }
         else if (type == typeof(short))
         {
            size = sizeof(short);
         }
         else if (type == typeof(ushort))
         {
            size = sizeof(ushort);
         }
         else if (type == typeof(Guid))
         {
            size = 16;
         }
         else if (type == typeof(DateTime))
         {
            size = sizeof(long);
         }

         return size;
      }

      /// <summary>
      /// Gets the size of the type.
      /// </summary>
      protected int TypeSize(string typeName)
      {
         return TypeSize(Type.GetType(typeName));
      }

      #endregion
   }
}
