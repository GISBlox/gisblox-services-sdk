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
         return await HttpGet<RDPoint>(this.HttpClient, requestUri, cancellationToken).ConfigureAwait(false);
      }

      public async Task<Location> ToRDSComplete(Coordinate coordinate, CancellationToken cancellationToken = default)
      {
         var requestUri = "reproject/toRDS";
         HttpClient.DefaultRequestHeaders.Add("Complete", "1");
         return await HttpGet<Location>(this.HttpClient, requestUri, cancellationToken).ConfigureAwait(false);
      }
   }
}
