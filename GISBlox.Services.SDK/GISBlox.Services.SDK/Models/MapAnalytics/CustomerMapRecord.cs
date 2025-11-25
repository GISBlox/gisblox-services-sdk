// ------------------------------------------------------------
// Copyright (c) Bartels Online.  All rights reserved.
// ------------------------------------------------------------

using System.Collections.Generic;

namespace GISBlox.Services.SDK.Models
{
   /// <summary>
   /// Represents the customer maps record containing a list of customer maps.
   /// </summary>
   public class CustomerMapRecord
   {
      /// <summary>
      /// A list of customer maps.
      /// </summary>
      public List<CustomerMap> Maps { get; set; }
   }
}
