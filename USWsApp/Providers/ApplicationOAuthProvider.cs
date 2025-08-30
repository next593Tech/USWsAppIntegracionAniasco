using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using USWsLibrary.Models;
using Microsoft.Owin;
using System.Configuration;

namespace USWsApp.Providers
{
    public class ApplicationOAuthProvider : OAuthAuthorizationServerProvider
    {
        private readonly string _publicClientId;

        public ApplicationOAuthProvider(string publicClientId)
        {
            if (publicClientId == null)
            {
                throw new ArgumentNullException("publicClientId");
            }

            _publicClientId = publicClientId;
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {

            //context.OwinContext.Request.Headers.Add("Access-Control-Allow-Origin", new[] { "http://localhost:8207" });

            //var RemoteIpAddress = ((Microsoft.Owin.OwinRequest)context.OwinContext.Request).RemoteIpAddress.ToString();
            //var RemotePort = ((Microsoft.Owin.OwinRequest)context.OwinContext.Request).RemotePort.ToString();

            //var RemoteIpAddress = context.Request.RemoteIpAddress.ToString();
            //var RemotePort = context.Request.RemotePort.ToString();



            //if (RemoteIpAddress=="::1" && RemotePort == "8206")
            //{
            //    context.SetError("invalid_grant", "The user name or password is incorrect.");
            //    return;
            //}


            var userManager = context.OwinContext.GetUserManager<ApplicationUserManager>();

            ApplicationUser user = await userManager.FindAsync(context.UserName, context.Password);

            if (user == null)
            {
                context.SetError("invalid_grant", "The user name or password is incorrect.");
                return;
            }

            ClaimsIdentity oAuthIdentity = await user.GenerateUserIdentityAsync(userManager,
               OAuthDefaults.AuthenticationType);
            ClaimsIdentity cookiesIdentity = await user.GenerateUserIdentityAsync(userManager,
                CookieAuthenticationDefaults.AuthenticationType);

            AuthenticationProperties properties = CreateProperties(user.UserName);
            AuthenticationTicket ticket = new AuthenticationTicket(oAuthIdentity, properties);
            context.Validated(ticket);
            context.Request.Context.Authentication.SignIn(cookiesIdentity);
          

        }


         /// <summary>
        /// add the allow-origin header only if the origin domain is found on the     
        /// allowedOrigin list
        /// </summary>
        /// <param name="context"></param>
        private void SetCORSPolicy(IOwinContext context)
        {
            string allowedUrls = ConfigurationManager.AppSettings["allowedOrigins"];

            if (!String.IsNullOrWhiteSpace(allowedUrls))
            {
                var list = allowedUrls.Split(',');
                if (list.Length > 0)
                {

                    string origin = context.Request.Headers.Get("Origin");
                    var found = list.Where(item => item == origin).Any();
                    if (found){
                        context.Response.Headers.Add("Access-Control-Allow-Origin",
                                                     new string[] { origin });
                    }                    
                }
                
            }
            context.Response.Headers.Add("Access-Control-Allow-Headers", 
                                   new string[] {"Authorization", "Content-Type" });
            context.Response.Headers.Add("Access-Control-Allow-Methods", 
                                   new string[] {"OPTIONS", "POST" });

        }                
    

        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }

            return Task.FromResult<object>(null);
        }

        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            // Resource owner password credentials does not provide a client ID.
            if (context.ClientId == null)
            {
                context.Validated();
            }

            return Task.FromResult<object>(null);
        }

        public override Task ValidateClientRedirectUri(OAuthValidateClientRedirectUriContext context)
        {
            if (context.ClientId == _publicClientId)
            {
                Uri expectedRootUri = new Uri(context.Request.Uri, "/");

                if (expectedRootUri.AbsoluteUri == context.RedirectUri)
                {
                    context.Validated();
                }
            }

            return Task.FromResult<object>(null);
        }

        public static AuthenticationProperties CreateProperties(string userName)
        {
            IDictionary<string, string> data = new Dictionary<string, string>
            {
                { "userName", userName }
            };
            return new AuthenticationProperties(data);
        }
    }
}