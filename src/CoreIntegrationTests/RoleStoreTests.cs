﻿namespace IntegrationTests
{
	using System.Linq;
	using System.Threading.Tasks;
	using MongoDB.Identity;
	using MongoDB.Bson;
	using NUnit.Framework;
    using MongoDB.Driver;
    using static NUnit.StaticExpect.Expectations;

    [TestFixture]
	public class RoleStoreTests : UserIntegrationTestsBase
	{
		[Test]
		public async Task Create_NewRole_Saves()
		{
			var roleName = "admin";
			var role = new MongoIdentityRole(roleName);
			var manager = GetRoleManager();

			await manager.CreateAsync(role);

			var savedRole = Roles.Find(FilterDefinition<MongoIdentityRole>.Empty).Single();
			Expect(savedRole.Name, Is.EqualTo(roleName));
			Expect(savedRole.NormalizedName, Is.EqualTo("ADMIN"));
		}

		[Test]
		public async Task FindByName_SavedRole_ReturnsRole()
		{
			var roleName = "name";
			var role = new MongoIdentityRole {Name = roleName};
			var manager = GetRoleManager();
			await manager.CreateAsync(role);

			// note: also tests normalization as FindByName now uses normalization
			var foundRole = await manager.FindByNameAsync(roleName);

			Expect(foundRole, Is.Not.Null);
			Expect(foundRole.Name, Is.EqualTo(roleName));
		}

		[Test]
		public async Task FindById_SavedRole_ReturnsRole()
		{
			var roleId = ObjectId.GenerateNewId().ToString();
			var role = new MongoIdentityRole {Name = "name"};
			role.Id = roleId;
			var manager = GetRoleManager();
			await manager.CreateAsync(role);

			var foundRole = await manager.FindByIdAsync(roleId);

			Expect(foundRole, Is.Not.Null);
			Expect(foundRole.Id, Is.EqualTo(roleId));
		}

		[Test]
		public async Task Delete_ExistingRole_Removes()
		{
			var role = new MongoIdentityRole {Name = "name"};
			var manager = GetRoleManager();
			await manager.CreateAsync(role);
			Expect(Roles.Find(FilterDefinition<MongoIdentityRole>.Empty).ToList(), Is.Not.Empty);

			await manager.DeleteAsync(role);

			Expect(Roles.Find(FilterDefinition<MongoIdentityRole>.Empty).ToList(), Is.Empty);
		}

		[Test]
		public async Task Update_ExistingRole_Updates()
		{
			var role = new MongoIdentityRole {Name = "name"};
			var manager = GetRoleManager();
			await manager.CreateAsync(role);
			var savedRole = await manager.FindByIdAsync(role.Id);
			savedRole.Name = "newname";

			await manager.UpdateAsync(savedRole);

			var changedRole = Roles.Find(FilterDefinition<MongoIdentityRole>.Empty).Single();
			Expect(changedRole, Is.Not.Null);
			Expect(changedRole.Name, Is.EqualTo("newname"));
		}

		[Test]
		public async Task SimpleAccessorsAndGetters()
		{
			var role = new MongoIdentityRole
			{
				Name = "name"
			};
			var manager = GetRoleManager();
			await manager.CreateAsync(role);

			Expect(await manager.GetRoleIdAsync(role), Is.EqualTo(role.Id));
			Expect(await manager.GetRoleNameAsync(role), Is.EqualTo("name"));

			await manager.SetRoleNameAsync(role, "newName");
			Expect(await manager.GetRoleNameAsync(role), Is.EqualTo("newName"));
		}
	}
}