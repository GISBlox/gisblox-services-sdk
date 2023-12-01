// ------------------------------------------------------------
// Copyright (c) Bartels Online.  All rights reserved.
// ------------------------------------------------------------

using System.Collections.Generic;

namespace GISBlox.Services.SDK.Models
{
   /// <summary>
   /// Interface for a GWB record.
   /// </summary>
   public interface IGWBRecord
   {
      /// <summary>
      /// Metadata of the result record set. 
      /// </summary>
      GWBMetaData MetaData { get; set; }

      /// <summary>
      /// The result record set with a list of GWB objects that match the request.
      /// </summary> 
      List<GWB> RecordSet { get; set; }
   }
}
