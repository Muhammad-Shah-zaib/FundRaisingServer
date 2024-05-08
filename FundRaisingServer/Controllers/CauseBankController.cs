using FundRaisingServer.Models.DTOs.Cause;
using Microsoft.AspNetCore.Mvc;

namespace FundRaisingServer.Controllers;

[ApiController]
[Route("/[controller]")]
public class CauseBankController(ICauseBankService causeBankService): ControllerBase{
    private readonly ICauseBankService _causeBankService = causeBankService;

    [HttpGet]
    [Route("GetBankAmount")]
    public async Task<ActionResult<CauseBankResponseDto>> GetBankAmount()
    {
        var bankAmount = await _causeBankService.GetBankAmount();
        return Ok(bankAmount);
    }
}