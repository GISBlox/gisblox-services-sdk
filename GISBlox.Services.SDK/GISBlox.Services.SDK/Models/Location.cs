namespace GISBlox.Services.SDK.Models
{
   /// <summary>
   /// This class represents a location on earth in both RDNew (EPSG:28992) and WGS84 (EPSG:4326) projections.
   /// </summary>
   public class Location
   {
      /// <summary>
      /// The X location ('Easting') in RDNew projection (EPSG:28992).
      /// </summary>
      public double X { get; set; }

      /// <summary>
      /// The Y location ('Northing') in RDNew projection (EPSG:28992).
      /// </summary>
      public double Y { get; set; }

      /// <summary>
      /// The latitude (east-west position) in WGS84 Mercator projection (EPSG:4326).
      /// </summary>
      public double Lat { get; set; }

      /// <summary>
      /// The longitude (north-south) in WGS84 Mercator projection (EPSG:4326).
      /// </summary>
      public double Lon { get; set; }

      /// <summary>
      /// Initializes a new instance of the GISBlox.Services.SDK.Models.Location class. 
      /// </summary>
      public Location()
      { }

      /// <summary>
      /// Initializes a new instance of the GISBlox.Services.SDK.Models.Location class.
      /// </summary>
      /// <param name="x">The X location ('Easting') in RDNew projection (EPSG:28992).</param>
      /// <param name="y">The Y location ('Northing') in RDNew projection (EPSG:28992).</param>
      /// <param name="latitude">The latitude (east-west position) in WGS84 Mercator projection (EPSG:4326).</param>
      /// <param name="longitude">The longitude (north-south) in WGS84 Mercator projection (EPSG:4326).</param>
      public Location(int x, int y, double latitude, double longitude)
      {
         this.X = x;
         this.Y = y;
         this.Lat = latitude;
         this.Lon = longitude;
      }
   }
}
