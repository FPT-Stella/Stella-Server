
using FPTStella.Application.Common.Interfaces.Services;
using FPTStella.Contracts.DTOs.CLO_PLO_Mappings;
using Microsoft.AspNetCore.Mvc;

namespace FPTStella.API.Controllers
    {
        [Route("api/[controller]")]
        [ApiController]
        public class CLO_PLO_MappingController : BaseController
        {
            private readonly ICLO_PLO_MappingService _cloPlaMappingService;

            public CLO_PLO_MappingController(ICLO_PLO_MappingService cloPlaMappingService)
            {
                _cloPlaMappingService = cloPlaMappingService;
            }

            [HttpGet("clo/{ploId}")]
            public async Task<IActionResult> GetCloIdsByPloId(Guid ploId)
            {
                try
                {
                    var clos = await _cloPlaMappingService.GetCLOsWithDetailsByPloIdAsync(ploId);
                    return Ok(clos);
                }
                catch (Exception ex)
                {
                    return HandleException(ex);
                }
            }

            [HttpPost("batch")]
            public async Task<IActionResult> CreateMappingBatch([FromBody] CreateCLO_PLO_MappingBatchDto createMappingBatchDto)
            {
                try
                {
                    var createdMappings = await _cloPlaMappingService.CreateMappingBatchAsync(createMappingBatchDto);
                    return Ok(createdMappings);
                }
                catch (ArgumentException ex)
                {
                    return BadRequest(new { message = ex.Message });
                }
                catch (KeyNotFoundException ex)
                {
                    return NotFound(new { message = ex.Message });
                }
                catch (Exception ex)
                {
                    return HandleException(ex);
                }
            }

            [HttpPost]
            public async Task<IActionResult> CreateMapping([FromBody] CreateCLO_PLO_MappingDto createMappingDto)
            {
                try
                {
                    await _cloPlaMappingService.CreateMappingAsync(createMappingDto);
                    return NoContent();
                }
                catch (Exception ex)
                {
                    return HandleException(ex);
                }
            }

            [HttpGet("plo/{cloId}")]
            public async Task<IActionResult> GetPloIdsByCloId(Guid cloId)
            {
                try
                {
                    var ploIds = await _cloPlaMappingService.GetPloIdsByCloIdAsync(cloId);
                    return Ok(ploIds);
                }
                catch (Exception ex)
                {
                    return HandleException(ex);
                }
            }

            [HttpDelete("clo/{cloId}")]
            public async Task<IActionResult> DeleteMappingsByCloId(Guid cloId)
            {
                try
                {
                    await _cloPlaMappingService.DeleteMappingsByCloIdAsync(cloId);
                    return NoContent();
                }
                catch (Exception ex)
                {
                    return HandleException(ex);
                }
            }

            [HttpDelete("plo/{ploId}")]
            public async Task<IActionResult> DeleteMappingsByPloId(Guid ploId)
            {
                try
                {
                    await _cloPlaMappingService.DeleteMappingsByPloIdAsync(ploId);
                    return NoContent();
                }
                catch (Exception ex)
                {
                    return HandleException(ex);
                }
            }

            [HttpPut("batch-update")]
            public async Task<IActionResult> UpdateMappingsAsync([FromBody] UpdateCLO_PLO_MappingBatchDto updateMappingBatchDto)
            {
                if (updateMappingBatchDto == null || updateMappingBatchDto.Mappings == null || !updateMappingBatchDto.Mappings.Any())
                {
                    return BadRequest("No mappings provided for update.");
                }

                try
                {
                    var result = await _cloPlaMappingService.UpdateMappingsAsync(updateMappingBatchDto);
                    return Ok(result);
                }
                catch (ArgumentException ex)
                {
                    return BadRequest(ex.Message);
                }
                catch (Exception ex)
                {
                    return HandleException(ex);
                }
            }

            [HttpPatch("clo-plo-mapping")]
            public async Task<IActionResult> UpdateCloPloMapping([FromBody] PatchCloPloMappingDto dto)
            {
                try
                {
                    await _cloPlaMappingService.UpdateCloPlaMappingAsync(dto);
                    return NoContent();
                }
                catch (ArgumentException ex)
                {
                    return BadRequest(ex.Message);
                }
            }

            [HttpPatch("clo-mapping")]
            public async Task<IActionResult> UpdateCloMapping([FromBody] PatchCloMappingDto dto)
            {
                try
                {
                    await _cloPlaMappingService.UpdateCloMappingAsync(dto);
                    return NoContent();
                }
                catch (ArgumentException ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }
}
