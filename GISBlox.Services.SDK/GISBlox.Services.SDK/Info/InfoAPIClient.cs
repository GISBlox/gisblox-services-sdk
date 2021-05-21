// ------------------------------------------------------------
// Copyright (c) Bartels Online.  All rights reserved.
// ------------------------------------------------------------

using GISBlox.Services.SDK.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace GISBlox.Services.SDK.Info
{
   public class InfoAPIClient : ApiClient, IInfoAPI
   {
      public InfoAPIClient(HttpClient httpClient) : base(httpClient)
      {
      }

      public async Task<List<Subscription>> GetSubscriptions(CancellationToken cancellationToken = default)
      {
         var requestUri = "info/subscriptions";         
         return await HttpGet<List<Subscription>>(this.HttpClient, requestUri, cancellationToken);
      }
   }
}
