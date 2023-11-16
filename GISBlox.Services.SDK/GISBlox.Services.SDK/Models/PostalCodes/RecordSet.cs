// ------------------------------------------------------------
// Copyright (c) Bartels Online.  All rights reserved.
// ------------------------------------------------------------

namespace GISBlox.Services.SDK.Models
{
   /// <summary>
   /// Metadata about the result record set, including bounding box information and record count.
   /// </summary>
   public class RecordSet
   {
      /// <summary>
      /// The amount of returned geometries.
      /// </summary>
      public int TotalRecords { get; set; }

      /// <summary>
      /// Represents the bounding box for the returned geometries.
      /// </summary>
      public BoundingBox BBox { get; set; }
   }
}
