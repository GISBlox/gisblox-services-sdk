// ------------------------------------------------------------
// Copyright (c) Bartels Online.  All rights reserved.
// ------------------------------------------------------------

using System.Collections.Generic;

namespace GISBlox.Services.SDK.Models
{
   /// <summary>
   /// This class represents an audience analysis record.
   /// </summary>
   public class AudienceAnalysisRecord : IAudienceAnalysisRecord
   {
      /// <summary>
      /// A list of audience analysis results.
      /// </summary>
      public List<AudienceAnalysisResult> Results { get; set; }
   }
}
