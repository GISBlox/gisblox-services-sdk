namespace GISBlox.Services.SDK.Tests
{
   [TestClass]
   public class ConversionTests
   {
      GISBloxClient _client;
      const string BASE_URL = "https://services.gisblox.com";
      
      [TestInitialize]
      public void Init()
      {
         string serviceKey = Environment.GetEnvironmentVariable("ServiceKey");
         _client = GISBloxClient.CreateClient(BASE_URL, serviceKey);
      }

      [TestCleanup]
      public void Cleanup()
      {
         _client.Dispose();
      }

      [TestMethod]
      public async Task ConvertPoint()
      {         
         WKT wkt = new("POINT (30 10 5)");
         string geoJson = await ConvertToGeoJson(wkt);
         Assert.IsNotNull(geoJson);         
      }

      private async Task<string> ConvertToGeoJson(WKT wkt, bool asFeatureCollection = false)
      {
         return await _client.Conversion.ToGeoJson(wkt, asFeatureCollection);         
      }
   }
}