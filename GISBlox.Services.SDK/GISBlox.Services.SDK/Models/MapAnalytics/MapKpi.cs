// ------------------------------------------------------------
// Copyright (c) Bartels Online.  All rights reserved.
// ------------------------------------------------------------

using System.Collections.Generic;

namespace GISBlox.Services.SDK.Models
{
   /// <summary>
   /// Represents a collection of Key Performance Indicators (KPIs) for a specific map.
   /// </summary>
   public class MapKpi
   {
      /// <summary>
      /// The unique identifier of the map.
      /// </summary>      
      public string MapId { get; set; }

      /// <summary>
      /// The name of the map.
      /// </summary>      
      public string MapName { get; set; }

      /// <summary>
      /// A collection of key performance indicators (KPIs) associated with the map.
      /// </summary>      
      public List<Kpi> Kpis { get; set; }
   }
}
