// ------------------------------------------------------------
// Copyright (c) Bartels Online.  All rights reserved.
// ------------------------------------------------------------

using System.ComponentModel;

namespace GISBlox.Services.SDK.Models
{
   /// <summary>
   /// Represents the date range for analytics queries.
   /// </summary>
   public enum AnalyticsDateRangeEnum
   {

      /// <summary>
      /// Represents a one-week date range.
      /// </summary>
      [Description("Represents a one-week date range.")]
      OneWeek = 7,

      /// <summary>
      /// Represents a time span of two weeks.
      /// </summary>
      [Description("Represents a time span of two weeks.")]
      TwoWeeks = 14,

      /// <summary>
      /// Represents a time span of three weeks.
      /// </summary>
      [Description("Represents a time span of three weeks.")]
      ThreeWeeks = 21,

      /// <summary>
      /// Represents a time span of one month.
      /// </summary>
      [Description("Represents a time span of one month.")]
      OneMonth = 31
   };
}
