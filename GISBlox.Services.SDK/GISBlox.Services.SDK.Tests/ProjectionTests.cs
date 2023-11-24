namespace GISBlox.Services.SDK.Tests
{
   [TestClass]
   public class ProjectionTests
   {
      GISBloxClient _client;
      const string BASE_URL = "https://services.gisblox.com";
      const int API_QUOTA_DELAY = 2500;  // Allows to run all tests together without exceeding API call quota

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

      #region ToRDS

      [TestMethod]
      public async Task ReprojectToRDS()
      {
         Coordinate coord = new(51.998929, 4.375587);
         RDPoint rdPoint = await _client.Projection.ToRDS(coord);

         Assert.IsNotNull(rdPoint, "Response is empty.");
         Assert.IsTrue(rdPoint.X == 85530 && rdPoint.Y == 446100);
      }

      [TestMethod]
      public async Task ReprojectToRDSComplete()
      {
         Coordinate coord = new(51.998929, 4.375587);
         Location location = await _client.Projection.ToRDSComplete(coord);

         Assert.IsNotNull(location, "Response is empty.");
         Assert.IsTrue(location.X == 85530 && location.Y == 446100 && location.Lat == 51.998929 && location.Lon == 4.375587);

         await Task.Delay(API_QUOTA_DELAY);
      }

      [TestMethod]
      public async Task ReprojectToRDSMultiple()
      {
         List<Coordinate> coords = new()
         {
            new Coordinate(51.998929, 4.375587),
            new Coordinate(53.1, 4.2),
            new Coordinate(53.11, 4.3)
         };
         List<RDPoint> rdPoints = await _client.Projection.ToRDS(coords);

         Assert.IsNotNull(rdPoints.Any(), "Response is empty.");
         Assert.IsTrue(rdPoints[0].X == 85530 && rdPoints[0].Y == 446100);
         Assert.IsTrue(rdPoints[1].X == 75483 && rdPoints[1].Y == 568787);
         Assert.IsTrue(rdPoints[2].X == 82197 && rdPoints[2].Y == 569794);

         await Task.Delay(API_QUOTA_DELAY * 2);
      }

      [TestMethod]
      public async Task ReprojectToRDSMultipleComplete()
      {
         List<Coordinate> coords = new()
         {
            new Coordinate(51.998929, 4.375587),
            new Coordinate(53.1, 4.2),
            new Coordinate(53.11, 4.3)
         };
         List<Location> loc = await _client.Projection.ToRDSComplete(coords);

         Assert.IsNotNull(loc.Any(), "Response is empty.");
         Assert.IsTrue(loc[0].X == 85530 && loc[0].Y == 446100 && loc[0].Lat == 51.998929 && loc[0].Lon == 4.375587);
         Assert.IsTrue(loc[1].X == 75483 && loc[1].Y == 568787 && loc[1].Lat == 53.1 && loc[1].Lon == 4.2);
         Assert.IsTrue(loc[2].X == 82197 && loc[2].Y == 569794 && loc[2].Lat == 53.11 && loc[2].Lon == 4.3);

         await Task.Delay(API_QUOTA_DELAY);
      }

      #endregion

      #region ToWGS84

      [TestMethod]
      public async Task ReprojectToWGS84()
      {
         RDPoint rdPoint = new(85530, 446100);
         Coordinate coord = await _client.Projection.ToWGS84(rdPoint, 6);        // Round the coordinate to 6 digits

         Assert.IsNotNull(coord, "Response is empty.");
         Assert.IsTrue(coord.Lat == 51.998927 && coord.Lon == 4.375584);
      }

      [TestMethod]
      public async Task ReprojectToWGS84Complete()
      {
         RDPoint rdPoint = new(85530, 446100);
         Location location = await _client.Projection.ToWGS84Complete(rdPoint);  // No rounding

         Assert.IsNotNull(location, "Response is empty.");
         Assert.IsTrue(location.Lat == 51.998927449317591 && location.Lon == 4.3755841993518345 && location.X == 85530 && location.Y == 446100);

         await Task.Delay(API_QUOTA_DELAY);
      }

      [TestMethod]
      public async Task ReprojectToWGS84Multiple()
      {
         List<RDPoint> rdPoints = new()
         {
            new RDPoint(100000, 555000),
            new RDPoint(1, 2),
            new RDPoint(111000, 550000)
         };
         List<Coordinate> coords = await _client.Projection.ToWGS84(rdPoints);   // No rounding

         Assert.IsNotNull(coords.Any(), "Response is empty.");
         Assert.IsTrue(coords[0].Lat == 52.9791861737104 && coords[0].Lon == 4.56833613045079);
         Assert.IsTrue(coords[1].Lat == 0 && coords[1].Lon == 0);
         Assert.IsTrue(coords[2].Lat == 52.93526683092437 && coords[2].Lon == 4.7327735938900535);

         await Task.Delay(API_QUOTA_DELAY * 2);
      }

      [TestMethod]
      public async Task ReprojectToWGS84MultipleComplete()
      {
         List<RDPoint> rdPoints = new()
         {
            new RDPoint(100000, 555000),
            new RDPoint(1, 2),
            new RDPoint(111000, 550000)
         };
         List<Location> coords = await _client.Projection.ToWGS84Complete(rdPoints, 5);   // Round the coordinates to 5 digits

         Assert.IsNotNull(coords.Any(), "Response is empty.");
         Assert.IsTrue(coords[0].Lat == 52.97919 && coords[0].Lon == 4.56834 && coords[0].X == 100000 && coords[0].Y == 555000);
         Assert.IsTrue(coords[1].Lat == 0 && coords[1].Lon == 0 && coords[1].X == -9999 && coords[1].Y == -9999);
         Assert.IsTrue(coords[2].Lat == 52.93527 && coords[2].Lon == 4.73277 && coords[2].X == 111000 && coords[2].Y == 550000);
      }

      #endregion
   }
}