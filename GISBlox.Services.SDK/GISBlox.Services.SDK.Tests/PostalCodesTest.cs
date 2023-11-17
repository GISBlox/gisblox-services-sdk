using GISBlox.Services.SDK.Models;

namespace GISBlox.Services.SDK.Tests
{
   [TestClass]
   public class PostalCodesTest
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

      #region PC4

      [TestMethod]
      public async Task GetPostalCode4Record()
      {
         string id = "3811";
         PostalCode4Record record = await _client.PostalCodes.GetPostalCode4Record(id);

         Assert.IsNotNull(record, "Response is empty.");
         
         PostalCode4 pc = record.PostalCode[0];
         Assert.IsTrue(pc.Location.Gemeente == "Amersfoort" && pc.Location.Geometry.Centroid == "POINT (155029.15793771204 463047.87594218826)");
      }

      [TestMethod]
      public async Task GetPostalCode4Neighbours()
      {
         string id = "3811";
         bool includeSource = false;
         PostalCode4Record record = await _client.PostalCodes.GetPostalCode4Neighbours(id, includeSource);

         Assert.IsNotNull(record, "Response is empty.");
         Assert.IsTrue(record.PostalCode.Count == 6);

         List<string> expectedIDs = new() { "3817", "3814", "3816", "3813", "3812", "3818" };
         Assert.IsTrue(record.PostalCode.All(pc => expectedIDs.Contains(pc.Id)));
      }

      [TestMethod]
      public async Task GetPostalCode4NeighboursWithSource()
      {
         string id = "3811";
         bool includeSource = true;
         PostalCode4Record record = await _client.PostalCodes.GetPostalCode4Neighbours(id, includeSource);

         Assert.IsNotNull(record, "Response is empty.");
         Assert.IsTrue(record.PostalCode.Count == 7);

         List<string> expectedIDs = new() { "3811", "3817", "3814", "3816", "3813", "3812", "3818" };
         Assert.IsTrue(record.PostalCode.All(pc => expectedIDs.Contains(pc.Id)));
      }

      [TestMethod]
      public async Task GetPostalCode4ByGeometry()
      {
         string wkt = "LINESTRING(109935 561725, 110341 564040, 111430 565908)";
         PostalCode4Record record = await _client.PostalCodes.GetPostalCode4ByGeometry(wkt);

         Assert.IsNotNull(record, "Response is empty.");
         Assert.IsTrue(record.PostalCode.Count == 3);

         List<string> expectedIDs = new() { "1791", "1796", "1797" };
         Assert.IsTrue(record.PostalCode.All(pc => expectedIDs.Contains(pc.Id)));
      }

      [TestMethod]
      public async Task GetPostalCode4ByGeometryWithBuffer()
      {
         string wkt = "LINESTRING(109935 561725, 110341 564040, 111430 565908)";
         int buffer = 5000;    // meters, since CS of WKT is 28992.
         PostalCode4Record record = await _client.PostalCodes.GetPostalCode4ByGeometry(wkt, buffer);

         Assert.IsNotNull(record, "Response is empty.");
         Assert.IsTrue(record.PostalCode.Count == 5);

         List<string> expectedIDs = new() { "1791", "1793", "1795", "1796", "1797" };
         Assert.IsTrue(record.PostalCode.All(pc => expectedIDs.Contains(pc.Id)));
      }

      [TestMethod]
      public async Task GetPostalCode4ByGeometryWithBufferAndDifferentTargetEpsg()
      {
         string wkt = "POINT(121843 487293)";
         int buffer = 200;   // meters, since CS of WKT is 28992.
         PostalCode4Record record = await _client.PostalCodes.GetPostalCode4ByGeometry(wkt, buffer, 28992, 4326);

         Assert.IsNotNull(record, "Response is empty.");
         Assert.IsTrue(record.PostalCode.Count == 2);

         List<string> expectedIDs = new() { "1011", "1012" };
         Assert.IsTrue(record.PostalCode.All(pc => expectedIDs.Contains(pc.Id)));
         Assert.IsTrue(record.PostalCode[1].Location.Geometry.Centroid == "POINT (4.905333126288754 52.371542282338666)");
      }

      [TestMethod]
      public async Task GetPostalCode4ByGW()
      {
         int gemeenteId = 513;
         int wijkId = 51309;
         PostalCode4Record record = await _client.PostalCodes.GetPostalCode4ByGW(gemeenteId, wijkId);

         Assert.IsNotNull(record, "Response is empty.");
        
         PostalCode4 pc = record.PostalCode[0];
         Assert.IsTrue(pc.Id == "2809" && pc.Location.Gemeente == "Gouda" && pc.Location.Wijken == "Westergouwe");
      }


      #endregion
   }
}
