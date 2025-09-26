// ------------------------------------------------------------
// Copyright (c) Bartels Online.  All rights reserved.
// ------------------------------------------------------------

using Microsoft.Extensions.Caching.Memory;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace GISBlox.Services.SDK.UrlShortener
{
    /// <summary>
    /// The class contains methods to communicate with the URL Shortener API.
    /// </summary>
    /// <param name="httpClient">The HTTP client.</param>
    /// <param name="cache">The memory cache.</param>
    public class UrlShortenerAPIClient(HttpClient httpClient, IMemoryCache cache) : ApiClient(httpClient, cache), IUrlShortenerAPI
    {
        /// <summary>
        /// Shortens a (data) URL.
        /// </summary>
        /// <param name="url">The URL to shorten.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The shortened URL.</returns>
        public async Task<string> ShortenAsync(string url, CancellationToken cancellationToken = default)
        {
            var requestUri = "url/shorten";
            return await HttpPost<string>(HttpClient, requestUri, url, null, cancellationToken);
        }
    }
}
