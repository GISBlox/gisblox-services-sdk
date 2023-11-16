// ------------------------------------------------------------
// Copyright (c) Bartels Online.  All rights reserved.
// ------------------------------------------------------------

using GISBlox.Services.SDK.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace GISBlox.Services.SDK.PostalCodes
{
   /// <summary>
   /// This class...
   /// </summary>
   public class PostalCodesAPIClient : ApiClient, IPostalCodesAPI
   {
      /// <summary>
      /// Initializes a new instance of the GISBlox.Services.SDK.PostalCodes.PostalCodesAPIClient class.
      /// </summary>
      /// <param name="httpClient">The current instance of the HTTPClient class.</param>
      public PostalCodesAPIClient(HttpClient httpClient) : base(httpClient)
      { }

      /// <summary>
      /// Gets the postal code record for the specified postal code.
      /// </summary>
      /// <param name="id">A 4-digit Dutch postal code.</param>
      /// <param name="epsg">The EPSG code of the target coordinate system. Currently supports EPSG codes 4326 and 28992 only.</param>
      /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
      /// <returns>A <see cref="PostalCode4Record"/> type.</returns>
      public async Task<PostalCode4Record> GetPostalCode4Record(string id, int epsg = 28992, CancellationToken cancellationToken = default)
      {
         SetEpsgHeader(epsg);
         var requestUri = $"postalcodes4/{id}";
         return await HttpGet<PostalCode4Record>(this.HttpClient, requestUri, cancellationToken);
      }


      internal void SetEpsgHeader(int epsg)
      {
         SetRequestHeaderValue(HttpClient, "X-EPSG", epsg.ToString());
      }
   }
}
