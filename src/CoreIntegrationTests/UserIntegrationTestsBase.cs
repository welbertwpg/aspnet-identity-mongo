namespace IntegrationTests
{
	using System;
	using Microsoft.AspNetCore.Identity;
	using MongoDB.Identity;
	using Microsoft.Extensions.DependencyInjection;
	using MongoDB.Driver;
	using NUnit.Framework;

    public class UserIntegrationTestsBase
	{
		protected IMongoDatabase Database;
		protected IMongoCollection<MongoIdentityUser> Users;
		protected IMongoCollection<MongoIdentityRole> Roles;

		// note: for now we'll have interfaces to both the new and old apis for MongoDB, that way we don't have to update all the tests at once and risk introducing bugs
		protected IMongoDatabase DatabaseNewApi;
		protected IServiceProvider ServiceProvider;
		private readonly string _TestingConnectionString = $"mongodb://localhost:27017/{IdentityTesting}";
		private const string IdentityTesting = "identity-testing";

		[SetUp]
		public void BeforeEachTest()
		{
			var client = new MongoClient(_TestingConnectionString);

			Database = client.GetDatabase(IdentityTesting);
			Users = Database.GetCollection<MongoIdentityUser>("users");
			Roles = Database.GetCollection<MongoIdentityRole>("roles");

			DatabaseNewApi = client.GetDatabase(IdentityTesting);

			Database.DropCollection("users");
			Database.DropCollection("roles");

			ServiceProvider = CreateServiceProvider<MongoIdentityUser, MongoIdentityRole>();
		}

		protected UserManager<MongoIdentityUser> GetUserManager()
			=> ServiceProvider.GetService<UserManager<MongoIdentityUser>>();

		protected RoleManager<MongoIdentityRole> GetRoleManager()
			=> ServiceProvider.GetService<RoleManager<MongoIdentityRole>>();

		protected IServiceProvider CreateServiceProvider<TUser, TRole>(Action<IdentityOptions> optionsProvider = null)
			where TUser : MongoIdentityUser
			where TRole : MongoIdentityRole
		{
			var services = new ServiceCollection();
			optionsProvider = optionsProvider ?? (options => { });
			services.AddIdentity<TUser, TRole>(optionsProvider)
				.AddDefaultTokenProviders()
				.RegisterMongoStores<TUser, TRole>(_TestingConnectionString);

			services.AddLogging();

			return services.BuildServiceProvider();
		}
	}
}