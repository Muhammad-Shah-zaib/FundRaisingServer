using FundRaisingServer.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace FundRaisingServer.Controllers;

[ApiController]
[Route("/[controller]")]
public class DonatorController(IDonatorRepository donatorRepo, ILogger<DonatorController> logger) : ControllerBase
{
    // DIs
    private readonly IDonatorRepository _donatorRepo = donatorRepo ;
    private readonly ILogger<DonatorController> _logger = logger;

    // for gettting the donator count
    [HttpGet]
    [Route("GetDonatorCount")]
    public async Task<ActionResult<Int32>> GetDonatorCount()
    {
        try
        {
            return Ok(await _donatorRepo.GetDonatorCountAsync());
        }
        catch (Exception ex)
        {
            this._logger.LogError(ex, "Failed to get donator count.");
            return StatusCode(500, "Internal server error");
        }
    }
}