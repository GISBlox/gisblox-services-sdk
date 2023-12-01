namespace GISBlox.Services.SDK.Tests
{
   [TestClass]
   public class PostalCodeAreaTest
   {
      GISBloxClient _client;
      const string BASE_URL = "https://services.gisblox.com";
      const int API_QUOTA_DELAY = 1000;  // Allows to run all tests together without exceeding API call quota

      #region Initialization and cleanup

      [TestInitialize]
      public void Init()
      {
         // Get the service key from the test.runsettings file
         string serviceKey = Environment.GetEnvironmentVariable("ServiceKey", EnvironmentVariableTarget.User);

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
      public async Task GetGemeente()
      {
         int gemeenteId = 307;
         string gemeenteNaam = "Amersfoort";
         GWB gemeente = await _client.PostalCodes.AreaHelper.GetGemeente(gemeenteNaam);

         Assert.IsNotNull(gemeente, "Response is empty.");
         Assert.IsTrue(gemeente.ID == gemeenteId);

         await Task.Delay(API_QUOTA_DELAY);
      }

      [TestMethod]
      public async Task GetGemeenten()
      {
         GWBRecord record = await _client.PostalCodes.AreaHelper.GetGemeenten();

         Assert.IsNotNull(record, "Response is empty.");
         Assert.IsTrue(record.MetaData.TotalRecords == 345);

         await Task.Delay(API_QUOTA_DELAY);
      }

      [TestMethod]
      public async Task GetWijkenByGemeenteId()
      {
         int gemeenteIdAmersfoort = 307;
         GWBRecord record = await _client.PostalCodes.AreaHelper.GetWijken(gemeenteIdAmersfoort);

         Assert.IsNotNull(record, "Response is empty.");
         Assert.IsTrue(record.MetaData.TotalRecords == 33);

         await Task.Delay(API_QUOTA_DELAY);
      }

      [TestMethod]
      public async Task GetWijkenByGemeenteName()
      {
         string gemeente = "Amersfoort";
         GWBRecord record = await _client.PostalCodes.AreaHelper.GetWijken(gemeente);

         Assert.IsNotNull(record, "Response is empty.");
         Assert.IsTrue(record.MetaData.TotalRecords == 33);

         await Task.Delay(API_QUOTA_DELAY);
      }

      [TestMethod]
      public async Task GetBuurtenByWijkId()
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

      [TestMethod]
      public async Task GetBuurtenByGemeenteAndWijkIds()
      {
         int gemeenteIdAmersfoort = 307;
         int wijkIdStadskern = 30701;
         GWBRecord record = await _client.PostalCodes.AreaHelper.GetBuurten(gemeenteIdAmersfoort, wijkIdStadskern);

         Assert.IsNotNull(record, "Response is empty.");
         Assert.IsTrue(record.MetaData.TotalRecords == 9);

         string buurtnaam = "Hof";
         int expectedBuurtIdHof = 3070100;

         int buurtIdHof = record.RecordSet.Where(buurt => buurt.Naam == buurtnaam).SingleOrDefault().ID;
         Assert.IsTrue(buurtIdHof == expectedBuurtIdHof);

         await Task.Delay(API_QUOTA_DELAY);
      }

      [TestMethod]
      public async Task GetBuurtenByGemeenteAndWijkNames()
      {
         string gemeente = "Amersfoort";
         string wijk = "Stadskern";
         GWBRecord record = await _client.PostalCodes.AreaHelper.GetBuurten(gemeente, wijk);

         Assert.IsNotNull(record, "Response is empty.");
         Assert.IsTrue(record.MetaData.TotalRecords == 9);

         string buurtnaam = "Stadhuisplein";
         int expectedBuurtIdStadhuisplein = 3070107;

         int buurtIdHof = record.RecordSet.Where(buurt => buurt.Naam == buurtnaam).SingleOrDefault().ID;
         Assert.IsTrue(buurtIdHof == expectedBuurtIdStadhuisplein);

         await Task.Delay(API_QUOTA_DELAY);
      }

      [TestMethod]
      public async Task GetBuurtenByGemeenteAndWijkNamesCached()
      {
         string gemeente = "Amersfoort";
         string wijk = "Stadskern";
         GWBRecord record = await _client.PostalCodes.AreaHelper.GetBuurten(gemeente, wijk);

         Assert.IsNotNull(record, "Response is empty.");
         Assert.IsTrue(record.MetaData.TotalRecords == 9);

         string buurtnaam = "Stadhuisplein";
         int expectedBuurtIdStadhuisplein = 3070107;

         int buurtIdHof = record.RecordSet.Where(buurt => buurt.Naam == buurtnaam).SingleOrDefault().ID;
         Assert.IsTrue(buurtIdHof == expectedBuurtIdStadhuisplein);

         GWBRecord recordCached = await _client.PostalCodes.AreaHelper.GetBuurten(gemeente, wijk);
         Assert.IsNotNull(recordCached, "Response is empty.");
         Assert.IsTrue(recordCached.MetaData.TotalRecords == 9);

         await Task.Delay(API_QUOTA_DELAY);
      }
   }
}
