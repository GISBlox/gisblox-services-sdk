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
         _client = GISBloxClient.CreateClient(BASE_URL, serviceKey, applicationName: "GISBlox.Services.SDK.Tests");
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
         PostalCode4Record record = await _client.PostalCodes.GetPostalCodeRecord<PostalCode4Record>(id, CoordinateSystem.RDNew, CancellationToken.None);

         Assert.IsNotNull(record, "Response is empty.");
         
         PostalCode4 pc = record.PostalCode[0];
         Assert.IsTrue(pc.Location.Gemeente == "Amersfoort" && pc.Location.Geometry.Centroid == "POINT (155029 463048)");

         await Task.Delay(API_QUOTA_DELAY, CancellationToken.None);
      }

      [TestMethod]
      public async Task GetPostalCode4RecordCached()
      {
         string id = "3811";
         PostalCode4Record record = await _client.PostalCodes.GetPostalCodeRecord<PostalCode4Record>(id, CoordinateSystem.RDNew, CancellationToken.None);

         Assert.IsNotNull(record, "Response is empty.");

         PostalCode4 pc = record.PostalCode[0];
         Assert.IsTrue(pc.Location.Gemeente == "Amersfoort" && pc.Location.Geometry.Centroid == "POINT (155029 463048)");

         PostalCode4Record recordCached = await _client.PostalCodes.GetPostalCodeRecord<PostalCode4Record>(id, CoordinateSystem.RDNew, CancellationToken.None);
         
         Assert.IsNotNull(recordCached, "Response is empty.");
         Assert.AreEqual(recordCached.MetaData.Query, record.MetaData.Query);

         await Task.Delay(API_QUOTA_DELAY, CancellationToken.None);
      }

      [TestMethod]
      public async Task GetPostalCode4Neighbours()
      {
         string id = "3811";
         bool includeSource = false;
         PostalCode4Record record = await _client.PostalCodes.GetPostalCodeNeighbours<PostalCode4Record>(id, includeSource, CoordinateSystem.RDNew, CancellationToken.None);

         Assert.IsNotNull(record, "Response is empty.");
         Assert.HasCount(6, record.PostalCode);

         List<string> expectedIDs = ["3817", "3814", "3816", "3813", "3812", "3818"];
         Assert.IsTrue(record.PostalCode.All(pc => expectedIDs.Contains(pc.Id)));

         await Task.Delay(API_QUOTA_DELAY * 2, CancellationToken.None);
      }

      [TestMethod]
      public async Task GetPostalCode4NeighboursWithSource()
      {
         string id = "3811";
         bool includeSource = true;
         PostalCode4Record record = await _client.PostalCodes.GetPostalCodeNeighbours<PostalCode4Record>(id, includeSource, CoordinateSystem.RDNew, CancellationToken.None);

         Assert.IsNotNull(record, "Response is empty.");
         Assert.HasCount(7, record.PostalCode);

         List<string> expectedIDs = ["3811", "3817", "3814", "3816", "3813", "3812", "3818"];
         Assert.IsTrue(record.PostalCode.All(pc => expectedIDs.Contains(pc.Id)));

         await Task.Delay(API_QUOTA_DELAY * 2, CancellationToken.None);
      }

      [TestMethod]
      public async Task GetPostalCode4ByGeometry()
      {
         string wkt = "LINESTRING(109935 561725, 110341 564040, 111430 565908)";
         PostalCode4Record record = await _client.PostalCodes.GetPostalCodeByGeometry<PostalCode4Record>(wkt, 0, CoordinateSystem.RDNew, CoordinateSystem.RDNew, CancellationToken.None);

         Assert.IsNotNull(record, "Response is empty.");
         Assert.HasCount(3, record.PostalCode);

         List<string> expectedIDs = ["1791", "1796", "1797"];
         Assert.IsTrue(record.PostalCode.All(pc => expectedIDs.Contains(pc.Id)));

         await Task.Delay(API_QUOTA_DELAY * 2, CancellationToken.None);
      }

      [TestMethod]
      public async Task GetPostalCode4ByGeometryWithBuffer()
      {
         string wkt = "LINESTRING(109935 561725, 110341 564040, 111430 565908)";
         int buffer = 5000;    // meters, since CS of WKT is 28992.
         PostalCode4Record record = await _client.PostalCodes.GetPostalCodeByGeometry<PostalCode4Record>(wkt, buffer, CoordinateSystem.RDNew, CoordinateSystem.RDNew, CancellationToken.None);

         Assert.IsNotNull(record, "Response is empty.");
         Assert.HasCount(5, record.PostalCode);

         List<string> expectedIDs = ["1791", "1793", "1795", "1796", "1797"];
         Assert.IsTrue(record.PostalCode.All(pc => expectedIDs.Contains(pc.Id)));

         await Task.Delay(API_QUOTA_DELAY * 2, CancellationToken.None);
      }

      [TestMethod]
      public async Task GetPostalCode4ByGeometryWithBufferAndDifferentTargetEpsg()
      {
         string wkt = "POINT(121843 487293)";
         int buffer = 200;   // meters, since CS of WKT is 28992.
         PostalCode4Record record = await _client.PostalCodes.GetPostalCodeByGeometry<PostalCode4Record>(wkt, buffer, CoordinateSystem.RDNew, CoordinateSystem.WGS84, CancellationToken.None);

         Assert.IsNotNull(record, "Response is empty.");
         Assert.HasCount(2, record.PostalCode);

         List<string> expectedIDs = ["1011", "1012"];
         Assert.IsTrue(record.PostalCode.All(pc => expectedIDs.Contains(pc.Id)));

         PostalCode4 pc4 = record.PostalCode.Find(pc => pc.Id == "1011");
         Assert.AreEqual("POINT (4.905333126288753 52.37154228233867)", pc4.Location.Geometry.Centroid);

         await Task.Delay(API_QUOTA_DELAY * 2, CancellationToken.None);
      }

      [TestMethod]
      public async Task GetPostalCode4ByArea()
      {
         int gemeenteId = 513;
         string expectedGemeente = "Gouda";

         int wijkId = 51309;
         string expectedWijk = "Westergouwe";
                 
         string expectedPostalCode = "2809";

         PostalCode4Record record = await _client.PostalCodes.GetPostalCodeByArea<PostalCode4Record>(gemeenteId, wijkId, -1, CoordinateSystem.RDNew, CancellationToken.None);

         Assert.IsNotNull(record, "Response is empty.");
        
         PostalCode4 pc = record.PostalCode[0];
         Assert.AreEqual(expectedPostalCode, pc.Id);
         Assert.AreEqual(expectedGemeente, pc.Location.Gemeente);
         Assert.AreEqual(expectedWijk, pc.Location.Wijken);

         await Task.Delay(API_QUOTA_DELAY, CancellationToken.None);
      }

      [TestMethod]
      public async Task GetKeyFigures4()
      {
         string id = "3811";
         KerncijferRecord record = await _client.PostalCodes.GetKeyFigures(id, CancellationToken.None);

         Assert.IsNotNull(record, "Response is empty.");
         Assert.AreEqual(37, record.MetaData.TotalAttributes);

         await Task.Delay(API_QUOTA_DELAY, CancellationToken.None);
      }

      [TestMethod]
      public async Task RunAudienceAnalysisPC4NoWeights()
      {
         string postalCodes = "3068,3069";
         string preset = "Senioren";

         AudienceAnalysisRecord analysisRecord = await _client.PostalCodes.RunAudienceAnalysis(postalCodes, preset, CancellationToken.None);
         
         Assert.IsNotNull(analysisRecord, "Response is empty.");
         Assert.HasCount(2, analysisRecord.Results, "Unexpected number of items in the analysis result.");

         AudienceAnalysisResult result3068 = analysisRecord.Results.Find(result => result.PostalCode == "3068");
         double seniorenScore = GetDictionaryValue(result3068.TargetingScores, "SeniorenScore");
         Assert.AreEqual(0.554, seniorenScore, 0.001, "SeniorenScore insight is not as expected.");

         AudienceAnalysisResult result3069 = analysisRecord.Results.Find(result => result.PostalCode == "3069");
         double seniorenScore3069 = GetDictionaryValue(result3069.TargetingScores, "SeniorenScore");
         Assert.AreEqual(0.612, seniorenScore3069, 0.001, "SeniorenScore insight is not as expected.");

         await Task.Delay(API_QUOTA_DELAY, CancellationToken.None);
      }

      [TestMethod]
      public async Task RunAudienceAnalysisPC4Weights()
      {
         string postalCodes = "3068,3069";
         string preset = "Senioren";

         // Tweak the presets for seniors by assigning a higher weight to the 65Plus attribute and a lower weight to the Alleen attribute.         
         string weights = """{"Senior": { "65Plus": 0.4, "Alleen": 0.1 }}""";

         AudienceAnalysisRecord analysisRecord  = await _client.PostalCodes.RunAudienceAnalysis(postalCodes, preset, weights, CancellationToken.None);

         Assert.IsNotNull(analysisRecord, "Response is empty.");
         Assert.HasCount(2, analysisRecord.Results, "Unexpected number of items in the analysis result.");

         AudienceAnalysisResult result3068 = analysisRecord.Results.Find(result => result.PostalCode == "3068");
         double seniorenScore = GetDictionaryValue(result3068.TargetingScores, "SeniorenScore");
         Assert.AreEqual(0.439, seniorenScore, 0.001, "SeniorenScore insight is not as expected.");

         AudienceAnalysisResult result3069 = analysisRecord.Results.Find(result => result.PostalCode == "3069");
         double seniorenScore3069 = GetDictionaryValue(result3069.TargetingScores, "SeniorenScore");
         Assert.AreEqual(0.487, seniorenScore3069, 0.001, "SeniorenScore insight is not as expected.");

         await Task.Delay(API_QUOTA_DELAY, CancellationToken.None);
      }

      [TestMethod]
      public async Task RunAudienceAnalysisIllegalPostcodeCombination()
      {
         string postalCodes = "3068GE,3069";
         string preset = "Senioren";

         _ = Assert.ThrowsExactlyAsync<ArgumentException>(async () => await _client.PostalCodes.RunAudienceAnalysis(postalCodes, preset, CancellationToken.None), "Expected an ArgumentException for illegal postal codes.");
      }

      #endregion

      #region PC6

      [TestMethod]
      public async Task GetPostalCode6Record()
      {
         string id = "3811CJ";
         PostalCode6Record record = await _client.PostalCodes.GetPostalCodeRecord<PostalCode6Record>(id, CoordinateSystem.RDNew, CancellationToken.None);

         Assert.IsNotNull(record, "Response is empty.");

         PostalCode6 pc = record.PostalCode[0];
         Assert.IsTrue(pc.Location.Gemeente == "Amersfoort" && pc.Location.Geometry.Centroid == "POINT (155156 463160)");

         await Task.Delay(API_QUOTA_DELAY, CancellationToken.None);
      }

      [TestMethod]
      public async Task GetPostalCode6Neighbours()
      {
         string id = "3069BS";
         bool includeSource = false;
         PostalCode6Record record = await _client.PostalCodes.GetPostalCodeNeighbours<PostalCode6Record>(id, includeSource, CoordinateSystem.RDNew, CancellationToken.None);

         Assert.IsNotNull(record, "Response is empty.");
         Assert.HasCount(7, record.PostalCode);

         List<string> expectedIDs = ["3069BK", "3069BL", "3069BN", "3069BP", "3069BR", "3069BM", "3069BT"];
         Assert.IsTrue(record.PostalCode.All(pc => expectedIDs.Contains(pc.Id)));

         await Task.Delay(API_QUOTA_DELAY * 2, CancellationToken.None);
      }

      [TestMethod]
      public async Task GetPostalCode6NeighboursWithSource()
      {
         string id = "3069BS";
         bool includeSource = true;
         PostalCode6Record record = await _client.PostalCodes.GetPostalCodeNeighbours<PostalCode6Record>(id, includeSource, CoordinateSystem.RDNew, CancellationToken.None);

         Assert.IsNotNull(record, "Response is empty.");
         Assert.HasCount(8, record.PostalCode);

         List<string> expectedIDs = ["3069BS", "3069BK", "3069BL", "3069BN", "3069BP", "3069BR", "3069BM", "3069BT"];
         Assert.IsTrue(record.PostalCode.All(pc => expectedIDs.Contains(pc.Id)));

         await Task.Delay(API_QUOTA_DELAY * 2, CancellationToken.None);
      }

      [TestMethod]
      public async Task GetPostalCode6ByGeometry()
      {
         string wkt = "LINESTRING(109935 561725, 110341 564040, 111430 565908)";
         PostalCode6Record record = await _client.PostalCodes.GetPostalCodeByGeometry<PostalCode6Record>(wkt, 0, CoordinateSystem.RDNew, CoordinateSystem.RDNew, CancellationToken.None);

         Assert.IsNotNull(record, "Response is empty.");
         Assert.HasCount(3, record.PostalCode);

         List<string> expectedIDs = ["1791PB", "1796AZ", "1797RT"];
         Assert.IsTrue(record.PostalCode.All(pc => expectedIDs.Contains(pc.Id)));

         await Task.Delay(API_QUOTA_DELAY * 2, CancellationToken.None);
      }

      [TestMethod]
      public async Task GetPostalCode6ByGeometryWithBuffer()
      {
         string wkt = "LINESTRING(109935 561725, 110341 564040, 111430 565908)";
         int buffer = 750;    // meters, since CS of WKT is 28992.
         PostalCode6Record record = await _client.PostalCodes.GetPostalCodeByGeometry<PostalCode6Record>(wkt, buffer, CoordinateSystem.RDNew, CoordinateSystem.RDNew, CancellationToken.None);

         Assert.IsNotNull(record, "Response is empty.");
         Assert.HasCount(6, record.PostalCode);

         List<string> expectedIDs = ["1791PB", "1796AZ", "1797RT", "1791NT", "1796MV", "1791PE"];
         Assert.IsTrue(record.PostalCode.All(pc => expectedIDs.Contains(pc.Id)));

         await Task.Delay(API_QUOTA_DELAY * 2, CancellationToken.None);
      }

      [TestMethod]
      public async Task GetPostalCode6ByGeometryWithBufferAndDifferentTargetEpsg()
      {
         string wkt = "POINT(121843 487293)";
         int buffer = 50;   // meters, since CS of WKT is 28992.
         PostalCode6Record record = await _client.PostalCodes.GetPostalCodeByGeometry<PostalCode6Record>(wkt, buffer, CoordinateSystem.RDNew, CoordinateSystem.WGS84, CancellationToken.None);

         Assert.IsNotNull(record, "Response is empty.");
         Assert.HasCount(12, record.PostalCode);

         List<string> expectedIDs = ["1011MA", "1011JV", "1011JT", "1011JS", "1011JR", "1011JP", "1011HB", "1011ME", "1011GD", "1012CR", "1012CS", "1012CW"];
         Assert.IsTrue(record.PostalCode.All(pc => expectedIDs.Contains(pc.Id)));

         PostalCode6 pc6 = record.PostalCode.Find(pc => pc.Id == "1011HB");
         Assert.AreEqual("POINT (4.900588613654761 52.3709636802628)", pc6.Location.Geometry.Centroid);

         await Task.Delay(API_QUOTA_DELAY * 2, CancellationToken.None);
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

         PostalCode6Record record = await _client.PostalCodes.GetPostalCodeByArea<PostalCode6Record>(gemeenteId, wijkId, buurtId, CoordinateSystem.WGS84, CancellationToken.None);

         Assert.IsNotNull(record, "Response is empty.");

         PostalCode6 pc = record.PostalCode[0];
         Assert.AreEqual(expectedPostalCode, pc.Id);
         Assert.AreEqual(expectedGemeente, pc.Location.Gemeente);
         Assert.AreEqual(expectedWijk, pc.Location.Wijk);
         Assert.AreEqual(expectedBuurt, pc.Location.Buurt);

         await Task.Delay(API_QUOTA_DELAY, CancellationToken.None);
      }

      [TestMethod]
      public async Task GetKeyFigures6()
      {
         string id = "3811BB";
         KerncijferRecord record = await _client.PostalCodes.GetKeyFigures(id, CancellationToken.None);

         Assert.IsNotNull(record, "Response is empty.");
         Assert.AreEqual(35, record.MetaData.TotalAttributes);

         await Task.Delay(API_QUOTA_DELAY, CancellationToken.None);
      }

      [TestMethod]
      public async Task RunAudienceAnalysisPC6NoWeights()
      {
         string postalCodes = "3069KN, 3069KS,3069 LG";
         string preset = "Starters";

         AudienceAnalysisRecord analysisRecord = await _client.PostalCodes.RunAudienceAnalysis(postalCodes, preset, CancellationToken.None);

         Assert.IsNotNull(analysisRecord, "Response is empty.");
         Assert.HasCount(3, analysisRecord.Results, "Unexpected number of items in the analysis result.");

         AudienceAnalysisResult result3059KS = analysisRecord.Results.Find(result => result.PostalCode == "3069KS");

         double starterScore = GetDictionaryValue(result3059KS.TargetingScores, "StarterScore");
         Assert.AreEqual(0.048, starterScore, 0.001, "StarterScore insight is not as expected.");         

         double neutralStarterScore = GetDictionaryValue(result3059KS.NeutralScores, "StarterScore");
         Assert.AreEqual(0.040, neutralStarterScore, 0.001, "Neutral StarterScore insight is not as expected.");
         
         await Task.Delay(API_QUOTA_DELAY, CancellationToken.None);
      }      

      [TestMethod]
      public async Task RunAudienceAnalysisPC6Weights()
      {
         string postalCodes = "3069KN, 3069KS,3069 LG";
         string preset = "Starters";

         // Tweak the presets for starters by assigning a higher weight to the Young attribute.         
         string weights = """{"Starter": { "Young": 0.8 }}""";

         AudienceAnalysisRecord analysisRecord = await _client.PostalCodes.RunAudienceAnalysis(postalCodes, preset, weights, CancellationToken.None);

         Assert.IsNotNull(analysisRecord, "Response is empty.");
         Assert.HasCount(3, analysisRecord.Results, "Unexpected number of items in the analysis result.");

         AudienceAnalysisResult result3069LG = analysisRecord.Results.Find(result => result.PostalCode == "3069LG");

         double starterScore = GetDictionaryValue(result3069LG.TargetingScores, "StarterScore");
         Assert.AreEqual(0.112, starterScore, 0.001, "StarterScore insight is not as expected.");

         double neutralStarterScore = GetDictionaryValue(result3069LG.NeutralScores, "StarterScore");
         Assert.AreEqual(0.034, neutralStarterScore, 0.001, "Neutral StarterScore insight is not as expected.");

         await Task.Delay(API_QUOTA_DELAY, CancellationToken.None);
      }

      #endregion

      #region Helpers

      private static double GetDictionaryValue(Dictionary<string, object> dict, string key)
      {
         return dict.TryGetValue(key, out object value) == true
            ? value switch
            {
               JsonElement { ValueKind: JsonValueKind.Number } jsonNumber => jsonNumber.GetDouble(),
               JsonElement { ValueKind: JsonValueKind.String } jsonString when double.TryParse(jsonString.GetString(), out double parsed) => parsed,
               _ => Convert.ToDouble(value)
            } : 0;
      }

      #endregion
   }
}
