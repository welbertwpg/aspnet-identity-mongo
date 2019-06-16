namespace IntegrationTests
{
	using MongoDB.Identity;
	using MongoDB.Bson;
	using NUnit.Framework;
	using Tests;
    using static NUnit.StaticExpect.Expectations;

    [TestFixture]
	public class IdentityUserTests : UserIntegrationTestsBase
	{
		[Test]
		public void Insert_NoId_SetsId()
		{
            var user = new MongoIdentityUser
            {
                Id = null
            };

            Users.InsertOne(user);

			Expect(user.Id, Is.Not.Null);
			var parsed = user.Id.SafeParseObjectId();
			Expect(parsed, Is.Not.Null);
			Expect(parsed, Is.Not.EqualTo(ObjectId.Empty));
		}
	}
}