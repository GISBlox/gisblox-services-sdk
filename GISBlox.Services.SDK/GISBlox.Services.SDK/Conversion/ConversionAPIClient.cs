// ------------------------------------------------------------
// Copyright (c) Bartels Online.  All rights reserved.
// ------------------------------------------------------------

using GISBlox.Services.SDK.Common;
using GISBlox.Services.SDK.Models;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace GISBlox.Services.SDK.Conversion
{
   /// <summary>
   /// This class converts WKT geometry objects into GeoJSON. 
   /// </summary>
   /// <remarks>
   /// Initializes a new instance of the GISBlox.Services.SDK.Conversion.ConversionAPIClient class.
   /// </remarks>
   /// <param name="httpClient">The current instance of the HTTPClient class.</param>
   /// <param name="cache">The current instance of the MemoryCache class.</param>
   public class ConversionAPIClient(HttpClient httpClient, IMemoryCache cache) : ApiClient(httpClient, cache), IConversionAPI
   {
      /// <summary>
      /// Converts a WKT geometry string into a GeoJson Feature(Collection) string.
      /// </summary>
      /// <param name="wkt">A WKT type.</param>
      /// <param name="asFeatureCollection">Indicates whether to include the GeoJson feature in a feature collection.</param>
      /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
      /// <returns>A GeoJson string with the converted WKT geometry.</returns>
      public async Task<string> ToGeoJson(WKT wkt, bool asFeatureCollection = false, CancellationToken cancellationToken = default)
      {
         var requestUri = "convert/toGeoJson?format=WKT";

         Dictionary<string, string> customHeaders = new()
            {
                { "X-AsFeatureCollection", asFeatureCollection ? "1" : "0" }
            };

         return await HttpPost<dynamic, string>(HttpClient, requestUri, wkt, null, customHeaders, cancellationToken);
      }

      /// <summary>
      /// Converts a WKB geometry into a GeoJson Feature(Collection) string.
      /// </summary>
      /// <param name="wkb">A WKB type.</param>
      /// <param name="asFeatureCollection">Indicates whether to include the GeoJson feature in a feature collection.</param>
      /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
      /// <returns>A GeoJson string with the converted WKB geometry.</returns>
      public async Task<string> ToGeoJson(WKB wkb, bool asFeatureCollection = false, CancellationToken cancellationToken = default)
      {
         var requestUri = "convert/toGeoJson?format=WKB";

         Dictionary<string, string> customHeaders = new()
            {
                { "X-AsFeatureCollection", asFeatureCollection ? "1" : "0" }
            };

         return await HttpPost<dynamic, string>(HttpClient, requestUri, wkb, null, customHeaders, cancellationToken);
      }

      /// <summary>
      /// Converts a GeoJson string into a list of WKB geometries.
      /// </summary>
      /// <param name="geoJson">A GeoJson string.</param>
      /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
      /// <returns>A list of WKB geometries.</returns>
      public async Task<List<WKB>> ToWkb(string geoJson, CancellationToken cancellationToken = default)
      {
         var requestUri = "convert/toWkb";
         
         var result = await HttpPost<string, List<WKB>>(HttpClient, requestUri, geoJson, null, null, cancellationToken);
         PropertyValueNormalizer.NormalizeWkbList(result);
         return result;
      }

      /// <summary>
      /// Converts a GeoJson stream into a list of WKB geometries.
      /// </summary>
      /// <param name="stream">A stream containing the GeoJson data.</param>
      /// <param name="fileName">The name of the file.</param>
      /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
      /// <returns>A list of WKB geometries.</returns>
      public async Task<List<WKB>> ToWkb(Stream stream, string fileName, CancellationToken cancellationToken = default)
      {
         var requestUri = "convert/toWkb/" + fileName;

         var result = await HttpPost<List<WKB>>(HttpClient, requestUri, stream, "application/json", null, cancellationToken);
         PropertyValueNormalizer.NormalizeWkbList(result);
         return result;
      }

      /// <summary>
      /// Converts a GeoJson string into a list of WKT geometries.
      /// </summary>
      /// <param name="geoJson">A GeoJson string.</param>
      /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
      /// <returns>A list of WKT geometries.</returns>
      public async Task<List<WKT>> ToWkt(string geoJson, CancellationToken cancellationToken = default)
      {
         var requestUri = "convert/toWkt";
         
         var result = await HttpPost<string, List<WKT>>(HttpClient, requestUri, geoJson, null, null, cancellationToken);
         PropertyValueNormalizer.NormalizeWktList(result);
         return result;
      }

      /// <summary>
      /// Converts a GeoJson string into a list of WKT geometries.
      /// </summary>
      /// <param name="stream">A stream containing the GeoJson data.</param>
      /// <param name="fileName">The name of the file.</param>
      /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
      /// <returns>A list of WKT geometries.</returns>
      public async Task<List<WKT>> ToWkt(Stream stream, string fileName, CancellationToken cancellationToken = default)
      {
         var requestUri = "convert/toWkt/" + fileName;

         var result = await HttpPost<List<WKT>>(HttpClient, requestUri, stream, "application/json", null, cancellationToken);         
         PropertyValueNormalizer.NormalizeWktList(result);
         return result;
      }     
   }
}
