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
   }
}
