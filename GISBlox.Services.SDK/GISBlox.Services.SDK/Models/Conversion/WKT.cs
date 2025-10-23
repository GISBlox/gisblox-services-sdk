// ------------------------------------------------------------
// Copyright (c) Bartels Online.  All rights reserved.
// ------------------------------------------------------------

using System.Collections.Generic;

namespace GISBlox.Services.SDK.Models
{
   /// <summary>
   /// This class represents a WKT geometry string and its properties.
   /// </summary>
   public class WKT
   {
      /// <summary>
      /// Specifies the WKT geometry.
      /// </summary>
      public string Geometry { get; set; }

      /// <summary>
      /// Specifies the properties associated with the WKT geometry.
      /// </summary>
      public List<Dictionary<string, object>> Properties { get; set; }

      /// <summary>
      /// Initializes a new instance of GISBlox.Models.Conversion.WKT class.
      /// </summary>
      public WKT()
      { }

      /// <summary>
      /// Initializes a new instance of GISBlox.Models.Conversion.WKT class.
      /// </summary>
      /// <param name="geometry">A WKT geometry.</param>
      public WKT(string geometry)
      {
         this.Geometry = geometry;
      }

      /// <summary>
      /// Initializes a new instance of GISBlox.Models.Conversion.WKT class.
      /// </summary>
      /// <param name="geometry">A WKT geometry.</param>
      /// <param name="properties">A list of properties associated with the geometry.</param>
      public WKT(string geometry, List<Dictionary<string, object>> properties)
      {
         this.Geometry = geometry;
         this.Properties = properties;
      }
   }
}
