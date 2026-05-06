// ------------------------------------------------------------
// Copyright (c) Bartels Online.  All rights reserved.
// ------------------------------------------------------------

using System.Collections.Generic;

namespace GISBlox.Services.SDK.Models
{
   /// <summary>
   /// This class represents the result of an audience analysis for a specific postal code.
   /// </summary>
   public class AudienceAnalysisResult
   {
      /// <summary>
      /// The postal code for which results are generated.
      /// </summary>      
      public string PostalCode { get; set; }

      /// <summary>
      /// The collection of neutral sentiment scores associated with this instance.
      /// </summary>      
      public Dictionary<string, object> NeutralScores { get; set; }

      /// <summary>
      /// Represents the persona for the neutral sentiment category.
      /// </summary>      
      public Dictionary<string, object> NeutralScoresPersona { get; set; }

      /// <summary>
      /// The collection of targeting scores associated with this instance.
      /// </summary>      
      public Dictionary<string, object> TargetingScores { get; set; }

      /// <summary>
      /// Represents the preset that was used for the targeting scores in this instance.
      /// </summary>
      public Dictionary<string, object> TargetingScoresPreset { get; set; }

      /// <summary>
      /// The collection of percentage values associated with this instance.
      /// </summary>
      public Dictionary<string, object> Percentages { get; set; }
      
      /// <summary>
      /// The collection of key figures associated with this instance.
      /// </summary>
      public Dictionary<string, object> KeyFigures { get; set; }
   }
}
