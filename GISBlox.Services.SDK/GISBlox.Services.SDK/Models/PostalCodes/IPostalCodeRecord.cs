// ------------------------------------------------------------
// Copyright (c) Bartels Online.  All rights reserved.
// ------------------------------------------------------------

namespace GISBlox.Services.SDK.Models.PostalCodes
{
   /// <summary>
   /// Interface for a PostalCodeRecord.
   /// </summary>
   public interface IPostalCodeRecord
   {
      /// <summary>
      /// Specifies metadata for the returned geometries.
      /// </summary>
      MetaData MetaData { get; set; }
   }
}
