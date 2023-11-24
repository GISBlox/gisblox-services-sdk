// ------------------------------------------------------------
// Copyright (c) Bartels Online.  All rights reserved.
// ------------------------------------------------------------

namespace GISBlox.Services.SDK.Models
{
   /// <summary>
   /// This class specifies metadata for the returned geometries.
   /// </summary>
   public class MetaData
   {
      /// <summary>
      /// The spatial reference identifier code.
      /// </summary>
      public int EPSG { get; set; }

      /// <summary>
      /// The input data of the request (postal code, WKT string, etc).
      /// </summary>
      public string Query { get; set; }

      /// <summary>
      /// Metadata of the result record set. 
      /// </summary>
      public RecordSet RecordSet { get; set; }
   }
}
