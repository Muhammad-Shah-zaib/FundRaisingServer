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

        public CasesController(ICasesRepository casesRepo, ILogger<CasesController> logger,
            FundRaisingDbContext context)
        {
            _casesRepo = casesRepo;
            _logger = logger;
            this._context = context;
        }

        // API endpoint to get all the cases that are resolved
        [HttpGet]
        [Route("GetAllCases")]
        public async Task<IEnumerable<CaseResponseDto>> GetAllCases()
        {
            return await _casesRepo.GetAllCasesAsync();
        }

        // API endpoint to get a single case by its ID
        [HttpGet]
        [Route("GetCaseById/{id}")]
        public async Task<ActionResult<CaseResponseDto>> GetCaseById([FromRoute] int id)
        {
            var caseResponseDto = await _casesRepo.GetCaseByIdAsync(id);
            if (caseResponseDto == null)
            {
                return NotFound($"Case with Id: {id} not found.");
            }

            return caseResponseDto;
        }

        // API endpoint to add case
        [HttpPost]
        [Route("AddCase")]
        public async Task<IActionResult> AddCase([FromBody] AddCaseRequestDto addCaseRequestDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _casesRepo.AddCaseAsync(addCaseRequestDto);

            // Retrieve the latest case separately after adding it
            var latestCaseId = await this._context.Cases
                .OrderByDescending(c => c.CaseId)
                .Select(c => c.CaseId)
                .FirstOrDefaultAsync();


            // Create a new CaseLog for the latest case
            await this._context.CaseLogs.AddAsync(new CaseLog
            {
                LogType = CaseLogTypeEnum.CREATED_DATE.ToString(),
                LogTimestamp = DateTime.UtcNow,
                CaseId = latestCaseId,
                CollectedAmount = 0,
                CauseName = addCaseRequestDto.CauseName,
                UserCnic = addCaseRequestDto.UserCnic,
                RequiredAmount = addCaseRequestDto.RequiredDonations,
                ResolvedStatus = false,
                Title = addCaseRequestDto.Title,
                Description = addCaseRequestDto.Description,
                VerifiedStatus = addCaseRequestDto.VerifiedStatus
            });
            await this._context.SaveChangesAsync();

            return Ok();
        }
        
        // API endpoint to update case
        [HttpPut]
        [Route("UpdateCase/{id:int}")]
        public async Task<IActionResult> UpdateCase([FromRoute] int id,
            [FromBody] UpdateCaseRequestDto updateCaseRequestDto)
        {
            // checking the model state
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // checking if the case with the given ID exist or not
            var existingCase = await _casesRepo.GetCaseByIdAsync(id);
            if (existingCase == null) return NotFound($"Case, with ID {id}, does not exist");

            // updating the case if it exists
            await _casesRepo.UpdateCaseAsync(id, updateCaseRequestDto);
            // now we need to create an update log
            await this._context.CaseLogs.AddAsync(new CaseLog
            {
                LogType = CaseLogTypeEnum.UPDATED_DATE.ToString(),
                LogTimestamp = DateTime.UtcNow,
                CaseId = id,
                CollectedAmount = updateCaseRequestDto.CollectedDonations,
                RequiredAmount = updateCaseRequestDto.RequiredDonations,
                Title = updateCaseRequestDto.Title,
                Description = updateCaseRequestDto.Description,
                ResolvedStatus = updateCaseRequestDto.ResolvedStatus,
                VerifiedStatus = updateCaseRequestDto.VerifiedStatus,
                UserCnic = updateCaseRequestDto.UserCnic,
                CauseName = updateCaseRequestDto.CauseName
            });
            await this._context.SaveChangesAsync();
            return Ok();
        }

        // API endpoint to verify case
        [HttpPut]
        [Route("VerifyCase/{id:int}")]
        public async Task<ActionResult<CaseResponseDto>> VerifyCase([FromRoute] int id, [FromBody] int userCnic)
        {
            try
            {
                // first we will check if the case exists or not
                var existingCase = await _casesRepo.GetCaseByIdAsync(id);
                if (existingCase == null) return NotFound($"Case, with ID {id}, not found");
                if (existingCase.VerifiedStatus) return BadRequest($"Case# {id}, is already verified.");
                // now we can verify the case
                var verifiedCase = await _casesRepo.VerifyCaseAsync(id);

                // now we need to store the verified log
                await this._context.CaseLogs.AddAsync(new CaseLog
                {
                    LogType = CaseLogTypeEnum.VERIFIED_DATE.ToString(),
                    LogTimestamp = DateTime.UtcNow,
                    CaseId = existingCase.CaseId,
                    CollectedAmount = existingCase.CollectedDonations,
                    RequiredAmount = existingCase.RequiredDonations,
                    Title = existingCase.Title,
                    Description = existingCase.Description,
                    ResolvedStatus = existingCase.ResolvedStatus,
                    VerifiedStatus = true,
                    UserCnic = userCnic,
                    CauseName = existingCase.CauseName
                });
                await this._context.SaveChangesAsync();
                return Ok(verifiedCase);
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
        public async Task<ActionResult<CaseResponseDto>> UnVerifyCase([FromRoute] int id, [FromBody] int userCnic)
        {
            try
            {
                // first we will check if the case exists or not
                var existingCase = await _casesRepo.GetCaseByIdAsync(id);
                if (existingCase == null) return NotFound($"Case, with ID {id}, not found");
                if (!existingCase.ResolveStatus) return BadRequest($"Case# {id}, is not verified.");
                // now we can un-verify the case
                var unverifiedCase = await _casesRepo.UnVerifyCaseAsync(id);

                await this._context.CaseLogs.AddAsync(new CaseLog
                {
                    LogType = CaseLogTypeEnum.UNVERIFIED_DATE.ToString(),
                    LogTimestamp = DateTime.UtcNow,
                    CaseId = existingCase.CaseId,
                    CollectedAmount = existingCase.CollectedDonations,
                    RequiredAmount = existingCase.RequiredDonations,
                    Title = existingCase.Title,
                    Description = existingCase.Description,
                    ResolvedStatus = existingCase.ResolvedStatus,
                    VerifiedStatus = false,
                    UserCnic = userCnic,
                    CauseName = existingCase.CauseName
                });
                await this._context.SaveChangesAsync();
                return Ok(unverifiedCase);
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
        public async Task<IActionResult> ResolveCase([FromRoute] int id, [FromBody] int userCnic)
        {
            try
            {
                // first we will check if the case exists or not
                var existingCase = await this._context.Cases.FindAsync(id);
                if (existingCase == null) return NotFound();
                if (existingCase.ResolveStatus) return BadRequest($"Case {id}, is already resolved");
                // first we need to resolve the case
                await this._casesRepo.ResolveCaseAsync(id, userCnic);

                // Now we need to add resolved case log
                await this._context.CaseLogs.AddAsync(new CaseLog
                {
                    LogType = CaseLogTypeEnum.RESOLVED_DATE.ToString(),
                    LogTimestamp = DateTime.UtcNow,
                    CaseId = existingCase.CaseId,
                    CollectedAmount = existingCase.CollectedAmount,
                    RequiredAmount = existingCase.RequiredAmount,
                    Title = existingCase.Title,
                    Description = existingCase.Description,
                    ResolvedStatus = true,
                    VerifiedStatus = existingCase.VerifiedStatus,
                    UserCnic = userCnic,
                    CauseName = existingCase.CauseName
                });
                await this._context.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to resolve case.");
                throw;
            }
        }
    }
}