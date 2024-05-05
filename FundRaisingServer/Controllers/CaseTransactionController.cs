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
        public async Task<IEnumerable<CaseTransaction>> GetAllCaseTransactions()
        {
            return await _caseTransactionRepository.GetAllCaseTransactionsAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CaseTransaction>> GetCaseTransactionById(int id)
        {
            var caseTransaction = await _caseTransactionRepository.GetCaseTransactionByIdAsync(id);
            if (caseTransaction == null)
            {
                return NotFound();
            }
            return caseTransaction;
        }

        [HttpPost]
        public async Task<ActionResult<CaseTransaction>> CreateCaseTransaction(CaseTransaction caseTransaction)
        {
            await _caseTransactionRepository.CreateCaseTransactionAsync(caseTransaction);
            return CreatedAtAction(nameof(GetCaseTransactionById), new { id = caseTransaction.CaseTransactionId }, caseTransaction);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCaseTransaction(int id, CaseTransaction caseTransaction)
        {
            if (id != caseTransaction.CaseTransactionId)
            {
                return BadRequest();
            }

            await _caseTransactionRepository.UpdateCaseTransactionAsync(caseTransaction);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCaseTransaction(int id)
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