namespace Tests
{
	using System.Linq;
	using System.Security.Claims;
	using MongoDB.Identity;
	using NUnit.Framework;
    using static NUnit.StaticExpect.Expectations;

    public static class TestExtensions
	{
		public static void ExpectOnlyHasThisClaim(this MongoIdentityUser user, Claim expectedClaim)
		{
			Expect(user.Claims.Count, Is.EqualTo(1));
			var actualClaim = user.Claims.Single();
			Expect(actualClaim.Type, Is.EqualTo(expectedClaim.Type));
			Expect(actualClaim.Value, Is.EqualTo(expectedClaim.Value));
		}
	}
}