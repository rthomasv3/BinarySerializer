using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("CodeCompendium.BinarySerializerUnitTests")]

namespace CodeCompendium.BinarySerialization
{
   /// <summary>
   /// Class used to serialize/deserialize objects.
   /// </summary>
   public sealed class BinarySerializer
   {
      #region Fields

      private static readonly ushort _version = 1;
      private static readonly Endianness _systemEndianness = BitConverter.IsLittleEndian ? Endianness.LittleEndian : Endianness.BigEndian;
      private static readonly SerializerFactory _serializerFactory = new SerializerFactory();
      private readonly BinarySerializerOptions _serializerOptions;

      #endregion

      #region Constructor

      /// <summary>
      /// Creates a new instance of the <see cref="BinarySerializer"/> class.
      /// </summary>
      public BinarySerializer(BinarySerializerOptions serializerOptions = default)
      {
         _serializerOptions = serializerOptions;
      }

      #endregion

      #region Properties

      /// <summary>
      /// Gets the binary serializer options.
      /// </summary>
      public BinarySerializerOptions BinarySerializerOptions
      {
         get { return _serializerOptions; }
      }

      #endregion

      #region Public Methods

      /// <summary>
      /// Serializes an object into a compressed byte array.
      /// </summary>
      public byte[] Serialize<T>(T value)
      {
         return Serialize(value, _serializerOptions);
      }

      /// <summary>
      /// Deserializes the compressed byte array into an object.
      /// </summary>
      public T Deserialize<T>(byte[] bytes)
      {
         return Deserialize<T>(bytes, _serializerOptions);
      }

      /// <summary>
      /// Serializes an object into a compressed byte array.
      /// </summary>
      public static byte[] Serialize<T>(T value, BinarySerializerOptions serializerOptions = default)
      {
         if (value == null)
         {
            throw new ArgumentNullException(nameof(value));
         }

         List<byte> bytes = new List<byte>();

         if (serializerOptions == null)
         {
            serializerOptions = new BinarySerializerOptions();
         }

         Endianness endianness = serializerOptions.Endianness;
         if (endianness == Endianness.Default)
         {
            endianness = BitConverter.IsLittleEndian ? Endianness.LittleEndian : Endianness.BigEndian;
         }

         bytes.Add((byte)(endianness == Endianness.LittleEndian ? 0 : 1));

         if (endianness != _systemEndianness)
         {
            bytes.AddRange(BitConverter.GetBytes(_version).Reverse());
         }
         else
         {
            bytes.AddRange(BitConverter.GetBytes(_version));
         }

         ISerializer serializer = _serializerFactory.GetSerializer(_version);
         if (serializer != null)
         {
            bytes.AddRange(serializer.GetObjectBytes(value, endianness, serializerOptions.IgnoreReadOnlyProperties));
         }

         return Compress(bytes.ToArray());
      }

      /// <summary>
      /// Deserializes the compressed byte array into an object.
      /// </summary>
      public static T Deserialize<T>(byte[] bytes, BinarySerializerOptions serializerOptions = default)
      {
         if (bytes == null)
         {
            throw new ArgumentNullException(nameof(bytes));
         }

         T value = default;

         if (serializerOptions == null)
         {
            serializerOptions = new BinarySerializerOptions();
         }

         byte[] decompressedBytes = Decompress(bytes);

         Endianness endianness = decompressedBytes.First() == 0 ? Endianness.LittleEndian : Endianness.BigEndian;

         ISerializer serializer = _serializerFactory.GetSerializer(decompressedBytes);
         if (serializer != null)
         {
            using (MemoryStream memoryStream = new MemoryStream(decompressedBytes.Skip(3).ToArray()))
            {
               value = (T)serializer.GetObject(typeof(T), memoryStream, endianness, serializerOptions.PropertyNameCaseInsensitive);
            }
         }

         return value;
      }

      #endregion

      #region Private Methods

      private static byte[] Compress(byte[] bytes)
      {
         byte[] compressedBytes = null;

         using (MemoryStream outputStream = new MemoryStream())
         {
            using (DeflateStream compressionStream = new DeflateStream(outputStream, CompressionLevel.Optimal))
            {
               compressionStream.Write(bytes, 0, bytes.Length);
            }
            compressedBytes = outputStream.ToArray();
         }

         return compressedBytes;
      }

      private static byte[] Decompress(byte[] bytes)
      {
         byte[] decompressedBytes = null;

         using (MemoryStream input = new MemoryStream(bytes))
         {
            using (MemoryStream output = new MemoryStream())
            {
               using (DeflateStream dstream = new DeflateStream(input, CompressionMode.Decompress))
               {
                  dstream.CopyTo(output);
               }
               decompressedBytes = output.ToArray();
            }
         }

         return decompressedBytes;
      }

      #endregion
   }
}
