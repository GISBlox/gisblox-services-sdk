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
      public async Task GetGemeente()
      {
         int gemeenteId = 307;
         string gemeenteNaam = "Amersfoort";
         GWB gemeente = await _client.PostalCodes.AreaHelper.GetGemeente(gemeenteNaam, CancellationToken.None);

         Assert.IsNotNull(gemeente, "Response is empty.");
         Assert.AreEqual(gemeenteId, gemeente.ID);

         await Task.Delay(API_QUOTA_DELAY, CancellationToken.None);
      }

      [TestMethod]
      public async Task GetGemeenten()
      {
         GWBRecord record = await _client.PostalCodes.AreaHelper.GetGemeenten(CancellationToken.None);

         Assert.IsNotNull(record, "Response is empty.");
         Assert.AreEqual(345, record.MetaData.TotalRecords);

         await Task.Delay(API_QUOTA_DELAY, CancellationToken.None);
      }

      [TestMethod]
      public async Task GetWijkenByGemeenteId()
      {
         int gemeenteIdAmersfoort = 307;
         GWBRecord record = await _client.PostalCodes.AreaHelper.GetWijken(gemeenteIdAmersfoort, CancellationToken.None);

         Assert.IsNotNull(record, "Response is empty.");
         Assert.AreEqual(33, record.MetaData.TotalRecords);

         await Task.Delay(API_QUOTA_DELAY, CancellationToken.None);
      }

      [TestMethod]
      public async Task GetWijkenByGemeenteName()
      {
         string gemeente = "Amersfoort";
         GWBRecord record = await _client.PostalCodes.AreaHelper.GetWijken(gemeente, CancellationToken.None);

         Assert.IsNotNull(record, "Response is empty.");
         Assert.AreEqual(33, record.MetaData.TotalRecords);

         await Task.Delay(API_QUOTA_DELAY, CancellationToken.None);
      }

      [TestMethod]
      public async Task GetBuurtenByWijkId()
      {
         int wijkId = 30701;
         GWBRecord record = await _client.PostalCodes.AreaHelper.GetBuurten(wijkId, CancellationToken.None);

         Assert.IsNotNull(record, "Response is empty.");
         Assert.AreEqual(9, record.MetaData.TotalRecords);

         string buurtnaam = "Hof";
         int expectedBuurtIdHof = 3070100;

         int buurtIdHof = record.RecordSet.SingleOrDefault(buurt => buurt.Naam == buurtnaam).ID;
         Assert.AreEqual(expectedBuurtIdHof, buurtIdHof);

         await Task.Delay(API_QUOTA_DELAY, CancellationToken.None);
      }

      [TestMethod]
      public async Task GetBuurtenByGemeenteAndWijkIds()
      {
         int gemeenteIdAmersfoort = 307;
         int wijkIdStadskern = 30701;
         GWBRecord record = await _client.PostalCodes.AreaHelper.GetBuurten(gemeenteIdAmersfoort, wijkIdStadskern, CancellationToken.None);

         Assert.IsNotNull(record, "Response is empty.");
         Assert.AreEqual(9, record.MetaData.TotalRecords);

         string buurtnaam = "Hof";
         int expectedBuurtIdHof = 3070100;

         int buurtIdHof = record.RecordSet.SingleOrDefault(buurt => buurt.Naam == buurtnaam).ID;
         Assert.AreEqual(expectedBuurtIdHof, buurtIdHof);

         await Task.Delay(API_QUOTA_DELAY, CancellationToken.None);
      }

      [TestMethod]
      public async Task GetBuurtenByGemeenteAndWijkNames()
      {
         string gemeente = "Amersfoort";
         string wijk = "Stadskern";
         GWBRecord record = await _client.PostalCodes.AreaHelper.GetBuurten(gemeente, wijk, CancellationToken.None);

         Assert.IsNotNull(record, "Response is empty.");
         Assert.AreEqual(9, record.MetaData.TotalRecords);

         string buurtnaam = "Stadhuisplein";
         int expectedBuurtIdStadhuisplein = 3070107;

         int buurtIdHof = record.RecordSet.SingleOrDefault(buurt => buurt.Naam == buurtnaam).ID;
         Assert.AreEqual(expectedBuurtIdStadhuisplein, buurtIdHof);

         await Task.Delay(API_QUOTA_DELAY, CancellationToken.None);
      }

      [TestMethod]
      public async Task GetBuurtenByGemeenteAndWijkNamesCached()
      {
         string gemeente = "Amersfoort";
         string wijk = "Stadskern";
         GWBRecord record = await _client.PostalCodes.AreaHelper.GetBuurten(gemeente, wijk, CancellationToken.None);

         Assert.IsNotNull(record, "Response is empty.");
         Assert.AreEqual(9, record.MetaData.TotalRecords);

         string buurtnaam = "Stadhuisplein";
         int expectedBuurtIdStadhuisplein = 3070107;

         int buurtIdHof = record.RecordSet.SingleOrDefault(buurt => buurt.Naam == buurtnaam).ID;
         Assert.AreEqual(expectedBuurtIdStadhuisplein, buurtIdHof);

         GWBRecord recordCached = await _client.PostalCodes.AreaHelper.GetBuurten(gemeente, wijk, CancellationToken.None);
         Assert.IsNotNull(recordCached, "Response is empty.");
         Assert.AreEqual(9, recordCached.MetaData.TotalRecords);

         await Task.Delay(API_QUOTA_DELAY, CancellationToken.None);
      }
   }
}
