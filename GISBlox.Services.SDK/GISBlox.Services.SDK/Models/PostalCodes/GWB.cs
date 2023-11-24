// ------------------------------------------------------------
// Copyright (c) Bartels Online.  All rights reserved.
// ------------------------------------------------------------

namespace GISBlox.Services.SDK.Models
{
   /// <summary>
   /// Contains Gemeente, Wijk or Buurt information.
   /// </summary>
   public class GWB
   {
      /// <summary>
      /// The unique GWB identifier.
      /// </summary>
      public int ID { get; set; }

      /// <summary>
      /// The name of the GWB object.
      /// </summary>
      public string Naam { get; set; }
   }
}
