using GISBlox.Services.SDK.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
      /// Reprojects a coordinate to an RDPoint. Includes the source coordinates.
      /// </summary>
      /// <param name="coordinate">A Coordinate type.</param>
      /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
      /// <returns>An RDPoint type.</returns>
      Task<Location> ToRDSComplete(Coordinate coordinate, CancellationToken cancellationToken = default);
   }
}
