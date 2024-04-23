using FundRaisingServer.Models.DTOs;
using FundRaisingServer.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace FundRaisingServer.Controllers;

[ApiController]
[Route("/[controller]")]
public class CasesController(ICasesRepository casesRepo) : ControllerBase
{
    private readonly ICasesRepository _casesRepo = casesRepo;

    [HttpGet]
    [Route("GetAllCases")]
    public async Task<IEnumerable<CaseResponseDto>> GetAllCases()
    {
        return await this._casesRepo.GetAllCasesAsync();
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
    public async Task<IActionResult> UpdateCase([FromRoute] int id, [FromBody] CaseDto caseDto)
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
        return await this._casesRepo.VerifyCaseAsync(id);
    }
}