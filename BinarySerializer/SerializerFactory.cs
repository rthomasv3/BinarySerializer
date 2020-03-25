using System;
using System.Linq;

namespace CodeCompendium.BinarySerialization
{
   /// <summary>
   /// Class used to retrieve a compatible serializer.
   /// </summary>
   internal sealed class SerializerFactory
   {
      #region Fields

      private const string _versionNotSupported = "File version is not supported. Please install the latest version of this package and try again.";
      private readonly Endianness _systemEndianness = BitConverter.IsLittleEndian ? Endianness.LittleEndian : Endianness.BigEndian;

      #endregion

      #region Public Methods

      /// <summary>
      /// Gets a serializer for the specified file version.
      /// </summary>
      public ISerializer GetSerializer(byte[] bytes)
      {
         return GetSerializer(GetVersion(bytes));
      }

      /// <summary>
      /// Gets a serializer for the specified file version.
      /// </summary>
      public ISerializer GetSerializer(ushort version)
      {
         ISerializer serializer = null;

         if (version == 1)
         {
            serializer = new Version1.Serializer();
         }
         else
         {
            throw new NotSupportedException(_versionNotSupported);
         }

         return serializer;
      }

      #endregion

      #region Private Methods

      private ushort GetVersion(byte[] bytes)
      {
         if (bytes == null)
         {
            throw new ArgumentNullException(nameof(bytes));
         }

         ushort version = 0;

         if (bytes.Length > 2)
         {
            Endianness endianness = bytes.First() == 0 ? Endianness.LittleEndian : Endianness.BigEndian;

            if (endianness != _systemEndianness)
            {
               version = BitConverter.ToUInt16(bytes.Skip(1).Reverse().Take(2).ToArray(), 0);
            }
            else
            {
               version = BitConverter.ToUInt16(bytes.Skip(1).Take(2).ToArray(), 0);
            }
         }

         return version;
      }

      #endregion
   }
}
