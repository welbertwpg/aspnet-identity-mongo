﻿namespace MongoDB.Identity
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Security.Claims;
	using MongoDB.Bson;
	using MongoDB.Bson.Serialization.Attributes;
    using Microsoft.AspNetCore.Identity;

    public class MongoIdentityUser
	{
		public MongoIdentityUser()
		{
			Id = ObjectId.GenerateNewId().ToString();
			Roles = new List<string>();
			Logins = new List<MongoIdentityUserLogin>();
			Claims = new List<MongoIdentityUserClaim>();
			Tokens = new List<MongoIdentityUserToken>();
		}

		[BsonRepresentation(BsonType.ObjectId)]
		public virtual string Id { get; set; }

		public virtual string UserName { get; set; }

		public virtual string NormalizedUserName { get; set; }

		/// <summary>
		///     A random value that must change whenever a users credentials change
		///     (password changed, login removed)
		/// </summary>
		public virtual string SecurityStamp { get; set; }

		public virtual string Email { get; set; }

		public virtual string NormalizedEmail { get; set; }

		public virtual bool EmailConfirmed { get; set; }

		public virtual string PhoneNumber { get; set; }

		public virtual bool PhoneNumberConfirmed { get; set; }

		public virtual bool TwoFactorEnabled { get; set; }

		public virtual DateTime? LockoutEndDateUtc { get; set; }

		public virtual bool LockoutEnabled { get; set; }

		public virtual int AccessFailedCount { get; set; }

		[BsonIgnoreIfNull]
		public virtual List<string> Roles { get; set; }

		public virtual void AddRole(string role)
		{
			Roles.Add(role);
		}

		public virtual void RemoveRole(string role)
		{
			Roles.Remove(role);
		}

		[BsonIgnoreIfNull]
		public virtual string PasswordHash { get; set; }

		[BsonIgnoreIfNull]
		public virtual List<MongoIdentityUserLogin> Logins { get; set; }

		public virtual void AddLogin(UserLoginInfo login)
		{
			Logins.Add(new MongoIdentityUserLogin(login));
		}

		public virtual void RemoveLogin(string loginProvider, string providerKey)
		{
			Logins.RemoveAll(l => l.LoginProvider == loginProvider && l.ProviderKey == providerKey);
		}

		public virtual bool HasPassword()
		{
			return false;
		}

		[BsonIgnoreIfNull]
		public virtual List<MongoIdentityUserClaim> Claims { get; set; }

		public virtual void AddClaim(Claim claim)
		{
			Claims.Add(new MongoIdentityUserClaim(claim));
		}

		public virtual void RemoveClaim(Claim claim)
		{
			Claims.RemoveAll(c => c.Type == claim.Type && c.Value == claim.Value);
		}

		public virtual void ReplaceClaim(Claim existingClaim, Claim newClaim)
		{
			var claimExists = Claims
				.Any(c => c.Type == existingClaim.Type && c.Value == existingClaim.Value);
			if (!claimExists)
			{
				// note: nothing to update, ignore, no need to throw
				return;
			}
			RemoveClaim(existingClaim);
			AddClaim(newClaim);
		}

		[BsonIgnoreIfNull]
		public virtual List<MongoIdentityUserToken> Tokens { get; set; }

		private MongoIdentityUserToken GetToken(string loginProider, string name)
			=> Tokens
				.FirstOrDefault(t => t.LoginProvider == loginProider && t.Name == name);

		public virtual void SetToken(string loginProider, string name, string value)
		{
			var existingToken = GetToken(loginProider, name);
			if (existingToken != null)
			{
				existingToken.Value = value;
				return;
			}

			Tokens.Add(new MongoIdentityUserToken
			{
				LoginProvider = loginProider,
				Name = name,
				Value = value
			});
		}

		public virtual string GetTokenValue(string loginProider, string name)
		{
			return GetToken(loginProider, name)?.Value;
		}

		public virtual void RemoveToken(string loginProvider, string name)
		{
			Tokens.RemoveAll(t => t.LoginProvider == loginProvider && t.Name == name);
		}

		public override string ToString() => UserName;
	}
}