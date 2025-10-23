// ------------------------------------------------------------
// Copyright (c) Bartels Online.  All rights reserved.
// ------------------------------------------------------------

using GISBlox.Services.SDK.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace GISBlox.Services.SDK.Conversion
{
   /// <summary>
   /// Interface for ConversionAPI class.
   /// </summary>
   public interface IConversionAPI : IDisposable
   {
      /// <summary>
      /// Converts a WKT geometry string into a GeoJson Feature(Collection) string.
      /// </summary>
      /// <param name="wkt">A WKT type.</param>
      /// <param name="asFeatureCollection">Indicates whether to include the GeoJson feature in a feature collection.</param>
      /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
      /// <returns>A GeoJson string with the converted WKT geometry.</returns>
      Task<string> ToGeoJson(WKT wkt, bool asFeatureCollection = false, CancellationToken cancellationToken = default);

      /// <summary>
      /// Converts a WKB geometry into a GeoJson Feature(Collection) string.
      /// </summary>
      /// <param name="wkb">A WKB type.</param>
      /// <param name="asFeatureCollection">Indicates whether to include the GeoJson feature in a feature collection.</param>
      /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
      /// <returns>A GeoJson string with the converted WKB geometry.</returns>
      Task<string> ToGeoJson(WKB wkb, bool asFeatureCollection = false, CancellationToken cancellationToken = default);
      
      /// <summary>
      /// Converts a GeoJson string into a list of WKB geometries.
      /// </summary>
      /// <param name="geoJson">A GeoJson string.</param>
      /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
      /// <returns>A list of WKB geometries.</returns>
      Task<List<WKB>> ToWkb(string geoJson, CancellationToken cancellationToken = default);

      /// <summary>
      /// Converts a GeoJson stream into a list of WKB geometries.
      /// </summary>
      /// <param name="stream">A stream containing the GeoJson data.</param>
      /// <param name="fileName">The name of the file.</param>
      /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
      /// <returns>A list of WKB geometries.</returns>
      Task<List<WKB>> ToWkb(Stream stream, string fileName, CancellationToken cancellationToken = default);

      /// <summary>
      /// Converts a GeoJson string into a list of WKT geometries.
      /// </summary>
      /// <param name="geoJson">A GeoJson string.</param>
      /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
      /// <returns>A list of WKT geometries.</returns>
      Task<List<WKT>> ToWkt(string geoJson, CancellationToken cancellationToken = default);

      /// <summary>
      /// Converts a GeoJson string into a list of WKT geometries.
      /// </summary>
      /// <param name="stream">A stream containing the GeoJson data.</param>
      /// <param name="fileName">The name of the file.</param>
      /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
      /// <returns>A list of WKT geometries.</returns>
      Task<List<WKT>> ToWkt(Stream stream, string fileName, CancellationToken cancellationToken = default);
   }
}
