// ------------------------------------------------------------
// Copyright (c) Bartels Online.  All rights reserved.
// ------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace GISBlox.Services.SDK.Models
{
   /// <summary>
   /// Represents the map KPI record containing key performance indicators (KPIs) for a specific map over a defined date range.
   /// </summary>
   public class MapKpiRecord
   {
      /// <summary>
      /// The selected date range of the KPI data.
      /// </summary>      
      public string DateRange { get; set; }

      /// <summary>
      /// The start date of the date range.
      /// </summary>      
      public DateTime StartDate { get; set; }

      /// <summary>
      /// The end date of the date range.
      /// </summary>      
      public DateTime EndDate { get; set; }

      /// <summary>
      /// The collection of key performance indicators (KPIs) associated with the specified map.
      /// </summary>      
      public List<MapKpi> MapKpis { get; set; }
   }
}
