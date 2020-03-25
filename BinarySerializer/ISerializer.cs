using System;
using System.IO;

namespace CodeCompendium.BinarySerialization
{
   /// <summary>
   /// Interface that defines a class used to serialize and deserialize objects.
   /// </summary>
   internal interface ISerializer
   {
      /// <summary>
      /// Gets a byte array representing the provided object.
      /// </summary>
      byte[] GetObjectBytes(object obj, Endianness endianness, bool ignoreReadOnlyProps);

      /// <summary>
      /// Gets an object from the provided stream.
      /// </summary>
      object GetObject(Type type, Stream stream, Endianness endianness, bool propertyNameCaseInsensitive);
   }
}
