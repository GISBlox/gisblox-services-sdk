// ------------------------------------------------------------
// Copyright (c) Bartels Online.  All rights reserved.
// ------------------------------------------------------------

using System.Collections.Generic;

namespace GISBlox.Services.SDK.Models
{
    /// <summary>
    /// Represents a collection of customer files.
    /// </summary>
    public class CustomerFiles
    {
      /// <summary>
      /// The customer folder ID.
      /// </summary>      
      public string FolderId { get; set; }

      /// <summary>
      /// A list of data lake files.
      /// </summary>
      public List<DataLakeFile> Files { get; set; }
    }
}
