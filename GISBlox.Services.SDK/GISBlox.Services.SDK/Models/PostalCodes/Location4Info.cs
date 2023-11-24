// ------------------------------------------------------------
// Copyright (c) Bartels Online.  All rights reserved.
// ------------------------------------------------------------

namespace GISBlox.Services.SDK.Models
{
   /// <summary>
   /// This class contains location information of a postal code area.
   /// </summary>
   public class Location4Info
   {
      /// <summary>
      /// The postal code area's Well-Known Text geometry.
      /// </summary>      
      public WKTGeometry Geometry { get; set; }

      /// <summary>
      /// The district(s) within the postal code.
      /// </summary>      
      public string Wijken { get; set; }

      /// <summary>
      /// The postal code's gemeente
      /// </summary>      
      public string Gemeente { get; set; }
   }
}
