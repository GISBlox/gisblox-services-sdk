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

      /// <summary>
      /// Gets neighbouring postal code records of the specified postal code.
      /// </summary>
      /// <param name="id">A 4-digit Dutch postal code.</param>
      /// <param name="includeSourcePostalCode">Determines whether to include the source postal code in the result.</param>
      /// <param name="epsg">The EPSG code of the target coordinate system. Currently supports EPSG codes 4326 and 28992 only.</param>
      /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
      /// <returns>A <see cref="PostalCode4Record"/> type.</returns>
      Task<PostalCode4Record> GetPostalCode4Neighbours(string id, bool includeSourcePostalCode = false, int epsg = 28992, CancellationToken cancellationToken = default);

      /// <summary>
      /// Gets postal code records based on a WKT (well-known text) geometry string.
      /// </summary>
      /// <param name="wkt">The geometry of the location expressed in WKT (well-known text) format.</param>
      /// <param name="buffer">Buffer distance expressed in the unit of the WKT coordinate system.</param>
      /// <param name="wktEpsg">The EPSG code of the coordinate system of the specified geometry. Currently supports EPSG codes 4326 and 28992 only.</param>
      /// <param name="targetEpsg">The EPSG code of the target coordinate system. Currently supports EPSG codes 4326 and 28992 only.</param>
      /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
      /// <returns>A <see cref="PostalCode4Record"/> type.</returns>
      Task<PostalCode4Record> GetPostalCode4ByGeometry(string wkt, int buffer = 0, int wktEpsg = 28992, int targetEpsg = 28992, CancellationToken cancellationToken = default);

      /// <summary>
      /// Gets postal code records based on one or more district IDs.
      /// </summary>
      /// <param name="gemeenteId">A gemeente code (optional).</param>
      /// <param name="wijkId">A district ('wijk') code.</param>      
      /// <param name="epsg">The EPSG code of the target coordinate system. Currently supports EPSG codes 4326 and 28992 only.</param>
      /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
      /// <returns>A <see cref="PostalCode4Record"/> type.</returns>
      Task<PostalCode4Record> GetPostalCode4ByGW(int gemeenteId, int wijkId, int epsg = 28992, CancellationToken cancellationToken = default);

      /// <summary>
      /// Gets the key figures record for the specified postal code area.
      /// </summary>
      /// <param name="id">A 4-digit Dutch postal code.</param>
      /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
      /// <returns>A <see cref="KerncijferRecord"/> type.</returns>
      Task<KerncijferRecord> GetKeyFigures4Record(string id, CancellationToken cancellationToken = default);
   }
}
