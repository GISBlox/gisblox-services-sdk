// ------------------------------------------------------------
// Copyright (c) Bartels Online.  All rights reserved.
// ------------------------------------------------------------

using GISBlox.Services.SDK.Models;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace GISBlox.Services.SDK.PostalCodes
{
   /// <summary>
   /// 
   /// </summary>
   public class AreaCodeHelper : ApiClient
   {
      /// <summary>
      /// Initializes a new instance of the GISBlox.Services.SDK.PostalCodes.AreaCodeHelper class.
      /// </summary>
      /// <param name="httpClient">The current instance of the HTTPClient class.</param>
      public AreaCodeHelper(HttpClient httpClient) : base(httpClient)
      { }

      /// <summary>
      /// Query for a specific gemeente.
      /// </summary>
      /// <param name="name">A gemeente name.</param>
      /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
      /// <returns>A <see cref="GWB"/> type.</returns>
      public async Task<GWB> GetGemeente(string name, CancellationToken cancellationToken = default)
      {
         GWB gemeente = null;         
         GWBRecord records = await GetGemeenten(cancellationToken);
         if (records != null) 
         { 
            gemeente = records.RecordSet.Where(g => g.Naam.ToLower() == name.ToLower()).SingleOrDefault();
         }
         return gemeente;
      }

      /// <summary>
      /// Query for postal code's gemeenten. 
      /// </summary>
      /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
      /// <returns>A <see cref="GWBRecord"/> type.</returns>
      public async Task<GWBRecord> GetGemeenten(CancellationToken cancellationToken = default)
      {
         string requestUri = $"postalcodes/gwb/gemeenten";
         return await HttpGet<GWBRecord>(this.HttpClient, requestUri, cancellationToken);
      }      

      /// <summary>
      /// Query for postal code's 'wijken' by 'gemeente'. 
      /// </summary>
      /// <param name="gemeenteId">A gemeente ID.</param>
      /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
      /// <returns>A <see cref="GWBRecord"/> type.</returns>
      public async Task<GWBRecord> GetWijken(int gemeenteId, CancellationToken cancellationToken = default)
      {
         string requestUri = $"postalcodes/gwb/gemeente/{gemeenteId}/wijken";
         return await HttpGet<GWBRecord>(this.HttpClient, requestUri, cancellationToken);
      }

      /// <summary>
      /// Query for postal code's 'wijken' by 'gemeente'. 
      /// </summary>
      /// <param name="gemeente">A gemeente name.</param>
      /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
      /// <returns>A <see cref="GWBRecord"/> type.</returns>
      public async Task<GWBRecord> GetWijken(string gemeente, CancellationToken cancellationToken = default)
      {
         GWBRecord result = null;
         GWB gemeenteRecord = await GetGemeente(gemeente, cancellationToken);
         if (gemeenteRecord != null)
         {
            result = await GetWijken(gemeenteRecord.ID, cancellationToken);
         }
         return result;
      }

      /// <summary>
      /// Query for postal code's 'buurten' by 'wijk'. 
      /// </summary>
      /// <param name="wijkId">A wijk ID.</param>
      /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
      /// <returns>A <see cref="GWBRecord"/> type.</returns>
      public async Task<GWBRecord> GetBuurten(int wijkId, CancellationToken cancellationToken = default)
      {
         string requestUri = $"postalcodes/gwb/wijk/{wijkId}/buurten";
         return await HttpGet<GWBRecord>(this.HttpClient, requestUri, cancellationToken);
      }

      /// <summary>
      /// Query for postal code's 'buurten' by 'gemeente' and 'wijk'. 
      /// </summary>
      /// <param name="gemeenteId">A gemeente ID.</param>
      /// <param name="wijkId">A wijk ID.</param>
      /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
      /// <returns>A <see cref="GWBRecord"/> type.</returns>
      public async Task<GWBRecord> GetBuurten(int gemeenteId, int wijkId, CancellationToken cancellationToken = default)
      {
         GWBRecord buurten = null;
         GWBRecord wijken = await GetWijken(gemeenteId, cancellationToken);
         if (wijken != null)
         {
            GWB wijk = wijken.RecordSet.Where(w => w.ID == wijkId).SingleOrDefault();
            if (wijk != null)
            {
               buurten = await GetBuurten(wijk.ID, cancellationToken);
            }
         }
         return buurten;
      }

      /// <summary>
      /// Query for postal code's 'buurten' by 'gemeente' and 'wijk'. 
      /// </summary>
      /// <param name="gemeente">A gemeente name.</param>
      /// <param name="wijk">A wijk name.</param>
      /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
      /// <returns>A <see cref="GWBRecord"/> type.</returns>
      public async Task<GWBRecord> GetBuurten(string gemeente, string wijk, CancellationToken cancellationToken = default)
      {
         GWBRecord buurten = null;
         GWBRecord wijken = await GetWijken(gemeente, cancellationToken);
         if (wijken != null)
         {
            GWB result = wijken.RecordSet.Where(w => w.Naam.ToLower() == wijk.ToLower()).SingleOrDefault();
            if (result != null)
            {
               buurten = await GetBuurten(result.ID, cancellationToken);
            }
         }
         return buurten;
      }
   }
}
