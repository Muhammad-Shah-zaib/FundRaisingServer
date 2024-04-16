using FundRaisingServer.Models.DTOs;
using FundRaisingServer.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace FundRaisingServer.Controllers;

[ApiController]
[Route("/Cases")]
public class CasesController (ICasesRepository casesRepo): ControllerBase
{
    private readonly ICasesRepository _casesRepo = casesRepo;
    
    [HttpGet]
    public async Task<List<CasesDto>> GetAllCases()
    {
        return await this._casesRepo.GetAllCasesAsync();
    }
}