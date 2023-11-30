// ------------------------------------------------------------
// Copyright (c) Bartels Online.  All rights reserved.
// ------------------------------------------------------------

using GISBlox.Services.SDK.Models;
using GISBlox.Services.SDK.Models.PostalCodes;
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
      /// <param name="id">A Dutch postal code.</param>
      /// <param name="epsg">The target coordinate system.</param>
      /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
      /// <returns><see cref="IPostalCodeRecord"/></returns>
      Task<IPostalCodeRecord> GetPostalCodeRecord<IPostalCodeRecord>(string id, CoordinateSystem epsg = CoordinateSystem.RDNew, CancellationToken cancellationToken = default);

      /// <summary>
      /// Gets neighbouring postal code records of the specified postal code.
      /// </summary>
      /// <param name="id">A Dutch postal code.</param>
      /// <param name="includeSourcePostalCode">Determines whether to include the source postal code in the result.</param>
      /// <param name="epsg">The target coordinate system.</param>
      /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
      /// <returns><see cref="IPostalCodeRecord"/></returns>
      Task<IPostalCodeRecord> GetPostalCodeNeighbours<IPostalCodeRecord>(string id, bool includeSourcePostalCode = false, CoordinateSystem epsg = CoordinateSystem.RDNew, CancellationToken cancellationToken = default);

      /// <summary>
      /// Gets postal code records based on a WKT (well-known text) geometry string.
      /// </summary>
      /// <param name="wkt">The geometry of the location expressed in WKT (well-known text) format.</param>
      /// <param name="buffer">Buffer distance expressed in the unit of the WKT coordinate system.</param>
      /// <param name="wktEpsg">The coordinate system of the specified geometry.</param>
      /// <param name="targetEpsg">The target coordinate system.</param>
      /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
      /// <returns><see cref="IPostalCodeRecord"/></returns>
      Task<IPostalCodeRecord> GetPostalCodeByGeometry<IPostalCodeRecord>(string wkt, int buffer = 0, CoordinateSystem wktEpsg = CoordinateSystem.RDNew, CoordinateSystem targetEpsg = CoordinateSystem.RDNew, CancellationToken cancellationToken = default);

      /// <summary>
      /// Gets postal code records based on one or more area IDs.
      /// </summary>
      /// <param name="gemeenteId">A gemeente code.</param>
      /// <param name="wijkId">A district ('wijk') code.</param>   
      /// <param name="buurtId">A neighbourhood ('buurt') code. Considered only when a <see cref="PostalCode6Record"/> is requested.</param>
      /// <param name="epsg">The target coordinate system.</param>
      /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
      /// <returns><see cref="IPostalCodeRecord"/></returns>
      Task<IPostalCodeRecord> GetPostalCodeByArea<IPostalCodeRecord>(int gemeenteId, int wijkId, int buurtId = -1, CoordinateSystem epsg = CoordinateSystem.RDNew, CancellationToken cancellationToken = default);

      /// <summary>
      /// Gets the key figures record for the specified postal code area.
      /// </summary>
      /// <param name="id">A 4-digit Dutch postal code.</param>
      /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
      /// <returns>A <see cref="KerncijferRecord"/> type.</returns>
      Task<KerncijferRecord> GetKeyFigures4Record(string id, CancellationToken cancellationToken = default);

      
      /// <summary>
      /// Gets the key figures record for the specified postal code area.
      /// </summary>
      /// <param name="id">A 6-digit Dutch postal code.</param>
      /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
      /// <returns>A <see cref="KerncijferRecord"/> type.</returns>
      Task<KerncijferRecord> GetKeyFigures6Record(string id, CancellationToken cancellationToken = default);



      #region GWB

      /// <summary>
      /// Query for postal code's gemeenten. 
      /// </summary>
      /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
      /// <returns>A <see cref="GWBRecord"/> type.</returns>
      Task<GWBRecord> GetGemeenten(CancellationToken cancellationToken = default);

      /// <summary>
      /// Query for postal code's 'wijken' by 'gemeenten'. 
      /// </summary>
      /// <param name="gemeenteId">A gemeente ID.</param>
      /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
      /// <returns>A <see cref="GWBRecord"/> type.</returns>
      Task<GWBRecord> GetWijken(int gemeenteId, CancellationToken cancellationToken = default);

      /// <summary>
      /// Query for postal code's 'buurten' by 'wijken'. 
      /// </summary>
      /// <param name="wijkId">A wijk ID.</param>
      /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
      /// <returns>A <see cref="GWBRecord"/> type.</returns>
      Task<GWBRecord> GetBuurten(int wijkId, CancellationToken cancellationToken = default);

      #endregion
   }
}
