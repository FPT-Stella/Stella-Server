using FPTStella.Application.Common.Interfaces.Services;
using FPTStella.Contracts.DTOs.CLOs;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FPTStella.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CLOController : BaseController
    {
        private readonly ICLOService _cloService;

        /// <summary>
        /// Initializes a new instance of the CLOController class.
        /// </summary>
        /// <param name="cloService">The CLO service</param>
        public CLOController(ICLOService cloService)
        {
            _cloService = cloService;
        }

        /// <summary>
        /// Creates a new CLO.
        /// </summary>
        /// <param name="createCloDto">DTO containing CLO creation data</param>
        /// <returns>The created CLO</returns>
        [HttpPost]
        public async Task<IActionResult> CreateCLO([FromBody] CreateCLODto createCloDto)
        {
            try
            {
                var clo = await _cloService.CreateCLOAsync(createCloDto);
                return CreatedAtAction(nameof(GetCLOById), new { id = clo.Id }, clo);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        /// <summary>
        /// Gets all active CLOs.
        /// </summary>
        /// <returns>List of all active CLOs</returns>
        [HttpGet]
        public async Task<IActionResult> GetAllCLOs()
        {
            try
            {
                var clos = await _cloService.GetAllCLOsAsync();
                return Ok(clos);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        /// <summary>
        /// Gets a CLO by ID.
        /// </summary>
        /// <param name="id">CLO ID</param>
        /// <returns>CLO with the specified ID</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCLOById(Guid id)
        {
            try
            {
                var clo = await _cloService.GetCLOByIdAsync(id);
                return Ok(clo);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        /// <summary>
        /// Gets all CLOs for a specific subject.
        /// </summary>
        /// <param name="subjectId">Subject ID</param>
        /// <returns>List of CLOs for the subject</returns>
        [HttpGet("subject/{subjectId}")]
        public async Task<IActionResult> GetCLOsBySubjectId(Guid subjectId)
        {
            try
            {
                var clos = await _cloService.GetCLOsBySubjectIdAsync(subjectId);
                return Ok(clos);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        /// <summary>
        /// Gets CLOs by a list of subject IDs.
        /// </summary>
        /// <param name="subjectIds">List of subject IDs</param>
        /// <returns>List of CLOs for the subjects</returns>
        [HttpGet("subjects")]
        public async Task<IActionResult> GetCLOsBySubjectIds([FromQuery] List<Guid> subjectIds)
        {
            try
            {
                var clos = await _cloService.GetCLOsBySubjectIdsAsync(subjectIds);
                return Ok(clos);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        /// <summary>
        /// Updates an existing CLO.
        /// </summary>
        /// <param name="id">CLO ID</param>
        /// <param name="updateCLODto">DTO containing update data</param>
        /// <returns>NoContent on successful update</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCLO(Guid id, [FromBody] UpdateCLODto updateCLODto)
        {
            try
            {
                await _cloService.UpdateCLOAsync(id, updateCLODto);
                return NoContent();
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        /// <summary>
        /// Deletes a CLO by ID.
        /// </summary>
        /// <param name="id">CLO ID</param>
        /// <returns>NoContent on successful deletion</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCLO(Guid id)
        {
            try
            {
                await _cloService.DeleteCLOAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        /// <summary>
        /// Deletes all CLOs for a specific subject.
        /// </summary>
        /// <param name="subjectId">Subject ID</param>
        /// <returns>NoContent on successful deletion</returns>
        [HttpDelete("subject/{subjectId}")]
        public async Task<IActionResult> DeleteCLOsBySubjectId(Guid subjectId)
        {
            try
            {
                await _cloService.DeleteCLOsBySubjectIdAsync(subjectId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }
    }
}