using FundRaisingServer.Models.DTOs.CaseTransactions;
using FundRaisingServer.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace FundRaisingServer.Controllers
{
    [ApiController]
    [Route("/[controller]")]
    public class CaseTransactionController : ControllerBase
    {
        private readonly ICaseTransactionRepository _caseTransactionRepository;
        private readonly ILogger<CaseTransactionController> _logger;

        public CaseTransactionController(ICaseTransactionRepository caseTransactionRepository, ILogger<CaseTransactionController> logger)
        {
            _caseTransactionRepository = caseTransactionRepository;
            _logger = logger;
        }
        [HttpGet]
        [Route("GetTotalDonations")]
        public async Task<ActionResult<decimal>> GetTotalDonations()
        {
            try
            {
                return Ok(await _caseTransactionRepository.GetTotalDonationsAsync());
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Failed to get total donations.");
                return StatusCode(500, "Internal server error");
            }
        }
        
        [HttpGet]
        [Route("GetAllCaseTransactions")]
        public async Task<ActionResult<IEnumerable<CaseTransactionResponseDto>>> GetAllCaseTransactions()
        {
            try
            {
                return Ok(await _caseTransactionRepository.GetAllCaseTransactionsAsync());
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Failed to get all case transactions.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet]
        [Route("GetCaseTransactionById/{id:int}")]
        public async Task<ActionResult<CaseTransaction>> GetCaseTransactionById([FromRoute] int id)
        {
            var caseTransaction = await _caseTransactionRepository.GetCaseTransactionByIdAsync(id);
            if (caseTransaction == null)
            {
                return NotFound();
            }
            return caseTransaction;
        }

        [HttpPost]
        [Route("AddCaseTransaction")]
        public async Task<IActionResult> AddCaseTransaction(AddCaseTransactionRequestDto caseTransaction)
        {
            try
            {
                await _caseTransactionRepository.AddCaseTransactionAsync(caseTransaction);


                return Ok(true);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Failed to add case transaction.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete]
        [Route("DeleteCaseTransaction/{id:int}")]
        public async Task<IActionResult> DeleteCaseTransaction([FromRoute] int id)
        {
            try
            {
                var caseTransaction = await _caseTransactionRepository.GetCaseTransactionByIdAsync(id);
                if (caseTransaction == null)
                {
                    return NotFound();
                }

                await _caseTransactionRepository.DeleteCaseTransactionAsync(id);



                return NoContent();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Failed to delete case transaction.");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
