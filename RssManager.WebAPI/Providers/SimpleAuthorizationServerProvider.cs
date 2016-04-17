using Microsoft.Owin.Security.OAuth;
using RssManager.Objects.BO;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RssManager.WebAPI.Providers
{
    public class SimpleAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        private RssManager.Interfaces.Repository.IUserRepository userRepository = null;

        public SimpleAuthorizationServerProvider(RssManager.Interfaces.Repository.IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });
            User user = null;
            try
            {
                user = this.userRepository.GetByUsername(context.UserName) as User;
            }
            catch
            {

            }
            if (user == null || !user.Password.Equals(Helper.GetHashedString(context.Password + user.Guid)))
            {
                context.SetError("invalid_grant", "The user name or password is incorrect.");
                return;
            }

            System.Security.Principal.IIdentity i = new MyIdentity(user.UserName, context.Options.AuthenticationType);
            var identity = new ClaimsIdentity(i, new List<Claim> {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Role, "user")
            });

            context.Validated(identity);
        }
    }
}