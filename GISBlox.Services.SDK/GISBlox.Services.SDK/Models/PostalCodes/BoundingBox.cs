// ------------------------------------------------------------
// Copyright (c) Bartels Online.  All rights reserved.
// ------------------------------------------------------------

namespace GISBlox.Services.SDK.Models
{
   /// <summary>
   /// This class defines a rectangular region of the 2D coordinate plane.
   /// </summary>
   public class BoundingBox
   {
      /// <summary>
      /// The maximum X-coordinate.
      /// </summary>
      public double MaxX { get; set; }

      /// <summary>
      /// The minimum X-coordinate.
      /// </summary>
      public double MinX { get; set; }

      /// <summary>
      /// The maximum Y-coordinate.
      /// </summary>
      public double MaxY { get; set; }

      /// <summary>
      /// The minimum Y-coordinate.
      /// </summary>
      public double MinY { get; set; }
   }
}
