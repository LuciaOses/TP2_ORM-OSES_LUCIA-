using Application.Interfaces.IApprovalStatus;
using Application.Interfaces.IArea;
using Application.Interfaces.IProjectType;
using Application.Interfaces.IRole;
using Application.Interfaces.IUser;
using Application.Mappers;
using Application.UseCases;
using Infrastructure.Command;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Solicitud_De_Proyecto.Controllers
{
    [Route("api/")]
    [ApiController]
    public class InformationController(
        IUserService userService,
        IAreaService areaService,
        IProjectTypeService projectTypeService,
        IRoleService roleService,
        IApprovalStatusService approvalStatusService
    ) : ControllerBase
    {
        private readonly IUserService userService = userService;
        private readonly IAreaService areaService = areaService;
        private readonly IProjectTypeService projectTypeService = projectTypeService;
        private readonly IRoleService roleService = roleService;
        private readonly IApprovalStatusService approvalStatusService = approvalStatusService;

        /// <summary>
        /// Listado de Áreas
        /// </summary>
        [HttpGet("Area")]
        public async Task<IActionResult> GetAreas()
        {
            var areas = await areaService.GetAllAsync();
            return Ok(areas);
        }

        /// <summary>
        /// Listado de tipos de proyectos
        /// </summary>
        [HttpGet("ProjectType")]
        public async Task<IActionResult> GetAllProjectTypes()
        {
            var projectTypes = await projectTypeService.GetAllAsync();
            var projectTypesDto = ProjectTypeMapper.ToDtoList(projectTypes);
            return Ok(projectTypesDto);
        }

        /// <summary>
        /// Listado de roles de usuario
        /// </summary>
        [HttpGet ("Role")]
        public async Task<IActionResult> Get()
        {
            var roles = await roleService.GetAllAsync();
            var response = RoleMapper.ToDtoList(roles);
            return Ok(response);
        }

        /// <summary>
        /// Listado de estados para una solicitud de proyecto y pasos de aprobación
        /// </summary>
        [HttpGet("ApprovalStatus")]
        public async Task<IActionResult> GetAllStatuses()
        {
            var statuses = await approvalStatusService.GetAllAsync();
            var response = ApprovalStatusMapper.ToDtoList(statuses);
            return Ok(response);
        }

        /// <summary>
        /// Listado de usuarios
        /// </summary>
        [HttpGet("User")]
        public async Task<IActionResult> GetUsers()
        {
            var users = await userService.GetAllAsync();
            var usersDto = UserMapper.ToDtoList(users);
            return Ok(usersDto);
        }
    }
}