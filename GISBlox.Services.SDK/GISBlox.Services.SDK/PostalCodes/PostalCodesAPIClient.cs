// ------------------------------------------------------------
// Copyright (c) Bartels Online.  All rights reserved.
// ------------------------------------------------------------

using GISBlox.Services.SDK.Models;
using GISBlox.Services.SDK.Models.PostalCodes;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace GISBlox.Services.SDK.PostalCodes
{
   /// <summary>
   /// This class contains methods to communicate with the Dutch Postal Codes API.
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
      /// <param name="id">A Dutch postal code.</param>
      /// <param name="epsg">The target coordinate system.</param>
      /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
      /// <returns><see cref="IPostalCodeRecord"/></returns>
      public async Task<IPostalCodeRecord> GetPostalCodeRecord<IPostalCodeRecord>(string id, CoordinateSystem epsg = CoordinateSystem.RDNew, CancellationToken cancellationToken = default)
      {
         SetEpsgHeader((int)epsg);
         string requestUri = $"{BuildBaseUri<IPostalCodeRecord>()}/{id}";
         return await HttpGet<IPostalCodeRecord>(HttpClient, requestUri, cancellationToken);
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
         SetEpsgHeader((int)epsg);
         string requestUri = $"{BuildBaseUri<IPostalCodeRecord>()}/neighbours/{id}?includeSourcePostalCode={includeSourcePostalCode}";
         return await HttpGet<IPostalCodeRecord>(this.HttpClient, requestUri, cancellationToken);
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
         SetEpsgHeader((int)targetEpsg);
         string requestUri = $"{BuildBaseUri<IPostalCodeRecord>()}/geometry?wktEPSG={(int)wktEpsg}" + (buffer > 0 ? "&buffer=" + buffer : "");
         return await HttpPost<dynamic, IPostalCodeRecord>(this.HttpClient, requestUri, wkt, cancellationToken);
      }

      /// <summary>
      /// Gets postal code records based on one or more area IDs.
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
         SetEpsgHeader((int)epsg);         
         if (typeof(IPostalCodeRecord) == typeof(PostalCode4Record))
         {
            requestUri = $"postalcodes4/gw?gemeenteId={gemeenteId}&wijkId={wijkId}";
         }
         else
         {
            requestUri = $"postalcodes6/gwb?gemeenteId={gemeenteId}&wijkId={wijkId}&buurtId={buurtId}";
         }
         return await HttpGet<IPostalCodeRecord>(this.HttpClient, requestUri, cancellationToken);
      }          

      /// <summary>
      /// Gets the key figures record for the specified postal code area.
      /// </summary>
      /// <param name="id">A 4-digit Dutch postal code.</param>
      /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
      /// <returns>A <see cref="KerncijferRecord"/> type.</returns>
      public async Task<KerncijferRecord> GetKeyFigures4Record(string id, CancellationToken cancellationToken = default)
      {
         string requestUri = $"postalcodes4/keyfigures/{id}";
         return await HttpGet<KerncijferRecord>(this.HttpClient, requestUri, cancellationToken);
      }      
     
      /// <summary>
      /// Gets the key figures record for the specified postal code area.
      /// </summary>
      /// <param name="id">A 6-digit Dutch postal code.</param>
      /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
      /// <returns>A <see cref="KerncijferRecord"/> type.</returns>
      public async Task<KerncijferRecord> GetKeyFigures6Record(string id, CancellationToken cancellationToken = default)
      {
         string requestUri = $"postalcodes6/keyfigures/{id}";
         return await HttpGet<KerncijferRecord>(this.HttpClient, requestUri, cancellationToken);
      }      

      #region GWB

      /// <summary>
      /// Query for postal code's gemeenten. 
      /// </summary>
      /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
      /// <returns>A <see cref="GWBRecord"/> type.</returns>
      public async Task<GWBRecord> GetGemeenten(CancellationToken cancellationToken = default)
      {
         string requestUri = $"postalcodes/gwb/gemeenten";
         return await HttpGet<GWBRecord>(this.HttpClient, requestUri, cancellationToken);
      }

      /// <summary>
      /// Query for postal code's 'wijken' by 'gemeenten'. 
      /// </summary>
      /// <param name="gemeenteId">A gemeente ID.</param>
      /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
      /// <returns>A <see cref="GWBRecord"/> type.</returns>
      public async Task<GWBRecord> GetWijken(int gemeenteId, CancellationToken cancellationToken = default)
      {
         string requestUri = $"postalcodes/gwb/gemeente/{gemeenteId}/wijken";
         return await HttpGet<GWBRecord>(this.HttpClient, requestUri, cancellationToken);
      }

      /// <summary>
      /// Query for postal code's 'buurten' by 'wijken'. 
      /// </summary>
      /// <param name="wijkId">A wijk ID.</param>
      /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
      /// <returns>A <see cref="GWBRecord"/> type.</returns>
      public async Task<GWBRecord> GetBuurten(int wijkId, CancellationToken cancellationToken = default)
      {
         string requestUri = $"postalcodes/gwb/wijk/{wijkId}/buurten";
         return await HttpGet<GWBRecord>(this.HttpClient, requestUri, cancellationToken);
      }

      #endregion

      internal void SetEpsgHeader(int epsg)
      {
         SetRequestHeaderValue(HttpClient, "X-EPSG", epsg.ToString());
      }

      internal static string BuildBaseUri<IPostalCodeRecord>()
      {         
         return $"postalcodes{((typeof(IPostalCodeRecord) == typeof(PostalCode4Record)) ? "4" : "6")}";         
      }     
   }
}
