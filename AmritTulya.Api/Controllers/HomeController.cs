using AmritTulya.Api.Models;
using AmritTulya.EntityLayer;
using AmritTulya.Repository;
using Microsoft.Owin.Infrastructure;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Web.Http;

namespace AmritTulya.Api.Controllers
{
    public class HomeController : ApiController
    {
        [HttpPost]
        [AllowAnonymous]
        [Route("api/account/login")]
        public HttpResponseMessage Login([FromBody] UsersEntity login)
        {
            UserMasterRepository _repo = new UserMasterRepository();

            SimpleAuthorizationServerProvider sA = new SimpleAuthorizationServerProvider();
            if (_repo.ValidateUser(login.Username, login.Password) != null)
            {
                var identity = new ClaimsIdentity(Startup.OAuthBearerOptions.AuthenticationType);
                identity.AddClaim(new Claim(ClaimTypes.Name, login.Username));
                AuthenticationTicket ticket = new AuthenticationTicket(identity, new AuthenticationProperties());
                var currentUtc = new SystemClock().UtcNow;
                ticket.Properties.IssuedUtc = currentUtc;
                ticket.Properties.ExpiresUtc = currentUtc.Add(TimeSpan.FromMinutes(30));
                var token = Startup.OAuthBearerOptions.AccessTokenFormat.Protect(ticket);

                AccessTokenModel tm = new AccessTokenModel();
                tm.Username = login.Username;
                tm.token = token;
                tm.refresh_token = DateTime.UtcNow.AddMinutes(10).ToString();
                tm.expires_in = DateTime.UtcNow.AddDays(2);
                tm.token_type= ticket.ToString();

                ObjectContent<object> Content = new ObjectContent<object>(new
                {
                    UserName = login.Username,
                    AccessToken = token
                }, Configuration.Formatters.JsonFormatter);

                return Request.CreateResponse(HttpStatusCode.OK, tm);

            }
            return new HttpResponseMessage(HttpStatusCode.BadRequest);
        }
    }
}
