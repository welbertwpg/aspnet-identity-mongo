﻿namespace IntegrationTests
{
	using System.Linq;
	using System.Threading.Tasks;
	using MongoDB.Identity;
	using MongoDB.Bson;
	using NUnit.Framework;
    using MongoDB.Driver;
    using static NUnit.StaticExpect.Expectations;

    // todo low - validate all tests work
    [TestFixture]
	public class UserStoreTests : UserIntegrationTestsBase
	{
		[Test]
		public async Task Create_NewUser_Saves()
		{
			var userName = "name";
			var user = new MongoIdentityUser {UserName = userName};
			var manager = GetUserManager();

			await manager.CreateAsync(user);

			var savedUser = Users.Find(FilterDefinition<MongoIdentityUser>.Empty).Single();
			Expect(savedUser.UserName, Is.EqualTo(user.UserName));
		}

		[Test]
		public async Task FindByName_SavedUser_ReturnsUser()
		{
			var userName = "name";
			var user = new MongoIdentityUser {UserName = userName};
			var manager = GetUserManager();
			await manager.CreateAsync(user);

			var foundUser = await manager.FindByNameAsync(userName);

			Expect(foundUser, Is.Not.Null);
			Expect(foundUser.UserName, Is.EqualTo(userName));
		}

		[Test]
		public async Task FindByName_NoUser_ReturnsNull()
		{
			var manager = GetUserManager();

			var foundUser = await manager.FindByNameAsync("nouserbyname");

			Expect(foundUser, Is.Null);
		}

		[Test]
		public async Task FindById_SavedUser_ReturnsUser()
		{
			var userId = ObjectId.GenerateNewId().ToString();
			var user = new MongoIdentityUser {UserName = "name"};
			user.Id = userId;
			var manager = GetUserManager();
			await manager.CreateAsync(user);

			var foundUser = await manager.FindByIdAsync(userId);

			Expect(foundUser, Is.Not.Null);
			Expect(foundUser.Id, Is.EqualTo(userId));
		}

		[Test]
		public async Task FindById_NoUser_ReturnsNull()
		{
			var manager = GetUserManager();

			var foundUser = await manager.FindByIdAsync(ObjectId.GenerateNewId().ToString());

			Expect(foundUser, Is.Null);
		}

		[Test]
		public async Task FindById_IdIsNotAnObjectId_ReturnsNull()
		{
			var manager = GetUserManager();

			var foundUser = await manager.FindByIdAsync("notanobjectid");

			Expect(foundUser, Is.Null);
		}

		[Test]
		public async Task Delete_ExistingUser_Removes()
		{
			var user = new MongoIdentityUser {UserName = "name"};
			var manager = GetUserManager();
			await manager.CreateAsync(user);
			Expect(Users.Find(FilterDefinition<MongoIdentityUser>.Empty).ToList(), Is.Not.Empty);

			await manager.DeleteAsync(user);

			Expect(Users.Find(FilterDefinition<MongoIdentityUser>.Empty).ToList(), Is.Empty);
		}

		[Test]
		public async Task Update_ExistingUser_Updates()
		{
			var user = new MongoIdentityUser {UserName = "name"};
			var manager = GetUserManager();
			await manager.CreateAsync(user);
			var savedUser = await manager.FindByIdAsync(user.Id);
			savedUser.UserName = "newname";

			await manager.UpdateAsync(savedUser);

			var changedUser = Users.Find(FilterDefinition<MongoIdentityUser>.Empty).Single();
			Expect(changedUser, Is.Not.Null);
			Expect(changedUser.UserName, Is.EqualTo("newname"));
		}

		[Test]
		public async Task SimpleAccessorsAndGetters()
		{
			var user = new MongoIdentityUser
			{
				UserName = "username"
			};
			var manager = GetUserManager();
			await manager.CreateAsync(user);

			Expect(await manager.GetUserIdAsync(user), Is.EqualTo(user.Id));
			Expect(await manager.GetUserNameAsync(user), Is.EqualTo("username"));

			await manager.SetUserNameAsync(user, "newUserName");
			Expect(await manager.GetUserNameAsync(user), Is.EqualTo("newUserName"));
		}
	}
}