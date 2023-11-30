// ------------------------------------------------------------
// Copyright (c) Bartels Online.  All rights reserved.
// ------------------------------------------------------------

using GISBlox.Services.SDK.Models;
using System.Threading;
using System.Threading.Tasks;

namespace GISBlox.Services.SDK.PostalCodes
{
   /// <summary>
   /// 
   /// </summary>
   public interface IAreaCodeHelper
   {
      /// <summary>
      /// Query for a specific gemeente.
      /// </summary>
      /// <param name="name">A gemeente name.</param>
      /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
      /// <returns>A <see cref="GWB"/> type.</returns>
      Task<GWB> GetGemeente(string name, CancellationToken cancellationToken = default);

      /// <summary>
      /// Query all gemeenten. 
      /// </summary>
      /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
      /// <returns>A <see cref="GWBRecord"/> type.</returns>
      Task<GWBRecord> GetGemeenten(CancellationToken cancellationToken = default);

      /// <summary>
      /// Query for a specific wijk.
      /// </summary>
      /// <param name="gemeenteId">A gemeente ID.</param>
      /// <param name="wijkId">A wijk ID.</param>
      /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
      /// <returns>A <see cref="GWB"/> type.</returns>
      Task<GWB> GetWijk(int gemeenteId, int wijkId, CancellationToken cancellationToken = default);

      /// <summary>
      /// Query for a specific wijk.
      /// </summary>
      /// <param name="gemeente">A gemeente name.</param>
      /// <param name="wijk">A wijk name.</param>
      /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
      /// <returns>A <see cref="GWB"/> type.</returns>
      Task<GWB> GetWijk(string gemeente, string wijk, CancellationToken cancellationToken = default);

      /// <summary>
      /// Query for postal code's 'wijken' by 'gemeenten'. 
      /// </summary>
      /// <param name="gemeenteId">A gemeente ID.</param>
      /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
      /// <returns>A <see cref="GWBRecord"/> type.</returns>
      Task<GWBRecord> GetWijken(int gemeenteId, CancellationToken cancellationToken = default);

      /// <summary>
      /// Query for postal code's 'wijken' by 'gemeenten'. 
      /// </summary>
      /// <param name="gemeente">A gemeente name.</param>
      /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
      /// <returns>A <see cref="GWBRecord"/> type.</returns>
      Task<GWBRecord> GetWijken(string gemeente, CancellationToken cancellationToken = default);

      /// <summary>
      /// Query for postal code's 'buurten' by 'wijken'. 
      /// </summary>
      /// <param name="wijkId">A wijk ID.</param>
      /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
      /// <returns>A <see cref="GWBRecord"/> type.</returns>
      Task<GWBRecord> GetBuurten(int wijkId, CancellationToken cancellationToken = default);
   }
}
