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
        private readonly ICaseLogRepository _caseLogRepo;
        private readonly ILogger<CasesController> _logger;
        private readonly FundRaisingDbContext _context;

        public CasesController(ICasesRepository casesRepo, ICaseLogRepository caseLogRepo, ILogger<CasesController> logger,
            FundRaisingDbContext context)
        {
            _casesRepo = casesRepo;
            _logger = logger;
            this._context = context;
            this._caseLogRepo = caseLogRepo;
        }

        [HttpGet]
        [Route("GetResolvedCaseCount")]
        public async Task<ActionResult<Int32>> GetResolvedCaseCount()
        {
            try
            {
                return Ok(await _casesRepo.GetResolvedCaseCountAsync());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get resolved case count.");
                return StatusCode(500, "Internal server error");
            }
        }

        // API endpoint to get all the cases that are resolved
        [HttpGet]
        [Route("GetAllCases")]
        public async Task<IEnumerable<CaseResponseDto>> GetAllCases()
        {
            try
            {
                return await _casesRepo.GetAllCasesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get all cases.");
                throw;
            }
        }

        [HttpGet]
        [Route("GetAllVerifiedCases")]
        public async Task<IEnumerable<CaseResponseDto>> GetAllVerifiedCases()
        {
            try
            {
                return await _casesRepo.GetAllVerifiedCasesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get all veeqwrified cases.");
                throw;
            }
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
            var latestCase = await this._context.Cases
                .OrderByDescending(c => c.CaseId)
                .FirstOrDefaultAsync();
            
            await this._caseLogRepo.AddCaseLogAsync(existingCase: latestCase!, logType: CaseLogTypeEnum.CREATED_DATE,
                userCnic: addCaseRequestDto.UserCnic);

            return Ok("Case added successfully");
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
            var existingCase = await this._context.Cases.FindAsync(id);
            if (existingCase == null) return NotFound($"Case, with ID {id}, does not exist");

            // updating the case if it exists
            await _casesRepo.UpdateCaseAsync(id, updateCaseRequestDto);
            // now we need to create an update log
            await this._caseLogRepo.AddCaseLogAsync(existingCase: existingCase, logType: CaseLogTypeEnum.UPDATED_DATE,
                userCnic: updateCaseRequestDto.UserCnic);
            return Ok("Case has updated successfully");
        }

        // API endpoint to verify case
        [HttpPut]
        [Route("VerifyCase/{id:int}")]
        public async Task<ActionResult<CaseResponseDto>> VerifyCase([FromRoute] int id, [FromBody] CaseCnicDto caseCnicDto)
        {
            try
            {
                // first we will check if the case exists or not
                var existingCase = await this._context.Cases.FindAsync(id);
                if (existingCase == null) return NotFound($"Case, with ID {id}, not found");
                else if (existingCase.VerifiedStatus) return BadRequest($"Case# {id}, is already verified.");
                else if (existingCase.ResolveStatus) return BadRequest($"Case# {id}, has resolved.");
                // now we can verify the case
                var verifiedCase = await _casesRepo.VerifyCaseAsync(id);
                existingCase.VerifiedStatus = true;

                // now we need to store the verified log
                await this._caseLogRepo.AddCaseLogAsync(existingCase: existingCase, logType: CaseLogTypeEnum.VERIFIED_DATE,
                    userCnic: caseCnicDto.UserCnic);
                
                return Ok($"Case# {id} has been verified successfully");
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
        public async Task<ActionResult<CaseResponseDto>> UnVerifyCase([FromRoute] int id, [FromBody] CaseCnicDto caseCnicDto)
        {
            try
            {
                // first we will check if the case exists or not
                var existingCase = await this._context.Cases.FindAsync(id);
                if (existingCase == null) return NotFound($"Case# {id}, not found");
                else if (!existingCase.VerifiedStatus) return BadRequest($"Case# {id}, is already not verified.");
                else if (existingCase.ResolveStatus) return BadRequest($"Case# {id}, has resolved.");
                var unverifiedCase = await _casesRepo.UnVerifyCaseAsync(id);
                existingCase.VerifiedStatus = false;
                // now we can un-verify the case
                await this._caseLogRepo.AddCaseLogAsync(existingCase: existingCase, logType: CaseLogTypeEnum.UNVERIFIED_DATE,
                    userCnic: caseCnicDto.UserCnic);
                return Ok($"Case# {id} has been un-verified successfully");
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
        public async Task<IActionResult> ResolveCase([FromRoute] int id, [FromBody] CaseCnicDto caseCnicDto)
        {
            try
            {
                // first we will check if the case exists or not
                var existingCase = await this._context.Cases.FindAsync(id);
                if (existingCase == null) return NotFound();
                if (existingCase.ResolveStatus) return BadRequest($"Case {id}, is already resolved");
                // first we need to resolve the case
                await this._casesRepo.ResolveCaseAsync(id, caseCnicDto.UserCnic);
                existingCase.ResolveStatus = true;

                // Now we need to add resolved case log
                await this._caseLogRepo.AddCaseLogAsync(existingCase: existingCase, logType: CaseLogTypeEnum.RESOLVED_DATE,
                    userCnic: caseCnicDto.UserCnic);
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