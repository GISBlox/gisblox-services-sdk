namespace GISBlox.Services.SDK.Tests
{
   [TestClass]
   public class DataLakeTests
   {
      GISBloxClient _client;
      const string BASE_URL = "https://services.gisblox.com";
      
      const string TEST_GEOJSON_POINT = "{\"type\":\"Feature\",\"geometry\":{\"type\":\"Point\",\"coordinates\":[30,10,5]},\"properties\":{\"zValue\":23,\"name\":\"Single Point\"}}";

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

         Console.WriteLine($"Displaying the first 25 files in folder '{files.FolderId}':");
         files.Files.Take(25).ToList().ForEach(f => Console.WriteLine($"- {f.Name} - {f.Size} bytes - last modified: {f.ModifiedDate}"));
      }

      [TestMethod]
      public async Task UploadFile()
      {
         string fileName = "test_upload.json";
         string localPath = Path.Combine(Path.GetTempPath(), fileName);

         // Create a temporary file to upload         
         await File.WriteAllTextAsync(localPath, TEST_GEOJSON_POINT, CancellationToken.None);

         try
         {
            var result = await _client.DataLake.UploadFile(localPath, CancellationToken.None);
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
      public async Task UploadFileAsStream()
      {
         string fileName = "test_upload.json";
         string localPath = Path.Combine(Path.GetTempPath(), fileName);

         // Create a temporary file to upload
         await File.WriteAllTextAsync(localPath, TEST_GEOJSON_POINT, CancellationToken.None);

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
      public async Task DownloadFile()
      {
         string fileName = "test_upload.json";
         string localPath = Path.Combine(Path.GetTempPath(), fileName);

         bool success = await _client.DataLake.DownloadFile(fileName, localPath, CancellationToken.None);

         Assert.IsTrue(success);
         Assert.IsTrue(File.Exists(localPath));
      }

      [TestMethod]
      public async Task ExportFileAsGeoParquet()
      {
         string fileName = "test_upload.json";
         string localPath = Path.Combine(Path.GetTempPath(), "test.parquet");

         bool success = await _client.DataLake.ExportFileAsGeoParquet(fileName, localPath, CancellationToken.None);
         
         Assert.IsTrue(success);
         Assert.IsTrue(File.Exists(localPath));
      }

      [TestMethod]
      public async Task RoundTrip()
      {
         string fileName = "test_roundtrip.json";

         // Upload raw GeoJSON content as a file
         var result = await _client.DataLake.UploadFileData(TEST_GEOJSON_POINT, fileName, CancellationToken.None);
         Assert.IsTrue(result);

         // Wait to avoid exceeding API call quota
         await Task.Delay(1000, CancellationToken.None);

         // Verify we can download the same file content
         var downloadedContent = await _client.DataLake.DownloadFileData(fileName, CancellationToken.None);
         Assert.IsNotNull(downloadedContent);

         var contents = await _client.Conversion.ToWkt(downloadedContent, CancellationToken.None);

         Assert.HasCount(1, contents);
         Assert.AreEqual("POINT Z (30 10 5)", contents.First().Geometry);
         Assert.AreEqual("Single Point", contents.First().Properties[0]["name"]?.ToString());
         Assert.AreEqual("23", contents.First().Properties[0]["zValue"]?.ToString());

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
}
