namespace GISBlox.Services.SDK.Tests
{
   [TestClass]
   public class ConversionTests
   {
      GISBloxClient _client;
      const string BASE_URL = "https://services.gisblox.com";

      #region Initialization and cleanup

      [TestInitialize]
      public void Init()
      {
         // Get the service key from the test.runsettings file
         string serviceKey = Environment.GetEnvironmentVariable("ServiceKey");
         
         // Create the service client object
         _client = GISBloxClient.CreateClient(BASE_URL, serviceKey);
      }

      [TestCleanup]
      public void Cleanup()
      {
         _client.Dispose();
      }

      #endregion

      [TestMethod]
      public async Task ConvertPoint()
      {         
         WKT wkt = new("POINT (30 10 5)");
         string geoJson = await ConvertToGeoJson(wkt);         
         
         Assert.IsNotNull(geoJson, "Response is empty.");
         Assert.IsTrue(await IsValidGeoJson(geoJson, "POINT"), "Invalid GeoJSON.");
      }

      #region Private methods

      /// <summary>
      /// Calls the API and returns the results.
      /// </summary>
      /// <param name="wkt">A WKT geometry string.</param>
      /// <param name="asFeatureCollection">Indicates whether to include the GeoJson feature in a feature collection.</param>
      /// <returns>A GeoJson string with the converted WKT geometry.</returns>
      private async Task<string> ConvertToGeoJson(WKT wkt, bool asFeatureCollection = false)
      {
         return await _client.Conversion.ToGeoJson(wkt, asFeatureCollection);         
      }

      /// <summary>
      /// Performs a basic GeoJson validity test. It checks whether: 
      /// - The geometry type matches the expected type
      /// - The geometry contains any coordinates
      /// </summary>
      /// <param name="geoJson">A GeoJson string.</param>
      /// <param name="expectedType">The expected geometry type.</param>
      /// <returns>True if valid, False if not.</returns>
      private async static Task<bool> IsValidGeoJson(string geoJson, string expectedType)
      {
         bool isValid = false;
         try
         {
            JsonDocument doc = await Task.Run(() => JsonDocument.Parse(geoJson));
            JsonElement jsonObject = doc.RootElement;
            if (jsonObject.TryGetProperty("geometry", out var typeProperty) && typeProperty.ValueKind == JsonValueKind.Object)
            {
               if (typeProperty.TryGetProperty("type", out var geomType) && geomType.ValueKind == JsonValueKind.String)               
               {
                  // Type check
                  string typeName = geomType.GetString().ToUpper();
                  if (typeName == expectedType.ToUpper())
                  {
                     // Any coordinates?
                     if (typeProperty.TryGetProperty("coordinates", out var coordinates) && coordinates.ValueKind == JsonValueKind.Array)
                     {
                        isValid = true;
                     }
                  }
               }               
            }
         }
         catch (Exception)
         {
            throw;
         }
         return isValid;
      }

      #endregion
   }
}