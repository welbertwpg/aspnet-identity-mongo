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
	public class EnsureWeCanExtendIdentityRoleTests : UserIntegrationTestsBase
	{
		private RoleManager<ExtendedIdentityRole> _Manager;
		private ExtendedIdentityRole _Role;

		public class ExtendedIdentityRole : MongoIdentityRole
		{
			public string ExtendedField { get; set; }
		}

		[SetUp]
		public void BeforeEachTestAfterBase()
		{
			_Manager = CreateServiceProvider<MongoIdentityUser, ExtendedIdentityRole>()
				.GetService<RoleManager<ExtendedIdentityRole>>();
			_Role = new ExtendedIdentityRole
			{
				Name = "admin"
			};
		}

		[Test]
		public async Task Create_ExtendedRoleType_SavesExtraFields()
		{
			_Role.ExtendedField = "extendedField";

			await _Manager.CreateAsync(_Role);

			var savedRole = Roles.FindSync<ExtendedIdentityRole>(FilterDefinition<MongoIdentityRole>.Empty).Single();
			Expect(savedRole.ExtendedField, Is.EqualTo("extendedField"));
		}

		[Test]
		public async Task Create_ExtendedRoleType_ReadsExtraFields()
		{
			_Role.ExtendedField = "extendedField";

			await _Manager.CreateAsync(_Role);

			var savedRole = await _Manager.FindByIdAsync(_Role.Id);
			Expect(savedRole.ExtendedField, Is.EqualTo("extendedField"));
		}
	}
}