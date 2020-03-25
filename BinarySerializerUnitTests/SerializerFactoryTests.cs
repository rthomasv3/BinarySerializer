using System;
using System.Collections.Generic;
using System.Linq;
using CodeCompendium.BinarySerialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CodeCompendium.BinarySerializerUnitTests
{
   [TestClass]
   public sealed class SerializerFactoryTests
   {
      [TestMethod]
      [ExpectedException(typeof(ArgumentNullException))]
      public void GetSerializer_NullProvided_ThrowsArgumentNullException()
      {
         SerializerFactory factory = new SerializerFactory();

         ISerializer serializer = factory.GetSerializer(null);
      }

      [TestMethod]
      public void GetSerializer_VersionValid_SerializerReturned()
      {
         SerializerFactory factory = new SerializerFactory();

         ISerializer serializer = factory.GetSerializer(1);

         Assert.IsNotNull(serializer);
      }

      [TestMethod]
      [ExpectedException(typeof(NotSupportedException))]
      public void GetSerializer_VersionInvalid_ThrowsNotSupportedException()
      {
         SerializerFactory factory = new SerializerFactory();

         ISerializer serializer = factory.GetSerializer(0);
      }

      [TestMethod]
      public void GetSerializer_LittleEndianBytesProvided_SerializerReturned()
      {
         SerializerFactory factory = new SerializerFactory();
         List<byte> bytes = new List<byte>();
         bytes.Add(0);
         if (BitConverter.IsLittleEndian)
         {
            bytes.AddRange(BitConverter.GetBytes((ushort)1));
         }
         else
         {
            bytes.AddRange(BitConverter.GetBytes((ushort)1).Reverse());
         }

         ISerializer serializer = factory.GetSerializer(bytes.ToArray());

         Assert.IsNotNull(serializer);
      }

      [TestMethod]
      public void GetSerializer_BigEndianBytesProvided_SerializerReturned()
      {
         SerializerFactory factory = new SerializerFactory();
         List<byte> bytes = new List<byte>();
         bytes.Add(1);
         if (BitConverter.IsLittleEndian)
         {
            bytes.AddRange(BitConverter.GetBytes((ushort)1).Reverse());
         }
         else
         {
            bytes.AddRange(BitConverter.GetBytes((ushort)1));
         }

         ISerializer serializer = factory.GetSerializer(bytes.ToArray());

         Assert.IsNotNull(serializer);
      }

      [TestMethod]
      [ExpectedException(typeof(NotSupportedException))]
      public void GetSerializer_InvalidVersionBytesProvided_ThrowsNotSupportedException()
      {
         SerializerFactory factory = new SerializerFactory();
         List<byte> bytes = new List<byte>();
         bytes.Add(0);
         if (BitConverter.IsLittleEndian)
         {
            bytes.AddRange(BitConverter.GetBytes((ushort)0));
         }
         else
         {
            bytes.AddRange(BitConverter.GetBytes((ushort)0).Reverse());
         }

         ISerializer serializer = factory.GetSerializer(bytes.ToArray());
      }
   }
}
