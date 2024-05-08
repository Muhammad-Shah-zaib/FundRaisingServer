using FundRaisingServer.Models.DTOs.Cause;
using FundRaisingServer.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace FundRaisingServer.Controllers;

[ApiController]
[Route("/[controller]")]
public class CauseBankController(ICauseBankService causeBankService): ControllerBase{
    private readonly ICauseBankService _causeBankService = causeBankService;

    [HttpGet]
    [Route("GetAllBankAmount")]
    public async Task<ActionResult<CauseBankResponseDto>> GetAllBankAmount()
    {
        var bankAmount = await _causeBankService.GetBankAmountAsync();
        return Ok(bankAmount);
    }

    [HttpGet]   
    [Route("GetAllCauses")]
    public async Task<ActionResult<IEnumerable<CauseResponseDto>>> GetAllCauses()
    {
        return Ok(await this._causeBankService.GetAllCausesAsync());
    }
}