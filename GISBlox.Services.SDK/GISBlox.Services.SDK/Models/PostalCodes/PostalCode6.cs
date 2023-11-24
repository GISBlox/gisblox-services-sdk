// ------------------------------------------------------------
// Copyright (c) Bartels Online.  All rights reserved.
// ------------------------------------------------------------

namespace GISBlox.Services.SDK.Models
{
   /// <summary>
   /// Represents a postal code.
   /// </summary>
   public class PostalCode6
   {
      /// <summary>
      /// The postal code for which results are generated.
      /// </summary>     
      public string Id { get; set; }

      /// <summary>
      /// Location data for the postal code.
      /// </summary>      
      public Location6Info Location { get; set; }
   }
}
