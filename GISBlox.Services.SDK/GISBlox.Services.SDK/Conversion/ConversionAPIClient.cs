using GISBlox.Services.SDK.Models;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace GISBlox.Services.SDK.Conversion
{
   public class ConversionAPIClient : ApiClient, IConversionAPI
   {
      public ConversionAPIClient(HttpClient httpClient) : base(httpClient)
      {
      }

      public async Task<string> ToGeoJson(WKT wkt, bool asFeatureCollection = false, CancellationToken cancellationToken = default)
      {
         var requestUri = "convert/toGeoJson";
         SetRequestHeaderValue("AsFeatureCollection", asFeatureCollection ? "1" : "0");
         return await HttpPost<dynamic, string>(this.HttpClient, requestUri, wkt, cancellationToken);
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
