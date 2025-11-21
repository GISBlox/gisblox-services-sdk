// ------------------------------------------------------------
// Copyright (c) Bartels Online.  All rights reserved.
// ------------------------------------------------------------

namespace GISBlox.Services.SDK.Models
{
   /// <summary>
   /// Represents a Key Performance Indicator (KPI).
   /// The KPI contains both current and historic values along with trend information.
   /// The historic value is typically from the previous comparable period (date range).
   /// </summary>
   public class Kpi
   {
      /// <summary>
      /// The name of the KPI.
      /// </summary>      
      public string Name { get; set; }

      /// <summary>
      /// The numeric value of the KPI.
      /// </summary>      
      public decimal Value { get; set; }

      /// <summary>
      /// The historic numeric value of the KPI in the previous period.
      /// </summary>      
      public decimal ValuePrev { get; set; }

      /// <summary>
      /// The trend indicator of the KPI (1 for up, -1 for down, 0 for no change).
      /// </summary>      
      public int Trend { get; set; }

      /// <summary>
      /// The difference percentage between the current and historic KPI values.
      /// </summary>      
      public int DiffPct { get; set; }
   }
}
