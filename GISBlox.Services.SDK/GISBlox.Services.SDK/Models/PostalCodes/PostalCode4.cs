// ------------------------------------------------------------
// Copyright (c) Bartels Online.  All rights reserved.
// ------------------------------------------------------------

namespace GISBlox.Services.SDK.Models
{
   /// <summary>
   /// Represents a postal code area.
   /// </summary>
   public class PostalCode4
   {
      /// <summary>
      /// The postal code for which results are generated.
      /// </summary>      
      public string Id { get; set; }

      /// <summary>
      /// Location data for the postal code area.
      /// </summary>      
      public Location4Info Location { get; set; }
   }
}
