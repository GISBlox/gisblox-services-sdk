// ------------------------------------------------------------
// Copyright (c) Bartels Online.  All rights reserved.
// ------------------------------------------------------------

using GISBlox.Services.SDK.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GISBlox.Services.SDK.MapAnalytics
{
   /// <summary>
   /// Interface for MapAnalyticsAPI class.
   /// </summary>
   public interface IMapAnalyticsAPI : IDisposable
   {
      /// <summary>
      /// Gets the engagement metrics for a specific map.
      /// </summary>
      /// <param name="mapId">The ID of the map.</param>
      /// <param name="dateRange">The date range for the engagement data.</param>
      /// <param name="endDate">The end date for the engagement data (optional). If not specified, the end date will be set to yesterday.</param>
      /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
      /// <returns>An engagement record for the specified map.</returns>
      Task<EngagementRecord> GetMapEngagement(string mapId, AnalyticsDateRangeEnum dateRange, DateTime? endDate = null, CancellationToken cancellationToken = default);

      /// <summary>
      /// Gets the KPI data for a specific map.
      /// </summary>
      /// <param name="mapId">The ID of the map.</param>
      /// <param name="dateRange">The date range for the KPI data.</param>
      /// <param name="endDate">The end date for the KPI data (optional). If not specified, the end date will be set to yesterday.</param>
      /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
      /// <returns>A KPI record for the specified map.</returns>
      Task<MapKpiRecord> GetMapKpis(string mapId, AnalyticsDateRangeEnum dateRange, DateTime? endDate = null, CancellationToken cancellationToken = default);

      /// <summary>
      /// Gets the KPI data for every tracked map.
      /// </summary>
      /// <param name="dateRange">The date range for which to retrieve the KPIs.</param>
      /// <param name="endDate">The end date for the KPI data (optional). If not specified, the end date will be set to yesterday.</param>
      /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
      /// <returns>A KPI record for each map.</returns>
      Task<MapKpiRecord> GetMapsKpis(AnalyticsDateRangeEnum dateRange, DateTime? endDate = null, CancellationToken cancellationToken = default);

      /// <summary>
      /// Gets a list of maps that are tracked for the customer.
      /// </summary>
      /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
      /// <returns>A customer map record with a list of tracked maps and their IDs.</returns>
      Task<CustomerMapRecord> ListTrackedMaps(CancellationToken cancellationToken = default);
   }
}
