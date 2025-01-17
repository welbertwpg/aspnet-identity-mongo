﻿namespace Tests
{
	using System.Security.Claims;
	using MongoDB.Identity;
	using NUnit.Framework;
    using static NUnit.StaticExpect.Expectations;

    [TestFixture]
	public class IdentityUserClaimTests
	{
		[Test]
		public void Create_FromClaim_SetsTypeAndValue()
		{
			var claim = new Claim("type", "value");

			var userClaim = new MongoIdentityUserClaim(claim);

			Expect(userClaim.Type, Is.EqualTo("type"));
			Expect(userClaim.Value, Is.EqualTo("value"));
		}

		[Test]
		public void ToSecurityClaim_SetsTypeAndValue()
		{
			var userClaim = new MongoIdentityUserClaim {Type = "t", Value = "v"};

			var claim = userClaim.ToSecurityClaim();

			Expect(claim.Type, Is.EqualTo("t"));
			Expect(claim.Value, Is.EqualTo("v"));
		}

		[Test]
		public void ReplaceClaim_NoExistingClaim_Ignores()
		{
			// note: per EF implemention - only existing claims are updated by looping through them so that impl ignores too
			var user = new MongoIdentityUser();
			var newClaim = new Claim("newType", "newValue");

			user.ReplaceClaim(newClaim, newClaim);

			Expect(user.Claims, Is.Empty);
		}

		[Test]
		public void ReplaceClaim_ExistingClaim_Replaces()
		{
			var user = new MongoIdentityUser();
			var firstClaim = new Claim("type", "value");
			user.AddClaim(firstClaim);
			var newClaim = new Claim("newType", "newValue");

			user.ReplaceClaim(firstClaim, newClaim);

			user.ExpectOnlyHasThisClaim(newClaim);
		}

		[Test]
		public void ReplaceClaim_ValueMatchesButTypeDoesNot_DoesNotReplace()
		{
			var user = new MongoIdentityUser();
			var firstClaim = new Claim("type", "sameValue");
			user.AddClaim(firstClaim);
			var newClaim = new Claim("newType", "sameValue");

			user.ReplaceClaim(new Claim("wrongType", "sameValue"), newClaim);

			user.ExpectOnlyHasThisClaim(firstClaim);
		}

		[Test]
		public void ReplaceClaim_TypeMatchesButValueDoesNot_DoesNotReplace()
		{
			var user = new MongoIdentityUser();
			var firstClaim = new Claim("sameType", "value");
			user.AddClaim(firstClaim);
			var newClaim = new Claim("sameType", "newValue");

			user.ReplaceClaim(new Claim("sameType", "wrongValue"), newClaim);

			user.ExpectOnlyHasThisClaim(firstClaim);
		}
	}
}