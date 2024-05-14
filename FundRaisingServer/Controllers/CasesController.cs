using FundRaisingServer.Models.DTOs;
using FundRaisingServer.Models.DTOs.Case;
using FundRaisingServer.Models.DTOs.CaseLog;
using FundRaisingServer.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;



// THIS CONTROLLER POSSESS FOLLOWING API endpoints
// GetAllCases - to get all the resolved cases
// GetCaseById - to get a case by its id
// AddCase - to add a new case
// UpdateCase - to update a case
// VerifyCase - to verify a case
// UnVerifyCase - to un-verify a case
// ResolveCase - to resolve a case
// CloseCase - to close a case



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

        // API endpoint to get all the cases that are not resolved
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

        // API endpoint to add case
        [HttpPost]
        [Route("AddCase")]
        public async Task<IActionResult> AddCase([FromBody] AddCaseRequestDto caseDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _casesRepo.AddCaseAsync(caseDto);

            // Retrieve the latest case separately after adding it
            var latestCaseId = await this._context.Cases
            .OrderByDescending(c => c.CaseId)
            .Select(c => c.CaseId)
            .FirstOrDefaultAsync();


            // Create a new CaseLog for the latest case
            var newCaseLog = new CaseLog
            {
                LogType = CaseLogTypeEnum.CREATED_DATE.ToString(),
                LogTimestamp = DateTime.UtcNow,
                CaseId = latestCaseId,
                CollectedAmount = 0,
                CauseName = caseDto.CauseName,
                UserCnic = caseDto.UserCnic,
                RequiredAmount = caseDto.RequiredDonations,
                ResolvedStatus = false,
                Title = caseDto.Title,
                Description = caseDto.Description,
                VerifiedStatus = caseDto.VerifiedStatus
            };

            await this._context.CaseLogs.AddAsync(newCaseLog);

            return Ok();
        }


        // API endpoint to update case
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

        // API endpoint to verify case
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

        // API endpoint to un-verify case
        [HttpPut]
        [Route("UnVerifyCase/{id:int}")]
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

        // API endpoint to resolve case
        [HttpPut]
        [Route("ResolveCase/{id:int}")]
        public async Task<IActionResult> ResolveCase([FromRoute] int id, [FromBody] UpdateCaseRequestDto updateCaseRequestDto)
        {
            try
            {
                // first we need to resolve the case
                await _casesRepo.ResolveCaseAsync(id, updateCaseRequestDto);

                // Add a new case log
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

        // API endpoint to close case
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
    }
}
