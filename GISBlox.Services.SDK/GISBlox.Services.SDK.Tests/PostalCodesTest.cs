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

         bool ok = record.PostalCode.Where(pc => pc.Id == "3817").Count() == 1;
         
      }

      [TestMethod]
      public async Task GetPostalCode4NeighboursWithSource()
      {
         string id = "3811";
         bool includeSource = true;
         PostalCode4Record record = await _client.PostalCodes.GetPostalCode4Neighbours(id, includeSource);

         Assert.IsNotNull(record, "Response is empty.");
         Assert.IsTrue(record.PostalCode.Count == 7);

         bool ok = record.PostalCode.Where(pc => pc.Id == "3817").Count() == 1;

      }

      #endregion
   }
}
