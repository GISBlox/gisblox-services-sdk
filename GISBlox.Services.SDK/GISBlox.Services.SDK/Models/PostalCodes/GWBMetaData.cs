// ------------------------------------------------------------
// Copyright (c) Bartels Online.  All rights reserved.
// ------------------------------------------------------------

namespace GISBlox.Services.SDK.Models
{
   /// <summary>
   /// This class specifies metadata for the returned record set.
   /// </summary>
   public class GWBMetaData
   {
      /// <summary>
      /// Describes the type of GWB request.
      /// </summary>      
      public string GWBType { get; set; }

      /// <summary>
      /// The ID of the GWB parent.
      /// </summary>      
      public int ParentID { get; set; }

      /// <summary>
      /// The amount of returned records.
      /// </summary>      
      public int TotalRecords { get; set; }
   }
}
