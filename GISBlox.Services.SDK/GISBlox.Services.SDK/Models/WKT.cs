// ------------------------------------------------------------
// Copyright (c) Bartels Online.  All rights reserved.
// ------------------------------------------------------------

namespace GISBlox.Services.SDK.Models
{
   /// <summary>
   /// This class represents a WKT geometry string representation.
   /// </summary>
   public class WKT
   {
      /// <summary>
      /// Specifies the WKT geometry.
      /// </summary>
      public string Geometry { get; set; }

      /// <summary>
      /// Initializes a new instance of the GISBlox.Services.SDK.Models.WKT class.
      /// </summary>
      public WKT()
      { }

      /// <summary>
      /// Initializes a new instance of the GISBlox.Services.SDK.Models.WKT class.
      /// </summary>
      /// <param name="geometry">A WKT geometry.</param>
      public WKT(string geometry)
      {
         this.Geometry = geometry;
      }
   }
}
