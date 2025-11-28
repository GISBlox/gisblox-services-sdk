// ------------------------------------------------------------
// Copyright (c) Bartels Online.  All rights reserved.
// ------------------------------------------------------------

using GISBlox.Services.SDK.Models;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GISBlox.Services.SDK.MapAnalytics
{
   /// <summary>
   /// This class provides methods to interact with the Map Analytics API, providing access to data of tracked maps.
   /// </summary>
   /// <remarks>
   /// Initializes a new instance of the GISBlox.Services.SDK.MapAnalytics.MapAnalyticsAPIClient class.
   /// </remarks>
   /// <param name="httpClient">The current instance of the HTTPClient class.</param>
   /// <param name="cache">The current instance of the MemoryCache class.</param>
   public class MapAnalyticsAPIClient(HttpClient httpClient, IMemoryCache cache) : ApiClient(httpClient, cache), IMapAnalyticsAPI
   {
      /// <summary>
      /// Gets the engagement metrics for a specific map.
      /// </summary>
      /// <param name="mapId">The ID of the map.</param>
      /// <param name="dateRange">The date range for the engagement data.</param>
      /// <param name="endDate">The end date for the engagement data (optional). If not specified, the end date will be set to yesterday.</param>
      /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
      /// <returns>An engagement record for the specified map.</returns>
      public async Task<EngagementRecord> GetMapEngagement(string mapId, AnalyticsDateRangeEnum dateRange, DateTime? endDate = null, CancellationToken cancellationToken = default)
      {
         var sb = new StringBuilder($"mapanalytics/engagement/{mapId}?range={(int)dateRange}");
         if (endDate.HasValue)
            sb.Append("&day=").Append(endDate.Value.ToString("yyyy-MM-dd"));

         var requestUri = sb.ToString();
         return await HttpGet<EngagementRecord>(HttpClient, Cache, requestUri, null, null, cancellationToken);
      }

      /// <summary>
      /// Gets the KPI data for a specific map.
      /// </summary>
      /// <param name="mapId">The ID of the map.</param>
      /// <param name="dateRange">The date range for the KPI data.</param>
      /// <param name="endDate">The end date for the KPI data (optional). If not specified, the end date will be set to yesterday.</param>
      /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
      /// <returns>A KPI record for the specified map.</returns>
      public async Task<MapKpiRecord> GetMapKpis(string mapId, AnalyticsDateRangeEnum dateRange, DateTime? endDate = null, CancellationToken cancellationToken = default)
      {
         var sb = new StringBuilder($"mapanalytics/kpis/{mapId}?range={(int)dateRange}");
         if (endDate.HasValue)
            sb.Append("&day=").Append(endDate.Value.ToString("yyyy-MM-dd"));

         var requestUri = sb.ToString();
         return await HttpGet<MapKpiRecord>(HttpClient, Cache, requestUri, null, null, cancellationToken);
      }

      /// <summary>
      /// Gets the KPI data for every tracked map.
      /// </summary>
      /// <param name="dateRange">The date range for which to retrieve the KPIs.</param>
      /// <param name="endDate">The end date for the KPI data (optional). If not specified, the end date will be set to yesterday.</param>
      /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
      /// <returns>A KPI record for each map.</returns>
      public async Task<MapKpiRecord> GetMapsKpis(AnalyticsDateRangeEnum dateRange, DateTime? endDate = null, CancellationToken cancellationToken = default)
      {
         var sb = new StringBuilder($"mapanalytics/kpis?range={(int)dateRange}");
         if (endDate.HasValue)
            sb.Append("&day=").Append(endDate.Value.ToString("yyyy-MM-dd"));

         var requestUri = sb.ToString();
         return await HttpGet<MapKpiRecord>(HttpClient, Cache, requestUri, null, null, cancellationToken);
      }

      /// <summary>
      /// Gets a list of maps that are tracked for the customer.
      /// </summary>
      /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
      /// <returns>A customer map record with a list of tracked maps and their IDs.</returns>
      public async Task<CustomerMapRecord> ListTrackedMaps(CancellationToken cancellationToken = default)
      {
         const string requestUri = "mapanalytics/list";
         return await HttpGet<CustomerMapRecord>(HttpClient, Cache, requestUri, null, null, cancellationToken);
      }
   }
}
