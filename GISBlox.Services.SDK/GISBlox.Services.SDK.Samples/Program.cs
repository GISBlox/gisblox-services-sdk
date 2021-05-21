using GISBlox.Services.SDK.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GISBlox.Services.SDK.Samples
{
   class Program
   {
      static async Task Main(string[] args)
      {
         if (args.Length < 2)
         {
            await Console.Error.WriteLineAsync("Usage: <GISBlox Services base url> <service key>");
            return;
         }

         var baseUrl = args[0];
         var serviceKey = args[1];

         try
         {
            Console.WriteLine("Creating client");
            using (var client = GISBloxClient.CreateClient(baseUrl, serviceKey))
            {
               //await ProjectionAPI(client);
               //await ConversionAPI(client);
               await InfoAPI(client);
            }
         }
         catch (Exception ex)
         {
            await Console.Error.WriteLineAsync($"Exception: { ex.Message}");
         }
         finally
         {
            Console.WriteLine("Press enter to exit");
            Console.ReadLine();
         }
      }

      private static async Task ProjectionAPI(GISBloxClient client)
      {
         #region RDS
         
         WriteLogHeader("Reprojecting WGS84 to RDS (simple)");

         Coordinate coord = new Coordinate(51.998929, 4.375587);         
         RDPoint rdPoint = await client.Projection.ToRDS(coord);

         Console.WriteLine($"RDPoint - X:{ rdPoint.X } Y:{ rdPoint.Y }");
                  
         WriteLogHeader("Reprojecting multiple coordinates to RDS (simple)");
         List<Coordinate> coords = new List<Coordinate>
         {
            new Coordinate(51.998929, 4.375587),
            new Coordinate(53.1, 4.2),
            new Coordinate(53.11, 4.3)
         };
         List<RDPoint> rdPoints = await client.Projection.ToRDS(coords);

         rdPoints.ForEach(r => Console.WriteLine($"RDPoint - X:{ r.X } Y:{ r.Y }"));
                  
         WriteLogHeader("Reprojecting WGS84 to RDS (full)");  
         Location location = await client.Projection.ToRDSComplete(coord);

         Console.WriteLine($"Coordinate - Lat: { location.Lat } Lon: { location.Lon } -> RDPoint - X:{ location.X } Y:{ location.Y }");
                  
         WriteLogHeader("Reprojecting multiple coordinates to RDS (full)");                  
         List<Location> locations = await client.Projection.ToRDSComplete(coords);

         locations.ForEach(location => Console.WriteLine($"Coordinate - Lat: { location.Lat } Lon: { location.Lon } -> RDPoint - X:{ location.X } Y:{ location.Y }"));

         #endregion
         
         #region WGS84
         
         WriteLogHeader("Reprojecting RDS to WGS84 (simple)");

         rdPoint = new RDPoint(85530, 446100);         
         coord = await client.Projection.ToWGS84(rdPoint, 6);                 // Round the coordinates to 6 digits

         Console.WriteLine($"Coordinate - Lat: { coord.Lat } Lon: { coord.Lon }");
                  
         WriteLogHeader("Reprojecting multiple RDPoints to WGS84 (simple)");
         rdPoints = new List<RDPoint>
         {
            new RDPoint(100000, 555000),
            new RDPoint(1, 2),
            new RDPoint(111000, 550000)
         };
         coords = await client.Projection.ToWGS84(rdPoints);                  // No rounding

         coords.ForEach(c => Console.WriteLine($"Coordinate - Lat: { c.Lat } Lon: { c.Lon }"));
                  
         WriteLogHeader("Reprojecting RDS to WGS84 (full)");
         location = await client.Projection.ToWGS84Complete(rdPoint);

         Console.WriteLine($"RDPoint - X:{ location.X } Y:{ location.Y } -> Coordinate - Lat: { location.Lat } Lon: { location.Lon }");
                  
         WriteLogHeader("Reprojecting multiple RDPoints to WGS84 (full)");
         locations = await client.Projection.ToWGS84Complete(rdPoints, 5);    // Round the coordinates to 5 digits

         locations.ForEach(location => Console.WriteLine($"RDPoint - X:{ location.X } Y:{ location.Y } -> Coordinate - Lat: { location.Lat } Lon: { location.Lon }"));

         #endregion
      }

      private static async Task ConversionAPI(GISBloxClient client)
      {         
         WriteLogHeader("Convert POINT");

         WKT wkt = new WKT("POINT (30 10 5)");
         await ConvertToGeoJson(wkt, client);

         WriteLogHeader("Convert MULTIPOINT");

         wkt = new WKT("MULTIPOINT ((10 40), (40 30 2), (20 20), (30 10))");
         await ConvertToGeoJson(wkt, client);

         WriteLogHeader("Convert LINESTRING");

         wkt = new WKT("LINESTRING (30 10, 10 30, 40 40)");
         await ConvertToGeoJson(wkt, client, true);         // Include in a FeatureCollection

         WriteLogHeader("Convert MULTILINESTRING");

         wkt = new WKT("MULTILINESTRING ((10 10, 20 20, 10 40),(40 40, 30 30, 40 20, 30 10))");
         await ConvertToGeoJson(wkt, client);

         WriteLogHeader("Convert POLYGON");

         wkt = new WKT("POLYGON ((30 10, 40 40, 20 40, 10 20, 30 10))");
         await ConvertToGeoJson(wkt, client);

         WriteLogHeader("Convert POLYGON with inner ring");

         wkt = new WKT("POLYGON ((35 10, 45 45, 15 40, 10 20, 35 10),(20 30, 35 35, 30 20, 20 30))");
         await ConvertToGeoJson(wkt, client);

         WriteLogHeader("Convert MULTIPOLYGON");

         wkt = new WKT("MULTIPOLYGON (((30 20, 45 40, 10 40, 30 20)),((15 5, 40 10, 10 20, 5 10, 15 5)))");
         await ConvertToGeoJson(wkt, client, true);         // Include in a FeatureCollection

         WriteLogHeader("Convert MULTIPOLYGON with inner ring");

         wkt = new WKT("MULTIPOLYGON (((40 40, 20 45, 45 30, 40 40)),((20 35, 10 30, 10 10, 30 5, 45 20, 20 35),(30 20, 20 15, 20 25, 30 20)))");
         await ConvertToGeoJson(wkt, client);         
      }

      private async static Task ConvertToGeoJson(WKT wkt, GISBloxClient client, bool asFeatureCollection = false)
      {
         string geoJson = await client.Conversion.ToGeoJson(wkt, asFeatureCollection);
         Console.WriteLine(geoJson);
      }

      private async static Task InfoAPI(GISBloxClient client)
      {
         List<Subscription> subscriptions = await client.Info.GetSubscriptions();
      }

      private static void WriteLogHeader(string text) 
      {
         Console.WriteLine();
         Console.WriteLine(new string('=', text.Length));
         Console.WriteLine(text);
      }
   }
}
