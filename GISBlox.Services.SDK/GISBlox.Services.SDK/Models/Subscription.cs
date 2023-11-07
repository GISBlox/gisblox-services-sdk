// ------------------------------------------------------------
// Copyright (c) Bartels Online.  All rights reserved.
// ------------------------------------------------------------

using System;

namespace GISBlox.Services.SDK.Models
{
   /// <summary>
   /// This class contains GISBlox service subscription information.
   /// </summary>
   public class Subscription
   {
      /// <summary>
      /// Specifies the subscription code.
      /// </summary>
      public string Code { get; set; }
            
      /// <summary>
      /// Represents the name of the subscription.
      /// </summary>
      public string Name { get; set; }
            
      /// <summary>
      /// Contains a description of the subscription.
      /// </summary>
      public string Description { get; set; }

      /// <summary>
      /// Specifies the subscription registration date.
      /// </summary>
      public DateTime RegisterDate { get; set; }

      /// <summary>
      /// Specifies the subscription expiration date.
      /// </summary>
      public DateTime ExpirationDate { get; set; }

      /// <summary>
      /// Specifies whether the subscription has expired.
      /// </summary>
      public bool Expired { get; set; }
   }
}
