// ------------------------------------------------------------
// Copyright (c) Bartels Online.  All rights reserved.
// ------------------------------------------------------------

using System.Collections.Generic;

namespace GISBlox.Services.SDK.Models.PostalCodes
{
   /// <summary>
   /// Interface for a PostalCode6Record.
   /// </summary>
   public interface IPostalCode6Record : IPostalCodeRecord
   {      
      /// <summary>
      /// A list of postal code areas that match the request.
      /// </summary>
      List<PostalCode6> PostalCode { get; set; }
   }
}
