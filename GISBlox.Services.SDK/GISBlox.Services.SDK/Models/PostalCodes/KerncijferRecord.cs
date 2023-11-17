// ------------------------------------------------------------
// Copyright (c) Bartels Online.  All rights reserved.
// ------------------------------------------------------------

namespace GISBlox.Services.SDK.Models
{
   /// <summary>
   /// This class represents a key figures record.
   /// </summary>
   public class KerncijferRecord
   {
      /// <summary>
      /// Metadata of the result record set. 
      /// </summary>
      public KerncijferMetaData MetaData { get; set; }

      /// <summary>
      /// The result record set with a key figures that match the request.
      /// </summary>
      public Kerncijfers Recordset { get; set; }
   }
}
