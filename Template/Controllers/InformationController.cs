using Application.Interfaces.IApprovalStatus;
using Application.Interfaces.IArea;
using Application.Interfaces.IProjectType;
using Application.Interfaces.IRole;
using Application.Interfaces.IUser;
using Microsoft.AspNetCore.Mvc;

namespace Solicitud_De_Proyecto.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InformationController(IUserService userService, IAreaService areaService, IProjectTypeService projectTypeService, IRoleService roleService, IApprovalStatusService approvalStatusService) : ControllerBase
    {
        private readonly IUserService userService = userService;
        private readonly IAreaService areaService = areaService;
        private readonly IProjectTypeService projectTypeService = projectTypeService;
        private readonly IRoleService roleService = roleService;
        private readonly IApprovalStatusService approvalStatusService = approvalStatusService;

        [HttpGet("Area")]
        public async Task<IActionResult> GetAreas()
        {
            var areas = await areaService.GetAllAsync();
            return Ok(areas);
        }
        [HttpGet("ProjectType")]
        public async Task<IActionResult> GetProjectTypes()
        {
            var projectTypes = await projectTypeService.GetAllAsync();
            return Ok(projectTypes);
        }
        [HttpGet("Role")]
        public async Task<IActionResult> GetRoles()
        {
            var roles = await roleService.GetAllAsync();
            return Ok(roles);
        }
        [HttpGet("ApprovalStatus")]
        public async Task<IActionResult> GetApprovalStatus()
        {
            var approvalStatus = await approvalStatusService.GetAllAsync();
            return Ok(approvalStatus);
        }
        [HttpGet("User")]
        public async Task<IActionResult> GetUsers()
        {
            var users = await userService.GetAllAsync();
            return Ok(users);
        }
    }
}

