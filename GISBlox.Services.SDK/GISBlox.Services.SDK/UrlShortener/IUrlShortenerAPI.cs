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
        Task<string> Shorten(string url, CancellationToken cancellationToken = default);

        /// <summary>
        /// Creates a GeoJSON (data) URL.
        /// </summary>
        /// <param name="geoJson">The GeoJSON data to host.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The created GeoJSON URL.</returns>
        Task<string> CreateGeoJsonUrl(string geoJson, CancellationToken cancellationToken = default);
    }
}
