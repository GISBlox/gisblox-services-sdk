// ------------------------------------------------------------
// Copyright (c) Bartels Online.  All rights reserved.
// ------------------------------------------------------------

using System;
using System.Threading;
using System.Threading.Tasks;

namespace GISBlox.Services.SDK.UrlShortener
{
    /// <summary>
    /// Interface for UrlShortenerAPI class.
    /// </summary>
    public interface IUrlShortenerAPI : IDisposable
    {
        /// <summary>
        /// Shortens a (data) URL.
        /// </summary>
        /// <param name="url">The URL to shorten.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The shortened URL.</returns>
        Task<string> ShortenAsync(string url, CancellationToken cancellationToken = default);
    }
}
