namespace GISBlox.Services.SDK.Tests
{
    [TestClass]
    public class UrlShortenerTests
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

        [TestMethod]
        public async Task ShortenDataUrl()
        {
            string longUrl = "https://www.gisblox.com/1";

            string shortUrl = await _client.UrlShortener.Shorten(longUrl, CancellationToken.None);

            Assert.IsNotNull(shortUrl);
            Assert.IsTrue(Guid.TryParse(shortUrl.Split('/').Last(), out _));
        }  
        
        [TestMethod]
        public async Task CreateGeoJsonDataUrl()
        {
            string geoJson = @"{
                ""type"": ""FeatureCollection"",
                ""features"": [
                    {
                        ""type"": ""Feature"",
                        ""geometry"": {
                            ""type"": ""Point"",
                            ""coordinates"": [102.0, 0.5]
                        },
                        ""properties"": {
                            ""prop0"": ""value0""
                        }
                    }
                ]
            }";
            
            string url = await _client.UrlShortener.CreateGeoJsonUrl(geoJson, CancellationToken.None);            
            Assert.IsNotNull(url);
            Assert.IsTrue(Guid.TryParse(url.Split('/').Last(), out _));

            string dataFromUrl = await FollowGeoJsonDataUrl(url);
            Assert.IsNotNull(dataFromUrl);
            
            Assert.AreEqual(
                System.Text.RegularExpressions.Regex.Replace(geoJson, @"\s+", ""),
                System.Text.RegularExpressions.Regex.Replace(dataFromUrl, @"\s+", ""));
        }

        public static async Task<string> FollowGeoJsonDataUrl(string url)
        {            
            using HttpClient httpClient = new();
            HttpResponseMessage response = await httpClient.GetAsync(url);
            return await response.Content.ReadAsStringAsync();            
        }
    }
}
