// ------------------------------------------------------------
// Copyright (c) Bartels Online.  All rights reserved.
// ------------------------------------------------------------

using GISBlox.Services.SDK.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace GISBlox.Services.SDK.Projection
{
   public class ProjectionAPIClient : ApiClient, IProjectionAPI
   {
      public ProjectionAPIClient(HttpClient httpClient) : base(httpClient)
      {
      }

      public async Task<RDPoint> ToRDS(Coordinate coordinate, CancellationToken cancellationToken = default)
      {
         var requestUri = "reproject/toRDS";
         SetRequestHeaderValue(this.HttpClient, "Complete", "0");
         return await HttpPost<dynamic, RDPoint>(this.HttpClient, requestUri, coordinate, cancellationToken);
      }

      public async Task<List<RDPoint>> ToRDS(List<Coordinate> coordinates, CancellationToken cancellationToken = default)
      {
         var requestUri = "reproject/toRDS/batch";
         SetRequestHeaderValue(this.HttpClient, "Complete", "0");
         return await HttpPost<dynamic, List<RDPoint>>(this.HttpClient, requestUri, coordinates, cancellationToken);
      }

      public async Task<Location> ToRDSComplete(Coordinate coordinate, CancellationToken cancellationToken = default)
      {
         var requestUri = "reproject/toRDS";
         SetRequestHeaderValue(this.HttpClient, "Complete", "1");
         return await HttpPost<dynamic, Location>(this.HttpClient, requestUri, coordinate, cancellationToken);
      }

      public async Task<List<Location>> ToRDSComplete(List<Coordinate> coordinates, CancellationToken cancellationToken = default)
      {
         var requestUri = "reproject/toRDS/batch";
         SetRequestHeaderValue(this.HttpClient, "Complete", "1");
         return await HttpPost<dynamic, List<Location>>(this.HttpClient, requestUri, coordinates, cancellationToken);
      }

      public async Task<Coordinate> ToWGS84(RDPoint rdPoint, CancellationToken cancellationToken = default)
      {
         var requestUri = "reproject/toWGS84";
         SetRequestHeaderValue(this.HttpClient, "Complete", "0");
         return await HttpPost<dynamic, Coordinate>(this.HttpClient, requestUri, rdPoint, cancellationToken);
      }

      public async Task<List<Coordinate>> ToWGS84(List<RDPoint> rdPoints, CancellationToken cancellationToken = default)
      {
         var requestUri = "reproject/toWGS84/batch";
         SetRequestHeaderValue(this.HttpClient, "Complete", "0");
         return await HttpPost<dynamic, List<Coordinate>>(this.HttpClient, requestUri, rdPoints, cancellationToken);
      }

      public async Task<Location> ToWGS84Complete(RDPoint rdPoint, CancellationToken cancellationToken = default)
      {
         var requestUri = "reproject/toWGS84";
         SetRequestHeaderValue(this.HttpClient, "Complete", "1");
         return await HttpPost<dynamic, Location>(this.HttpClient, requestUri, rdPoint, cancellationToken);
      }

      public async Task<List<Location>> ToWGS84Complete(List<RDPoint> rdPoints, CancellationToken cancellationToken = default)
      {
         var requestUri = "reproject/toWGS84/batch";
         SetRequestHeaderValue(this.HttpClient, "Complete", "1");
         return await HttpPost<dynamic, List<Location>>(this.HttpClient, requestUri, rdPoints, cancellationToken);
      }     
   }
}
