namespace GISBlox.Services.SDK.Tests
{
    [TestClass]
    public class UrlShortenerTests
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
        public async Task ShortenDataUrl()
        {
            string longUrl = "https://www.gisblox.com/1";

            string shortUrl = await _client.UrlShortener.ShortenAsync(longUrl, CancellationToken.None);

            Assert.IsNotNull(shortUrl);
            Assert.IsTrue(Guid.TryParse(shortUrl.Split('/').Last(), out _));
        }        
    }
}
