using RestApiBL.Repo;
using System;

using System.Collections.Generic;

using System.Web.Http;
using RestApiBL.Models;

using System.Security.Claims;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;


namespace RestApiDemo.Controllers
{
    [Authorize]
    public class UserController : ApiController
    {
        UserRepo userRepo=new UserRepo();
        
        [HttpGet]
        public IHttpActionResult Get()
        {
            
           var userList= userRepo.getUsersList();
            return Ok(userList);
            
           
        }


        [HttpGet]
        public IHttpActionResult Get(int Id)
        { 

            var user= userRepo.getEmployeeById(Id);
            if (user !=null) 
            {
                return Ok(user);
            }
            else
            {
                return BadRequest("User not found!!");
            }


        }


        [HttpPost]
        public IHttpActionResult  Post([FromBody] tblUser user)
        {
            try
            {
                user.Password = userRepo.encrypt(user.Password);
                user.CreatedDate=DateTime.Now;
                userRepo.saveUser(user);
                return Ok("User added successfully");

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
           
        }

        [HttpPut]
        public IHttpActionResult Put([FromBody] tblUser user)
        {
            try
            {

               
                userRepo.saveUser(user);
                return Ok("User updated successfully");

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpDelete]
        public IHttpActionResult Delete(int Id)
        {
            try
            {

                userRepo.deleteUser(Id);
                return Ok("User deleted successfully");

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        
        [AllowAnonymous]
       
        public IHttpActionResult authenticate(string EmailId,string Password)
        {
            var user=userRepo.authenticateUser(EmailId,Password);
            if (user != null)
            {
               return Ok(generateToken(user));
            }
            else { 
                return BadRequest("Invalid User");
            }
        }

        [AllowAnonymous]
        public string generateToken(tblUser user)
        {
            string key = "my_secret_key_12345"; //Secret key which will be used later during validation    
            var issuer = "http://mysite.com";  //normally this will be your site URL    

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            //Create a List of Claims, Keep claims name short    
            var permClaims = new List<Claim>();
            permClaims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            permClaims.Add(new Claim("valid", "1"));
            permClaims.Add(new Claim("userid", Convert.ToString(user.Id)));
            permClaims.Add(new Claim("name", user.UserName));

            //Create Security Token object by giving required parameters    
            var token = new JwtSecurityToken(issuer, //Issure    
                            issuer,  //Audience    
                            permClaims,
                            expires: DateTime.Now.AddDays(1),
                            signingCredentials: credentials);
            var jwt_token = new JwtSecurityTokenHandler().WriteToken(token);
            return  jwt_token;
        }


    }
}