using Application.Exceptions;
using Application.Interfaces.IProjectProporsal;
using Application.Mappers;
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
        public async Task<ActionResult<IEnumerable<ProjectShort>>> GetProjects(
            [FromQuery] ProjectFilterRequest filters)
        {
            var result = await _service.SearchProjects(filters);
            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProjectCreate request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiError { Message = "Datos del proyecto inválidos." });
            }

            try
            {
                var proposal = await _service.CreateProjectProposal(
                    request.Title,
                    request.Description,
                    request.Area,
                    request.Type,
                    request.Amount,
                    request.Duration,
                    request.User
                );

                var response = ProjectMapper.ToDetailResponse(proposal);

                return CreatedAtAction(nameof(GetProjectById), new { id = response.Id }, response);
            }
            catch (Exception ex) when (ex is ExceptionBadRequest or ExceptionNotFound or ExceptionConflict)
            {

                return ex switch
                {
                    ExceptionBadRequest => BadRequest(new ApiError { Message = ex.Message }),
                    ExceptionNotFound => NotFound(new ApiError { Message = ex.Message }),
                    ExceptionConflict => Conflict(new ApiError { Message = ex.Message }),
                    _ => StatusCode(500, new ApiError { Message = "Error interno del servidor." })
                };
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiError { Message = "Error interno del servidor." });
            }
        }

        [HttpPatch("{id}/decision")]
        [ProducesResponseType(typeof(ProjectShort), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiError), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiError), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiError), StatusCodes.Status409Conflict)]
        public async Task<ActionResult<ProjectShort>> TakeDecision(Guid id, [FromBody] DecisionStep request)
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
            catch (ExceptionBadRequest ex)
            {
                return BadRequest(new ApiError { Message = ex.Message });
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
            if (result.Status?.Id == 3)
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