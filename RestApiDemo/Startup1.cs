using Microsoft.Owin;
using Owin;
using System;
using System.Threading.Tasks;
using Microsoft.Owin.Security.Jwt;
using Microsoft.Owin.Security;
using Microsoft.IdentityModel.Tokens;
using System.Text;

[assembly: OwinStartup(typeof(RestApiDemo.Startup1))]

namespace RestApiDemo
{
    public class Startup1
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseJwtBearerAuthentication(
                           new JwtBearerAuthenticationOptions
                           {
                               AuthenticationMode = AuthenticationMode.Active,
                               TokenValidationParameters = new TokenValidationParameters()
                               {
                                   ValidateIssuer = true,
                                   ValidateAudience = true,
                                   ValidateIssuerSigningKey = true,
                                   ValidIssuer = "http://mysite.com", //some string, normally web url,  
                                   ValidAudience = "http://mysite.com",
                                   IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("my_secret_key_12345"))
                               }
                           });
        }
    }
}
