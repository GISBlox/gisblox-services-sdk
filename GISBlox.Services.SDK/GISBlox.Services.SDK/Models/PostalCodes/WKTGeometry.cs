// ------------------------------------------------------------
// Copyright (c) Bartels Online.  All rights reserved.
// ------------------------------------------------------------

namespace GISBlox.Services.SDK.Models
{
   /// <summary>
   /// This class represents a WKT geometry representation.
   /// </summary>
   public class WKTGeometry
   {
      /// <summary>
      /// Specifies the WKT geometry.
      /// </summary>            
      public string WKT { get; set; }

      /// <summary>
      /// The center location of the WKT geometry.
      /// </summary>      
      public string Centroid { get; set; }

      /// <summary>
      /// The area of the postal code in meters squared.
      /// </summary>      
      public double AreaM2 { get; set; }

      /// <summary>
      /// The perimeter of the postal code area in meters.
      /// </summary>      
      public double PerimeterM { get; set; }

      /// <summary>
      /// The amount of vertices of the postal code area.
      /// </summary>      
      public int Vertices { get; set; }
   }
}
