namespace CodeCompendium.BinarySerialization
{
   /// <summary>
   /// Enum used to set byte ordering.
   /// </summary>
   public enum Endianness
   {
      /// <summary>
      /// System default endianness.
      /// </summary>
      Default = 0,
      /// <summary>
      /// Byte ordering that places the most significant byte first.
      /// </summary>
      BigEndian = 1,
      /// <summary>
      /// Byte ordering that places the least significant byte first.
      /// </summary>
      LittleEndian = 2
   }
}
