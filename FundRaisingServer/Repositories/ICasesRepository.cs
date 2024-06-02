using FundRaisingServer.Models.DTOs;
using FundRaisingServer.Models.DTOs.Case;
using FundRaisingServer.Models.DTOs.CaseLog;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FundRaisingServer.Repositories;

public interface ICasesRepository
{
        Task<IEnumerable<CaseResponseDto>> GetAllCasesAsync();

        Task<IEnumerable<CaseResponseDto>> GetAllVerifiedCasesAsync();

        Task<CaseResponseDto?> GetCaseByIdAsync(int id);

        Task AddCaseAsync(AddCaseRequestDto caseDto);

        Task UpdateCaseAsync(int id, UpdateCaseRequestDto caseDto);

        Task<CaseResponseDto> VerifyCaseAsync(int id);

        Task<CaseResponseDto> UnVerifyCaseAsync(int id);

        Task<CaseResponseDto?> UpdateCaseCollectedAmountAsync(int caseId, decimal amount);

        Task<bool?> ResolveCaseAsync(int caseId, int userCnic);
}

