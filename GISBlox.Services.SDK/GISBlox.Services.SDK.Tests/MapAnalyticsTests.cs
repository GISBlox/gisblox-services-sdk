namespace GISBlox.Services.SDK.Tests
{
   [TestClass]
   public class MapAnalyticsTests
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
         _client = GISBloxClient.CreateClient(BASE_URL, serviceKey, applicationName: "GISBlox.Services.SDK.Tests");
      }

      [TestCleanup]
      public void Cleanup()
      {
         _client.Dispose();
      }

      #endregion

      [TestMethod]
      public async Task ListTrackedMaps()
      {
         var record = await _client.MapAnalytics.ListTrackedMaps();

         Assert.IsNotNull(record);
         Assert.IsNotNull(record.Maps);
      }

      [TestMethod]
      public async Task GetMapEngagement()
      {
         string mapId = "7D6C2135-C878-4945-8622-60D3FE9B4BC3";
         AnalyticsDateRangeEnum dateRange = AnalyticsDateRangeEnum.ThreeWeeks;

         var record = await _client.MapAnalytics.GetMapEngagement(mapId, dateRange);

         Assert.IsNotNull(record, "Response is empty.");
         Assert.AreEqual(mapId, record.MapId);

         var engagements = record.Engagements;
         
         Assert.HasCount(21, engagements);
      }

      [TestMethod]
      public async Task GetMapEngagementWithEndDate()
      {
         DateTime endDate = DateTime.Parse("2025-11-21");
         string mapId = "7D6C2135-C878-4945-8622-60D3FE9B4BC3";
         AnalyticsDateRangeEnum dateRange = AnalyticsDateRangeEnum.ThreeWeeks;

         var record = await _client.MapAnalytics.GetMapEngagement(mapId, dateRange, endDate);

         Assert.IsNotNull(record, "Response is empty.");

         Assert.AreEqual(mapId, record.MapId);
         Assert.AreEqual(dateRange.ToString(), record.DateRange);
         Assert.AreEqual(endDate.Add(new TimeSpan(23, 59, 59)), record.EndDate);
         Assert.AreEqual(endDate.AddDays(-(int)dateRange + 1), record.StartDate);
         
         var engagements = record.Engagements;

         Assert.HasCount(21, engagements);
      }

      [TestMethod]
      public async Task GetMapKpis()
      {
         string mapId = "7D6C2135-C878-4945-8622-60D3FE9B4BC3";
         AnalyticsDateRangeEnum dateRange = AnalyticsDateRangeEnum.ThreeWeeks;

         var record = await _client.MapAnalytics.GetMapKpis(mapId, dateRange);

         Assert.IsNotNull(record, "Response is empty.");
         Assert.AreEqual(dateRange.ToString(), record.DateRange);

         var map = record.MapKpis[0];

         Assert.HasCount(4, map.Kpis);
         Assert.AreEqual(mapId, map.MapId);         

         Assert.IsTrue(map.Kpis.Any(k => k.Name == "Views"), "Views KPI missing.");
         Assert.IsTrue(map.Kpis.Any(k => k.Name == "Interactions"), "Interactions KPI missing.");
         Assert.IsTrue(map.Kpis.Any(k => k.Name == "ViewDuration"), "ViewDuration KPI missing.");
         Assert.IsTrue(map.Kpis.Any(k => k.Name == "ViewDurationAvg"), "ViewDurationAvg KPI missing.");
      }

      [TestMethod]
      public async Task GetMapKpisWithEndDate()
      {
         DateTime endDate = DateTime.Parse("2025-11-21");
         string mapId = "7D6C2135-C878-4945-8622-60D3FE9B4BC3";
         AnalyticsDateRangeEnum dateRange = AnalyticsDateRangeEnum.TwoWeeks;

         var record = await _client.MapAnalytics.GetMapKpis(mapId, dateRange, endDate);

         Assert.IsNotNull(record, "Response is empty.");
         Assert.AreEqual(dateRange.ToString(), record.DateRange);
         Assert.AreEqual(endDate.Add(new TimeSpan(23, 59, 59)), record.EndDate);
         Assert.AreEqual(endDate.AddDays(-(int)dateRange + 1), record.StartDate);

         var map = record.MapKpis[0];

         Assert.HasCount(4, map.Kpis);
         Assert.AreEqual(mapId, map.MapId);

         Assert.IsTrue(map.Kpis.Any(k => k.Name == "Views"), "Views KPI missing.");
         Assert.IsTrue(map.Kpis.Any(k => k.Name == "Interactions"), "Interactions KPI missing.");
         Assert.IsTrue(map.Kpis.Any(k => k.Name == "ViewDuration"), "ViewDuration KPI missing.");
         Assert.IsTrue(map.Kpis.Any(k => k.Name == "ViewDurationAvg"), "ViewDurationAvg KPI missing.");
      }

      [TestMethod]
      public async Task GetMapsKpis()
      {
         AnalyticsDateRangeEnum dateRange = AnalyticsDateRangeEnum.TwoWeeks;

         var record = await _client.MapAnalytics.GetMapsKpis(dateRange);

         Assert.IsNotNull(record);

         Assert.AreEqual(dateRange.ToString(), record.DateRange);
         Assert.IsTrue(record.MapKpis.Any(k => k.Kpis.Count == 4));

         foreach (var map in record.MapKpis)
         {
            Assert.IsTrue(map.Kpis.Any(k => k.Name == "Views"), "Views KPI missing.");
            Assert.IsTrue(map.Kpis.Any(k => k.Name == "Interactions"), "Interactions KPI missing.");
            Assert.IsTrue(map.Kpis.Any(k => k.Name == "ViewDuration"), "ViewDuration KPI missing.");
            Assert.IsTrue(map.Kpis.Any(k => k.Name == "ViewDurationAvg"), "ViewDurationAvg KPI missing.");
         }
      }

      [TestMethod]
      public async Task GetMapsKpisWithEndDate()
      {
         DateTime endDate = DateTime.Parse("2025-11-21");
         AnalyticsDateRangeEnum dateRange = AnalyticsDateRangeEnum.TwoWeeks;

         var record = await _client.MapAnalytics.GetMapsKpis(dateRange, endDate);

         Assert.IsNotNull(record);

         Assert.AreEqual(dateRange.ToString(), record.DateRange);
         Assert.AreEqual(endDate.Add(new TimeSpan(23, 59, 59)), record.EndDate);
         Assert.AreEqual(endDate.AddDays(-(int)dateRange + 1), record.StartDate);
         
         Assert.IsTrue(record.MapKpis.Any(k => k.Kpis.Count == 4));

         foreach (var map in record.MapKpis)
         {
            Assert.IsTrue(map.Kpis.Any(k => k.Name == "Views"), "Views KPI missing.");
            Assert.IsTrue(map.Kpis.Any(k => k.Name == "Interactions"), "Interactions KPI missing.");
            Assert.IsTrue(map.Kpis.Any(k => k.Name == "ViewDuration"), "ViewDuration KPI missing.");
            Assert.IsTrue(map.Kpis.Any(k => k.Name == "ViewDurationAvg"), "ViewDurationAvg KPI missing.");
         }
      }
   }
}
