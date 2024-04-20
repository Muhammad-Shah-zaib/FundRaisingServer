using FundRaisingServer.Models.DTOs;
using FundRaisingServer.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace FundRaisingServer.Controllers
{
    [ApiController]
    [Route("/[controller]")]
    public class CasesController : ControllerBase
    {
        private readonly ICasesRepository _casesRepo;

        public CasesController(ICasesRepository casesRepo)
        {
            _casesRepo = casesRepo;
        }

        [HttpGet]
        [Route("GetAllCases")]
        public async Task<List<CasesDto>> GetAllCases()
        {
            return await _casesRepo.GetAllCasesAsync();
        }

        [HttpPost]
        [Route("AddCase")]
        public async Task<IActionResult> AddCase([FromBody] CasesDto caseDto)
        {
            // Perform validation if needed
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _casesRepo.AddCaseAsync(caseDto);
            return Ok();
        }

        [HttpPut]
        [Route("UpdateCase/{id}")]
        public async Task<IActionResult> UpdateCase(int id, [FromBody] CasesDto caseDto)
        {
            // Perform validation if needed
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingCase = await _casesRepo.GetCaseByIdAsync(id);
            if (existingCase == null)
            {
                return NotFound();
            }

            await _casesRepo.UpdateCaseAsync(id, caseDto);
            return Ok();
        }
    }
}
