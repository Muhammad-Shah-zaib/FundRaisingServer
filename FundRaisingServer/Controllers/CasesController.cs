using FundRaisingServer.Models.DTOs;
using FundRaisingServer.Models.DTOs.Case;
using FundRaisingServer.Models.DTOs.CaseLog;
using FundRaisingServer.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace FundRaisingServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CasesController : ControllerBase
    {
        private readonly ICasesRepository _casesRepo;
        private readonly ILogger<CasesController> _logger;
        private readonly FundRaisingDbContext _context;

        public CasesController(ICasesRepository casesRepo, ILogger<CasesController> logger, FundRaisingDbContext context)
        {
            _casesRepo = casesRepo;
            _logger = logger;
            this._context = context;
        }

        [HttpGet]
        [Route("GetAllCases")]
        public async Task<IEnumerable<CaseResponseDto>> GetAllCases()
        {
            return await _casesRepo.GetAllCasesAsync();
        }

        [HttpGet]
        [Route("GetCaseById/{id}")]
        public async Task<ActionResult<CaseResponseDto>> GetCaseById([FromRoute] int id)
        {
            var caseResponseDto = await _casesRepo.GetCaseByIdAsync(id);
            if (caseResponseDto == null)
            {
                return NotFound();
            }

            return caseResponseDto;
        }

        [HttpPost]
        [Route("AddCase")]
        public async Task<IActionResult> AddCase([FromBody] AddCaseRequestDto caseDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _casesRepo.AddCaseAsync(caseDto);

            var newCaseLog = new CaseLog
            {
                LogType = CaseLogTypeEnum.CREATED_DATE.ToString(),
                LogTimestamp = DateTime.UtcNow,
                CaseId = caseDto.CaseId,
                UserCnic = caseDto.DonorCnic
            };

            await this._context.CaseLogs.AddAsync(newCaseLog);

            return Ok();
        }

        [HttpPut]
        [Route("UpdateCase/{id}")]
        public async Task<IActionResult> UpdateCase([FromRoute] int id, [FromBody] UpdateCaseRequestDto updateCaseRequestDto)
        {
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

            var newCaseLog = new CaseLog
            {

                LogType = CaseLogTypeEnum.UPDATED_DATE.ToString(),
                LogTimestamp = DateTime.UtcNow,
                CaseId = updateCaseRequestDto.CaseId,
                CollectedAmount = updateCaseRequestDto.CollectedDonations,
                RequiredAmount = updateCaseRequestDto.RequiredDonations,
                Title = updateCaseRequestDto.Title,
                Description = updateCaseRequestDto.Description

            };
            await this._context.CaseLogs.AddAsync(newCaseLog);



            return Ok();
        }
        [HttpPut]
        [Route("VerifyCase/{id}")]
        public async Task<ActionResult<CaseResponseDto>> VerifyCase([FromRoute] int id, [FromBody] UpdateCaseRequestDto updateCaseRequestDto)
        {
            try
            {
                var verifiedCase = await _casesRepo.VerifyCaseAsync(id);

                var newCaseLog = new CaseLog
                {
                    LogType = CaseLogTypeEnum.VERIFIED_DATE.ToString(),
                    LogTimestamp = DateTime.UtcNow,
                    CaseId = updateCaseRequestDto.CaseId,
                    CollectedAmount = updateCaseRequestDto.CollectedDonations,
                    RequiredAmount = updateCaseRequestDto.RequiredDonations,
                    Title = updateCaseRequestDto.Title,
                    Description = updateCaseRequestDto.Description
                };
           
            await this._context.CaseLogs.AddAsync(newCaseLog);


                return verifiedCase;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to verify case.");
                throw;
            }
        }

        [HttpPut]
        [Route("UnVerifyCase/{id}")]
        public async Task<ActionResult<CaseResponseDto>> UnVerifyCase([FromRoute] int id, [FromBody] UpdateCaseRequestDto updateCaseRequestDto)
        {
            try
            {
                var unverifiedCase = await _casesRepo.UnVerifyCaseAsync(id);

                var newCaseLog = new CaseLog
                {
                    LogType = CaseLogTypeEnum.UNVERIFIED_DATE.ToString(),
                    LogTimestamp = DateTime.UtcNow,
                    CaseId = updateCaseRequestDto.CaseId,
                    CollectedAmount = updateCaseRequestDto.CollectedDonations,
                    RequiredAmount = updateCaseRequestDto.RequiredDonations,
                    Title = updateCaseRequestDto.Title,
                    Description = updateCaseRequestDto.Description
                };
                await this._context.CaseLogs.AddAsync(newCaseLog);


                return unverifiedCase;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to un-verify case.");
                throw;
            }
        }

        [HttpPut]
        [Route("ResolveCase/{id}")]
        public async Task<IActionResult> ResolveCase([FromRoute] int id, [FromBody] UpdateCaseRequestDto updateCaseRequestDto)
        {
            try
            {
                await _casesRepo.ResolveCaseAsync(id, updateCaseRequestDto);

                var newCaseLog = new CaseLog
                {
                    LogType = CaseLogTypeEnum.RESOLVED_DATE.ToString(),
                    LogTimestamp = DateTime.UtcNow,
                    CaseId = updateCaseRequestDto.CaseId,
                    CollectedAmount = updateCaseRequestDto.CollectedDonations,
                    RequiredAmount = updateCaseRequestDto.RequiredDonations,
                    Title = updateCaseRequestDto.Title,
                    Description = updateCaseRequestDto.Description,
                    VerifiedStatus = updateCaseRequestDto.VerifiedStatus,
                    ResolvedStatus = updateCaseRequestDto.ResolvedStatus
                };

                await this._context.CaseLogs.AddAsync(newCaseLog);

                return Ok("case has been resolved :)");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to resolve case.");
                throw;
            }
        }

        [HttpPut]
        [Route("CloseCase/{id}")]
        public async Task<IActionResult> CloseCase([FromRoute] int id, [FromBody] UpdateCaseRequestDto updateCaseRequestDto)
        {
                var caseEntity = await this._context.Cases.FindAsync(id);
            try
            {                

                var newCaseLog = new CaseLog()
                {
                    LogType = CaseLogTypeEnum.CLOSED_DATE.ToString(),
                    LogTimestamp = DateTime.UtcNow,
                    CaseId = updateCaseRequestDto.CaseId,
                    CollectedAmount = updateCaseRequestDto.CollectedDonations,
                    RequiredAmount = updateCaseRequestDto.RequiredDonations,
                    Title = updateCaseRequestDto.Title,
                    Description = updateCaseRequestDto.Description,
                    VerifiedStatus = updateCaseRequestDto.VerifiedStatus,
                    ResolvedStatus = updateCaseRequestDto.ResolvedStatus
                };

                await this._context.CaseLogs.AddAsync(newCaseLog);


                return Ok();
            }


            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to close case.");
                throw;
            }
        }

        // [HttpDelete]
        // [Route("DeleteCase/{id}")]
        // public async Task<ActionResult<CaseResponseDto>> DeleteCase([FromRoute] int id)
        // {
        //     try
        //     {
        //         var existingCase = await _casesRepo.GetCaseByIdAsync(id);
        //         if (existingCase == null)
        //         {
        //             return NotFound();
        //         }

        //         if (existingCase.CollectedDonations > 0)
        //         {
        //             // If there are collected donations, mark the case as resolved and unverified
        //             existingCase.ResolveStatus = true;
        //             existingCase.VerifiedStatus = false;

        //             // Log the case resolution
        //             var newCaseLog = new CaseLog
        //             {
        //                 LogType = CaseLogTypeEnum.RESOLVED_DATE.ToString(),
        //                 LogTimestamp = DateTime.UtcNow,
        //                 CaseId = existingCase.CaseId,
        //                 CollectedAmount = existingCase.CollectedDonations,
        //                 RequiredAmount = existingCase.RequiredDonations,
        //                 Title = existingCase.Title,
        //                 Description = existingCase.Description,
        //                 VerifiedStatus = existingCase.VerifiedStatus,
        //                 ResolvedStatus = existingCase.ResolvedStatus
        //             };

        //             await _casesRepo.AddCaseLogAsync(newCaseLog);
        //         }
        //         else
        //         {
        //             // If there are no collected donations, simply delete the case
        //             await _casesRepo.DeleteCaseAsync(id);

        //             // Log the case deletion
        //             var newCaseLog = new CaseLog
        //             {
        //                 LogType = CaseLogTypeEnum.DELETED_DATE.ToString(),
        //                 LogTimestamp = DateTime.UtcNow,
        //                 CaseId = existingCase.CaseId,
        //                 CollectedAmount = existingCase.CollectedDonations,
        //                 RequiredAmount = existingCase.RequiredDonations,
        //                 Title = existingCase.Title,
        //                 Description = existingCase.Description,
        //                 VerifiedStatus = existingCase.VerifiedStatus,
        //                 ResolvedStatus = existingCase.ResolvedStatus

        //             };

        //             await _casesRepo.AddCaseLogAsync(newCaseLog);
        //         }

        //         return Ok(existingCase);
        //     }
        //     catch (Exception ex)
        //     {
        //         _logger.LogError(ex, "Failed to delete case.");
        //         return StatusCode(500, "Internal server error");
        //     }
        // }
    }
}
