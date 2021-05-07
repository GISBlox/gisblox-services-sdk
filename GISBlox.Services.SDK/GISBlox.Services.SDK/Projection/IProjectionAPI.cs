using GISBlox.Services.SDK.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GISBlox.Services.SDK.Projection
{
   public interface IProjectionAPI : IDisposable
   {
      /// <summary>
      /// Reprojects a coordinate to an RDPoint.
      /// </summary>
      /// <param name="coordinate">A Coordinate type.</param>
      /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
      /// <returns>An RDPoint type.</returns>
      Task<RDPoint> ToRDS(Coordinate coordinate, CancellationToken cancellationToken = default);

      /// <summary>
      /// Reprojects multiple coordinates to RDPoints.
      /// </summary>
      /// <param name="coordinates">A List with Coordinate types.</param>
      /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
      /// <returns>A List with RDPoint types.</returns>
      Task<List<RDPoint>> ToRDS(List<Coordinate> coordinates, CancellationToken cancellationToken = default);

      /// <summary>
      /// Reprojects a coordinate to a location. Includes the source coordinate.
      /// </summary>
      /// <param name="coordinate">A Coordinate type.</param>
      /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
      /// <returns>An Location type.</returns>
      Task<Location> ToRDSComplete(Coordinate coordinate, CancellationToken cancellationToken = default);

      /// <summary>
      /// Reprojects multiple coordinates to locations. Includes the source coordinates.
      /// </summary>
      /// <param name="coordinates">A List with Coordinate types.</param>
      /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
      /// <returns>A List with Location types.</returns>
      Task<List<Location>> ToRDSComplete(List<Coordinate> coordinates, CancellationToken cancellationToken = default);

      /// <summary>
      /// Reprojects an RDPoint to a coordinate.
      /// </summary>
      /// <param name="rdPoint">An RDPoint type.</param>
      /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
      /// <returns>A Coordinate type.</returns>
      Task<Coordinate> ToWGS84(RDPoint rdPoint, CancellationToken cancellationToken = default);

      /// <summary>
      /// Reprojects multiple RDPoints to coordinates.
      /// </summary>
      /// <param name="rdPoints">A List with RDPoint types.</param>
      /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
      /// <returns>A List with Coordinate types.</returns>
      Task<List<Coordinate>> ToWGS84(List<RDPoint> rdPoints, CancellationToken cancellationToken = default);

      /// <summary>
      /// Reprojects an RDPoint to a location. Includes the source RDPoint.
      /// </summary>
      /// <param name="rdPoint">An RDPoint type.</param>
      /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
      /// <returns>A Location type.</returns>
      Task<Location> ToWGS84Complete(RDPoint rdPoint, CancellationToken cancellationToken = default);

      /// <summary>
      /// Reprojects multiple RDPoints to locations. Includes the source RDPoints.
      /// </summary>
      /// <param name="rdPoints">A List with RDPoint types.</param>
      /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
      /// <returns>A List with Location types.</returns>
      Task<List<Location>> ToWGS84Complete(List<RDPoint> rdPoints, CancellationToken cancellationToken = default);
   }
}
