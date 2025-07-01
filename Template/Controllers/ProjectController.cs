using Application.Exceptions;
using Application.Interfaces.IProjectProporsal;
using Application.Request;
using Application.Response;
using Application.UseCases;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Solicitud_De_Proyecto.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectController(IProjectProposalService service, GetProjectById getProjectById, UpdateProjectProposal updateProjectProposal) : ControllerBase
    {
        private readonly IProjectProposalService _service = service;
        private readonly GetProjectById _getProjectById = getProjectById;
        private readonly UpdateProjectProposal _updateProjectProposal = updateProjectProposal;

        /// <param name="filters">Filtros: título, estado, solicitante, usuario aprobador.</param>
        /// <returns>Listado de proyectos resumido.</returns>
        /// <response code="200">Success</response>
        /// <response code="400">Bad Request</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ProjectShort>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiError), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<ProjectShort>>> GetProjects([FromQuery] ProjectFilterRequest filters)
        {
            try
            {
                var result = await _service.SearchProjects(filters);
                return Ok(result);
            }
            catch (ExceptionBadRequest ex)
            {
                return BadRequest(new ApiError { Message = ex.Message });
            }
        }

        /// <param name="request">Datos del proyecto.</param>
        /// <response code="201">Created.</response>
        /// <response code="400">Bad Request</response>
        [HttpPost]
        [ProducesResponseType(typeof(ProjectProposalResponseDetail), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiError), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] ProjectCreate request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiError { Message = "Datos del proyecto inválidos." });
            }

            try
            {
                var response = await _service.CreateProjectProposal(request);
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
            catch (Exception)
            {
                return StatusCode(500, new ApiError { Message = "Error interno del servidor." });
            }
        }

        [HttpPatch("{id}/decision")]
        [ProducesResponseType(typeof(ProjectProposalResponseDetail), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiError), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiError), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiError), StatusCodes.Status409Conflict)]
        public async Task<ActionResult<ProjectProposalResponseDetail>> TakeDecision(Guid id, [FromBody] DecisionStep request)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiError { Message = "El modelo de decisión es inválido." });

            if (request.Id <= 0 || request.User <= 0 || request.Status < 2 || request.Status > 4)
                return BadRequest(new ApiError { Message = "Los datos del usuario o estado son inválidos." });

            try
            {
                var result = await _service.TakeDecision(id, request);
                return Ok(result);
            }
            catch (ExceptionNotFound)
            {
                return NotFound(new ApiError { Message = "Proyecto no encontrado." });
            }
            catch (ExceptionBadRequest ex)
            {
                return BadRequest(new ApiError { Message = ex.Message });
            }
            catch (ValidationException ex)
            {
                return BadRequest(new ApiError { Message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new ApiError { Message = ex.Message });
            }
            catch (ExceptionConflict ex)
            {
                return Conflict(new ApiError { Message = ex.Message });
            }
        }

        [HttpPatch("{id}")]
        [ProducesResponseType(typeof(ProjectProposalResponseDetail), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiError), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ProjectProposalResponseDetail>> UpdateProject(Guid id, [FromBody] ProjectUpdate request)
        {
            if (!ModelState.IsValid ||
                string.IsNullOrWhiteSpace(request.Title) ||
                string.IsNullOrWhiteSpace(request.Description) ||
                request.Duration <= 0)
            {
                return BadRequest(new ApiError { Message = "Datos de actualización inválidos" });
            }

            try
            {
                var result = await _updateProjectProposal.ExecuteAsync(id, request);
                return Ok(result);
            }

            catch (ExceptionBadRequest ex)
            {
                return BadRequest(new ApiError { Message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ProjectProposalResponseDetail), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiError), StatusCodes.Status400BadRequest)]

        public async Task<IActionResult> GetProjectById(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest(new ApiError
                {
                    Message = "Datos de actualización inválidos"
                });
            }

            var result = await _getProjectById.ExecuteAsync(id);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }
    }
}