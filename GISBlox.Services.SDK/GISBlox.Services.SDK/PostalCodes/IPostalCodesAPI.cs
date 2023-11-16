// ------------------------------------------------------------
// Copyright (c) Bartels Online.  All rights reserved.
// ------------------------------------------------------------

using GISBlox.Services.SDK.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GISBlox.Services.SDK.PostalCodes
{
   /// <summary>
   /// Interface for PostalCodesAPI class.
   /// </summary>
   public interface IPostalCodesAPI : IDisposable
   {
      /// <summary>
      /// Gets the postal code record for the specified postal code.
      /// </summary>
      /// <param name="id">A 4-digit Dutch postal code.</param>
      /// <param name="epsg">The EPSG code of the target coordinate system. Currently supports EPSG codes 4326 and 28992 only.</param>
      /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
      /// <returns>A <see cref="PostalCode4Record"/> type.</returns>
      Task<PostalCode4Record> GetPostalCode4Record(string id, int epsg = 28992, CancellationToken cancellationToken = default);
   }
}
