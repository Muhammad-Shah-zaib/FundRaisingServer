using FundRaisingServer.Models.DTOs.CaseTransactions;
using FundRaisingServer.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace FundRaisingServer.Controllers;

    [ApiController]
    [Route("/[controller]")]
    public class CaseTransactionController : ControllerBase
    {
        private readonly ICaseTransactionRepository _caseTransactionRepository;

        public CaseTransactionController(ICaseTransactionRepository caseTransactionRepository)
        {
            _caseTransactionRepository = caseTransactionRepository;
        }

        [HttpGet]
        [Route("GetAllCaseTransactions")]
        public async Task<ActionResult<IEnumerable<CaseTransactionResponseDto>>> GetAllCaseTransactions()
        {
            try
            {
                // getting the caseTransactions
                return Ok(await this._caseTransactionRepository.GetAllCaseTransactionsAsync());
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, "Internal server error");
            }
            // return await _caseTransactionRepository.GetAllCaseTransactionsAsync();
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
            await _caseTransactionRepository.AddCaseTransactionAsync(caseTransaction);
            return Ok(true);
        }

        [HttpPut]
        [Route("UpdateCaseTransaction/{id:int}")]
        public async Task<IActionResult> UpdateCaseTransaction([FromRoute] int id, CaseTransaction caseTransaction)
        {
            if (id != caseTransaction.CaseTransactionId)
            {
                return BadRequest();
            }

            await _caseTransactionRepository.UpdateCaseTransactionAsync(caseTransaction);

            return NoContent();
        }

        [HttpDelete]
        [Route("DeleteCaseTransaction/{id:int}")]
        public async Task<IActionResult> DeleteCaseTransaction([FromRoute] int id)
        {
            var caseTransaction = await _caseTransactionRepository.GetCaseTransactionByIdAsync(id);
            if (caseTransaction == null)
            {
                return NotFound();
            }

            await _caseTransactionRepository.DeleteCaseTransactionAsync(id);

            return NoContent();
        }
    }