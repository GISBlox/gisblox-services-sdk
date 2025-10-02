// ------------------------------------------------------------
// Copyright (c) Bartels Online.  All rights reserved.
// ------------------------------------------------------------

using GISBlox.Services.SDK.Models;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace GISBlox.Services.SDK.DataLake
{
    /// <summary>
    /// Interface for DataLakeAPI class.
    /// </summary>
    public interface IDataLakeAPI : IDisposable
    {
        /// <summary>
        /// Deletes a file from the data lake.
        /// </summary>
        /// <param name="fileName">A file name.</param>
        /// <param name="cancellationToken">The cancellation token to use for the operation.</param>
        /// <returns>True if successful, False if not.</returns>
        Task<bool> DeleteFile(string fileName, CancellationToken cancellationToken = default);

        /// <summary>        
        /// Downloads a file from the data lake and saves it to the specified local path.
        /// </summary>
        /// <param name="fileName">The name of the file to download.</param>
        /// <param name="localPath">The local target location for the file.</param>
        /// <param name="cancellationToken">The cancellation token to use for the operation.</param>        
        Task<string> DownloadFile(string fileName, string localPath, CancellationToken cancellationToken = default);

        /// <summary>
        /// Downloads the data of a file from the data lake.
        /// </summary>
        /// <param name="fileName">The name of the file.</param>
        /// <param name="cancellationToken">The cancellation token to use for the operation.</param>        
        Task<string> DownloadFileData(string fileName, CancellationToken cancellationToken = default);

        /// <summary>        
        /// Returns a list of files in the customer's data lake.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token to use for the operation.</param>        
        Task<CustomerFiles> GetCustomerFiles(CancellationToken cancellationToken = default);

        /// <summary>
        /// Returns the customer's data lake folder information.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token to use for the operation.</param>
        Task<CustomerFolder> GetCustomerFolder(CancellationToken cancellationToken = default);

        /// <summary>
        /// Uploads a file to the data lake from the specified local path.
        /// </summary>
        /// <param name="localPath">The path to the local file.</param>
        /// <param name="cancellationToken">The cancellation token to use for the operation.</param>
        /// <returns>True if successful, False if not.</returns>
        Task<bool> UploadFile(string localPath, CancellationToken cancellationToken = default);

        /// <summary>
        /// Creates a file in the data lake from the specified stream.
        /// </summary>
        /// <param name="stream">The stream from which to create a file.</param>
        /// <param name="fileName">The name for the new file in the data lake.</param>
        /// <param name="cancellationToken">The cancellation token to use for the operation.</param>
        /// <returns>True if successful, False if not.</returns>
        Task<bool> UploadFile(Stream stream, string fileName, CancellationToken cancellationToken = default);

        /// <summary>
        /// Creates a file in the data lake from the specified JSON data.
        /// </summary>
        /// <param name="jsonData">The JSON data.</param>
        /// <param name="fileName">The name for the new file in the data lake.</param>
        /// <param name="cancellationToken">The cancellation token to use for the operation.</param>
        /// <returns>True if successful, False if not.</returns>
        Task<bool> UploadFileData(string jsonData, string fileName, CancellationToken cancellationToken = default);
    }
}
