using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
   }
}
