// ------------------------------------------------------------
// Copyright (c) Bartels Online.  All rights reserved.
// ------------------------------------------------------------

namespace GISBlox.Services.SDK.Models
{
   /// <summary>
   /// This class represents a location on the earth's surface.
   /// </summary>
   public class Coordinate
   {
      /// <summary>
      /// Specifies the east-west position of a point on the earth's surface.
      /// </summary>
      public double Lat { get; set; }

      /// <summary>
      /// Specifies the north-south position of a point on the earth's surface.
      /// </summary>
      public double Lon { get; set; }

      /// <summary>
      /// Initializes a new instance of the GISBlox.Services.SDK.Models.Coordinate class.
      /// </summary>
      public Coordinate()
      { }

      /// <summary>
      /// Initializes a new instance of the GISBlox.Services.SDK.Models.Coordinate class.
      /// </summary>
      /// <param name="latitude">Specifies the east-west position of a point on the earth's surface.</param>
      /// <param name="longitude">Specifies the north-south position of a point on the earth's surface.</param>
      public Coordinate(double latitude, double longitude)
      {
         this.Lat = latitude;
         this.Lon = longitude;
      }
   }
}
