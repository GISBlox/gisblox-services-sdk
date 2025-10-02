// ------------------------------------------------------------
// Copyright (c) Bartels Online.  All rights reserved.
// ------------------------------------------------------------

using GISBlox.Services.SDK.Models;
using Microsoft.Extensions.Caching.Memory;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace GISBlox.Services.SDK.DataLake
{
    /// <summary>
    /// This class contains methods to interact with the Data Lake API.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the GISBlox.Services.SDK.PostalCodes.AreaCodeHelper class.
    /// </remarks>
    /// <param name="httpClient">The current instance of the HTTPClient class.</param>
    /// <param name="cache">The current instance of the MemoryCache class.</param>
    public class DataLakeAPIClient(HttpClient httpClient, IMemoryCache cache) : ApiClient(httpClient, cache), IDataLakeAPI
    {

        /// <summary>
        /// Deletes a file from the data lake.
        /// </summary>
        /// <param name="fileName">A file name.</param>
        /// <param name="cancellationToken">The cancellation token to use for the operation.</param>
        /// <returns>True if successful, False if not.</returns>
        public async Task<bool> DeleteFile(string fileName, CancellationToken cancellationToken = default)
        {
            var requestUri = $"datalake/delete/{fileName}";
            try
            {
                await HttpDelete(HttpClient, requestUri, null, cancellationToken);
                return true;
            }
            catch (ClientApiException)
            {
                throw;
            }
        }

        /// <summary>        
        /// Downloads a file from the data lake and saves it to the specified local path.
        /// </summary>
        /// <param name="fileName">The name of the file to download.</param>
        /// <param name="localPath">The local target location for the file.</param>
        /// <param name="cancellationToken">The cancellation token to use for the operation.</param> 
        public Task<string> DownloadFile(string fileName, string localPath, CancellationToken cancellationToken = default)
        {
            var requestUri = $"datalake/load/{fileName}";
            return DownloadJsonFileToDisk(HttpClient, requestUri, localPath, null, cancellationToken);
        }

        /// <summary>
        /// Downloads the data of a file from the data lake.
        /// </summary>
        /// <param name="fileName">The name of the file.</param>
        /// <param name="cancellationToken">The cancellation token to use for the operation.</param>
        public Task<string> DownloadFileData(string fileName, CancellationToken cancellationToken = default)
        {
            var requestUri = $"datalake/load/{fileName}";
            return DownloadJsonFile(HttpClient, Cache, requestUri, false, null, cancellationToken);
        }

        /// <summary>        
        /// Returns a list of files in the customer's data lake.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token to use for the operation.</param>
        public Task<CustomerFiles> GetCustomerFiles(CancellationToken cancellationToken = default)
        {
            var requestUri = $"datalake/list";
            return HttpGet<CustomerFiles>(HttpClient, Cache, requestUri, null, null, cancellationToken);
        }

        /// <summary>
        /// Returns the customer's data lake folder information.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token to use for the operation.</param>
        public Task<CustomerFolder> GetCustomerFolder(CancellationToken cancellationToken = default)
        {
            var requestUri = $"datalake/folder";
            return HttpGet<CustomerFolder>(HttpClient, Cache, requestUri, null, null, cancellationToken);
        }

        /// <summary>
        /// Uploads a file to the data lake from the specified local path.
        /// </summary>
        /// <param name="localPath">The path to the local file.</param>
        /// <param name="cancellationToken">The cancellation token to use for the operation.</param>
        /// <returns>True if successful, False if not.</returns>
        public Task<bool> UploadFile(string localPath, CancellationToken cancellationToken = default)
        {
            using var stream = new FileStream(localPath, FileMode.Open, FileAccess.Read);
            var fileName = Path.GetFileName(localPath);
            return UploadFile(stream, fileName, cancellationToken);
        }

        /// <summary>
        /// Creates a file in the data lake from the specified stream.
        /// </summary>
        /// <param name="stream">The stream from which to create a file.</param>
        /// <param name="fileName">The name for the new file in the data lake.</param>
        /// <param name="cancellationToken">The cancellation token to use for the operation.</param>
        /// <returns>True if successful, False if not.</returns>
        public async Task<bool> UploadFile(Stream stream, string fileName, CancellationToken cancellationToken = default)
        {
            var requestUri = $"datalake/save/{fileName}";
            try
            {
                await HttpPost(HttpClient, requestUri, stream, "application/json", null, cancellationToken);
                return true;
            }
            catch (ClientApiException)
            {
                throw;                
            }
        }

        /// <summary>
        /// Creates a file in the data lake from the specified JSON data.
        /// </summary>
        /// <param name="jsonData">The JSON data.</param>
        /// <param name="fileName">The name for the new file in the data lake.</param>
        /// <param name="cancellationToken">The cancellation token to use for the operation.</param>
        /// <returns>True if successful, False if not.</returns>
        public async Task<bool> UploadFileData(string jsonData, string fileName, CancellationToken cancellationToken = default)
        {
            var requestUri = $"datalake/save/{fileName}";
            try
            {                
                await HttpPost(HttpClient, requestUri, jsonData, null, cancellationToken);
                return true;
            }
            catch (ClientApiException)
            {
                throw;
            }
        }
    }
}
