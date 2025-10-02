namespace GISBlox.Services.SDK.Tests
{
    [TestClass]
    public class DataLakeTests
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
        public async Task ShowFolder()
        {
            var folder = await _client.DataLake.GetCustomerFolder(CancellationToken.None);
            
            Assert.IsNotNull(folder);
            Assert.IsNotNull(folder.FolderId);

            Console.WriteLine($"Customer Folder ID: {folder.FolderId}");
        }

        [TestMethod]
        public async Task ListFiles()
        {
            var files = await _client.DataLake.GetCustomerFiles(CancellationToken.None);
         
            Assert.IsNotNull(files);
            Assert.IsNotNull(files.Files);
            
            files.Files.ForEach(f => Console.WriteLine($"- {f}"));
        }        

        [TestMethod]
        public async Task DownloadFile()
        {
            string fileName = "route24.json";
            string localPath = Path.Combine(Path.GetTempPath(), fileName);
            
            var localFilePath = await _client.DataLake.DownloadFile(fileName, localPath, CancellationToken.None);
            
            Assert.IsNotNull(localFilePath);
            Assert.IsTrue(File.Exists(localFilePath));           
        }

        [TestMethod]
        public async Task UploadFile()
        {
            string fileName = "test_upload.json";
            string localPath = Path.Combine(Path.GetTempPath(), fileName);
            
            // Create a temporary file to upload
            await File.WriteAllTextAsync(localPath, "{ \"test\": \"data\" }");

            try
            {
                var result = await _client.DataLake.UploadFile(localPath, CancellationToken.None);
                Assert.IsTrue(result);
            }
            catch(Exception ex)
            {
                Assert.Fail($"Upload failed: {ex.Message}");
            }
            finally
            {
                // Clean up the temporary file
                if (File.Exists(localPath))
                {
                    File.Delete(localPath);
                }
            }
        }       

        [TestMethod]
        public async Task UploadFileAsStream()
        {
            string fileName = "test_upload.json";
            string localPath = Path.Combine(Path.GetTempPath(), fileName);

            // Create a temporary file to upload
            await File.WriteAllTextAsync(localPath, "{ \"test\": \"data\" }");

            try
            {
                using var stream = new FileStream(localPath, FileMode.Open, FileAccess.Read);
                
                var result = await _client.DataLake.UploadFile(stream, fileName, CancellationToken.None);             
                Assert.IsTrue(result);
            }
            catch (Exception ex)
            {
                Assert.Fail($"Upload failed: {ex.Message}");
            }
            finally
            {
                // Clean up the temporary file
                if (File.Exists(localPath))
                {
                    File.Delete(localPath);
                }
            }
        }

        [TestMethod]
        public async Task UploadAndDownloadFileData()
        {
            string fileName = "file_from_raw_json_data.json";            
            
            string jsonData = "{\"Id\":1,\"Name\":\"random object\",\"CreatedAt\":\"2025-10-02T12:32:14.9328506+02:00\"}";

            var result = await _client.DataLake.UploadFileData(jsonData, fileName, CancellationToken.None);

            Assert.IsTrue(result);

            // Wait to avoid exceeding API call quota
            await Task.Delay(API_QUOTA_DELAY);

            // Verify we can download the same file content
            var downloadedContent = await _client.DataLake.DownloadFileData(fileName, CancellationToken.None);
            Assert.IsNotNull(downloadedContent);

            // Verify we can deserialize the content
            var testDeserialization = JsonSerializer.Deserialize<RandomObject>(downloadedContent);
            Assert.IsTrue(testDeserialization != null);
            Assert.AreEqual(1, testDeserialization.Id);
            Assert.AreEqual("random object", testDeserialization.Name);
            Assert.AreEqual(DateTime.Parse("2025-10-02T12:32:14.9328506+02:00"), testDeserialization.CreatedAt);    

            // Clean up the test file
            await _client.DataLake.DeleteFile(fileName, CancellationToken.None);
        }

        [TestMethod]
        public async Task DeleteFile()
        {
            string fileName = "test_upload.json";

            try
            {
                var result = await _client.DataLake.DeleteFile(fileName, CancellationToken.None);
                Assert.IsTrue(result);
            }
            catch (Exception ex)
            {
                Assert.Fail($"Delete failed: {ex.Message}");
            }
        }       
    }

    public class RandomObject
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
