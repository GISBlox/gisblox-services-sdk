// ------------------------------------------------------------
// Copyright (c) Bartels Online.  All rights reserved.
// ------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace GISBlox.Services.SDK.Models
{
   /// <summary>
   /// Represents the engagement record containing engagement data for a specific map over a defined date range.
   /// </summary>
   public class EngagementRecord
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
      /// The selected date range of the engagement data.
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
      /// The collection of engagements associated with the specified map.
      /// </summary>      
      public List<Engagement> Engagements { get; set; }
   }
}
