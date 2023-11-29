// ------------------------------------------------------------
// Copyright (c) Bartels Online.  All rights reserved.
// ------------------------------------------------------------

using GISBlox.Services.SDK.Models;
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

      public async Task<T> GetPostalCodeRecordAsync<T>(string id, CoordinateSystem epsg = CoordinateSystem.RDNew, CancellationToken cancellationToken = default) 
      {
         SetEpsgHeader((int)epsg);
         
         bool streetLevel = id.Length == 6;
         string requestUri = $"postalcodes{(streetLevel ? "6" : "4")}/{id}";         
         
         
         return await HttpGet<T>(HttpClient, requestUri, cancellationToken);
         

         


         //var requestUri = $"postalcodes4/{id}";
         //return await HttpGet<PostalCode4Record>(this.HttpClient, requestUri, cancellationToken);
      }


      #region PC4

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

      /// <summary>
      /// Gets neighbouring postal code records of the specified postal code.
      /// </summary>
      /// <param name="id">A 4-digit Dutch postal code.</param>
      /// <param name="includeSourcePostalCode">Determines whether to include the source postal code in the result.</param>
      /// <param name="epsg">The EPSG code of the target coordinate system. Currently supports EPSG codes 4326 and 28992 only.</param>
      /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
      /// <returns>A <see cref="PostalCode4Record"/> type.</returns>
      public async Task<PostalCode4Record> GetPostalCode4Neighbours(string id, bool includeSourcePostalCode = false, int epsg = 28992, CancellationToken cancellationToken = default)
      {
         SetEpsgHeader(epsg);
         var requestUri = $"postalcodes4/neighbours/{id}?includeSourcePostalCode={includeSourcePostalCode}";
         return await HttpGet<PostalCode4Record>(this.HttpClient, requestUri, cancellationToken);
      }

      /// <summary>
      /// Gets postal code records based on a WKT (well-known text) geometry string.
      /// </summary>
      /// <param name="wkt">The geometry of the location expressed in WKT (well-known text) format.</param>
      /// <param name="buffer">Buffer distance expressed in the unit of the WKT coordinate system.</param>
      /// <param name="wktEpsg">The EPSG code of the coordinate system of the specified geometry. Currently supports EPSG codes 4326 and 28992 only.</param>
      /// <param name="targetEpsg">The EPSG code of the target coordinate system. Currently supports EPSG codes 4326 and 28992 only.</param>
      /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
      /// <returns>A <see cref="PostalCode4Record"/> type.</returns>
      public async Task<PostalCode4Record> GetPostalCode4ByGeometry(string wkt, int buffer = 0, int wktEpsg = 28992, int targetEpsg = 28992, CancellationToken cancellationToken = default)
      {
         SetEpsgHeader(targetEpsg);
         var requestUri = $"postalcodes4/geometry?wktEPSG={wktEpsg}" + (buffer > 0 ? "&buffer=" + buffer : "");
         return await HttpPost<dynamic, PostalCode4Record>(this.HttpClient, requestUri, wkt, cancellationToken);
      }

      /// <summary>
      /// Gets postal code records based on one or more district IDs.
      /// </summary>
      /// <param name="gemeenteId">A gemeente code.</param>
      /// <param name="wijkId">A district ('wijk') code.</param>      
      /// <param name="epsg">The EPSG code of the target coordinate system. Currently supports EPSG codes 4326 and 28992 only.</param>
      /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
      /// <returns>A <see cref="PostalCode4Record"/> type.</returns>
      public async Task<PostalCode4Record> GetPostalCode4ByGW(int gemeenteId, int wijkId, int epsg = 28992, CancellationToken cancellationToken = default)
      {
         SetEpsgHeader(epsg);
         var requestUri = $"postalcodes4/gw?gemeenteId={gemeenteId}&wijkId={wijkId}";
         return await HttpGet<PostalCode4Record>(this.HttpClient, requestUri, cancellationToken);
      }

      /// <summary>
      /// Gets the key figures record for the specified postal code area.
      /// </summary>
      /// <param name="id">A 4-digit Dutch postal code.</param>
      /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
      /// <returns>A <see cref="KerncijferRecord"/> type.</returns>
      public async Task<KerncijferRecord> GetKeyFigures4Record(string id, CancellationToken cancellationToken = default)
      {
         var requestUri = $"postalcodes4/keyfigures/{id}";
         return await HttpGet<KerncijferRecord>(this.HttpClient, requestUri, cancellationToken);
      }

      #endregion

      #region PC6

      /// <summary>
      /// Gets the postal code record for the specified postal code.
      /// </summary>
      /// <param name="id">A 6-digit Dutch postal code.</param>
      /// <param name="epsg">The EPSG code of the target coordinate system. Currently supports EPSG codes 4326 and 28992 only.</param>
      /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
      /// <returns>A <see cref="PostalCode6Record"/> type.</returns>
      public async Task<PostalCode6Record> GetPostalCode6Record(string id, int epsg = 28992, CancellationToken cancellationToken = default)
      {
         SetEpsgHeader(epsg);
         var requestUri = $"postalcodes6/{id}";
         return await HttpGet<PostalCode6Record>(this.HttpClient, requestUri, cancellationToken);
      }

      /// <summary>
      /// Gets neighbouring postal code records of the specified postal code.
      /// </summary>
      /// <param name="id">A 6-digit Dutch postal code.</param>
      /// <param name="includeSourcePostalCode">Determines whether to include the source postal code in the result.</param>
      /// <param name="epsg">The EPSG code of the target coordinate system. Currently supports EPSG codes 4326 and 28992 only.</param>
      /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
      /// <returns>A <see cref="PostalCode4Record"/> type.</returns>
      public async Task<PostalCode6Record> GetPostalCode6Neighbours(string id, bool includeSourcePostalCode = false, int epsg = 28992, CancellationToken cancellationToken = default)
      {
         SetEpsgHeader(epsg);
         var requestUri = $"postalcodes6/neighbours/{id}?includeSourcePostalCode={includeSourcePostalCode}";
         return await HttpGet<PostalCode6Record>(this.HttpClient, requestUri, cancellationToken);
      }

      /// <summary>
      /// Gets postal code records based on a WKT (well-known text) geometry string.
      /// </summary>
      /// <param name="wkt">The geometry of the location expressed in WKT (well-known text) format.</param>
      /// <param name="buffer">Buffer distance expressed in the unit of the WKT coordinate system.</param>
      /// <param name="wktEpsg">The EPSG code of the coordinate system of the specified geometry. Currently supports EPSG codes 4326 and 28992 only.</param>
      /// <param name="targetEpsg">The EPSG code of the target coordinate system. Currently supports EPSG codes 4326 and 28992 only.</param>
      /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
      /// <returns>A <see cref="PostalCode6Record"/> type.</returns>
      public async Task<PostalCode6Record> GetPostalCode6ByGeometry(string wkt, int buffer = 0, int wktEpsg = 28992, int targetEpsg = 28992, CancellationToken cancellationToken = default)
      {
         SetEpsgHeader(targetEpsg);
         var requestUri = $"postalcodes6/geometry?wktEPSG={wktEpsg}" + (buffer > 0 ? "&buffer=" + buffer : "");
         return await HttpPost<dynamic, PostalCode6Record>(this.HttpClient, requestUri, wkt, cancellationToken);
      }

      /// <summary>
      /// Gets postal code records based on one or more area IDs.
      /// </summary>
      /// <param name="gemeenteId">A gemeente code.</param>
      /// <param name="wijkId">A district ('wijk') code.</param>   
      /// <param name="buurtId">A neighbourhood ('buurt') code.</param>
      /// <param name="epsg">The EPSG code of the target coordinate system. Currently supports EPSG codes 4326 and 28992 only.</param>
      /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
      /// <returns>A <see cref="PostalCode4Record"/> type.</returns>
      public async Task<PostalCode6Record> GetPostalCode6ByGWB(int gemeenteId, int wijkId, int buurtId, int epsg = 28992, CancellationToken cancellationToken = default)
      {
         SetEpsgHeader(epsg);
         var requestUri = $"postalcodes6/gwb?gemeenteId={gemeenteId}&wijkId={wijkId}&buurtId={buurtId}";
         return await HttpGet<PostalCode6Record>(this.HttpClient, requestUri, cancellationToken);
      }

      /// <summary>
      /// Gets the key figures record for the specified postal code area.
      /// </summary>
      /// <param name="id">A 6-digit Dutch postal code.</param>
      /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
      /// <returns>A <see cref="KerncijferRecord"/> type.</returns>
      public async Task<KerncijferRecord> GetKeyFigures6Record(string id, CancellationToken cancellationToken = default)
      {
         var requestUri = $"postalcodes6/keyfigures/{id}";
         return await HttpGet<KerncijferRecord>(this.HttpClient, requestUri, cancellationToken);
      }

      #endregion

      #region GWB

      /// <summary>
      /// Query for postal code's gemeenten. 
      /// </summary>
      /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
      /// <returns>A <see cref="GWBRecord"/> type.</returns>
      public async Task<GWBRecord> GetGemeenten(CancellationToken cancellationToken = default)
      {
         var requestUri = $"postalcodes/gwb/gemeenten";
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
         var requestUri = $"postalcodes/gwb/gemeente/{gemeenteId}/wijken";
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
         var requestUri = $"postalcodes/gwb/wijk/{wijkId}/buurten";
         return await HttpGet<GWBRecord>(this.HttpClient, requestUri, cancellationToken);
      }

      #endregion

      internal void SetEpsgHeader(int epsg)
      {
         SetRequestHeaderValue(HttpClient, "X-EPSG", epsg.ToString());
      }
   }
}
