// ------------------------------------------------------------
// Copyright (c) Bartels Online.  All rights reserved.
// ------------------------------------------------------------

using GISBlox.Services.SDK.Models;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Net.Http;
using System.Text.RegularExpressions;
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
   public partial class PostalCodesAPIClient(HttpClient httpClient, IMemoryCache cache) : ApiClient(httpClient, cache), IPostalCodesAPI
   {
      [GeneratedRegex(@"\s+")]
      private static partial Regex RemoveSpaces();

      private readonly AreaCodeHelper _areaHelper = new(httpClient, cache);
      
      /// <summary>
      /// Contains methods to retrieve area records for Gemeente, Wijk and/or Buurt queries. 
      /// </summary>
      public AreaCodeHelper AreaHelper { get { return _areaHelper; } }

      /// <summary>
      /// Gets the postal code record for the specified postal code.
      /// </summary>
      /// <param name="id">A Dutch postal code.</param>
      /// <param name="epsg">The target coordinate system.</param>
      /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
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
      /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
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
      /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
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
      /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
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
      /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
      /// <returns><see cref="KerncijferRecord"/></returns>
      public async Task<KerncijferRecord> GetKeyFigures(string id, CancellationToken cancellationToken = default)
      {
         string requestUri = $"postalcodes{(id.Length == 4 ? "4" : "6")}/keyfigures/{id}";
         return await HttpGet<KerncijferRecord>(HttpClient, Cache, requestUri, null, null, cancellationToken);
      }

      /// <summary>
      /// Runs an audience analysis using the specified postal codes and preset configuration.
      /// </summary>
      /// <param name="postalCodes">A comma-separated list of postal codes to include in the analysis. Cannot be null or empty.</param>
      /// <param name="preset">The name of the preset configuration to use for the analysis. Determines the analysis parameters and metrics.</param>
      /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
      /// <returns><see cref="AudienceAnalysisResult"/></returns>
      public async Task<AudienceAnalysisResult> RunAudienceAnalysis(string postalCodes, string preset, CancellationToken cancellationToken = default)
      {
         return await RunAudienceAnalysis(postalCodes, preset, null, cancellationToken);
      }

      /// <summary>
      /// Runs an audience analysis using the specified postal codes, preset configuration, and custom weights.
      /// </summary>
      /// <param name="postalCodes">A comma-separated list of postal codes to include in the analysis. Cannot be null or empty.</param>
      /// <param name="preset">The name of the preset configuration to use for the analysis. Determines the analysis parameters and metrics.</param>
      /// <param name="weightsJson">A JSON string representing custom weights for the analysis. Overrides the default weights in the preset.</param>
      /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
      /// <returns><see cref="AudienceAnalysisResult"/></returns>
      public async Task<AudienceAnalysisResult> RunAudienceAnalysis(string postalCodes, string preset, string weightsJson, CancellationToken cancellationToken = default)
      {
         string cleanPostalCodes = RemoveSpaces().Replace(postalCodes, "");
         string requestUri = $"{BuildBaseAnalysisUri(cleanPostalCodes)}?postalCodes={cleanPostalCodes}&preset={preset}";
         
         return await HttpPost<AudienceAnalysisResult>(HttpClient, requestUri, weightsJson, null, cancellationToken);
      }

      #region Internal helper methods

      /// <summary>
      /// Returns the postal code service URI based on the passed type.
      /// </summary>
      /// <typeparam name="IPostalCodeRecord">A class that inherits from <see cref="IPostalCodeRecord"/>.</typeparam>
      /// <returns>The proper postal code service URI.</returns>
      internal static string BuildBaseUri<IPostalCodeRecord>()
      {         
         return $"postalcodes{((typeof(IPostalCodeRecord) == typeof(PostalCode4Record)) ? "4" : "6")}";         
      }

      /// <summary>
      /// Returns the postal code analysis service URI based on the digit length of the provided postal codes.
      /// </summary>
      /// <param name="postalCodes">A comma-separated list of postal code strings to evaluate.</param>
      /// <returns>The base URI for the postal code analysis service.</returns>
      /// <exception cref="ArgumentException"></exception>
      internal static string BuildBaseAnalysisUri(string postalCodes)
      {
         int digitLength = GetPostalCodeDigitLength(postalCodes);
         if (digitLength == 4)
         {
            return "postalcodes4/keyfigures/analysis";
         }
         else if (digitLength == 6)
         {
            return "postalcodes6/keyfigures/analysis";
         }
         else
         {
            throw new ArgumentException("Invalid postal code format. Postal codes must be either 4 or 6 digits long.");
         }
      }

      /// <summary>
      /// Determines the digit length of the provided postal codes, returning 4 or 6 if all codes match a consistent length.
      /// </summary>
      /// <remarks>This method ignores empty entries and trims whitespace from each code before evaluation.
      /// If the list contains a mix of 4- and 6-digit codes, or codes of other lengths, the method returns -1.</remarks>
      /// <param name="postalCodes">A comma-separated list of postal code strings to evaluate. Each code should contain only digits and may be
      /// separated by commas with optional whitespace.</param>
      /// <returns>The digit length of the postal codes if all codes are either 4 or 6 digits long; otherwise, -1.</returns>
      internal static int GetPostalCodeDigitLength(string postalCodes)
      {         
         string[] codes = postalCodes.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);         
         if (CheckDigits(codes, 4))
         {
            return 4;
         }
         if (CheckDigits(codes, 6))
         {
            return 6;
         }
         return -1;
      }

      /// <summary>
      /// Determines whether all strings in the specified array have the given length.
      /// </summary>
      /// <param name="codes">An array of strings to check for matching lengths. Cannot be null.</param>
      /// <param name="length">The length to compare against each string in the array. Must be non-negative.</param>
      /// <returns>true if all strings in the array have the specified length; otherwise, false.</returns>
      internal static bool CheckDigits(string[] codes, int length)
      {
         foreach (var code in codes)
         {
            if (code.Length != length)
            {
               return false;
            }
         }
         return true;
      }

      #endregion
   }
}
