// ------------------------------------------------------------
// Copyright (c) Bartels Online.  All rights reserved.
// ------------------------------------------------------------

using System.Collections.Generic;

namespace GISBlox.Services.SDK.Models.PostalCodes
{
   /// <summary>
   /// Interface for a PostalCode4Record.
   /// </summary>
   public interface IPostalCode4Record : IPostalCodeRecord
   {
      /// <summary>
      /// A list of postal code areas that match the request.
      /// </summary>
      List<PostalCode4> PostalCode { get; set; }
   }
}
