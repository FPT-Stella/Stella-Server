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

        [HttpGet("po/{poId}")]
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

        [HttpGet("plo/{ploId}")]
        public async Task<IActionResult> GetPoIdsByPloId(Guid ploId)
        {
            try
            {
                var poIds = await _poPloMappingService.GetPoIdsByPloIdAsync(ploId);
                return Ok(poIds);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

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
    }
}
