using BookStore.DTO;
using BookStore.DTOs;
using BookStore.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BookStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IConfiguration config;

        public AccountController(UserManager<ApplicationUser> userManager ,RoleManager<IdentityRole> roleManager ,IConfiguration config)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.config = config;
        }

        [HttpPost("/register")]
        public async Task<ActionResult> Register(RegisterDTO registerdata)
        {
          if(ModelState.IsValid)
            {
                ApplicationUser newuser = new()
                {
                    Email = registerdata.Email,
                    UserName = registerdata.UserName,
                };

                var identityResult = await userManager.CreateAsync(newuser, registerdata.Password);


                 if(identityResult.Succeeded)
                { const string userRole = "User";
                    var roleExists = await roleManager.RoleExistsAsync(userRole);

                    if (!roleExists)
                    {
                       await roleManager.CreateAsync(new IdentityRole(userRole));
                    }
                    await userManager.AddToRoleAsync(newuser, userRole);
                    return Ok("created User Successfully");
                }
                foreach (var item in identityResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, item.Description);
                }
            }

            return BadRequest(ModelState);


           
        }
        [HttpPost("/login")]
        public async Task<ActionResult> Login(LoginDTO loginData) 
        {
            if(ModelState.IsValid)
            {

                ApplicationUser user = await userManager.FindByNameAsync(loginData.UserName);

                if(user is not null)
                {
                  bool result = await userManager.CheckPasswordAsync(user, loginData.Password);

                    if(result)
                    {
                        string jti = Guid.NewGuid().ToString();
                        var userRoles = await userManager.GetRolesAsync(user);


                        List<Claim> claims = new List<Claim>();
                        claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id));
                        claims.Add(new Claim(ClaimTypes.Name, user.UserName));
                        claims.Add(new Claim(JwtRegisteredClaimNames.Jti, jti));
                        if (userRoles != null)
                        {
                            foreach (var role in userRoles)
                            {
                                claims.Add(new Claim(ClaimTypes.Role, role));
                            }
                        }

                        SymmetricSecurityKey securityKey = new(Encoding.UTF8.GetBytes(config["JWT:Key"]));
                        SigningCredentials signingCredentials = new(securityKey, SecurityAlgorithms.HmacSha256);

                        JwtSecurityToken Usertoken = new JwtSecurityToken(
                           issuer: config["JWT:Iss"],
                           audience: config["JWT:Aud"],
                           claims: claims,
                           expires: DateTime.Now.AddDays(2),
                           signingCredentials: signingCredentials
                            );

                        return Ok(new
                        {
                            expired = DateTime.Now.AddDays(2),
                            token = new JwtSecurityTokenHandler().WriteToken(Usertoken)
                        }); ;
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "UserName or password is worng");
                }

            }

            return BadRequest(ModelState);
        }

       



    }
}
