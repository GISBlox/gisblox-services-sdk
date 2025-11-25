// ------------------------------------------------------------
// Copyright (c) Bartels Online.  All rights reserved.
// ------------------------------------------------------------

using System;

namespace GISBlox.Services.SDK.Models
{
   /// <summary>
   /// Represents a customer map.
   /// </summary>
   public class CustomerMap
   {
      /// <summary>
      /// The unique identifier for the map.
      /// </summary>
      public Guid Id { get; set; }

      /// <summary>
      /// The name of the map.
      /// </summary>
      public string Name { get; set; }
   }
}
