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
   public interface IInfoAPI : IDisposable
   {
      Task<List<Subscription>> GetSubscriptions(CancellationToken cancellationToken = default);
   }
}
