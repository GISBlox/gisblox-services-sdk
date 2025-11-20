// ------------------------------------------------------------
// Copyright (c) Bartels Online.  All rights reserved.
// ------------------------------------------------------------

using System.Collections.Generic;

namespace GISBlox.Services.SDK.Models
{
   /// <summary>
   /// This class represents a WKB geometry and its properties.
   /// </summary>
   public class WKB
   {
      /// <summary>
      /// Specifies the WKB geometry.
      /// </summary>
      public byte[] Geometry { get; set; }

      /// <summary>
      /// Specifies the properties associated with the WKB geometry.
      /// </summary>
      public List<Dictionary<string, object>> Properties { get; set; }

      /// <summary>
      /// Initializes a new instance of GISBlox.Services.SDK.Models.WKB class.
      /// </summary>
      public WKB()
      { }

      /// <summary>
      /// Initializes a new instance of GISBlox.Services.SDK.Models.WKB class with the specified geometry.
      /// </summary>
      /// <param name="geometry">The WKB geometry.</param>
      public WKB(byte[] geometry)
      {
         this.Geometry = geometry;
      }

      /// <summary>
      /// Initializes a new instance of GISBlox.Services.SDK.Models.WKB class with the specified geometry and properties.
      /// </summary>
      /// <param name="geometry">The WKB geometry.</param>
      /// <param name="properties">The properties associated with the WKB geometry.</param>
      public WKB(byte[] geometry, List<Dictionary<string, object>> properties)
      {
         this.Geometry = geometry;
         this.Properties = properties;
      }
   }
}
