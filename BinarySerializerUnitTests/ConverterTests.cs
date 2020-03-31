
using System;
using System.Collections;
using CodeCompendium.BinarySerialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CodeCompendium.BinarySerializerUnitTests
{
   [TestClass]
   public sealed class ConverterTests
   {
      [TestMethod]
      public void GetBytes_BoolProvided_BytesReturned()
      {
         TestConverter converter = new TestConverter();

         byte[] bytes = converter.TestGetBytes(true, Endianness.LittleEndian);

         Assert.AreEqual(sizeof(bool), bytes.Length);
         Assert.AreEqual(1, bytes[0]);
      }

      [TestMethod]
      public void GetBytes_ByteProvided_BytesReturned()
      {
         byte b = 1;
         TestConverter converter = new TestConverter();

         byte[] bytes = converter.TestGetBytes(b, Endianness.LittleEndian);

         Assert.AreEqual(sizeof(byte), bytes.Length);
         Assert.AreEqual(b, bytes[0]);
      }

      [TestMethod]
      public void GetBytes_CharProvided_BytesReturned()
      {
         TestConverter converter = new TestConverter();

         byte[] bytes = converter.TestGetBytes('a', Endianness.LittleEndian);

         Assert.AreEqual(sizeof(char), bytes.Length);
      }

      [TestMethod]
      public void GetBytes_DecimalProvided_BytesReturned()
      {
         TestConverter converter = new TestConverter();

         byte[] bytes = converter.TestGetBytes(5m, Endianness.LittleEndian);

         Assert.AreEqual(sizeof(decimal), bytes.Length);
      }

      [TestMethod]
      public void GetBytes_DoubleProvided_BytesReturned()
      {
         TestConverter converter = new TestConverter();

         byte[] bytes = converter.TestGetBytes(5d, Endianness.LittleEndian);

         Assert.AreEqual(sizeof(double), bytes.Length);
      }

      [TestMethod]
      public void GetBytes_FloatProvided_BytesReturned()
      {
         TestConverter converter = new TestConverter();

         byte[] bytes = converter.TestGetBytes(5f, Endianness.LittleEndian);

         Assert.AreEqual(sizeof(float), bytes.Length);
      }

      [TestMethod]
      public void GetBytes_IntProvided_BytesReturned()
      {
         TestConverter converter = new TestConverter();

         byte[] bytes = converter.TestGetBytes(5, Endianness.LittleEndian);

         Assert.AreEqual(sizeof(int), bytes.Length);
      }

      [TestMethod]
      public void GetBytes_UIntProvided_BytesReturned()
      {
         TestConverter converter = new TestConverter();

         byte[] bytes = converter.TestGetBytes(5u, Endianness.LittleEndian);

         Assert.AreEqual(sizeof(uint), bytes.Length);
      }

      [TestMethod]
      public void GetBytes_LongProvided_BytesReturned()
      {
         TestConverter converter = new TestConverter();

         byte[] bytes = converter.TestGetBytes(5L, Endianness.LittleEndian);

         Assert.AreEqual(sizeof(long), bytes.Length);
      }

      [TestMethod]
      public void GetBytes_ULongProvided_BytesReturned()
      {
         TestConverter converter = new TestConverter();

         byte[] bytes = converter.TestGetBytes(5UL, Endianness.LittleEndian);

         Assert.AreEqual(sizeof(ulong), bytes.Length);
      }

      [TestMethod]
      public void GetBytes_ShortProvided_BytesReturned()
      {
         TestConverter converter = new TestConverter();

         byte[] bytes = converter.TestGetBytes((short)5, Endianness.LittleEndian);

         Assert.AreEqual(sizeof(short), bytes.Length);
      }

      [TestMethod]
      public void GetBytes_UShortProvided_BytesReturned()
      {
         TestConverter converter = new TestConverter();

         byte[] bytes = converter.TestGetBytes((ushort)5, Endianness.LittleEndian);

         Assert.AreEqual(sizeof(ushort), bytes.Length);
      }

      [TestMethod]
      public void GetBytes_StringProvided_BytesReturned()
      {
         TestConverter converter = new TestConverter();

         byte[] bytes = converter.TestGetBytes("Test", Endianness.LittleEndian);

         Assert.IsTrue(bytes.Length > 0);
      }

      [TestMethod]
      public void GetBytes_GuidProvided_BytesReturned()
      {
         TestConverter converter = new TestConverter();

         byte[] bytes = converter.TestGetBytes(Guid.NewGuid(), Endianness.LittleEndian);

         Assert.AreEqual(16, bytes.Length);
      }

      [TestMethod]
      public void GetBytes_DateTimeProvided_BytesReturned()
      {
         TestConverter converter = new TestConverter();

         byte[] bytes = converter.TestGetBytes(DateTime.Now, Endianness.LittleEndian);

         Assert.AreEqual(sizeof(long), bytes.Length);
      }

      [TestMethod]
      public void GetBytes_EnumProvided_BytesReturned()
      {
         TestConverter converter = new TestConverter();

         byte[] bytes = converter.TestGetBytes(StringComparison.Ordinal, Endianness.LittleEndian);

         Assert.AreEqual(sizeof(int), bytes.Length);
      }

      [TestMethod]
      [ExpectedException(typeof(NotSupportedException))]
      public void GetBytes_InvalidTypeProvided_ThrowsNotSupportedException()
      {
         TestConverter converter = new TestConverter();

         byte[] bytes = converter.TestGetBytes(new Tuple<int>(1), Endianness.LittleEndian);
      }

      [TestMethod]
      [ExpectedException(typeof(NotSupportedException))]
      public void GetBytes_NullProvided_ThrowsNotSupportedException()
      {
         TestConverter converter = new TestConverter();

         byte[] bytes = converter.TestGetBytes((object)null, Endianness.LittleEndian);
      }

      [TestMethod]
      public void GetValue_DecimalBytesProvided_DecimalReturned()
      {
         decimal value = 5.4m;
         TestConverter converter = new TestConverter();
         byte[] bytes = converter.TestGetBytes(value, Endianness.LittleEndian);

         decimal result = converter.TestGetValue<decimal>(bytes, Endianness.LittleEndian);

         Assert.AreEqual(value, result);
      }

      [TestMethod]
      public void GetValue_StringBytesProvided_StringReturned()
      {
         string value = "test";
         TestConverter converter = new TestConverter();
         byte[] bytes = converter.TestGetBytes(value, Endianness.LittleEndian);

         string result = converter.TestGetValue<string>(bytes, Endianness.LittleEndian);

         Assert.AreEqual(value, result);
      }

      [TestMethod]
      public void GetValue_BoolBytesProvided_BoolReturned()
      {
         bool value = true;
         TestConverter converter = new TestConverter();
         byte[] bytes = converter.TestGetBytes(value, Endianness.LittleEndian);

         bool result = converter.TestGetValue<bool>(bytes, Endianness.LittleEndian);

         Assert.AreEqual(value, result);
      }

      [TestMethod]
      public void GetValue_ByteBytesProvided_ByteReturned()
      {
         byte value = 1;
         TestConverter converter = new TestConverter();
         byte[] bytes = converter.TestGetBytes(value, Endianness.LittleEndian);

         byte result = converter.TestGetValue<byte>(bytes, Endianness.LittleEndian);

         Assert.AreEqual(value, result);
      }

      [TestMethod]
      public void GetValue_CharBytesProvided_CharReturned()
      {
         char value = 'a';
         TestConverter converter = new TestConverter();
         byte[] bytes = converter.TestGetBytes(value, Endianness.LittleEndian);

         char result = converter.TestGetValue<char>(bytes, Endianness.LittleEndian);

         Assert.AreEqual(value, result);
      }

      [TestMethod]
      public void GetValue_DoubleBytesProvided_DoubleReturned()
      {
         double value = 3.9d;
         TestConverter converter = new TestConverter();
         byte[] bytes = converter.TestGetBytes(value, Endianness.LittleEndian);

         double result = converter.TestGetValue<double>(bytes, Endianness.LittleEndian);

         Assert.AreEqual(value, result);
      }

      [TestMethod]
      public void GetValue_FloatBytesProvided_FloatReturned()
      {
         float value = 6.5f;
         TestConverter converter = new TestConverter();
         byte[] bytes = converter.TestGetBytes(value, Endianness.LittleEndian);

         float result = converter.TestGetValue<float>(bytes, Endianness.LittleEndian);

         Assert.AreEqual(value, result);
      }

      [TestMethod]
      public void GetValue_IntBytesProvided_IntReturned()
      {
         int value = 256;
         TestConverter converter = new TestConverter();
         byte[] bytes = converter.TestGetBytes(value, Endianness.LittleEndian);

         int result = converter.TestGetValue<int>(bytes, Endianness.LittleEndian);

         Assert.AreEqual(value, result);
      }

      [TestMethod]
      public void GetValue_UIntBytesProvided_UIntReturned()
      {
         uint value = 512;
         TestConverter converter = new TestConverter();
         byte[] bytes = converter.TestGetBytes(value, Endianness.LittleEndian);

         uint result = converter.TestGetValue<uint>(bytes, Endianness.LittleEndian);

         Assert.AreEqual(value, result);
      }

      [TestMethod]
      public void GetValue_LongBytesProvided_LongReturned()
      {
         long value = long.MaxValue - 1;
         TestConverter converter = new TestConverter();
         byte[] bytes = converter.TestGetBytes(value, Endianness.LittleEndian);

         long result = converter.TestGetValue<long>(bytes, Endianness.LittleEndian);

         Assert.AreEqual(value, result);
      }

      [TestMethod]
      public void GetValue_ULongBytesProvided_ULongReturned()
      {
         ulong value = ulong.MaxValue - 1;
         TestConverter converter = new TestConverter();
         byte[] bytes = converter.TestGetBytes(value, Endianness.LittleEndian);

         ulong result = converter.TestGetValue<ulong>(bytes, Endianness.LittleEndian);

         Assert.AreEqual(value, result);
      }

      [TestMethod]
      public void GetValue_ShortBytesProvided_ShortReturned()
      {
         short value = 1024;
         TestConverter converter = new TestConverter();
         byte[] bytes = converter.TestGetBytes(value, Endianness.LittleEndian);

         short result = converter.TestGetValue<short>(bytes, Endianness.LittleEndian);

         Assert.AreEqual(value, result);
      }

      [TestMethod]
      public void GetValue_UShortBytesProvided_UShortReturned()
      {
         ushort value = 2048;
         TestConverter converter = new TestConverter();
         byte[] bytes = converter.TestGetBytes(value, Endianness.LittleEndian);

         ushort result = converter.TestGetValue<ushort>(bytes, Endianness.LittleEndian);

         Assert.AreEqual(value, result);
      }

      [TestMethod]
      public void GetValue_GuidBytesProvided_GuidReturned()
      {
         Guid value = Guid.NewGuid();
         TestConverter converter = new TestConverter();
         byte[] bytes = converter.TestGetBytes(value, Endianness.LittleEndian);

         Guid result = converter.TestGetValue<Guid>(bytes, Endianness.LittleEndian);

         Assert.AreEqual(value, result);
      }

      [TestMethod]
      public void GetValue_DateTimeBytesProvided_DateTimeReturned()
      {
         DateTime value = DateTime.Now;
         TestConverter converter = new TestConverter();
         byte[] bytes = converter.TestGetBytes(value, Endianness.LittleEndian);

         DateTime result = converter.TestGetValue<DateTime>(bytes, Endianness.LittleEndian);

         Assert.AreEqual(value, result);
      }

      [TestMethod]
      public void GetValue_EnumBytesProvided_EnumReturned()
      {
         StringComparison value = StringComparison.Ordinal;
         TestConverter converter = new TestConverter();
         byte[] bytes = converter.TestGetBytes(value, Endianness.LittleEndian);

         StringComparison result = converter.TestGetValue<StringComparison>(bytes, Endianness.LittleEndian);

         Assert.AreEqual(value, result);
      }

      [TestMethod]
      [ExpectedException(typeof(NotSupportedException))]
      public void GetValue_InvalidType_ThrowsNotSupportedException()
      {
         TestConverter converter = new TestConverter();

         IList result = converter.TestGetValue<IList>(new byte[] { }, Endianness.LittleEndian);
      }

      [TestMethod]
      public void IsValueType_NullProvided_FalseReturned()
      {
         TestConverter converter = new TestConverter();

         bool result = converter.TestIsValueType(null as Type);

         Assert.IsFalse(result);
      }

      [TestMethod]
      public void IsValueType_ReferenceTypeProvided_FalseReturned()
      {
         TestConverter converter = new TestConverter();

         bool result = converter.TestIsValueType(typeof(IList));

         Assert.IsFalse(result);
      }

      [TestMethod]
      public void IsValueType_PrimitiveTypeProvided_TrueReturned()
      {
         TestConverter converter = new TestConverter();

         bool result = converter.TestIsValueType(typeof(int));

         Assert.IsTrue(result);
      }

      [TestMethod]
      public void IsValueType_DecimalTypeProvided_TrueReturned()
      {
         TestConverter converter = new TestConverter();

         bool result = converter.TestIsValueType(typeof(decimal));

         Assert.IsTrue(result);
      }

      [TestMethod]
      public void IsValueType_StringTypeProvided_TrueReturned()
      {
         TestConverter converter = new TestConverter();

         bool result = converter.TestIsValueType(typeof(string));

         Assert.IsTrue(result);
      }

      [TestMethod]
      public void IsValueType_GuidTypeProvided_TrueReturned()
      {
         TestConverter converter = new TestConverter();

         bool result = converter.TestIsValueType(typeof(Guid));

         Assert.IsTrue(result);
      }

      [TestMethod]
      public void IsValueType_DateTimeTypeProvided_TrueReturned()
      {
         TestConverter converter = new TestConverter();

         bool result = converter.TestIsValueType(typeof(DateTime));

         Assert.IsTrue(result);
      }

      [TestMethod]
      public void IsValueType_EnumTypeProvided_TrueReturned()
      {
         TestConverter converter = new TestConverter();

         bool result = converter.TestIsValueType(typeof(StringComparison));

         Assert.IsTrue(result);
      }

      [TestMethod]
      public void IsValueType_PrimitiveTypeNameProvided_TrueReturned()
      {
         TestConverter converter = new TestConverter();

         bool result = converter.TestIsValueType(typeof(int).AssemblyQualifiedName);

         Assert.IsTrue(result);
      }

      [TestMethod]
      public void IsStringType_NullProvided_FalseReturned()
      {
         TestConverter converter = new TestConverter();

         bool result = converter.TestIsStringType(null);

         Assert.IsFalse(result);
      }

      [TestMethod]
      public void IsStringType_PrimitiveTypeProvided_FalseReturned()
      {
         TestConverter converter = new TestConverter();

         bool result = converter.TestIsStringType(typeof(int));

         Assert.IsFalse(result);
      }

      [TestMethod]
      public void IsStringType_StringTypeProvided_TrueReturned()
      {
         TestConverter converter = new TestConverter();

         bool result = converter.TestIsStringType(typeof(string));

         Assert.IsTrue(result);
      }

      [TestMethod]
      public void TypeSize_NullProvided_ZeroReturned()
      {
         TestConverter converter = new TestConverter();

         int result = converter.TestTypeSize(null as Type);

         Assert.AreEqual(0, result);
      }

      [TestMethod]
      public void TypeSize_BoolTypeProvided_SizeReturned()
      {
         TestConverter converter = new TestConverter();

         int result = converter.TestTypeSize(typeof(bool));

         Assert.AreEqual(sizeof(bool), result);
      }

      [TestMethod]
      public void TypeSize_ByteTypeProvided_SizeReturned()
      {
         TestConverter converter = new TestConverter();

         int result = converter.TestTypeSize(typeof(byte));

         Assert.AreEqual(sizeof(byte), result);
      }

      [TestMethod]
      public void TypeSize_CharTypeProvided_SizeReturned()
      {
         TestConverter converter = new TestConverter();

         int result = converter.TestTypeSize(typeof(char));

         Assert.AreEqual(sizeof(char), result);
      }

      [TestMethod]
      public void TypeSize_DecimalTypeProvided_SizeReturned()
      {
         TestConverter converter = new TestConverter();

         int result = converter.TestTypeSize(typeof(decimal));

         Assert.AreEqual(sizeof(decimal), result);
      }

      [TestMethod]
      public void TypeSize_DoubleTypeProvided_SizeReturned()
      {
         TestConverter converter = new TestConverter();

         int result = converter.TestTypeSize(typeof(double));

         Assert.AreEqual(sizeof(double), result);
      }

      [TestMethod]
      public void TypeSize_FloatTypeProvided_SizeReturned()
      {
         TestConverter converter = new TestConverter();

         int result = converter.TestTypeSize(typeof(float));

         Assert.AreEqual(sizeof(float), result);
      }

      [TestMethod]
      public void TypeSize_IntTypeProvided_SizeReturned()
      {
         TestConverter converter = new TestConverter();

         int result = converter.TestTypeSize(typeof(int));

         Assert.AreEqual(sizeof(int), result);
      }

      [TestMethod]
      public void TypeSize_UIntTypeProvided_SizeReturned()
      {
         TestConverter converter = new TestConverter();

         int result = converter.TestTypeSize(typeof(uint));

         Assert.AreEqual(sizeof(uint), result);
      }

      [TestMethod]
      public void TypeSize_LongTypeProvided_SizeReturned()
      {
         TestConverter converter = new TestConverter();

         int result = converter.TestTypeSize(typeof(long));

         Assert.AreEqual(sizeof(long), result);
      }

      [TestMethod]
      public void TypeSize_ULongTypeProvided_SizeReturned()
      {
         TestConverter converter = new TestConverter();

         int result = converter.TestTypeSize(typeof(ulong));

         Assert.AreEqual(sizeof(ulong), result);
      }

      [TestMethod]
      public void TypeSize_ShortTypeProvided_SizeReturned()
      {
         TestConverter converter = new TestConverter();

         int result = converter.TestTypeSize(typeof(short));

         Assert.AreEqual(sizeof(short), result);
      }

      [TestMethod]
      public void TypeSize_UShortTypeProvided_SizeReturned()
      {
         TestConverter converter = new TestConverter();

         int result = converter.TestTypeSize(typeof(ushort));

         Assert.AreEqual(sizeof(ushort), result);
      }

      [TestMethod]
      public void TypeSize_GuidTypeProvided_SizeReturned()
      {
         TestConverter converter = new TestConverter();

         int result = converter.TestTypeSize(typeof(Guid));

         Assert.AreEqual(16, result);
      }

      [TestMethod]
      public void TypeSize_DateTimeTypeProvided_SizeReturned()
      {
         TestConverter converter = new TestConverter();

         int result = converter.TestTypeSize(typeof(DateTime));

         Assert.AreEqual(sizeof(long), result);
      }

      [TestMethod]
      public void TypeSize_EnumTypeProvided_SizeReturned()
      {
         TestConverter converter = new TestConverter();

         int result = converter.TestTypeSize(typeof(StringComparison));

         Assert.AreEqual(sizeof(int), result);
      }

      [TestMethod]
      public void TypeSize_PrimitiveTypeNameProvided_SizeReturned()
      {
         TestConverter converter = new TestConverter();

         int result = converter.TestTypeSize(typeof(int).AssemblyQualifiedName);

         Assert.AreEqual(sizeof(int), result);
      }
   }

   internal sealed class TestConverter : Converter
   {
      public byte[] TestGetBytes<T>(T value, Endianness endianness)
      {
         return GetBytes<T>(value, endianness);
      }

      public T TestGetValue<T>(byte[] bytes, Endianness endianness)
      {
         return GetValue<T>(bytes, endianness);
      }

      public bool TestIsValueType(Type type)
      {
         return IsValueType(type);
      }

      public bool TestIsValueType(string typeName)
      {
         return IsValueType(typeName);
      }

      public bool TestIsStringType(Type type)
      {
         return IsStringType(type);
      }

      public int TestTypeSize(Type type)
      {
         return TypeSize(type);
      }

      public int TestTypeSize(string typeName)
      {
         return TypeSize(typeName);
      }
   }
}
