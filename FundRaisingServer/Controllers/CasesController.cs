using FundRaisingServer.Models.DTOs;
using FundRaisingServer.Models.DTOs.Case;
using FundRaisingServer.Repositories;
using Microsoft.AspNetCore.Http.HttpResults;
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

        [HttpPut]
        [Route("ResolveCase/{id:int}")]
        public async Task<IActionResult> ResolveCase([FromRoute] int id)
        {
            try
            {
                var checkCase = await this._casesRepo.ResolveCaseAsync(id);

                return checkCase switch
                {
                    null => new NotFoundResult(),
                    true => Ok("Case has been resolved"),
                    _ => StatusCode(500, "Internal server Error")
                };
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            
        }
        
        [HttpGet]
        [Route("GetAllCases")]
        public async Task<IEnumerable<CaseResponseDto>> GetAllCases()
        {
            return await _casesRepo.GetAllCasesAsync();
        }

        [HttpPost]
        [Route("AddCase")]
        public async Task<IActionResult> AddCase([FromBody] AddCaseRequestDto caseDto)
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
        public async Task<IActionResult> UpdateCase([FromRoute] int id, [FromBody] UpdateCaseRequestDto updateCaseRequestDto)
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

            await _casesRepo.UpdateCaseAsync(id, updateCaseRequestDto);
            return Ok();
        }

        [HttpDelete]
        [Route("DeleteCase/{id}")]
        public async Task<ActionResult<Case>> DeleteCase([FromRoute] int id)
        {
            // we need to update this...
            /*
            * This will only work if the case has no case Fund
            * tuple linkage i.e ( cases_funds table has foreign
            * key constraint on the case_id column ) So we have
            * delete the case fund tuple first before deleting
            * the case.
            */

            // Delete the case fund tuple first
            // didnt implemented this yet
            // Implement this here and remove the comments
            // also check for edge cases like if the case fund
            // tuple is not found etc.
            // and return the appropriate responses
            // and wrap it in try catch block

            // Deleting Case
            var existingCase = await _casesRepo.GetCaseByIdAsync(id);
            if (existingCase == null) return NotFound();

            await _casesRepo.DeleteCaseAsync(id);
            return Ok(existingCase);
        }

        [HttpPut]
        [Route("VerifyCase/{id}")]
        public async Task<ActionResult<CaseResponseDto>> VerifyCase([FromRoute] int id)
        {
            try
            {
                return await this._casesRepo.VerifyCaseAsync(id);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        [HttpPut]
        [Route("UnVerifyCase/{id:int}")]
        public async Task<ActionResult<CaseResponseDto>> UnVerifyCase([FromRoute] int id)
        {
            // IMPLEMENTING THIS FUNCTION RN
            try
            {
                return await this._casesRepo.UnVerifyCaseAsync(id);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
    }
}