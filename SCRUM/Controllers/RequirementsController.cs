using AutoMapper;
using DevExpress.Xpo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using SCRUM.Helpers;
using SCRUM.Helpers.Authorization;
using SCRUM.Models;
using SCRUM.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCRUM.Controllers
{
    [ApiController]
    [Route("api/")]
    public class RequirementsController : Controller
    {
        private readonly ILogger<RequirementsController> logger;
        private readonly IAuthorizationService authorizationService;

        private readonly ProjectRequirementService projectRequirementService;
        private readonly ProjectService projectService;
        private readonly LoginService loginService;

        public RequirementsController(ILogger<RequirementsController> logg,
            UnitOfWork uow,
            IAuthorizationService authService,
            IMapper mapper)
        {
            logger = logg;
            authorizationService = authService;
            projectRequirementService = new ProjectRequirementService(uow, mapper);
            projectService = new ProjectService(uow, mapper);
            loginService = new LoginService(uow, mapper);
        }


        #region Project UNPROTECTED (only login required) Create and Read, PROTECTED Update and Delete

        [HttpGet("projects")]
        public async Task<IActionResult> GetAllProjects()
        {
            try
            {
                if(User.Identity?.Name != null)
                {
                    var result = await projectService.GetProjects();
                    return Ok(result);
                }
                return Unauthorized();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("projects/{tag}")]
        public async Task<IActionResult> GetProjectByTag(string tag)
        {
            try
            {
                if (User.Identity?.Name != null)
                {
                    var result = await projectService.GetByTag(tag);
                    return Ok(result);
                }
                return Unauthorized();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("projects")]
        public async Task<IActionResult> CreateProject([FromBody] ProjectModel projectModel)
        {
            try
            {
                if (User.Identity?.Name != null)
                {
                    var userOid = await GetUserOid();
                    await projectService.CreateProject(projectModel, userOid);
                    return Created("", null);
                }
                return Unauthorized();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("projects")]
        public async Task<IActionResult> EditProject([FromBody] ProjectModel projectModel)
        {
            try
            {
                var auth = await authorizationService.AuthorizeAsync(User, projectModel.Oid, Permissions.UpdateProject);
                if (auth.Succeeded)
                {
                    await projectService.EditProject(projectModel);
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

        [HttpDelete("projects/{projectId}")]
        public async Task<IActionResult> DeleteProject(int projectId)
        {
            try
            {
                var auth = await authorizationService.AuthorizeAsync(User, projectId, Permissions.DeleteProject);
                if(auth.Succeeded)
                {
                    await projectService.DeleteProject(projectId);
                    return Accepted();
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

        #endregion

        #region Pending Requirement

        [HttpGet("pendingrequirements/my/{projectId}")]
        public async Task<IActionResult> ReadMyPendingRequirements(int projectId)
        {
            try
            {
                var auth = await authorizationService.AuthorizeAsync(User, projectId, Permissions.ReadProposals);
                if (auth.Succeeded)
                {
                    var userId = await GetUserOid();
                    var result = await projectRequirementService.GetPendingBy(projectId, userId);
                    return Ok(result);
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

        [HttpGet("pendingrequirements/{projectId}")]
        public async Task<IActionResult> ReadAllPendingRequirements(int projectId)
        {
            try
            {
                var auth = await authorizationService.AuthorizeAsync(User, projectId, Permissions.ReadProposals);

                if (auth.Succeeded)
                {
                    var result = await projectRequirementService.GetPendingAll(projectId);
                    return Ok(result);
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

        [HttpPost("pendingrequirements")]
        public async Task<IActionResult> CreatePendingReq([FromBody] PendingRequirementModel pendingRequirement)
        {
            try
            {
                var auth = await authorizationService.AuthorizeAsync(User, pendingRequirement.ProjectId, Permissions.CreateProposal);
                if (auth.Succeeded)
                {
                    var userOid = await GetUserOid();
                    await projectRequirementService.CreatePendingRequirement(pendingRequirement, userOid);
                    return Created("", null);
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

        [HttpPut("pendingrequirements")]
        public async Task<IActionResult> EditPendingReq([FromBody] PendingRequirementModel pendingRequirement)
        {
            try
            {
                var userOid = await GetUserOid();
                AuthorizationResult auth;
                if (pendingRequirement.CreatorId == userOid)
                {
                    auth = await authorizationService.AuthorizeAsync(User, pendingRequirement.ProjectId, Permissions.UpdateMyProposal);
                }
                else
                {
                    auth = await authorizationService.AuthorizeAsync(User, pendingRequirement.ProjectId, Permissions.UpdateProposal);
                }
                
                if (auth.Succeeded)
                {
                    await projectRequirementService.EditPendingRequrement(pendingRequirement);
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

        [HttpDelete("pendingrequirements/{projectId}/{pendingId}")]
        public async Task<IActionResult> RejectePendingRequirement(int projectId, int pendingId)
        {
            try
            {
                var auth = await authorizationService.AuthorizeAsync(User, projectId, Permissions.DeclineProposal);
                if (auth.Succeeded)
                {
                    await projectRequirementService.RejectPendingRequirement(pendingId);
                    return Accepted();
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

        #endregion

        #region Requirement

        [HttpPost("requirements/{pendingId}")]
        public async Task<IActionResult> SubmitPendingRequirement(int pendingId, [FromBody] RequirementModel requirement)
        {
            try
            {
                var auth = await authorizationService.AuthorizeAsync(User, requirement.ProjectId, Permissions.AcceptProposal);
                if (auth.Succeeded)
                {
                    await projectRequirementService.CreateRequirement(pendingId, requirement);
                    return Ok();
                }
                else
                {
                    return Unauthorized();
                }
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("requirements/{projectId}")]
        public async Task<IActionResult> GetRequirements(int projectId)
        {
            try
            {
                var auth = await authorizationService.AuthorizeAsync(User, projectId, Permissions.ReadRequirements);
                if (auth.Succeeded)
                {
                    var result = await projectRequirementService.GetRequirements(projectId);
                    return Ok(result);
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

        [HttpPut("requirements")]
        public async Task<IActionResult> EditRequirement([FromBody] RequirementModel requirement)
        {
            try
            {
                var auth = await authorizationService.AuthorizeAsync(User, requirement.ProjectId, Permissions.UpdateNotStartedRequirement);
                if (auth.Succeeded)
                {
                    await projectRequirementService.EditRequirement(requirement);
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

        [HttpPut("requirements/started")]
        public async Task<IActionResult> EditStartedRequirement([FromBody] RequirementModel requirement)
        {
            try
            {
                var auth = await authorizationService.AuthorizeAsync(User, requirement.ProjectId, Permissions.UpdateRequirement);
                if (auth.Succeeded)
                {
                    await projectRequirementService.EditRequirement(requirement);
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

        [HttpPut("requirements/status")]
        public async Task<IActionResult> EditRequirementStatus([FromBody] RequirementModel requirement)
        {
            try
            {
                var auth = await authorizationService.AuthorizeAsync(User, requirement.ProjectId, Permissions.ChangeRequirementStatusAndTime);
                if (auth.Succeeded)
                {
                    await projectRequirementService.EditRequirementStatus(requirement);
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

        [HttpPut("requirements/order")]
        public async Task<IActionResult> ReorderRequirements([FromBody] List<RequirementModel> requirementList)
        {
            try
            {
                
                var auth = await authorizationService.AuthorizeAsync(User, requirementList.FirstOrDefault().ProjectId, Permissions.ReorderRequirements);
                if (auth.Succeeded)
                {
                    await projectRequirementService.ReorderRequirements(requirementList);
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

        #endregion

        #region Helper methods
        
        private async Task<int> GetUserOid()
        {
            try
            {
                var user = await loginService.GetUserOid(User.Identity?.Name);
                return user;
            }
            catch (AggregateException)
            {
                return 0;
            }
        }

        #endregion
    }
}
