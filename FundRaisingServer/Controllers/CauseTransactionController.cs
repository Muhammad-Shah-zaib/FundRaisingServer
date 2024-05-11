using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FundRaisingServer.Models;
using FundRaisingServer.Dtos;
using System;
using System.Linq;
using System.Threading.Tasks;
namespace FundRaisingServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
public class CauseTransactionController : ControllerBase
{
    private readonly FundRaisingDbContext _context;
    private readonly ILogger _logger;

    public CauseTransactionController(FundRaisingDbContext context, ILogger<CauseTransactionController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpPost]
    public async Task<ActionResult> AddCauseTransaction(CauseTransaction causeTransaction)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            _context.CauseTransactions.Add(causeTransaction);

            var cause = await _context.Causes.FindAsync(causeTransaction.CauseId);
            if (cause == null)
            {
                return NotFound();
            }

            cause.CollectedAmount += causeTransaction.TransactionAmount;

            await _context.SaveChangesAsync();

            LogCauseTransaction(causeTransaction);

            return Ok();
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Error adding cause transaction");
            return StatusCode(500, "Internal server error");
        }
    }

    private void LogCauseTransaction(CauseTransaction causeTransaction)
{
    var collectedAmountAtTransaction = _context.CauseTransactions
        .Where(ct => ct.CauseId == causeTransaction.CauseId)
        .Sum(ct => ct.TransactionAmount);

    var newCauseLog = new CauseLog
    {
        LogType = "Transaction made",
        LogTimestamp = DateTime.Now,
        CauseTitle = causeTransaction.Cause.CauseTitle,
        UserCnic = causeTransaction.DonorCnic,
        CauseId = causeTransaction.CauseId,
        CollectedAmount = collectedAmountAtTransaction
    };

    _context.CauseLogs.Add(newCauseLog);
    _context.SaveChangesAsync();
}}}