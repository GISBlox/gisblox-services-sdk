// ------------------------------------------------------------
// Copyright (c) Bartels Online.  All rights reserved.
// ------------------------------------------------------------

using System.Collections.Generic;

namespace GISBlox.Services.SDK.Models
{
   /// <summary>
   /// This class represents the Gemeente, Wijk or Buurt record.
   /// </summary>
   public class GWBRecord
   {
      /// <summary>
      /// Metadata of the result record set. 
      /// </summary>
      public GWBMetaData MetaData { get; set; }

      /// <summary>
      /// The result record set with a list of GWB objects that match the request.
      /// </summary> 
      public List<GWB> RecordSet { get; set; }
   }
}
