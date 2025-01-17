﻿namespace IntegrationTests
{
	using System.Linq;
	using System.Threading.Tasks;
	using Microsoft.AspNetCore.Identity;
	using MongoDB.Identity;
	using Microsoft.Extensions.DependencyInjection;
	using NUnit.Framework;
    using MongoDB.Driver;
    using static NUnit.StaticExpect.Expectations;

    // todo low - validate all tests work
    [TestFixture]
	public class UserPasswordStoreTests : UserIntegrationTestsBase
	{
		[Test]
		public async Task HasPassword_NoPassword_ReturnsFalse()
		{
			var user = new MongoIdentityUser {UserName = "bob"};
			var manager = GetUserManager();
			await manager.CreateAsync(user);

			var hasPassword = await manager.HasPasswordAsync(user);

			Expect(hasPassword, Is.False);
		}

		[Test]
		public async Task AddPassword_NewPassword_CanFindUserByPassword()
		{
			var user = new MongoIdentityUser {UserName = "bob"};
			var manager = CreateServiceProvider<MongoIdentityUser, MongoIdentityRole>(options =>
				{
					options.Password.RequireDigit = false;
					options.Password.RequireNonAlphanumeric = false;
					options.Password.RequireUppercase = false;
				})
				.GetService<UserManager<MongoIdentityUser>>();
			await manager.CreateAsync(user);

			var result = await manager.AddPasswordAsync(user, "testtest");
			Expect(result.Succeeded, Is.True);

			var userByName = await manager.FindByNameAsync("bob");
			Expect(userByName, Is.Not.Null);
			var passwordIsValid = await manager.CheckPasswordAsync(userByName, "testtest");
			Expect(passwordIsValid, Is.True);
		}

		[Test]
		public async Task RemovePassword_UserWithPassword_SetsPasswordNull()
		{
			var user = new MongoIdentityUser {UserName = "bob"};
			var manager = GetUserManager();
			await manager.CreateAsync(user);
			await manager.AddPasswordAsync(user, "testtest");

			await manager.RemovePasswordAsync(user);

			var savedUser = Users.Find(FilterDefinition<MongoIdentityUser>.Empty).Single();
			Expect(savedUser.PasswordHash, Is.Null);
		}
	}
}