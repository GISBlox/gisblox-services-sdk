// ------------------------------------------------------------
// Copyright (c) Bartels Online.  All rights reserved.
// ------------------------------------------------------------

using GISBlox.Services.SDK.Models;
using Microsoft.Extensions.Caching.Memory;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace GISBlox.Services.SDK.PostalCodes
{
   /// <summary>
   /// This class contains methods to communicate with the Dutch Postal Codes API.
   /// </summary>
   /// <remarks>
   /// Initializes a new instance of the GISBlox.Services.SDK.PostalCodes.PostalCodesAPIClient class.
   /// </remarks>
   /// <param name="httpClient">The current instance of the HTTPClient class.</param>
   /// <param name="cache">The current instance of the MemoryCache class.</param>
   public class PostalCodesAPIClient(HttpClient httpClient, IMemoryCache cache) : ApiClient(httpClient, cache), IPostalCodesAPI
   {
      readonly AreaCodeHelper _areaHelper = new(httpClient, cache);

      /// <summary>
      /// Contains methods to retrieve area records for Gemeente, Wijk and/or Buurt queries. 
      /// </summary>
      public AreaCodeHelper AreaHelper { get { return _areaHelper; } }

      /// <summary>
      /// Gets the postal code record for the specified postal code.
      /// </summary>
      /// <param name="id">A Dutch postal code.</param>
      /// <param name="epsg">The target coordinate system.</param>
      /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
      /// <returns><see cref="IPostalCodeRecord"/></returns>
      public async Task<IPostalCodeRecord> GetPostalCodeRecord<IPostalCodeRecord>(string id, CoordinateSystem epsg = CoordinateSystem.RDNew, CancellationToken cancellationToken = default)
      {
         string requestUri = $"{BuildBaseUri<IPostalCodeRecord>()}/{id}";
         string epsgValue = ((int)epsg).ToString();

         return await HttpGet<IPostalCodeRecord>(HttpClient, Cache, requestUri, epsgValue, null, cancellationToken);
      }

      /// <summary>
      /// Gets neighbouring postal code records of the specified postal code.
      /// </summary>
      /// <param name="id">A Dutch postal code.</param>
      /// <param name="includeSourcePostalCode">Determines whether to include the source postal code in the result.</param>
      /// <param name="epsg">The target coordinate system.</param>
      /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
      /// <returns><see cref="IPostalCodeRecord"/></returns>
      public async Task<IPostalCodeRecord> GetPostalCodeNeighbours<IPostalCodeRecord>(string id, bool includeSourcePostalCode = false, CoordinateSystem epsg = CoordinateSystem.RDNew, CancellationToken cancellationToken = default)
      {
         string requestUri = $"{BuildBaseUri<IPostalCodeRecord>()}/neighbours/{id}?includeSourcePostalCode={includeSourcePostalCode}";
         string epsgValue = ((int)epsg).ToString();

         return await HttpGet<IPostalCodeRecord>(HttpClient, Cache, requestUri, epsgValue, null, cancellationToken);
      }

      /// <summary>
      /// Gets postal code records based on a WKT (well-known text) geometry string.
      /// </summary>
      /// <param name="wkt">The geometry of the location expressed in WKT (well-known text) format.</param>
      /// <param name="buffer">Buffer distance expressed in the unit of the WKT coordinate system.</param>
      /// <param name="wktEpsg">The coordinate system of the specified geometry.</param>
      /// <param name="targetEpsg">The target coordinate system.</param>
      /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
      /// <returns><see cref="IPostalCodeRecord"/></returns>
      public async Task<IPostalCodeRecord> GetPostalCodeByGeometry<IPostalCodeRecord>(string wkt, int buffer = 0, CoordinateSystem wktEpsg = CoordinateSystem.RDNew, CoordinateSystem targetEpsg = CoordinateSystem.RDNew, CancellationToken cancellationToken = default)
      {
         string requestUri = $"{BuildBaseUri<IPostalCodeRecord>()}/geometry?wktEPSG={(int)wktEpsg}" + (buffer > 0 ? "&buffer=" + buffer : "");
         string epsgValue = ((int)targetEpsg).ToString();

         return await HttpPost<dynamic, IPostalCodeRecord>(HttpClient, requestUri, wkt, epsgValue, null, cancellationToken);
      }

      /// <summary>
      /// Gets postal code records based on one or more area IDs. Use the methods in the <see cref="AreaHelper"/> class to retrieve the IDs.
      /// </summary>
      /// <param name="gemeenteId">A gemeente code.</param>
      /// <param name="wijkId">A district ('wijk') code.</param>   
      /// <param name="buurtId">A neighbourhood ('buurt') code. Considered only when a <see cref="PostalCode6Record"/> is requested.</param>
      /// <param name="epsg">The target coordinate system.</param>
      /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
      /// <returns><see cref="IPostalCodeRecord"/></returns>
      public async Task<IPostalCodeRecord> GetPostalCodeByArea<IPostalCodeRecord>(int gemeenteId, int wijkId, int buurtId = -1, CoordinateSystem epsg = CoordinateSystem.RDNew, CancellationToken cancellationToken = default)
      {
         string requestUri;
         string epsgValue = ((int)epsg).ToString();

         if (typeof(IPostalCodeRecord) == typeof(PostalCode4Record))
         {
            requestUri = $"postalcodes4/gw?gemeenteId={gemeenteId}&wijkId={wijkId}";
         }
         else
         {
            requestUri = $"postalcodes6/gwb?gemeenteId={gemeenteId}&wijkId={wijkId}&buurtId={buurtId}";
         }
         return await HttpGet<IPostalCodeRecord>(HttpClient, Cache, requestUri, epsgValue, null, cancellationToken);
      }

      /// <summary>
      /// Gets the key figures record for the specified postal code area.
      /// </summary>
      /// <param name="id">A Dutch postal code.</param>
      /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
      /// <returns><see cref="KerncijferRecord"/></returns>
      public async Task<KerncijferRecord> GetKeyFigures(string id, CancellationToken cancellationToken = default)
      {
         string requestUri = $"postalcodes{(id.Length == 4 ? "4" : "6")}/keyfigures/{id}";
         return await HttpGet<KerncijferRecord>(HttpClient, Cache, requestUri, null, null, cancellationToken);
      }      

      /// <summary>
      /// Returns the postal code service URI based on the passed type.
      /// </summary>
      /// <typeparam name="IPostalCodeRecord">A class that inherits from <see cref="IPostalCodeRecord"/>.</typeparam>
      /// <returns>The proper postal code service URI.</returns>
      internal static string BuildBaseUri<IPostalCodeRecord>()
      {         
         return $"postalcodes{((typeof(IPostalCodeRecord) == typeof(PostalCode4Record)) ? "4" : "6")}";         
      }     
   }
}
