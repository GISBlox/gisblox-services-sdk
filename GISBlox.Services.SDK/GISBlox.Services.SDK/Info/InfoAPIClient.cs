﻿// ------------------------------------------------------------
// Copyright (c) Bartels Online.  All rights reserved.
// ------------------------------------------------------------

using GISBlox.Services.SDK.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace GISBlox.Services.SDK.Info
{
   /// <summary>
   /// This class contains methods to communicate with the Info API.
   /// </summary>
   public class InfoAPIClient : ApiClient, IInfoAPI
   {
      /// <summary>
      /// Initializes a new instance of the GISBlox.Services.SDK.Info.InfoAPIClient class.
      /// </summary>
      /// <param name="httpClient">The current instance of the HTTPClient class.</param>
      public InfoAPIClient(HttpClient httpClient) : base(httpClient)
      { }

      /// <summary>
      /// Returns the subscriptions of the authorized user.
      /// </summary>
      /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
      /// <returns>A List with <see cref="Subscription"/> types.</returns>
      public async Task<List<Subscription>> GetSubscriptions(CancellationToken cancellationToken = default)
      {
         var requestUri = "info/subscriptions";         
         return await HttpGet<List<Subscription>>(this.HttpClient, requestUri, cancellationToken);
      }
   }
}
