using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GISBlox.Services.SDK.Models;

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
               await ProjectionAPI(client);
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

         Console.WriteLine("==================================");
         Console.WriteLine("Reprojecting WGS84 to RDS (simple)");
         Coordinate coord = new Coordinate(51.998929, 4.375587);
         
         RDPoint rdPoint = await client.Projection.ToRDS(coord);
         Console.WriteLine($"RDPoint - X:{ rdPoint.X } Y:{ rdPoint.Y }");

         Console.WriteLine("\r\n=================================================");
         Console.WriteLine("Reprojecting multiple coordinates to RDS (simple)");

         List<Coordinate> coords = new List<Coordinate>
         {
            new Coordinate(51.998929, 4.375587),
            new Coordinate(53.1, 4.2),
            new Coordinate(53.11, 4.3)
         };

         List<RDPoint> rdPoints = await client.Projection.ToRDS(coords);
         rdPoints.ForEach(r => Console.WriteLine($"RDPoint - X:{ r.X } Y:{ r.Y }"));

         Console.WriteLine("\r\n================================");
         Console.WriteLine("Reprojecting WGS84 to RDS (full)");        
         
         Location location = await client.Projection.ToRDSComplete(coord);
         Console.WriteLine($"Coordinate - Lat: { location.Lat } Lon: { location.Lon } -> RDPoint - X:{ location.X } Y:{ location.Y }");

         Console.WriteLine("\r\n===============================================");
         Console.WriteLine("Reprojecting multiple coordinates to RDS (full)");
                  
         List<Location> locations = await client.Projection.ToRDSComplete(coords);
         locations.ForEach(location => Console.WriteLine($"Coordinate - Lat: { location.Lat } Lon: { location.Lon } -> RDPoint - X:{ location.X } Y:{ location.Y }"));

         #endregion
         
         #region WGS84

         Console.WriteLine("\r\n==================================");
         Console.WriteLine("Reprojecting RDS to WGS84 (simple)");
         rdPoint = new RDPoint(100000, 555000);         

         coord = await client.Projection.ToWGS84(rdPoint);
         Console.WriteLine($"Coordinate - Lat: { coord.Lat } Lon: { coord.Lon }");

         Console.WriteLine("\r\n=================================================");
         Console.WriteLine("Reprojecting multiple RDPoints to WGS84 (simple)");

         rdPoints = new List<RDPoint>
         {
            new RDPoint(100000, 555000),
            new RDPoint(1, 2),
            new RDPoint(111000, 550000)
         };

         coords = await client.Projection.ToWGS84(rdPoints);
         coords.ForEach(c => Console.WriteLine($"Coordinate - Lat: { c.Lat } Lon: { c.Lon }"));

         Console.WriteLine("\r\n================================");
         Console.WriteLine("Reprojecting RDS to WGS84 (full)");

         location = await client.Projection.ToWGS84Complete(rdPoint);
         Console.WriteLine($"RDPoint - X:{ location.X } Y:{ location.Y } -> Coordinate - Lat: { location.Lat } Lon: { location.Lon }");

         Console.WriteLine("\r\n==============================================");
         Console.WriteLine("Reprojecting multiple RDPoints to WGS84 (full)");

         locations = await client.Projection.ToWGS84Complete(rdPoints);
         locations.ForEach(location => Console.WriteLine($"RDPoint - X:{ location.X } Y:{ location.Y } -> Coordinate - Lat: { location.Lat } Lon: { location.Lon }"));

         #endregion
      }
   }
}
