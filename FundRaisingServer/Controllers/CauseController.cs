using Microsoft.AspNetCore.Mvc;
using FundRaisingServer.Models;
using FundRaisingServer.Dtos;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FundRaisingServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CauseController : ControllerBase
    {
        private readonly FundRaisingDbContext _context;

        public CauseController(FundRaisingDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult<Cause>> AddCause([FromBody] CauseDto causeDto)
        {
            try
            {
                var newCause = new Cause
                {
                    CauseTitle = causeDto.CauseTitle,
                    Description = causeDto.Description,
                    CollectedAmount = 0 // Assuming initial collected amount is zero
                };
                
                _context.Causes.Add(newCause);
                await _context.SaveChangesAsync();

                var latestCause = _context.Causes.OrderByDescending(c => c.CauseId).FirstOrDefault();

                var newCauseLog = new CauseLog
                {
                    LogType = "CREATED",
                    LogTimestamp = DateTime.Now,
                    CauseTitle = causeDto.CauseTitle,
                    UserCnic = causeDto.UserCnic,
                    CauseId = newCause.CauseId
                };

                _context.CauseLogs.Add(newCauseLog);
                await _context.SaveChangesAsync();

                return Ok(latestCause);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
public async Task<ActionResult> DeleteCause(int id,CauseDto causeDto)
{
    var cause = await _context.Causes.FindAsync(id);
    if (cause == null)
    {
        return NotFound();
    }

    if (cause.CollectedAmount>0 | cause.ClosedStatus)
    {
        return BadRequest("Cannot delete cause with a non-zero collected amount. Please close the cause first.");
    }

    try
    {

        // Delete all associated cause transactions
        var causeTransactions = _context.CauseTransactions.Where(ct => ct.CauseId == id);
        _context.CauseTransactions.RemoveRange(causeTransactions);
        _context.Causes.Remove(cause);
        await _context.SaveChangesAsync();

        var newCauseLog = new CauseLog
        {
            LogType = "DELETED",
            LogTimestamp = DateTime.Now,
            CauseTitle = cause.CauseTitle,
            UserCnic = causeDto.UserCnic,
            CauseId = cause.CauseId
        };

        _context.CauseLogs.Add(newCauseLog);
        await _context.SaveChangesAsync();

        return Ok();
    }
    catch (Exception ex)
    {
        return StatusCode(500, $"Internal server error: {ex.Message}");
    }
}

[HttpPut("{id}/close")]
public async Task<ActionResult> CloseCause(int id, CauseDto causeDto)
{
    var cause = await _context.Causes.FindAsync(id);
    if (cause == null)
    {
        return NotFound();
    }

    if (cause.CollectedAmount == 0)
    {
        return BadRequest("Cannot close cause with a zero collected amount. Please add transactions to the cause first.");
    }

    try
    {
        cause.ClosedStatus = true;
        await _context.SaveChangesAsync();

        var newCauseLog = new CauseLog
        {
            LogType = "CLOSED",
            LogTimestamp = DateTime.Now,
            CauseTitle = cause.CauseTitle,
            UserCnic = causeDto.UserCnic,
            CauseId = cause.CauseId
        };

        _context.CauseLogs.Add(newCauseLog);
        await _context.SaveChangesAsync();

        return Ok();
    }
    catch (Exception ex)
    {
        return StatusCode(500, $"Internal server error: {ex.Message}");
    }
}
[HttpPut("{id}")]
public async Task<ActionResult> UpdateCause(int id, CauseUpdateDto causeUpdateDto)
{
    if (!ModelState.IsValid)
    {
        return BadRequest(ModelState);
    }
    try
    {
        var cause = await _context.Causes.FindAsync(id);
        if (cause == null)
        {
            return NotFound();
        }

        cause.CauseTitle = causeUpdateDto.Title;
        cause.Description = causeUpdateDto.Description;
        await _context.SaveChangesAsync();
        var newCauseLog = new CauseLog
        {
            LogType = "UPDATED",
            LogTimestamp = DateTime.Now,
            CauseTitle = cause.CauseTitle,
            UserCnic = causeUpdateDto.UserCnic,
            CauseId = cause.CauseId,
            CollectedAmount = cause.CollectedAmount
        };
        _context.CauseLogs.Add(newCauseLog);
        await _context.SaveChangesAsync();

        return Ok();
    }
    catch (Exception ex)
    {
        return StatusCode(500, $"Internal server error: {ex.Message}");
    }
}
  }
    }
