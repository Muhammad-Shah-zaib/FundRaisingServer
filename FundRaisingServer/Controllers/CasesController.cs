using FundRaisingServer.Models.DTOs;
using FundRaisingServer.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace FundRaisingServer.Controllers;

[ApiController]
[Route("/[controller]")]
public class CasesController (ICasesRepository casesRepo): ControllerBase
{
    private readonly ICasesRepository _casesRepo = casesRepo;
    
    [HttpGet]
    [Route("GetAllCases")]
    public async Task<List<CasesDto>> GetAllCases()
    {
        return await this._casesRepo.GetAllCasesAsync();
    }
}