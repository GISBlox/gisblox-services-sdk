// ------------------------------------------------------------
// Copyright (c) Bartels Online.  All rights reserved.
// ------------------------------------------------------------

using GISBlox.Services.SDK.Models;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace GISBlox.Services.SDK.Projection
{
   /// <summary>
   /// This class reprojects coordinates to Rijksdriehoeksstelsel locations and vice versa.
   /// </summary>
   public class ProjectionAPIClient : ApiClient, IProjectionAPI
   {
      /// <summary>
      /// Initializes a new instance of the GISBlox.Services.SDK.Projection.ProjectionAPIClient class.
      /// </summary>
      /// <param name="httpClient">The current instance of the HTTPClient class.</param>
      /// <param name="cache">The current instance of the MemoryCache class.</param>
      public ProjectionAPIClient(HttpClient httpClient, IMemoryCache cache) : base(httpClient, cache)
      { }

      /// <summary>
      /// Reprojects a coordinate to an RDPoint.
      /// </summary>
      /// <param name="coordinate">A Coordinate type.</param>
      /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
      /// <returns>An <see cref="RDPoint"/> type.</returns>
      public async Task<RDPoint> ToRDS(Coordinate coordinate, CancellationToken cancellationToken = default)
      {
         SetCompleteHeader(false);
         var requestUri = "reproject/toRDS";         
         return await HttpPost<dynamic, RDPoint>(HttpClient, requestUri, coordinate, cancellationToken);
      }

      /// <summary>
      /// Reprojects multiple coordinates to RDPoints.
      /// </summary>
      /// <param name="coordinates">A List with Coordinate types.</param>
      /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
      /// <returns>A List with <see cref="RDPoint"/> types.</returns>
      public async Task<List<RDPoint>> ToRDS(List<Coordinate> coordinates, CancellationToken cancellationToken = default)
      {
         SetCompleteHeader(false);
         var requestUri = "reproject/toRDS/batch";         
         return await HttpPost<dynamic, List<RDPoint>>(HttpClient, requestUri, coordinates, cancellationToken);
      }

      /// <summary>
      /// Reprojects a coordinate to a location. Includes the source coordinate.
      /// </summary>
      /// <param name="coordinate">A Coordinate type.</param>
      /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
      /// <returns>A <see cref="Location"/> type.</returns>
      public async Task<Location> ToRDSComplete(Coordinate coordinate, CancellationToken cancellationToken = default)
      {         
         SetCompleteHeader(true);
         var requestUri = "reproject/toRDS";
         return await HttpPost<dynamic, Location>(HttpClient, requestUri, coordinate, cancellationToken);
      }

      /// <summary>
      /// Reprojects multiple coordinates to locations. Includes the source coordinates.
      /// </summary>
      /// <param name="coordinates">A List with Coordinate types.</param>
      /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
      /// <returns>A List with <see cref="Location"/> types.</returns>
      public async Task<List<Location>> ToRDSComplete(List<Coordinate> coordinates, CancellationToken cancellationToken = default)
      {         
         SetCompleteHeader(true);
         var requestUri = "reproject/toRDS/batch";
         return await HttpPost<dynamic, List<Location>>(HttpClient, requestUri, coordinates, cancellationToken);
      }

      /// <summary>
      /// Reprojects an RDPoint to a coordinate.
      /// </summary>
      /// <param name="rdPoint">An RDPoint type.</param>
      /// <param name="decimals">Rounds the coordinate to the specified amount of fractional digits.</param>
      /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
      /// <returns>A <see cref="Coordinate"/> type.</returns>
      public async Task<Coordinate> ToWGS84(RDPoint rdPoint, int decimals = -1, CancellationToken cancellationToken = default)
      {         
         SetCompleteHeader(false);
         SetDecimalsHeader(decimals);
         var requestUri = "reproject/toWGS84";
         return await HttpPost<dynamic, Coordinate>(HttpClient, requestUri, rdPoint, cancellationToken);
      }

      /// <summary>
      /// Reprojects multiple RDPoints to coordinates.
      /// </summary>
      /// <param name="rdPoints">A List with RDPoint types.</param>
      /// <param name="decimals">Rounds the coordinate to the specified amount of fractional digits.</param>
      /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
      /// <returns>A List with <see cref="Coordinate"/> types.</returns>
      public async Task<List<Coordinate>> ToWGS84(List<RDPoint> rdPoints, int decimals = -1, CancellationToken cancellationToken = default)
      {
         SetCompleteHeader(false);
         SetDecimalsHeader(decimals);
         var requestUri = "reproject/toWGS84/batch";         
         return await HttpPost<dynamic, List<Coordinate>>(HttpClient, requestUri, rdPoints, cancellationToken);
      }

      /// <summary>
      /// Reprojects an RDPoint to a location. Includes the source RDPoint.
      /// </summary>
      /// <param name="rdPoint">An RDPoint type.</param>
      /// <param name="decimals">Rounds the coordinate to the specified amount of fractional digits.</param>
      /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
      /// <returns>A <see cref="Location"/> type.</returns>
      public async Task<Location> ToWGS84Complete(RDPoint rdPoint, int decimals = -1, CancellationToken cancellationToken = default)
      {       
         SetCompleteHeader(true);
         SetDecimalsHeader(decimals);
         var requestUri = "reproject/toWGS84";
         return await HttpPost<dynamic, Location>(HttpClient, requestUri, rdPoint, cancellationToken);
      }

      /// <summary>
      /// Reprojects multiple RDPoints to locations. Includes the source RDPoints.
      /// </summary>
      /// <param name="rdPoints">A List with RDPoint types.</param>
      /// <param name="decimals">Rounds the coordinate to the specified amount of fractional digits.</param>
      /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
      /// <returns>A List with <see cref="Location"/> types.</returns>
      public async Task<List<Location>> ToWGS84Complete(List<RDPoint> rdPoints, int decimals = -1, CancellationToken cancellationToken = default)
      {         
         SetCompleteHeader(true);         
         SetDecimalsHeader(decimals);
         var requestUri = "reproject/toWGS84/batch";
         return await HttpPost<dynamic, List<Location>>(HttpClient, requestUri, rdPoints, cancellationToken);
      }   
      
      internal void SetCompleteHeader(bool complete)
      {
         SetRequestHeaderValue(HttpClient, "X-Complete", complete ? "1" : "0");
      }

      internal void SetDecimalsHeader(int decimals)
      {
         SetRequestHeaderValue(HttpClient, "X-Rounding-Decimals", decimals == -1 ? "-1" : decimals.ToString());
      }
   }
}
