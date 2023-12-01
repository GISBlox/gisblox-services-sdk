// ------------------------------------------------------------
// Copyright (c) Bartels Online.  All rights reserved.
// ------------------------------------------------------------

using GISBlox.Services.SDK.Models;
using Microsoft.Extensions.Caching.Memory;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace GISBlox.Services.SDK.Conversion
{
   /// <summary>
   /// This class converts WKT geometry objects into GeoJSON. 
   /// </summary>
   public class ConversionAPIClient : ApiClient, IConversionAPI
   {
      /// <summary>
      /// Initializes a new instance of the GISBlox.Services.SDK.Conversion.ConversionAPIClient class.
      /// </summary>
      /// <param name="httpClient">The current instance of the HTTPClient class.</param>
      /// <param name="cache">The current instance of the MemoryCache class.</param>
      public ConversionAPIClient(HttpClient httpClient, IMemoryCache cache) : base(httpClient, cache)
      { }

      /// <summary>
      /// Converts a WKT geometry string into a GeoJson Feature(Collection) string.
      /// </summary>
      /// <param name="wkt">A WKT type.</param>
      /// <param name="asFeatureCollection">Indicates whether to include the GeoJson feature in a feature collection.</param>
      /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
      /// <returns>A GeoJson string with the converted WKT geometry.</returns>
      public async Task<string> ToGeoJson(WKT wkt, bool asFeatureCollection = false, CancellationToken cancellationToken = default)
      {
         var requestUri = "convert/toGeoJson";
         SetRequestHeaderValue(HttpClient, "X-AsFeatureCollection", asFeatureCollection ? "1" : "0");
         return await HttpPost<dynamic, string>(HttpClient, requestUri, wkt, cancellationToken);
      }     
   }
}
