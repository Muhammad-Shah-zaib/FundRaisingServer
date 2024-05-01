using FundRaisingServer.Models.DTOs.CaseLog;
using FundRaisingServer.Services;
using Microsoft.AspNetCore.Mvc;

namespace FundRaisingServer.Controllers;

[ApiController]
[Route("/[controller]")]
public class CaseLogController (CaseLogService caseLogService): ControllerBase
{
    private readonly CaseLogService _caseLogService = caseLogService;
    
    [HttpPost]
    [Route("AddCaseLog")]
    public async Task<IActionResult> AddNewCaseLogAsync([FromBody] NewCaseLogRequestDto caseLogRequestDto)
    {
        try
        {
            var result = await _caseLogService.AddNewCaseLogAsync(caseLogRequestDto);
            return Ok(result);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}