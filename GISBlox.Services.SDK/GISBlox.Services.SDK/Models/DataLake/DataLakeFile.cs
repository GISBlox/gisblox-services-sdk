using System;

namespace GISBlox.Services.SDK.Models
{
   /// <summary>
   /// Represents a file in the data lake.
   /// </summary>
   public class DataLakeFile
   {
      /// <summary>
      /// The creation date of the file.
      /// </summary>
      public DateTimeOffset? CreateDate { get; set; }

      /// <summary>
      /// The last modified date of the file.
      /// </summary>
      public DateTimeOffset? ModifiedDate { get; set; }

      /// <summary>
      /// The size of the file in bytes.
      /// </summary>
      public long? Size { get; set; }

      /// <summary>
      /// The content type of the file.
      /// </summary> 
      public string ContentType { get; set; }

      /// <summary>
      /// The name of the file.
      /// </summary>
      public string Name { get; set; }
   }
}
