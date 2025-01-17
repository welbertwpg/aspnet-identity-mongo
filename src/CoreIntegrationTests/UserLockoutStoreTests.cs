﻿namespace IntegrationTests
{
	using System;
	using System.Threading;
	using System.Threading.Tasks;
	using Microsoft.AspNetCore.Identity;
	using MongoDB.Identity;
	using Microsoft.Extensions.DependencyInjection;
	using NUnit.Framework;
    using static NUnit.StaticExpect.Expectations;

    // todo low - validate all tests work
    [TestFixture]
	public class UserLockoutStoreTests : UserIntegrationTestsBase
	{
		[Test]
		public async Task AccessFailed_IncrementsAccessFailedCount()
		{
			var manager = GetUserManagerWithThreeMaxAccessAttempts();
			var user = new MongoIdentityUser {UserName = "bob"};
			await manager.CreateAsync(user);

			await manager.AccessFailedAsync(user);

			Expect(await manager.GetAccessFailedCountAsync(user), Is.EqualTo(1));
		}

		private UserManager<MongoIdentityUser> GetUserManagerWithThreeMaxAccessAttempts()
		{
			return CreateServiceProvider<MongoIdentityUser, MongoIdentityRole>(options => options.Lockout.MaxFailedAccessAttempts = 3)
				.GetService<UserManager<MongoIdentityUser>>();
		}

		[Test]
		public void IncrementAccessFailedCount_ReturnsNewCount()
		{
			var store = new MongoUserStore<MongoIdentityUser>(null);
			var user = new MongoIdentityUser {UserName = "bob"};

			var count = store.IncrementAccessFailedCountAsync(user, default(CancellationToken));

			Expect(count.Result, Is.EqualTo(1));
		}

		[Test]
		public async Task ResetAccessFailed_AfterAnAccessFailed_SetsToZero()
		{
			var manager = GetUserManagerWithThreeMaxAccessAttempts();
			var user = new MongoIdentityUser {UserName = "bob"};
			await manager.CreateAsync(user);
			await manager.AccessFailedAsync(user);

			await manager.ResetAccessFailedCountAsync(user);

			Expect(await manager.GetAccessFailedCountAsync(user), Is.EqualTo(0));
		}

		[Test]
		public async Task AccessFailed_NotOverMaxFailures_NoLockoutEndDate()
		{
			var manager = GetUserManagerWithThreeMaxAccessAttempts();
			var user = new MongoIdentityUser {UserName = "bob"};
			await manager.CreateAsync(user);

			await manager.AccessFailedAsync(user);

			Expect(await manager.GetLockoutEndDateAsync(user), Is.Null);
		}

		[Test]
		public async Task AccessFailed_ExceedsMaxFailedAccessAttempts_LocksAccount()
		{
			var manager = CreateServiceProvider<MongoIdentityUser, MongoIdentityRole>(options =>
				{
					options.Lockout.MaxFailedAccessAttempts = 0;
					options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromHours(1);
				})
				.GetService<UserManager<MongoIdentityUser>>();

			var user = new MongoIdentityUser {UserName = "bob"};
			await manager.CreateAsync(user);

			await manager.AccessFailedAsync(user);

			var lockoutEndDate = await manager.GetLockoutEndDateAsync(user);
			Expect(lockoutEndDate?.Subtract(DateTime.UtcNow).TotalHours, Is.GreaterThan(0.9).And.LessThan(1.1));
		}

		[Test]
		public async Task SetLockoutEnabled()
		{
			var manager = GetUserManager();
			var user = new MongoIdentityUser {UserName = "bob"};
			await manager.CreateAsync(user);

			await manager.SetLockoutEnabledAsync(user, true);
			Expect(await manager.GetLockoutEnabledAsync(user));

			await manager.SetLockoutEnabledAsync(user, false);
			Expect(await manager.GetLockoutEnabledAsync(user), Is.False);
		}
	}
}