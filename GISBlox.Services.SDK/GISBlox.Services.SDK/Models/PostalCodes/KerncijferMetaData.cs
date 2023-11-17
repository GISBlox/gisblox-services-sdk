// ------------------------------------------------------------
// Copyright (c) Bartels Online.  All rights reserved.
// ------------------------------------------------------------

namespace GISBlox.Services.SDK.Models
{
   /// <summary>
   /// This class specifies metadata for the returned record set.
   /// </summary>
   public class KerncijferMetaData
   {
      /// <summary>
      /// The postal code for which results are generated.
      /// </summary>      
      public string Id { get; set; }

      /// <summary>
      /// The amount of returned key figures.
      /// </summary>      
      public int TotalAttributes { get; set; }
   }
}
