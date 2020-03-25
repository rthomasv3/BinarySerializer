using System;
using CodeCompendium.BinarySerialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CodeCompendium.BinarySerializerUnitTests
{
   [TestClass]
   public class BinarySerializerTests
   {
      [TestMethod]
      public void Constructor_ArgumentsProvided_PropertiesSet()
      {
         BinarySerializerOptions options = new BinarySerializerOptions();

         BinarySerializer serializer = new BinarySerializer(options);

         Assert.AreEqual(options, serializer.BinarySerializerOptions);
      }

      [TestMethod]
      [ExpectedException(typeof(ArgumentNullException))]
      public void Serialize_ValueNull_ThrowsArgumentNullException()
      {
         BinarySerializerOptions options = new BinarySerializerOptions();
         BinarySerializer serializer = new BinarySerializer(options);

         serializer.Serialize<object>(null);
      }

      [TestMethod]
      public void Serialize_ValueAndOptionsProvided_ByteArrayCreated()
      {
         byte[] bytes = BinarySerializer.Serialize(new Tuple<int>(1), new BinarySerializerOptions());

         Assert.IsNotNull(bytes);
      }

      [TestMethod]
      public void Serialize_OptionsNull_DefaultOptionsUsed()
      {
         BinarySerializer serializer = new BinarySerializer();

         byte[] bytes = serializer.Serialize(new Tuple<int>(1));

         Assert.IsNotNull(bytes);
      }

      [TestMethod]
      [ExpectedException(typeof(ArgumentNullException))]
      public void Deserialize_BytesNull_ThrowsArgumentNullException()
      {
         BinarySerializerOptions options = new BinarySerializerOptions();
         BinarySerializer serializer = new BinarySerializer(options);

         serializer.Deserialize<object>(null);
      }

      [TestMethod]
      public void Deserialize_OptionsNull_DefaultOptionsUsed()
      {
         Tuple<int> value = new Tuple<int>(1);
         BinarySerializer serializer = new BinarySerializer();

         byte[] bytes = serializer.Serialize(value);
         Tuple<int> deserialized = serializer.Deserialize<Tuple<int>>(bytes);

         Assert.AreEqual(value.Item1, deserialized.Item1);
      }

      [TestMethod]
      public void Deserialize_BytesProvided_ObjectDeserialized()
      {
         Tuple<int> value = new Tuple<int>(1);
         byte[] bytes = BinarySerializer.Serialize(value);

         Tuple<int> deserialized = BinarySerializer.Deserialize<Tuple<int>>(bytes);

         Assert.AreEqual(value.Item1, deserialized.Item1);
      }

      [TestMethod]
      public void Deserialize_OptionsLittleEndian_ObjectDeserialized()
      {
         Tuple<int> value = new Tuple<int>(1);
         BinarySerializerOptions options = new BinarySerializerOptions(Endianness.LittleEndian);
         byte[] bytes = BinarySerializer.Serialize(value, options);

         Tuple<int> deserialized = BinarySerializer.Deserialize<Tuple<int>>(bytes);

         Assert.AreEqual(value.Item1, deserialized.Item1);
      }

      [TestMethod]
      public void Deserialize_OptionsBigEndian_ObjectDeserialized()
      {
         Tuple<int> value = new Tuple<int>(1);
         BinarySerializerOptions options = new BinarySerializerOptions(Endianness.BigEndian);
         byte[] bytes = BinarySerializer.Serialize(value, options);

         Tuple<int> deserialized = BinarySerializer.Deserialize<Tuple<int>>(bytes);

         Assert.AreEqual(value.Item1, deserialized.Item1);
      }
   }
}
