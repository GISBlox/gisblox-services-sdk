using GISBlox.Services.SDK.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
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
         SetRequestHeaderValue("Complete", "0");
         return await HttpPost<dynamic, RDPoint>(this.HttpClient, requestUri, coordinate, cancellationToken);
      }

      public async Task<List<RDPoint>> ToRDS(List<Coordinate> coordinates, CancellationToken cancellationToken = default)
      {
         var requestUri = "reproject/toRDS/batch";
         SetRequestHeaderValue("Complete", "0");
         return await HttpPost<dynamic, List<RDPoint>>(this.HttpClient, requestUri, coordinates, cancellationToken);
      }

      public async Task<Location> ToRDSComplete(Coordinate coordinate, CancellationToken cancellationToken = default)
      {
         var requestUri = "reproject/toRDS";
         SetRequestHeaderValue("Complete", "1");
         return await HttpPost<dynamic, Location>(this.HttpClient, requestUri, coordinate, cancellationToken);
      }

      public async Task<List<Location>> ToRDSComplete(List<Coordinate> coordinates, CancellationToken cancellationToken = default)
      {
         var requestUri = "reproject/toRDS/batch";
         SetRequestHeaderValue("Complete", "1");
         return await HttpPost<dynamic, List<Location>>(this.HttpClient, requestUri, coordinates, cancellationToken);
      }

      internal void SetRequestHeaderValue(string headerName, string headerValue)
      {
         if (HttpClient.DefaultRequestHeaders.Contains(headerName))
         {
            HttpClient.DefaultRequestHeaders.Remove(headerName);
         }
         HttpClient.DefaultRequestHeaders.Add(headerName, headerValue);
      }
   }
}
