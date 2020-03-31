using System;
using System.Collections.Generic;
using System.IO;
using CodeCompendium.BinarySerialization;
using CodeCompendium.BinarySerialization.Version1;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CodeCompendium.BinarySerializerUnitTests
{
   [TestClass]
   public sealed class Version1SerializerTests
   {
      [TestMethod]
      [ExpectedException(typeof(ArgumentNullException))]
      public void GetObjectBytes_NullObjectProvided_ThrowsArgumentNullException()
      {
         Serializer serializer = new Serializer();

         serializer.GetObjectBytes(null, Endianness.LittleEndian, false);
      }

      [TestMethod]
      public void GetObjectBytes_ObjectProvided_BytesReturned()
      {
         Serializer serializer = new Serializer();

         byte[] result = serializer.GetObjectBytes(new Tuple<int>(1), Endianness.LittleEndian, false);

         Assert.IsNotNull(result);
      }

      [TestMethod]
      public void GetObjectBytes_IgnoreReadonlyPropertiesTrue_PropertiesIgnored()
      {
         Serializer serializer = new Serializer();

         byte[] ignore = serializer.GetObjectBytes(new Tuple<int>(1), Endianness.LittleEndian, true);
         byte[] dontIgnore = serializer.GetObjectBytes(new Tuple<int>(1), Endianness.LittleEndian, false);

         Assert.IsTrue(dontIgnore.Length > ignore.Length);
      }

      [TestMethod]
      [ExpectedException(typeof(ArgumentNullException))]
      public void GetObject_NullTypeProvided_ThrowsArgumentNullException()
      {
         Serializer serializer = new Serializer();

         serializer.GetObject(null, new MemoryStream(), Endianness.LittleEndian, true);
      }

      [TestMethod]
      [ExpectedException(typeof(ArgumentNullException))]
      public void GetObject_NullStreamProvided_ThrowsArgumentNullException()
      {
         Serializer serializer = new Serializer();

         serializer.GetObject(typeof(Serializer), null, Endianness.LittleEndian, true);
      }

      [TestMethod]
      public void GetObject_TestSetterClassByteStreamProvided_MatchingInstanceReturned()
      {
         Serializer serializer = new Serializer();
         TestSetterClass testClass = new TestSetterClass()
         {
            IntProperty = 1,
            FloatProperty = 2.0f,
            StringProperty = "Test String",
            GuidProperty = Guid.NewGuid(),
            IntListProperty = new List<int>() { 1, 2, 3 },
            IntArrayProperty = new int[] { 3, 2, 1 },
            DictionaryProperty = new Dictionary<int, string>() { { 1, "One" }, { 2, "Two" } },
            ObjectProperty = new Tuple<int, int>(1, 2)
         };
         byte[] bytes = serializer.GetObjectBytes(testClass, Endianness.LittleEndian, false);

         MemoryStream memoryStream = new MemoryStream(bytes);
         TestSetterClass result = serializer.GetObject(typeof(TestSetterClass), memoryStream, Endianness.LittleEndian, true) as TestSetterClass;
         memoryStream.Dispose();

         Assert.AreEqual(testClass.IntProperty, result.IntProperty);
         Assert.AreEqual(testClass.FloatProperty, result.FloatProperty);
         Assert.AreEqual(testClass.StringProperty, result.StringProperty);
         Assert.AreEqual(testClass.GuidProperty, result.GuidProperty);
         CollectionAssert.AreEqual(testClass.IntListProperty, result.IntListProperty);
         CollectionAssert.AreEqual(testClass.IntArrayProperty, result.IntArrayProperty);
         CollectionAssert.AreEqual(testClass.DictionaryProperty, result.DictionaryProperty);
         Assert.AreEqual(testClass.ObjectProperty.Item1, result.ObjectProperty.Item1);
         Assert.AreEqual(testClass.ObjectProperty.Item2, result.ObjectProperty.Item2);
      }

      [TestMethod]
      public void GetObject_TestSetterClassByteStreamProvided_MatchingTestSetterRemovedClassInstanceReturned()
      {
         Serializer serializer = new Serializer();
         TestSetterClass testClass = new TestSetterClass()
         {
            IntProperty = 1,
            FloatProperty = 2.0f,
            StringProperty = "Test String",
            GuidProperty = Guid.NewGuid(),
            IntListProperty = new List<int>() { 1, 2, 3 },
            IntArrayProperty = new int[] { 3, 2, 1 },
            DictionaryProperty = new Dictionary<int, string>() { { 1, "One" }, { 2, "Two" } },
            ObjectProperty = new Tuple<int, int>(1, 2)
         };
         byte[] bytes = serializer.GetObjectBytes(testClass, Endianness.LittleEndian, false);

         MemoryStream memoryStream = new MemoryStream(bytes);
         TestSetterRemovedClass result = serializer.GetObject(typeof(TestSetterRemovedClass), memoryStream, Endianness.LittleEndian, true) as TestSetterRemovedClass;
         memoryStream.Dispose();

         Assert.AreEqual(testClass.IntProperty, result.IntProperty);
         Assert.AreEqual(testClass.StringProperty, result.StringProperty);
         Assert.AreEqual(testClass.GuidProperty, result.GuidProperty);
      }

      [TestMethod]
      public void GetObject_TestSetterClassByteStreamProvided_MatchingTestSetterAddedClassInstanceReturned()
      {
         Serializer serializer = new Serializer();
         TestSetterClass testClass = new TestSetterClass()
         {
            IntProperty = 1,
            FloatProperty = 2.0f,
            StringProperty = "Test String",
            GuidProperty = Guid.NewGuid(),
            IntListProperty = new List<int>() { 1, 2, 3 },
            IntArrayProperty = new int[] { 3, 2, 1 },
            DictionaryProperty = new Dictionary<int, string>() { { 1, "One" }, { 2, "Two" } },
            ObjectProperty = new Tuple<int, int>(1, 2),
            EnumProperty = StringComparison.Ordinal
         };
         byte[] bytes = serializer.GetObjectBytes(testClass, Endianness.LittleEndian, false);

         MemoryStream memoryStream = new MemoryStream(bytes);
         TestSetterAddedClass result = serializer.GetObject(typeof(TestSetterAddedClass), memoryStream, Endianness.LittleEndian, true) as TestSetterAddedClass;
         memoryStream.Dispose();

         Assert.AreEqual(false, result.BoolProperty);
         Assert.AreEqual(testClass.IntProperty, result.IntProperty);
         Assert.AreEqual(testClass.FloatProperty, result.FloatProperty);
         Assert.AreEqual(testClass.StringProperty, result.StringProperty);
         Assert.AreEqual(testClass.GuidProperty, result.GuidProperty);
         CollectionAssert.AreEqual(testClass.IntListProperty, result.IntListProperty);
         CollectionAssert.AreEqual(testClass.IntArrayProperty, result.IntArrayProperty);
         CollectionAssert.AreEqual(testClass.DictionaryProperty, result.DictionaryProperty);
         Assert.AreEqual(testClass.ObjectProperty.Item1, result.ObjectProperty.Item1);
         Assert.AreEqual(testClass.ObjectProperty.Item2, result.ObjectProperty.Item2);
         Assert.AreEqual(testClass.EnumProperty, result.EnumProperty);
      }

      [TestMethod]
      public void GetObject_TestSetterClassCaseSensitivePropertyNames_PropertyNotSet()
      {
         Serializer serializer = new Serializer();
         TestSetterClass testClass = new TestSetterClass()
         {
            IntProperty = 1
         };
         byte[] bytes = serializer.GetObjectBytes(testClass, Endianness.LittleEndian, false);

         MemoryStream memoryStream = new MemoryStream(bytes);
         TestSetterCaseSensitiveClass result = serializer.GetObject(typeof(TestSetterCaseSensitiveClass), memoryStream, Endianness.LittleEndian, false) as TestSetterCaseSensitiveClass;
         memoryStream.Dispose();

         Assert.AreNotEqual(testClass.IntProperty, result.intProperty);
      }

      [TestMethod]
      public void GetObject_TestConstructorClassByteStreamProvided_MatchingInstanceReturned()
      {
         Serializer serializer = new Serializer();
         TestConstructorClass testClass = new TestConstructorClass(1, "Test Value");
         byte[] bytes = serializer.GetObjectBytes(testClass, Endianness.LittleEndian, false);

         MemoryStream memoryStream = new MemoryStream(bytes);
         TestConstructorClass result = serializer.GetObject(typeof(TestConstructorClass), memoryStream, Endianness.LittleEndian, true) as TestConstructorClass;
         memoryStream.Dispose();

         Assert.AreEqual(testClass.IntProperty, result.IntProperty);
         Assert.AreEqual(testClass.StringProperty, result.StringProperty);
      }

      [TestMethod]
      public void GetObject_TestConstructorClassCaseSensitivePropertyNames_PropertyNotSet()
      {
         Serializer serializer = new Serializer();
         TestConstructorClass testClass = new TestConstructorClass(1, "Test Value");
         byte[] bytes = serializer.GetObjectBytes(testClass, Endianness.LittleEndian, false);

         MemoryStream memoryStream = new MemoryStream(bytes);
         TestConstructorClass result = serializer.GetObject(typeof(TestConstructorClass), memoryStream, Endianness.LittleEndian, false) as TestConstructorClass;
         memoryStream.Dispose();

         Assert.AreNotEqual(testClass.IntProperty, result.IntProperty);
         Assert.AreNotEqual(testClass.StringProperty, result.StringProperty);
      }

      [TestMethod]
      public void GetObject_TestConstructorClassByteStreamProvided_MatchingTestConstructorRemovedClassInstanceReturned()
      {
         Serializer serializer = new Serializer();
         TestConstructorClass testClass = new TestConstructorClass(1, "Test Value");
         byte[] bytes = serializer.GetObjectBytes(testClass, Endianness.LittleEndian, false);

         MemoryStream memoryStream = new MemoryStream(bytes);
         TestConstructorRemovedClass result = serializer.GetObject(typeof(TestConstructorRemovedClass), memoryStream, Endianness.LittleEndian, true) as TestConstructorRemovedClass;
         memoryStream.Dispose();

         Assert.AreEqual(testClass.IntProperty, result.IntProperty);
      }

      [TestMethod]
      public void GetObject_TestConstructorClassByteStreamProvided_MatchingTestConstructorAddedClassInstanceReturned()
      {
         Serializer serializer = new Serializer();
         TestConstructorClass testClass = new TestConstructorClass(1, "Test Value");
         byte[] bytes = serializer.GetObjectBytes(testClass, Endianness.LittleEndian, false);

         MemoryStream memoryStream = new MemoryStream(bytes);
         TestConstructorAddedClass result = serializer.GetObject(typeof(TestConstructorAddedClass), memoryStream, Endianness.LittleEndian, true) as TestConstructorAddedClass;
         memoryStream.Dispose();

         Assert.AreEqual(false, result.BoolProperty);
         Assert.AreEqual(testClass.IntProperty, result.IntProperty);
         Assert.AreEqual(testClass.StringProperty, result.StringProperty);
         Assert.AreEqual(null, result.TestSetterClassProperty);
      }
   }

   public class TestSetterClass
   {
      public int IntProperty { get; set; }
      public float FloatProperty { get; set; }
      public string StringProperty { get; set; }
      public Guid GuidProperty { get; set; }
      public List<int> IntListProperty { get; set; }
      public int[] IntArrayProperty { get; set; }
      public Dictionary<int, string> DictionaryProperty { get; set; }
      public Tuple<int, int> ObjectProperty { get; set; }
      public StringComparison EnumProperty { get; set; }
   }

   public class TestSetterRemovedClass
   {
      public int IntProperty { get; set; }
      public string StringProperty { get; set; }
      public Guid GuidProperty { get; set; }
   }

   public class TestSetterAddedClass
   {
      public bool BoolProperty { get; set; }
      public int IntProperty { get; set; }
      public float FloatProperty { get; set; }
      public string StringProperty { get; set; }
      public Guid GuidProperty { get; set; }
      public List<int> IntListProperty { get; set; }
      public int[] IntArrayProperty { get; set; }
      public Dictionary<int, string> DictionaryProperty { get; set; }
      public Tuple<int, int> ObjectProperty { get; set; }
      public StringComparison EnumProperty { get; set; }
   }

   public class TestSetterCaseSensitiveClass
   {
      public int intProperty { get; set; }
   }

   public class TestConstructorClass
   {
      public TestConstructorClass(int intProperty, string stringProperty)
      {
         IntProperty = intProperty;
         StringProperty = stringProperty;
      }
      public int IntProperty { get; }
      public string StringProperty { get; }
   }

   public class TestConstructorRemovedClass
   {
      public TestConstructorRemovedClass(int intProperty)
      {
         IntProperty = intProperty;
      }
      public int IntProperty { get; }
   }

   public class TestConstructorAddedClass
   {
      private TestConstructorAddedClass()
      {
      }
      public TestConstructorAddedClass(bool boolProperty, int intProperty, string stringProperty, 
         TestSetterClass testSetterClassProperty)
      {
         BoolProperty = boolProperty;
         IntProperty = intProperty;
         StringProperty = stringProperty;
         TestSetterClassProperty = testSetterClassProperty;
      }
      public TestConstructorAddedClass(bool boolProperty)
      {
         BoolProperty = boolProperty;
      }
      public bool BoolProperty { get; }
      public int IntProperty { get; }
      public string StringProperty { get; }
      public TestSetterClass TestSetterClassProperty { get; }
   }
}
