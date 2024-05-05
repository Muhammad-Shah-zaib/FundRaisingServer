using FundRaisingServer.Models.DTOs.CaseLog;
using FundRaisingServer.Repositories;
using FundRaisingServer.Services;
using Microsoft.AspNetCore.Mvc;

namespace FundRaisingServer.Controllers;

[ApiController]
[Route("/[controller]")]
public class CaseLogController (ICaseLogRepository caseLogService): ControllerBase
{
    private readonly ICaseLogRepository _caseLogService = caseLogService;
    
    [HttpPost]
    [Route("AddOrUpdateCaseLog")]
    public async Task<IActionResult> AddOrUpdateCaseLog([FromBody] AddCaseLogRequestDto caseLogRequestDto)
    {
        try
        {
            /*
             * we can call the addOrUpdate method
             * that will update the case logs
             * if it exists, or it will create
             * a new one if it doesn't exist
             */ 
            var result = await _caseLogService.AddOrUpdateCaseLogAsync(caseLogRequestDto);
            return Ok(result);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}