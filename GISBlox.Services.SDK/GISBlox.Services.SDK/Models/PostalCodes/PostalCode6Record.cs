// ------------------------------------------------------------
// Copyright (c) Bartels Online.  All rights reserved.
// ------------------------------------------------------------

using GISBlox.Services.SDK.Models.PostalCodes;
using System.Collections.Generic;

namespace GISBlox.Services.SDK.Models
{
   /// <summary>
   /// This class represents a postal code record.
   /// </summary>
   public class PostalCode6Record : IPostalCode6Record
   {
      /// <summary>
      /// Specifies metadata for the returned geometries.
      /// </summary>
      public MetaData MetaData { get; set; }

      /// <summary>
      /// A list of postal code areas that match the request.
      /// </summary>
      public List<PostalCode6> PostalCode { get; set; }
   }
}
