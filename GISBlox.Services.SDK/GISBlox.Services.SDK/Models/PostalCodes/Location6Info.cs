// ------------------------------------------------------------
// Copyright (c) Bartels Online.  All rights reserved.
// ------------------------------------------------------------

namespace GISBlox.Services.SDK.Models
{
   /// <summary>
   /// This class contains location information of a postal code.
   /// </summary>
   public class Location6Info
   {
      /// <summary>
      /// The postal code's Well-Known Text geometry.
      /// </summary>     
      public WKTGeometry Geometry { get; set; }

      /// <summary>
      /// The postal code's neighbourhood
      /// </summary>     
      public string Buurt { get; set; }

      /// <summary>
      /// The postal code's district
      /// </summary>      
      public string Wijk { get; set; }

      /// <summary>
      /// The postal code's gemeente
      /// </summary>      
      public string Gemeente { get; set; }
   }
}
