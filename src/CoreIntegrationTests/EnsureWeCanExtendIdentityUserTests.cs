namespace IntegrationTests
{
	using System.Linq;
	using System.Threading.Tasks;
	using Microsoft.AspNetCore.Identity;
	using MongoDB.Identity;
	using Microsoft.Extensions.DependencyInjection;
	using NUnit.Framework;
    using MongoDB.Driver;
    using static NUnit.StaticExpect.Expectations;

    [TestFixture]
	public class EnsureWeCanExtendIdentityUserTests : UserIntegrationTestsBase
	{
		private UserManager<ExtendedIdentityUser> _Manager;
		private ExtendedIdentityUser _User;

		public class ExtendedIdentityUser : MongoIdentityUser
		{
			public string ExtendedField { get; set; }
		}

		[SetUp]
		public void BeforeEachTestAfterBase()
		{
			_Manager = CreateServiceProvider<ExtendedIdentityUser, MongoIdentityRole>()
				.GetService<UserManager<ExtendedIdentityUser>>();
			_User = new ExtendedIdentityUser
			{
				UserName = "bob"
			};
		}

		[Test]
		public async Task Create_ExtendedUserType_SavesExtraFields()
		{
			_User.ExtendedField = "extendedField";

			await _Manager.CreateAsync(_User);

			var savedUser = Users.FindSync<ExtendedIdentityUser>(FilterDefinition<MongoIdentityUser>.Empty).Single();
			Expect(savedUser.ExtendedField, Is.EqualTo("extendedField"));
		}

		[Test]
		public async Task Create_ExtendedUserType_ReadsExtraFields()
		{
			_User.ExtendedField = "extendedField";

			await _Manager.CreateAsync(_User);

			var savedUser = await _Manager.FindByIdAsync(_User.Id);
			Expect(savedUser.ExtendedField, Is.EqualTo("extendedField"));
		}
	}
}