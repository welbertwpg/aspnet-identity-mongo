namespace MongoDB.Identity
{
	using MongoDB.Bson;
	using MongoDB.Bson.Serialization.Attributes;

	public class MongoIdentityRole
	{
		public MongoIdentityRole()
		{
			Id = ObjectId.GenerateNewId().ToString();
		}

		public MongoIdentityRole(string roleName) : this()
		{
			Name = roleName;
		}

		[BsonRepresentation(BsonType.ObjectId)]
		public string Id { get; set; }

		public string Name { get; set; }

		public string NormalizedName { get; set; }

		public override string ToString() => Name;
	}
}