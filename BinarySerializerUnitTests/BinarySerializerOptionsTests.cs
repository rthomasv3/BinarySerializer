using CodeCompendium.BinarySerialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CodeCompendium.BinarySerializerUnitTests
{
   [TestClass]
   public sealed class BinarySerializerOptionsTests
   {
      [TestMethod]
      public void Constructor_ArgumentsProvided_PropertiesSet()
      {
         BinarySerializerOptions options = new BinarySerializerOptions(Endianness.LittleEndian, true, false);

         Assert.AreEqual(Endianness.LittleEndian, options.Endianness);
         Assert.AreEqual(true, options.IgnoreReadOnlyProperties);
         Assert.AreEqual(false, options.PropertyNameCaseInsensitive);
      }
   }
}
