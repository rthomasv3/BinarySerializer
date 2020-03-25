namespace CodeCompendium.BinarySerialization
{
   /// <summary>
   /// Class used to configure binary serialization options.
   /// </summary>
   public sealed class BinarySerializerOptions
   {
      #region Constructor

      /// <summary>
      /// Creates a new intance of the <see cref="BinarySerializerOptions"/> class.
      /// </summary>
      public BinarySerializerOptions(Endianness endianness = Endianness.Default, bool ignoreReadOnlyProperties = false, 
         bool propertyNameCaseInsensitive = true)
      {
         Endianness = endianness;
         IgnoreReadOnlyProperties = ignoreReadOnlyProperties;
         PropertyNameCaseInsensitive = propertyNameCaseInsensitive;
      }

      #endregion

      #region Properties

      /// <summary>
      /// Gets or sets endianness.
      /// </summary>
      public Endianness Endianness { get; set; }

      /// <summary>
      /// If true, readonly properties will be ignored when a class is serialized.
      /// </summary>
      public bool IgnoreReadOnlyProperties { get; set; }

      /// <summary>
      /// If true, deserialization will ignore property case when assigning values.
      /// </summary>
      public bool PropertyNameCaseInsensitive { get; set; }

      #endregion
   }
}
