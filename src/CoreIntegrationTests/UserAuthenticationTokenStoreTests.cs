namespace CoreIntegrationTests
{
	using System.Threading.Tasks;
	using IntegrationTests;
	using MongoDB.Identity;
	using NUnit.Framework;
    using static NUnit.StaticExpect.Expectations;

    public class UserAuthenticationTokenStoreTests : UserIntegrationTestsBase
	{
		[Test]
		public async Task SetGetAndRemoveTokens()
		{
			// note: this is just an integration test, testing of IdentityUser behavior is in domain/unit tests
			var user = new MongoIdentityUser();
			var manager = GetUserManager();
			await manager.CreateAsync(user);

			await manager.SetAuthenticationTokenAsync(user, "loginProvider", "tokenName", "tokenValue");

			var tokenValue = await manager.GetAuthenticationTokenAsync(user, "loginProvider", "tokenName");
			Expect(tokenValue, Is.EqualTo("tokenValue"));

			await manager.RemoveAuthenticationTokenAsync(user, "loginProvider", "tokenName");
			var afterRemovedValue = await manager.GetAuthenticationTokenAsync(user, "loginProvider", "tokenName");
			Expect(afterRemovedValue, Is.Null);
		}
	}
}