namespace GISBlox.Services.SDK.Tests
{
   [TestClass]
   public class ConversionTests
   {
      GISBloxClient _client;
      const string BASE_URL = "https://services-private.gisblox.com";

      private const string WKB_POINT_30_10 = "AQEAAAAAAAAAAAA+QAAAAAAAACRA";
      private static readonly byte[] WKB_POINT_30_10_BYTES = [ 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 62, 64, 0, 0, 0, 0, 0, 0, 36, 64 ];
      private static readonly byte[] WKB_POINT_30_10_5_BYTES = [1, 233, 3, 0, 0, 0, 0, 0, 0, 0, 0, 62, 64, 0, 0, 0, 0, 0, 0, 36, 64, 0, 0, 0, 0, 0, 0, 20, 64];

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

      #region WKT/WKB -> GeoJson

      [TestMethod]
      public async Task ConvertPoint()
      {
         WKT wkt = new("POINT (30 10 5)");
         string geoJson = await ConvertToGeoJsonFromWKT(wkt);

         Assert.IsNotNull(geoJson, "Response is empty.");
         Assert.IsTrue(await IsValidGeoJson(geoJson, "POINT"), "Invalid GeoJSON.");
      }

      [TestMethod]
      public async Task ConvertPoint_WKB()
      { 
         WKB wkb = new(WKB_POINT_30_10_BYTES);
         string geoJson = await ConvertToGeoJsonFromWKB(wkb);

         Assert.IsNotNull(geoJson, "Response is empty.");
         Assert.IsTrue(await IsValidGeoJson(geoJson, "POINT"), "Invalid GeoJSON.");
      }

      [TestMethod]
      public async Task ConvertMultiPoint()
      {
         WKT wkt = new("MULTIPOINT ((10 40), (40 30 2), (20 20), (30 10))");
         string geoJson = await ConvertToGeoJsonFromWKT(wkt);

         Assert.IsNotNull(geoJson, "Response is empty.");
         Assert.IsTrue(await IsValidGeoJson(geoJson, "MULTIPOINT"), "Invalid GeoJSON.");
      }

      [TestMethod]
      public async Task ConvertMultiPoint_WKB()
      {
         // TODO: Replace with actual WKB for MULTIPOINT ((1040), (40302), (2020), (3010))
         byte[] wkbBytes = new byte[] { /* WKB bytes for MULTIPOINT ((1040), (40302), (2020), (3010)) */ };
         WKB wkb = new(wkbBytes);
         string geoJson = await ConvertToGeoJsonFromWKB(wkb);

         Assert.IsNotNull(geoJson, "Response is empty.");
         Assert.IsTrue(await IsValidGeoJson(geoJson, "MULTIPOINT"), "Invalid GeoJSON.");
      }

      [TestMethod]
      public async Task ConvertLineString()
      {
         WKT wkt = new("LINESTRING (30 10, 10 30, 40 40)"); ;
         string geoJson = await ConvertToGeoJsonFromWKT(wkt);

         Assert.IsNotNull(geoJson, "Response is empty.");
         Assert.IsTrue(await IsValidGeoJson(geoJson, "LINESTRING"), "Invalid GeoJSON.");
      }

      [TestMethod]
      public async Task ConvertLineString_WKB()
      {
         // TODO: Replace with actual WKB for LINESTRING (3010,1030,4040)
         byte[] wkbBytes = new byte[] { /* WKB bytes for LINESTRING (3010,1030,4040) */ };
         WKB wkb = new(wkbBytes);
         string geoJson = await ConvertToGeoJsonFromWKB(wkb);

         Assert.IsNotNull(geoJson, "Response is empty.");
         Assert.IsTrue(await IsValidGeoJson(geoJson, "LINESTRING"), "Invalid GeoJSON.");
      }

      [TestMethod]
      public async Task ConvertMultiLineString()
      {
         WKT wkt = new("MULTILINESTRING ((10 10, 20 20, 10 40),(40 40, 30 30, 40 20, 30 10))");
         string geoJson = await ConvertToGeoJsonFromWKT(wkt);

         Assert.IsNotNull(geoJson, "Response is empty.");
         Assert.IsTrue(await IsValidGeoJson(geoJson, "MULTILINESTRING"), "Invalid GeoJSON.");
      }

      [TestMethod]
      public async Task ConvertMultiLineString_WKB()
      {
         // TODO: Replace with actual WKB for MULTILINESTRING ((1010,2020,1040),(4040,3030,4020,3010))
         byte[] wkbBytes = new byte[] { /* WKB bytes for MULTILINESTRING ((1010,2020,1040),(4040,3030,4020,3010)) */ };
         WKB wkb = new(wkbBytes);
         string geoJson = await ConvertToGeoJsonFromWKB(wkb);

         Assert.IsNotNull(geoJson, "Response is empty.");
         Assert.IsTrue(await IsValidGeoJson(geoJson, "MULTILINESTRING"), "Invalid GeoJSON.");
      }

      [TestMethod]
      public async Task ConvertPolygon()
      {
         WKT wkt = new("POLYGON ((30 10, 40 40, 20 40, 10 20, 30 10))");
         string geoJson = await ConvertToGeoJsonFromWKT(wkt);

         Assert.IsNotNull(geoJson, "Response is empty.");
         Assert.IsTrue(await IsValidGeoJson(geoJson, "POLYGON"), "Invalid GeoJSON.");
      }

      [TestMethod]
      public async Task ConvertPolygon_WKB()
      {
         // TODO: Replace with actual WKB for POLYGON ((3010,4040,2040,1020,3010))
         byte[] wkbBytes = new byte[] { /* WKB bytes for POLYGON ((3010,4040,2040,1020,3010)) */ };
         WKB wkb = new(wkbBytes);
         string geoJson = await ConvertToGeoJsonFromWKB(wkb);

         Assert.IsNotNull(geoJson, "Response is empty.");
         Assert.IsTrue(await IsValidGeoJson(geoJson, "POLYGON"), "Invalid GeoJSON.");
      }

      [TestMethod]
      public async Task ConvertPolygonWithInnerRing()
      {
         WKT wkt = new("POLYGON ((35 10, 45 45, 15 40, 10 20, 35 10),(20 30, 35 35, 30 20, 20 30))");
         string geoJson = await ConvertToGeoJsonFromWKT(wkt);

         Assert.IsNotNull(geoJson, "Response is empty.");
         Assert.IsTrue(await IsValidGeoJson(geoJson, "POLYGON"), "Invalid GeoJSON.");
      }

      [TestMethod]
      public async Task ConvertPolygonWithInnerRing_WKB()
      {
         // TODO: Replace with actual WKB for POLYGON ((3510,4545,1540,1020,3510),(2030,3535,3020,2030))
         byte[] wkbBytes = new byte[] { /* WKB bytes for POLYGON ((3510,4545,1540,1020,3510),(2030,3535,3020,2030)) */ };
         WKB wkb = new(wkbBytes);
         string geoJson = await ConvertToGeoJsonFromWKB(wkb);

         Assert.IsNotNull(geoJson, "Response is empty.");
         Assert.IsTrue(await IsValidGeoJson(geoJson, "POLYGON"), "Invalid GeoJSON.");
      }

      [TestMethod]
      public async Task ConvertMultiPolygon()
      {
         WKT wkt = new("MULTIPOLYGON (((30 20, 45 40, 10 40, 30 20)),((15 5, 40 10, 10 20, 5 10, 15 5)))");
         string geoJson = await ConvertToGeoJsonFromWKT(wkt);

         Assert.IsNotNull(geoJson, "Response is empty.");
         Assert.IsTrue(await IsValidGeoJson(geoJson, "MULTIPOLYGON"), "Invalid GeoJSON.");
      }

      [TestMethod]
      public async Task ConvertMultiPolygon_WKB()
      {
         // TODO: Replace with actual WKB for MULTIPOLYGON (((3020,4540,1040,3020)),((155,4010,1020,510,155)))
         byte[] wkbBytes = new byte[] { /* WKB bytes for MULTIPOLYGON (((3020,4540,1040,3020)),((155,4010,1020,510,155))) */ };
         WKB wkb = new(wkbBytes);
         string geoJson = await ConvertToGeoJsonFromWKB(wkb);

         Assert.IsNotNull(geoJson, "Response is empty.");
         Assert.IsTrue(await IsValidGeoJson(geoJson, "MULTIPOLYGON"), "Invalid GeoJSON.");
      }

      [TestMethod]
      public async Task ConvertMultiPolygonWithInnerRing()
      {
         WKT wkt = new("MULTIPOLYGON (((40 40, 20 45, 45 30, 40 40)),((20 35, 10 30, 10 10, 30 5, 45 20, 20 35),(30 20, 20 15, 20 25, 30 20)))");
         string geoJson = await ConvertToGeoJsonFromWKT(wkt);

         Assert.IsNotNull(geoJson, "Response is empty.");
         Assert.IsTrue(await IsValidGeoJson(geoJson, "MULTIPOLYGON"), "Invalid GeoJSON.");
      }

      #endregion

      #region GeoJson -> WKT 

      [TestMethod]
      public async Task ToWkt_FromString()
      {
         string geoJson = "{\"type\":\"Feature\",\"geometry\":{\"type\":\"Point\",\"coordinates\":[30,10]},\"properties\":{}}";
         var wktList = await _client.Conversion.ToWkt(geoJson, CancellationToken.None);

         Assert.IsNotNull(wktList, "Returned WKT list is null.");
         Assert.IsGreaterThan(0, wktList.Count, "Returned WKT list is empty.");
         Assert.IsFalse(string.IsNullOrWhiteSpace(wktList[0].Geometry), "WKT geometry is empty.");

         Assert.AreEqual("POINT(30 10)", wktList[0].Geometry);
      }

      [TestMethod]
      public async Task ToWktWithProperties_FromString()
      {
         string geoJson = "{\"type\":\"Feature\",\"geometry\":{\"type\":\"Point\",\"coordinates\":[30,10,5]},\"properties\":{\"zValue\":23,\"name\":\"Single Point\"}}";
         var wktList = await _client.Conversion.ToWkt(geoJson, CancellationToken.None);

         Assert.IsNotNull(wktList, "Returned WKT list is null.");
         Assert.IsGreaterThan(0, wktList.Count, "Returned WKT list is empty.");
         Assert.IsFalse(string.IsNullOrWhiteSpace(wktList[0].Geometry), "WKT geometry is empty.");

         var wkt = wktList.FirstOrDefault();
         Assert.AreEqual("POINT Z (30 10 5)", wkt.Geometry);

         Assert.IsNotNull(wkt.Properties, "WKT properties are null.");
         Assert.IsGreaterThan(0, wkt.Properties.Count, "WKT properties are empty.");

         Assert.AreEqual(23L, wkt.Properties[0]["zValue"]);
         Assert.AreEqual("Single Point", wkt.Properties[0]["name"]);
      }

      [TestMethod]
      public async Task ToWkt_FromStream()
      {
         string geoJson = "{\"type\":\"Feature\",\"geometry\":{\"type\":\"Point\",\"coordinates\":[30,10]},\"properties\":{}}";
         using var stream = new System.IO.MemoryStream(System.Text.Encoding.UTF8.GetBytes(geoJson));
         var wktList = await _client.Conversion.ToWkt(stream, "test.json", CancellationToken.None);

         Assert.IsNotNull(wktList, "Returned WKT list is null.");
         Assert.IsGreaterThan(0, wktList.Count, "Returned WKT list is empty.");

         var wkt = wktList.FirstOrDefault();
         Assert.IsFalse(string.IsNullOrWhiteSpace(wkt.Geometry), "WKT geometry is empty.");

         Assert.AreEqual("POINT(30 10)", wkt.Geometry);
      }

      [TestMethod]
      public async Task ToWktWithProperties_FromStream()
      {
         string geoJson = "{\"type\":\"Feature\",\"geometry\":{\"type\":\"Point\",\"coordinates\":[30,10,5]},\"properties\":{\"zValue\":23,\"name\":\"Single Point\"}}";
         using var stream = new System.IO.MemoryStream(System.Text.Encoding.UTF8.GetBytes(geoJson));
         var wktList = await _client.Conversion.ToWkt(stream, "test.json", CancellationToken.None);

         Assert.IsNotNull(wktList, "Returned WKT list is null.");
         Assert.IsGreaterThan(0, wktList.Count, "Returned WKT list is empty.");

         var wkt = wktList.FirstOrDefault();
         Assert.IsFalse(string.IsNullOrWhiteSpace(wkt.Geometry), "WKT geometry is empty.");
         Assert.AreEqual("POINT Z (30 10 5)", wkt.Geometry);

         Assert.IsNotNull(wkt.Properties, "WKT properties are null.");
         Assert.IsGreaterThan(0, wkt.Properties.Count, "WKT properties are empty.");
         Assert.AreEqual(23L, wkt.Properties[0]["zValue"]);
         Assert.AreEqual("Single Point", wkt.Properties[0]["name"]);
      }

      #endregion

      #region GeoJson -> WKB

      [TestMethod]
      public async Task ToWkb_FromString()
      {
         string geoJson = "{\"type\":\"Feature\",\"geometry\":{\"type\":\"Point\",\"coordinates\":[30,10]},\"properties\":{}}";
         var wkbList = await _client.Conversion.ToWkb(geoJson, CancellationToken.None);

         Assert.IsNotNull(wkbList, "Returned WKB list is null.");
         Assert.IsGreaterThan(0, wkbList.Count, "Returned WKB list is empty.");

         var wkb = wkbList.FirstOrDefault();
         Assert.IsNotNull(wkb.Geometry, "WKB geometry is null.");
         Assert.IsGreaterThan(0, wkb.Geometry.Length, "WKB geometry is empty.");

         CollectionAssert.AreEqual(WKB_POINT_30_10_BYTES, wkb.Geometry, "WKB geometry does not match expected value.");
      }

      [TestMethod]
      public async Task ToWkbWithProperties_FromString()
      {
         string geoJson = "{\"type\":\"Feature\",\"geometry\":{\"type\":\"Point\",\"coordinates\":[30,10,5]},\"properties\":{\"zValue\":23,\"name\":\"Single Point\"}}";
         var wkbList = await _client.Conversion.ToWkb(geoJson, CancellationToken.None);

         Assert.IsNotNull(wkbList, "Returned WKB list is null.");
         Assert.IsGreaterThan(0, wkbList.Count, "Returned WKB list is empty.");
         
         var wkb = wkbList.FirstOrDefault();
         Assert.IsNotNull(wkb.Geometry, "WKB geometry is null.");
         Assert.IsGreaterThan(0, wkb.Geometry.Length, "WKB geometry is empty.");
         CollectionAssert.AreEqual(WKB_POINT_30_10_5_BYTES, wkb.Geometry, "WKB geometry does not match expected value.");

         Assert.IsNotNull(wkb.Properties, "WKB properties are null.");
         Assert.IsGreaterThan(0, wkb.Properties.Count, "WKB properties are empty.");
         Assert.AreEqual(23L, wkb.Properties[0]["zValue"]);
         Assert.AreEqual("Single Point", wkb.Properties[0]["name"]);
      }

      [TestMethod]
      public async Task ToWkb_FromStream()
      {
         string geoJson = "{\"type\":\"Feature\",\"geometry\":{\"type\":\"Point\",\"coordinates\":[30,10]},\"properties\":{}}";
         using var stream = new System.IO.MemoryStream(System.Text.Encoding.UTF8.GetBytes(geoJson));
         var wkbList = await _client.Conversion.ToWkb(stream, "test.json", CancellationToken.None);

         Assert.IsNotNull(wkbList, "Returned WKB list is null.");
         Assert.IsGreaterThan(0, wkbList.Count, "Returned WKB list is empty.");

         var wkb = wkbList.FirstOrDefault();
         Assert.IsNotNull(wkb.Geometry, "WKB geometry is null.");
         Assert.IsGreaterThan(0, wkb.Geometry.Length, "WKB geometry is empty.");

         CollectionAssert.AreEqual(WKB_POINT_30_10_BYTES, wkb.Geometry, "WKB geometry does not match expected value.");
      }

      [TestMethod]
      public async Task ToWkbWithProperties_FromStream()
      {
         string geoJson = "{\"type\":\"Feature\",\"geometry\":{\"type\":\"Point\",\"coordinates\":[30,10,5]},\"properties\":{\"zValue\":23,\"name\":\"Single Point\"}}";
         using var stream = new System.IO.MemoryStream(System.Text.Encoding.UTF8.GetBytes(geoJson));
         var wkbList = await _client.Conversion.ToWkb(stream, "test.json", CancellationToken.None);

         Assert.IsNotNull(wkbList, "Returned WKB list is null.");
         Assert.IsGreaterThan(0, wkbList.Count, "Returned WKB list is empty.");

         var wkb = wkbList.FirstOrDefault();
         Assert.IsNotNull(wkb.Geometry, "WKB geometry is null.");
         Assert.IsGreaterThan(0, wkb.Geometry.Length, "WKB geometry is empty.");

         CollectionAssert.AreEqual(WKB_POINT_30_10_5_BYTES, wkb.Geometry, "WKB geometry does not match expected value.");

         Assert.IsNotNull(wkb.Properties, "WKB properties are null.");
         Assert.IsGreaterThan(0, wkb.Properties.Count, "WKB properties are empty.");

         Assert.AreEqual(23L, wkb.Properties[0]["zValue"]);
         Assert.AreEqual("Single Point", wkb.Properties[0]["name"]);
      }

      #endregion

      #region Private methods

      /// <summary>
      /// Converts a WKT geometry into a GeoJson Feature(Collection) string.
      /// </summary>
      /// <param name="wkt">A WKT type.</param>
      /// <param name="asFeatureCollection">Indicates whether to include the GeoJson feature in a feature collection.</param>
      /// <returns>A GeoJson string with the converted WKT geometry.</returns>
      private async Task<string> ConvertToGeoJsonFromWKT(WKT wkt, bool asFeatureCollection = false)
      {
         return await _client.Conversion.ToGeoJson(wkt, asFeatureCollection);
      }

      /// <summary>
      /// Converts a WKB geometry into a GeoJson Feature(Collection) string.
      /// </summary>
      /// <param name="wkb">A WKB type.</param>
      /// <param name="asFeatureCollection">Indicates whether to include the GeoJson feature in a feature collection.</param>
      /// <returns>A GeoJson string with the converted WKB geometry.</returns>
      private async Task<string> ConvertToGeoJsonFromWKB(WKB wkb, bool asFeatureCollection = false)
      {
         // the byte array in wkb.Geometry should be sent over the line as base64 encoded string
         string base64Geometry = Convert.ToBase64String(wkb.Geometry);
         // we need to create a new WKT object with the base64 encoded geometry as string ???

         WKT encodedWKB = new WKT
         {
            Geometry = base64Geometry,
            Properties = wkb.Properties
         };
         return await ConvertToGeoJsonFromWKT(encodedWKB, asFeatureCollection);
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
                  string typeName = geomType.GetString();
                  if (typeName.Equals(expectedType, StringComparison.InvariantCultureIgnoreCase))
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