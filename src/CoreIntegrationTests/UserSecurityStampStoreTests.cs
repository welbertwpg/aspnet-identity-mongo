namespace IntegrationTests
{
	using System.Linq;
	using System.Threading.Tasks;
    using MongoDB.Driver;
    using MongoDB.Identity;
	using NUnit.Framework;
    using static NUnit.StaticExpect.Expectations;

    [TestFixture]
	public class UserSecurityStampStoreTests : UserIntegrationTestsBase
	{
		[Test]
		public async Task Create_NewUser_HasSecurityStamp()
		{
			var manager = GetUserManager();
			var user = new MongoIdentityUser {UserName = "bob"};

			await manager.CreateAsync(user);

			var savedUser = Users.Find(FilterDefinition<MongoIdentityUser>.Empty).Single();
			Expect(savedUser.SecurityStamp, Is.Not.Null);
		}

		[Test]
		public async Task GetSecurityStamp_NewUser_ReturnsStamp()
		{
			var manager = GetUserManager();
			var user = new MongoIdentityUser {UserName = "bob"};
			await manager.CreateAsync(user);

			var stamp = await manager.GetSecurityStampAsync(user);

			Expect(stamp, Is.Not.Null);
		}
	}
}