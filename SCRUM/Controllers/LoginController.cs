using AutoMapper;
using DevExpress.Xpo;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SCRUM.Helpers;
using SCRUM.Helpers.Authorization;
using SCRUM.Models;
using SCRUM.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SCRUM.Controllers
{
    [ApiController]
    [Route("api/")]
    public class LoginController : Controller
    {
        private readonly ILogger<RequirementsController> logger;
        private readonly LoginService loginService;
        private readonly IAuthorizationService authorizationService;

        public LoginController(ILogger<RequirementsController> log,
            UnitOfWork uow,
            IMapper mapper,
            IAuthorizationService authService)
        {
            logger = log;
            loginService = new LoginService(uow, mapper);
            authorizationService = authService;
        }
        #region Testing only
        
        //[HttpGet("getUsers")]
        //public async Task<IActionResult> GetUsers()
        //{
        //    try
        //    {
        //        return Ok(await loginService.GetUsers());
        //    }
        //    catch (Exception e)
        //    {
        //        return BadRequest(e.Message);
        //    }

        //}

        //[HttpGet("role")]
        //public async Task<IActionResult> GetRoles()
        //{
        //    try
        //    {
        //        var result = await loginService.GetRoles();

        //        return Ok(result);
        //    }
        //    catch (Exception e)
        //    {
        //        return BadRequest(e.Message);
        //    }
        //}

        //[HttpGet("roleaccess")]
        //public async Task<IActionResult> GetRoleAccess()
        //{
        //    try
        //    {
        //        var result = await loginService.GetRoleAccess();

        //        if (result != null)
        //        {
        //            return Ok(result);
        //        }
        //        return NoContent();
        //    }
        //    catch (Exception e)
        //    {
        //        return BadRequest(e.Message);

        //    }
        //}

        #endregion

        [HttpPost("signup")]
        public async Task<IActionResult> Register([FromBody] ProjectUserModel projectUser)
        {
            try
            {
                List<Claim> claims = (List<Claim>)await loginService.Register(projectUser);

                if (claims != null)
                {
                    var authProperties = new AuthenticationProperties
                    {
                        AllowRefresh = true
                    };

                    await HttpContext.SignInAsync(new ClaimsPrincipal(new ClaimsIdentity(
                        claims, CookieAuthenticationDefaults.AuthenticationScheme, "user", "role")), authProperties);

                    return Ok(projectUser.Name);
                }
                return StatusCode(409, "Nazwa " + projectUser.Name + " zajęta.");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

        }

        [HttpPost("signin")]
        public async Task<IActionResult> Login([FromBody] ProjectUserModel projectUser)
        {
            try
            {
                List<Claim> claims = (List<Claim>)await loginService.SignIn(projectUser);

                //var user = GetUser(User.Identity.Name); // = DOMENA\user

                if (claims == null)
                {
                    return NoContent();
                }

                var authProperties = new AuthenticationProperties
                {
                    AllowRefresh = true
                };

                await HttpContext.SignInAsync(new ClaimsPrincipal(new ClaimsIdentity(
                    claims, CookieAuthenticationDefaults.AuthenticationScheme, "user", "role")), authProperties);

                return Ok(projectUser.Name);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

        }

        [HttpGet("signout")]
        public async Task<IActionResult> Logout()
        {
            try
            {
                await HttpContext.SignOutAsync();
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("getMyProfile")]
        public IActionResult GetMyProfile()
        {
            try
            {
                var user = new
                {
                    name = User.Identity?.Name
                };

                if (user != null)
                {
                    return Ok(user);
                }
                return BadRequest();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("getMyPermissions")]
        public async Task<IActionResult> GetMyPermissions()
        {
            try
            {
                var user = new
                {
                    name = User.Identity?.Name
                };

                if (user != null)
                {
                    var permissions = await loginService.GetMyPermissions(user.name);
                    
                    return Ok(permissions);
                }
                return BadRequest();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        [HttpPost("role")]
        public async Task<IActionResult> AddRole([FromBody] RoleModel role)
        {
            try
            {
                var auth = await authorizationService.AuthorizeAsync(User, role.ProjectId, Permissions.AddRole);
                if (auth.Succeeded)
                {
                    await loginService.CreateRole(role);
                    return Ok();
                }
                else
                {
                    return Unauthorized();
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

    }
}
