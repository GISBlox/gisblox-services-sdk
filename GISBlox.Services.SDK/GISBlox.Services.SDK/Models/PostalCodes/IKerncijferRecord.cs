// ------------------------------------------------------------
// Copyright (c) Bartels Online.  All rights reserved.
// ------------------------------------------------------------

namespace GISBlox.Services.SDK.Models
{
   /// <summary>
   /// Interface for a Kerncijfer record.
   /// </summary>
   public interface IKerncijferRecord
   {
      /// <summary>
      /// Metadata of the result record set. 
      /// </summary>
      KerncijferMetaData MetaData { get; set; }

      /// <summary>
      /// The result record set with a key figures that match the request.
      /// </summary>
      Kerncijfers Recordset { get; set; }
   }
}
