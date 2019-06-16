using MongoDB.Driver;

namespace MongoDB.Identity
{
    public static class IndexChecks
    {
        public static void EnsureUniqueIndexOnNormalizedUserName<TUser>(IMongoCollection<TUser> users)
            where TUser : MongoIdentityUser
        {
            var userName = Builders<TUser>.IndexKeys.Ascending(t => t.NormalizedUserName);
            var unique = new CreateIndexOptions { Unique = true };
            var model = new CreateIndexModel<TUser>(userName, unique);
            users.Indexes.CreateOneAsync(model);
        }

        public static void EnsureUniqueIndexOnNormalizedRoleName<TRole>(IMongoCollection<TRole> roles)
            where TRole : MongoIdentityRole
        {
            var roleName = Builders<TRole>.IndexKeys.Ascending(t => t.NormalizedName);
            var unique = new CreateIndexOptions { Unique = true };
            var model = new CreateIndexModel<TRole>(roleName, unique);
            roles.Indexes.CreateOneAsync(model);
        }

        public static void EnsureUniqueIndexOnNormalizedEmail<TUser>(IMongoCollection<TUser> users)
            where TUser : MongoIdentityUser
        {
            var email = Builders<TUser>.IndexKeys.Ascending(t => t.NormalizedEmail);
            var unique = new CreateIndexOptions { Unique = true };
            var model = new CreateIndexModel<TUser>(email, unique);
            users.Indexes.CreateOneAsync(model);
        }

        /// <summary>
        ///     ASP.NET Core Identity now searches on normalized fields so these indexes are no longer required, replace with
        ///     normalized checks.
        /// </summary>
        public static class OptionalIndexChecks
        {
            public static void EnsureUniqueIndexOnUserName<TUser>(IMongoCollection<TUser> users)
                where TUser : MongoIdentityUser
            {
                var userName = Builders<TUser>.IndexKeys.Ascending(t => t.UserName);
                var unique = new CreateIndexOptions { Unique = true };
                var model = new CreateIndexModel<TUser>(userName, unique);
                users.Indexes.CreateOneAsync(model);
            }

            public static void EnsureUniqueIndexOnRoleName<TRole>(IMongoCollection<TRole> roles)
                where TRole : MongoIdentityRole
            {
                var roleName = Builders<TRole>.IndexKeys.Ascending(t => t.Name);
                var unique = new CreateIndexOptions { Unique = true };
                var model = new CreateIndexModel<TRole>(roleName, unique);
                roles.Indexes.CreateOneAsync(model);
            }

            public static void EnsureUniqueIndexOnEmail<TUser>(IMongoCollection<TUser> users)
                where TUser : MongoIdentityUser
            {
                var email = Builders<TUser>.IndexKeys.Ascending(t => t.Email);
                var unique = new CreateIndexOptions { Unique = true };
                var model = new CreateIndexModel<TUser>(email, unique);
                users.Indexes.CreateOneAsync(model);
            }
        }
    }
}
