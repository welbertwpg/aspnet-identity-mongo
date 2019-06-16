using Microsoft.AspNetCore.Identity;

namespace MongoDB.Identity
{
	public class MongoIdentityUserLogin
	{
		public MongoIdentityUserLogin(string loginProvider, string providerKey, string providerDisplayName)
		{
			LoginProvider = loginProvider;
			ProviderDisplayName = providerDisplayName;
			ProviderKey = providerKey;
		}

		public MongoIdentityUserLogin(UserLoginInfo login)
		{
			LoginProvider = login.LoginProvider;
			ProviderDisplayName = login.ProviderDisplayName;
			ProviderKey = login.ProviderKey;
		}

		public string LoginProvider { get; set; }
		public string ProviderDisplayName { get; set; }
		public string ProviderKey { get; set; }

		public UserLoginInfo ToUserLoginInfo()
		{
			return new UserLoginInfo(LoginProvider, ProviderKey, ProviderDisplayName);
		}
	}
}