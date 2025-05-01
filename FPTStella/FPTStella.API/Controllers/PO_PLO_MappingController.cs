using FPTStella.Application.Common.Interfaces.Services;
using FPTStella.Contracts.DTOs.PO_PLO_Mappings;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FPTStella.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PO_PLO_MappingController : BaseController
    {
        private readonly IPO_PLO_MappingService _poPloMappingService;

        public PO_PLO_MappingController(IPO_PLO_MappingService poPloMappingService)
        {
            _poPloMappingService = poPloMappingService;
        }

        [HttpGet("po/{ploId}")]
        public async Task<IActionResult> GetPoIdsByPoId(Guid ploId)
        {
            try
            {
                var poIds = await _poPloMappingService.GetPOsWithNameByPloIdAsync(ploId);
                return Ok(poIds);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }
        // Add this new endpoint to support batch creation
        [HttpPost("batch")]
        public async Task<IActionResult> CreateMappingBatch([FromBody] CreatePO_PLO_MappingBatchDto createMappingBatchDto)
        {
            try
            {
                var createdMappings = await _poPloMappingService.CreateMappingBatchAsync(createMappingBatchDto);
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
        public async Task<IActionResult> CreateMapping([FromBody] CreatePO_PLO_MappingDto createMappingDto)
        {
            try
            {
                await _poPloMappingService.CreateMappingAsync(createMappingDto);
                return NoContent();
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpGet("plo/{poId}")]
        public async Task<IActionResult> GetPloIdsByPoId(Guid poId)
        {
            try
            {
                var ploIds = await _poPloMappingService.GetPloIdsByPoIdAsync(poId);
                return Ok(ploIds);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        //[HttpGet("po/{ploId}")]
        //public async Task<IActionResult> GetPoIdsByPloId(Guid ploId)
        //{
        //    try
        //    {
        //        var poIds = await _poPloMappingService.GetPoIdsByPloIdAsync(ploId);
        //        return Ok(poIds);
        //    }
        //    catch (Exception ex)
        //    {
        //        return HandleException(ex);
        //    }
        //}

        [HttpDelete("po/{poId}")]
        public async Task<IActionResult> DeleteMappingsByPoId(Guid poId)
        {
            try
            {
                await _poPloMappingService.DeleteMappingsByPoIdAsync(poId);
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
                await _poPloMappingService.DeleteMappingsByPloIdAsync(ploId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }
        [HttpPut("batch-update")]
        public async Task<IActionResult> UpdateMappingsAsync([FromBody] UpdatePO_PLO_MappingBatchDto updateMappingBatchDto)
        {
            if (updateMappingBatchDto == null || updateMappingBatchDto.Mappings == null || !updateMappingBatchDto.Mappings.Any())
            {
                return BadRequest("No mappings provided for update.");
            }

            try
            {
                var result = await _poPloMappingService.UpdateMappingsAsync(updateMappingBatchDto);

                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                // Log the error here if necessary
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpPatch("api/po-plo-mapping")]
        public async Task<IActionResult> UpdatePoPloMapping([FromBody] PatchPloMappingDto dto)
        {
            try
            {
                await _poPloMappingService.UpdatePoPloMappingAsync(dto);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPatch("po-mapping")]
        public async Task<IActionResult> UpdatePoMapping([FromBody] PatchPoMappingDto dto)
        {
            try
            {
                await _poPloMappingService.UpdatePoMappingAsync(dto);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
