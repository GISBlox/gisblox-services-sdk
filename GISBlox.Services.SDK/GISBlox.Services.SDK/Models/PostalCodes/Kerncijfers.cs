// ------------------------------------------------------------
// Copyright (c) Bartels Online.  All rights reserved.
// ------------------------------------------------------------

using System.Collections.Generic;

namespace GISBlox.Services.SDK.Models
{
   /// <summary>
   /// This class contains key figures of a postal code area.
   /// </summary>
   public class Kerncijfers
   {
      /// <summary>
      /// Key figures about houses.
      /// </summary>
      public Dictionary<string, decimal> Woningen { get; set; }

      /// <summary>
      /// Key figures about inhabitants.
      /// </summary>
      public Dictionary<string, decimal> Inwoners { get; set; }

      /// <summary>
      /// Key figures about households.
      /// </summary>
      public Dictionary<string, decimal> Huishoudens { get; set; }
   }
}
