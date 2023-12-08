namespace GISBlox.Services.SDK.Tests
{
   [TestClass]
   public class InfoTests
   {
      const string BASE_URL = "https://services.gisblox.com";

      [TestMethod]
      public async Task GetSubscriptionInfo()
      {
         // Get the service key from the test.runsettings file
         string serviceKey = Environment.GetEnvironmentVariable("ServiceKey");

         // Create the service client object
         using (var client = GISBloxClient.CreateClient(BASE_URL, serviceKey))
         {
            List<Subscription> subscriptions = await client.Info.GetSubscriptions();
            subscriptions.ForEach(sub => Console.WriteLine($"\r\nName: {sub.Name} \r\nDescription: {sub.Description} \r\nRegistration date: {sub.RegisterDate} Expiration date: {sub.ExpirationDate} Expired: {sub.Expired}"));
            
            Assert.IsTrue(subscriptions.Count != 0);
         }
      }

      [TestMethod]
      public async Task GetSubscriptionInfoCached()
      {
         // Get the service key from the test.runsettings file
         string serviceKey = Environment.GetEnvironmentVariable("ServiceKey");

         // Create the service client object
         using (var client = GISBloxClient.CreateClient(BASE_URL, serviceKey))
         {
            List<Subscription> subscriptions = await client.Info.GetSubscriptions();
            subscriptions.ForEach(sub => Console.WriteLine($"\r\nName: {sub.Name} \r\nDescription: {sub.Description} \r\nRegistration date: {sub.RegisterDate} Expiration date: {sub.ExpirationDate} Expired: {sub.Expired}"));

            List<Subscription> subscriptionsCached = await client.Info.GetSubscriptions();
            Assert.IsTrue(subscriptionsCached.Count == subscriptions.Count);

            Assert.IsTrue(subscriptions.Count != 0);
         }
      }
   }
}
