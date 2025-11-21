// ------------------------------------------------------------
// Copyright (c) Bartels Online.  All rights reserved.
// ------------------------------------------------------------

using System;

namespace GISBlox.Services.SDK.Models
{
   /// <summary>
   /// Represents engagement data for a specific map.
   /// </summary>
   public class Engagement
   {
      /// <summary>
      /// The date of the engagement session.
      /// </summary>
      public DateTime SessionDate { get; set; }

      /// <summary>
      /// The total number of map views on the session date.
      /// </summary>
      public int Views { get; set; }

      /// <summary>
      /// The total duration of map views in seconds on the session date.
      /// </summary>
      public int ViewDuration { get; set; }

      /// <summary>
      /// The total amount of interactions with the map on the session date.
      /// </summary> 
      public int Interactions { get; set; }

      /// <summary>
      /// The number of times the map was zoomed in or out on the session date.
      /// </summary>
      public int ZoomCount { get; set; }

      /// <summary>
      /// The number of times the map was panned on the session date.
      /// </summary> 
      public int PanCount { get; set; }

      /// <summary>
      /// The number of times a map marker has been clicked on the session date.
      /// </summary>
      public int MarkerClickCount { get; set; }
   }
}
