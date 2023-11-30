namespace GISBlox.Services.SDK.Tests
{
   [TestClass]
   public class PostalCodesTest
   {
      GISBloxClient _client;
      const string BASE_URL = "https://services.gisblox.com";
      const int API_QUOTA_DELAY = 1000;  // Allows to run all tests together without exceeding API call quota

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

      #region PC4

      [TestMethod]
      public async Task GetPostalCode4Record()
      {
         string id = "3811";
         PostalCode4Record record = await _client.PostalCodes.GetPostalCodeRecord<PostalCode4Record>(id);

         Assert.IsNotNull(record, "Response is empty.");
         
         PostalCode4 pc = record.PostalCode[0];
         Assert.IsTrue(pc.Location.Gemeente == "Amersfoort" && pc.Location.Geometry.Centroid == "POINT (155029.15793771204 463047.87594218826)");
         
         await Task.Delay(API_QUOTA_DELAY);
      }

      [TestMethod]
      public async Task GetPostalCode4Neighbours()
      {
         string id = "3811";
         bool includeSource = false;
         PostalCode4Record record = await _client.PostalCodes.GetPostalCodeNeighbours<PostalCode4Record>(id, includeSource);

         Assert.IsNotNull(record, "Response is empty.");
         Assert.IsTrue(record.PostalCode.Count == 6);

         List<string> expectedIDs = new() { "3817", "3814", "3816", "3813", "3812", "3818" };
         Assert.IsTrue(record.PostalCode.All(pc => expectedIDs.Contains(pc.Id)));

         await Task.Delay(API_QUOTA_DELAY * 2);
      }

      [TestMethod]
      public async Task GetPostalCode4NeighboursWithSource()
      {
         string id = "3811";
         bool includeSource = true;
         PostalCode4Record record = await _client.PostalCodes.GetPostalCodeNeighbours<PostalCode4Record>(id, includeSource);

         Assert.IsNotNull(record, "Response is empty.");
         Assert.IsTrue(record.PostalCode.Count == 7);

         List<string> expectedIDs = new() { "3811", "3817", "3814", "3816", "3813", "3812", "3818" };
         Assert.IsTrue(record.PostalCode.All(pc => expectedIDs.Contains(pc.Id)));

         await Task.Delay(API_QUOTA_DELAY * 2);
      }

      [TestMethod]
      public async Task GetPostalCode4ByGeometry()
      {
         string wkt = "LINESTRING(109935 561725, 110341 564040, 111430 565908)";
         PostalCode4Record record = await _client.PostalCodes.GetPostalCodeByGeometry<PostalCode4Record>(wkt);

         Assert.IsNotNull(record, "Response is empty.");
         Assert.IsTrue(record.PostalCode.Count == 3);

         List<string> expectedIDs = new() { "1791", "1796", "1797" };
         Assert.IsTrue(record.PostalCode.All(pc => expectedIDs.Contains(pc.Id)));

         await Task.Delay(API_QUOTA_DELAY * 2);
      }

      [TestMethod]
      public async Task GetPostalCode4ByGeometryWithBuffer()
      {
         string wkt = "LINESTRING(109935 561725, 110341 564040, 111430 565908)";
         int buffer = 5000;    // meters, since CS of WKT is 28992.
         PostalCode4Record record = await _client.PostalCodes.GetPostalCodeByGeometry<PostalCode4Record>(wkt, buffer);

         Assert.IsNotNull(record, "Response is empty.");
         Assert.IsTrue(record.PostalCode.Count == 5);

         List<string> expectedIDs = new() { "1791", "1793", "1795", "1796", "1797" };
         Assert.IsTrue(record.PostalCode.All(pc => expectedIDs.Contains(pc.Id)));
         
         await Task.Delay(API_QUOTA_DELAY * 2);
      }

      [TestMethod]
      public async Task GetPostalCode4ByGeometryWithBufferAndDifferentTargetEpsg()
      {
         string wkt = "POINT(121843 487293)";
         int buffer = 200;   // meters, since CS of WKT is 28992.
         PostalCode4Record record = await _client.PostalCodes.GetPostalCodeByGeometry<PostalCode4Record>(wkt, buffer, CoordinateSystem.RDNew, CoordinateSystem.WGS84);

         Assert.IsNotNull(record, "Response is empty.");
         Assert.IsTrue(record.PostalCode.Count == 2);

         List<string> expectedIDs = new() { "1011", "1012" };
         Assert.IsTrue(record.PostalCode.All(pc => expectedIDs.Contains(pc.Id)));
         Assert.IsTrue(record.PostalCode[1].Location.Geometry.Centroid == "POINT (4.905333126288754 52.371542282338666)");

         await Task.Delay(API_QUOTA_DELAY * 2);
      }

      [TestMethod]
      public async Task GetPostalCode4ByArea()
      {
         int gemeenteId = 513;
         string expectedGemeente = "Gouda";

         int wijkId = 51309;
         string expectedWijk = "Westergouwe";
                 
         string expectedPostalCode = "2809";

         PostalCode4Record record = await _client.PostalCodes.GetPostalCodeByArea<PostalCode4Record>(gemeenteId, wijkId);

         Assert.IsNotNull(record, "Response is empty.");
        
         PostalCode4 pc = record.PostalCode[0];
         Assert.IsTrue(pc.Id == expectedPostalCode);
         Assert.IsTrue(pc.Location.Gemeente == expectedGemeente);
         Assert.IsTrue(pc.Location.Wijken == expectedWijk);

         await Task.Delay(API_QUOTA_DELAY);
      }

      [TestMethod]
      public async Task GetKeyFigures4()
      {
         string id = "3811";
         KerncijferRecord record = await _client.PostalCodes.GetKeyFigures(id);

         Assert.IsNotNull(record, "Response is empty.");
         Assert.IsTrue(record.MetaData.TotalAttributes == 37);

         await Task.Delay(API_QUOTA_DELAY);
      }

      #endregion

      #region PC6

      [TestMethod]
      public async Task GetPostalCode6Record()
      {
         string id = "3811CJ";
         PostalCode6Record record = await _client.PostalCodes.GetPostalCodeRecord<PostalCode6Record>(id);

         Assert.IsNotNull(record, "Response is empty.");

         PostalCode6 pc = record.PostalCode[0];
         Assert.IsTrue(pc.Location.Gemeente == "Amersfoort" && pc.Location.Geometry.Centroid == "POINT (155155.51254284632 463159.828901163)");

         await Task.Delay(API_QUOTA_DELAY);
      }

      [TestMethod]
      public async Task GetPostalCode6Neighbours()
      {
         string id = "3069BS";
         bool includeSource = false;
         PostalCode6Record record = await _client.PostalCodes.GetPostalCodeNeighbours<PostalCode6Record>(id, includeSource);

         Assert.IsNotNull(record, "Response is empty.");
         Assert.IsTrue(record.PostalCode.Count == 7);

         List<string> expectedIDs = new() { "3069BK", "3069BL", "3069BN", "3069BP", "3069BR", "3069BM", "3069BT" };
         Assert.IsTrue(record.PostalCode.All(pc => expectedIDs.Contains(pc.Id)));

         await Task.Delay(API_QUOTA_DELAY * 2);
      }

      [TestMethod]
      public async Task GetPostalCode6NeighboursWithSource()
      {
         string id = "3069BS";
         bool includeSource = true;
         PostalCode6Record record = await _client.PostalCodes.GetPostalCodeNeighbours<PostalCode6Record>(id, includeSource);

         Assert.IsNotNull(record, "Response is empty.");
         Assert.IsTrue(record.PostalCode.Count == 8);

         List<string> expectedIDs = new() { "3069BS", "3069BK", "3069BL", "3069BN", "3069BP", "3069BR", "3069BM", "3069BT" };
         Assert.IsTrue(record.PostalCode.All(pc => expectedIDs.Contains(pc.Id)));

         await Task.Delay(API_QUOTA_DELAY * 2);
      }

      [TestMethod]
      public async Task GetPostalCode6ByGeometry()
      {
         string wkt = "LINESTRING(109935 561725, 110341 564040, 111430 565908)";
         PostalCode6Record record = await _client.PostalCodes.GetPostalCodeByGeometry<PostalCode6Record>(wkt);

         Assert.IsNotNull(record, "Response is empty.");
         Assert.IsTrue(record.PostalCode.Count == 3);

         List<string> expectedIDs = new() { "1791PB", "1796AZ", "1797RT" };
         Assert.IsTrue(record.PostalCode.All(pc => expectedIDs.Contains(pc.Id)));

         await Task.Delay(API_QUOTA_DELAY * 2);
      }

      [TestMethod]
      public async Task GetPostalCode6ByGeometryWithBuffer()
      {
         string wkt = "LINESTRING(109935 561725, 110341 564040, 111430 565908)";
         int buffer = 750;    // meters, since CS of WKT is 28992.
         PostalCode6Record record = await _client.PostalCodes.GetPostalCodeByGeometry<PostalCode6Record>(wkt, buffer);

         Assert.IsNotNull(record, "Response is empty.");
         Assert.IsTrue(record.PostalCode.Count == 6);

         List<string> expectedIDs = new() { "1791PB", "1796AZ", "1797RT", "1791NT", "1796MV", "1791PE" };
         Assert.IsTrue(record.PostalCode.All(pc => expectedIDs.Contains(pc.Id)));

         await Task.Delay(API_QUOTA_DELAY * 2);
      }

      [TestMethod]
      public async Task GetPostalCode6ByGeometryWithBufferAndDifferentTargetEpsg()
      {
         string wkt = "POINT(121843 487293)";
         int buffer = 50;   // meters, since CS of WKT is 28992.
         PostalCode6Record record = await _client.PostalCodes.GetPostalCodeByGeometry<PostalCode6Record>(wkt, buffer, CoordinateSystem.RDNew, CoordinateSystem.WGS84);

         Assert.IsNotNull(record, "Response is empty.");
         Assert.IsTrue(record.PostalCode.Count == 12);

         List<string> expectedIDs = new() { "1011MA", "1011JV", "1011JT", "1011JS", "1011JR", "1011JP", "1011HB", "1011ME", "1011GD", "1012CR", "1012CS", "1012CW" };
         Assert.IsTrue(record.PostalCode.All(pc => expectedIDs.Contains(pc.Id)));
         Assert.IsTrue(record.PostalCode[1].Location.Geometry.Centroid == "POINT (4.899542319809452 52.37146607902681)");

         await Task.Delay(API_QUOTA_DELAY * 2);
      }

      [TestMethod]
      public async Task GetPostalCode6ByArea()
      {
         int gemeenteId = 513;
         string expectedGemeente = "Gouda";

         int wijkId = 51309;
         string expectedWijk = "Westergouwe";

         int buurtId = 5130904;
         string expectedBuurt = "Tuinenbuurt";

         string expectedPostalCode = "2809RA";

         PostalCode6Record record = await _client.PostalCodes.GetPostalCodeByArea<PostalCode6Record>(gemeenteId, wijkId, buurtId, CoordinateSystem.WGS84);

         Assert.IsNotNull(record, "Response is empty.");

         PostalCode6 pc = record.PostalCode[0];
         Assert.IsTrue(pc.Id == expectedPostalCode);
         Assert.IsTrue(pc.Location.Gemeente == expectedGemeente);
         Assert.IsTrue(pc.Location.Wijk == expectedWijk);
         Assert.IsTrue(pc.Location.Buurt == expectedBuurt);

         await Task.Delay(API_QUOTA_DELAY);
      }

      [TestMethod]
      public async Task GetKeyFigures6()
      {
         string id = "3811BB";
         KerncijferRecord record = await _client.PostalCodes.GetKeyFigures(id);

         Assert.IsNotNull(record, "Response is empty.");
         Assert.IsTrue(record.MetaData.TotalAttributes == 35);

         await Task.Delay(API_QUOTA_DELAY);
      }

      #endregion

      #region GWB

      [TestMethod]
      public async Task GetGemeenteId()
      {
         GWBRecord record = await _client.PostalCodes.AreaHelper.GetGemeenten();

         Assert.IsNotNull(record, "Response is empty.");
         Assert.IsTrue(record.MetaData.TotalRecords == 345);

         string gemeenteNaam = "Amersfoort";
         int gemeenteIdAmersfoort = record.RecordSet.Where(g => g.Naam == gemeenteNaam).SingleOrDefault().ID;
         Assert.IsTrue(gemeenteIdAmersfoort == 307);
         
         await Task.Delay(API_QUOTA_DELAY);
      }

      [TestMethod]
      public async Task GetWijkId()
      {
         int gemeenteId = 307;
         GWBRecord record = await _client.PostalCodes.AreaHelper.GetWijken(gemeenteId);

         Assert.IsNotNull(record, "Response is empty.");
         Assert.IsTrue(record.MetaData.TotalRecords == 33);

         string wijknaam = "Stadskern";
         int expectedWijkIdStadskern = 30701;

         int wijkIdStadskern = record.RecordSet.Where(wijk => wijk.Naam == wijknaam).SingleOrDefault().ID;
         Assert.IsTrue(wijkIdStadskern == expectedWijkIdStadskern);

         await Task.Delay(API_QUOTA_DELAY);
      }

      [TestMethod]
      public async Task GetBuurtId()
      {
         int wijkId = 30701;
         GWBRecord record = await _client.PostalCodes.AreaHelper.GetBuurten(wijkId);

         Assert.IsNotNull(record, "Response is empty.");
         Assert.IsTrue(record.MetaData.TotalRecords == 9);

         string buurtnaam = "Hof";
         int expectedBuurtIdHof = 3070100;

         int buurtIdHof = record.RecordSet.Where(buurt => buurt.Naam == buurtnaam).SingleOrDefault().ID;
         Assert.IsTrue(buurtIdHof == expectedBuurtIdHof);

         await Task.Delay(API_QUOTA_DELAY);
      }

      #endregion
   }
}
