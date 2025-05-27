using Application.Exceptions;
using Application.Interfaces.IProjectProporsal;
using Application.Request;
using Application.Response;
using Application.UseCases;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectProposalService _service;
        private readonly GetProjectById _getProjectById;
        private readonly UpdateProjectProposal _updateProjectProposal;

        public ProjectController(IProjectProposalService service, GetProjectById getProjectById, UpdateProjectProposal updateProjectProposal)
        {
            _service = service;
            _getProjectById = getProjectById;
            _updateProjectProposal = updateProjectProposal;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Project>>> GetProjects(
            [FromQuery] string? title,
            [FromQuery] int? status,
            [FromQuery] int? applicant,
            [FromQuery] int? approvalUser)
        {
            var filters = new ProjectFilterRequest
            {
                Title = title,
                Status = status,
                Applicant = applicant,
                ApprovalUser = approvalUser
            };

            var result = await _service.SearchProjects(filters);
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<Project>> Create([FromBody] ProjectCreate request)
        {
            if (!ModelState.IsValid || request.User <= 0 || request.Duration <= 0 || request.Type <= 0 || request.Area <= 0)
                return BadRequest(new ApiError { Message = "Datos del proyecto inválidos" });

            var exists = await _service.ExistingProject(request.Title);
            if (exists)
                return Conflict(new ApiError { Message = "Ya existe un proyecto creado con ese nombre" });

            var response = await _service.CreateProjectProposal(
                request.Title,
                request.Description,
                request.Area,
                request.Type,
                request.Amount,
                request.Duration,
                request.User);

            return Ok(response);
        }

        [HttpPatch("{id}/decision")]
        [ProducesResponseType(typeof(Project), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiError), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiError), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiError), StatusCodes.Status409Conflict)]
        public async Task<ActionResult<Project>> TakeDecision(Guid id, [FromBody] DecisionStep request)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiError { Message = "El modelo de decisión es inválido." });

            if (request.User <= 0 || request.Status < 2 || request.Status > 4)
                return BadRequest(new ApiError { Message = "Los datos del usuario o estado son inválidos." });

            try
            {
                var result = await _service.TakeDecision(id, request);

                if (result == null)
                    return NotFound(new ApiError { Message = "Proyecto no encontrado." });

                return Ok(result);
            }
            catch (ExceptionNotFound)
            {
                return NotFound(new ApiError { Message = "Proyecto no encontrado." });
            }
            catch (ValidationException ex)
            {
                return BadRequest(new ApiError { Message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new ApiError { Message = ex.Message });
            }
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateProject(Guid id, [FromBody] ProjectUpdate request)
        {
            if (!ModelState.IsValid || string.IsNullOrWhiteSpace(request.Title) || request.Duration <= 0)
                return BadRequest(new ApiError { Message = "Datos de actualización inválidos" });

            var result = await _updateProjectProposal.ExecuteAsync(id, request);
            if (result == null)
                return NotFound(new ApiError { Message = "Proyecto no encontrado" });

            if (result == ProjectDetailResponse.Conflict)
                return Conflict(new ApiError { Message = "El proyecto ya no se encuentra en un estado que permite modificaciones" });

            return Ok(result);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetProjectById(Guid id)
        {
            var result = await _getProjectById.ExecuteAsync(id);
            if (result == null) return NotFound();

            return Ok(result);
        }

    }
}