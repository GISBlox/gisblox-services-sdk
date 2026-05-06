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
      /// Contains methods to retrieve area records for Gemeente, Wijk and/or Buurt queries. 
      /// </summary>s
      AreaCodeHelper AreaHelper { get; }

      /// <summary>
      /// Gets the postal code record for the specified postal code.
      /// </summary>
      /// <param name="id">A Dutch postal code.</param>
      /// <param name="epsg">The target coordinate system.</param>
      /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
      /// <returns><see cref="IPostalCodeRecord"/></returns>
      Task<IPostalCodeRecord> GetPostalCodeRecord<IPostalCodeRecord>(string id, CoordinateSystem epsg = CoordinateSystem.RDNew, CancellationToken cancellationToken = default);

      /// <summary>
      /// Gets neighbouring postal code records of the specified postal code.
      /// </summary>
      /// <param name="id">A Dutch postal code.</param>
      /// <param name="includeSourcePostalCode">Determines whether to include the source postal code in the result.</param>
      /// <param name="epsg">The target coordinate system.</param>
      /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
      /// <returns><see cref="IPostalCodeRecord"/></returns>
      Task<IPostalCodeRecord> GetPostalCodeNeighbours<IPostalCodeRecord>(string id, bool includeSourcePostalCode = false, CoordinateSystem epsg = CoordinateSystem.RDNew, CancellationToken cancellationToken = default);

      /// <summary>
      /// Gets postal code records based on a WKT (well-known text) geometry string.
      /// </summary>
      /// <param name="wkt">The geometry of the location expressed in WKT (well-known text) format.</param>
      /// <param name="buffer">Buffer distance expressed in the unit of the WKT coordinate system.</param>
      /// <param name="wktEpsg">The coordinate system of the specified geometry.</param>
      /// <param name="targetEpsg">The target coordinate system.</param>
      /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
      /// <returns><see cref="IPostalCodeRecord"/></returns>
      Task<IPostalCodeRecord> GetPostalCodeByGeometry<IPostalCodeRecord>(string wkt, int buffer = 0, CoordinateSystem wktEpsg = CoordinateSystem.RDNew, CoordinateSystem targetEpsg = CoordinateSystem.RDNew, CancellationToken cancellationToken = default);

      /// <summary>
      /// Gets postal code records based on one or more area IDs. Use the methods in the <see cref="AreaHelper" /> class to retrieve the IDs.
      /// </summary>
      /// <param name="gemeenteId">A gemeente code.</param>
      /// <param name="wijkId">A district ('wijk') code.</param>   
      /// <param name="buurtId">A neighbourhood ('buurt') code. Considered only when a <see cref="PostalCode6Record"/> is requested.</param>
      /// <param name="epsg">The target coordinate system.</param>
      /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
      /// <returns><see cref="IPostalCodeRecord"/></returns>
      Task<IPostalCodeRecord> GetPostalCodeByArea<IPostalCodeRecord>(int gemeenteId, int wijkId, int buurtId = -1, CoordinateSystem epsg = CoordinateSystem.RDNew, CancellationToken cancellationToken = default);

      /// <summary>
      /// Gets the key figures record for the specified postal code area.
      /// </summary>
      /// <param name="id">A Dutch postal code.</param>
      /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
      /// <returns><see cref="KerncijferRecord"/></returns>
      Task<KerncijferRecord> GetKeyFigures(string id, CancellationToken cancellationToken = default);

      /// <summary>
      /// Runs an audience analysis using the specified postal codes and preset configuration.
      /// </summary>
      /// <param name="postalCodes">A comma-separated list of postal codes to include in the analysis. Cannot be null or empty.</param>
      /// <param name="preset">The name of the preset configuration to use for the analysis. Determines the analysis parameters and metrics.</param>
      /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
      /// <returns><see cref="AudienceAnalysisRecord"/></returns>
      Task<AudienceAnalysisRecord> RunAudienceAnalysis(string postalCodes, string preset, CancellationToken cancellationToken = default);

      /// <summary>
      /// Runs an audience analysis using the specified postal codes, preset configuration, and custom weights.
      /// </summary>
      /// <param name="postalCodes">A comma-separated list of postal codes to include in the analysis. Cannot be null or empty.</param>
      /// <param name="preset">The name of the preset configuration to use for the analysis. Determines the analysis parameters and metrics.</param>
      /// <param name="weightsJson">A JSON string representing custom weights for the analysis. Overrides the default weights in the preset.</param>
      /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
      /// <returns><see cref="AudienceAnalysisRecord"/></returns>
      Task<AudienceAnalysisRecord> RunAudienceAnalysis(string postalCodes, string preset, string weightsJson, CancellationToken cancellationToken = default);
   }
}
