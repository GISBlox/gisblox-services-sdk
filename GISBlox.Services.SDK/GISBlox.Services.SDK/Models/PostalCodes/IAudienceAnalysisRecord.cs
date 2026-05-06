// ------------------------------------------------------------
// Copyright (c) Bartels Online.  All rights reserved.
// ------------------------------------------------------------

using System.Collections.Generic;

namespace GISBlox.Services.SDK.Models
{
   /// <summary>
   /// Interface for an audience analysis record.
   /// </summary>
   public interface IAudienceAnalysisRecord
   {
      /// <summary>
      /// A list of audience analysis results.
      /// </summary>
      List<AudienceAnalysisResult> Results { get; set; }
   }
}
