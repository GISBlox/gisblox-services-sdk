// ------------------------------------------------------------
// Copyright (c) Bartels Online.  All rights reserved.
// ------------------------------------------------------------

using GISBlox.Services.SDK.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GISBlox.Services.SDK.Info
{
   /// <summary>
   /// Interface for InfoAPI class.
   /// </summary>
   public interface IInfoAPI : IDisposable
   {
      /// <summary>
      /// Returns the subscriptions of the authorized user.
      /// </summary>
      /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
      /// <returns>A List with Subscription types.</returns>
      Task<List<Subscription>> GetSubscriptions(CancellationToken cancellationToken = default);
   }
}
