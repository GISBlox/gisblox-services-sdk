using System;
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

         Console.WriteLine("Creating client");
         using (var client = GISBloxClient.CreateClient(baseUrl, serviceKey))
         {
            await ProjectionAPI(client);            
         }

         Console.WriteLine("Press enter to exit");
         Console.ReadLine();
      }

      private static async Task ProjectionAPI(GISBloxClient client)
      {
         Console.WriteLine("Reprojecting to RDS (simple)");

         Coordinate c = new Coordinate(51.998929, 4.375587);
         RDPoint rdPoint = await client.Projection.ToRDS(c);

         Console.WriteLine($"RDPoint - X:{ rdPoint.X } Y:{ rdPoint.Y }");

         Console.WriteLine("Reprojecting to RDS (full)");
      }
   }
}
