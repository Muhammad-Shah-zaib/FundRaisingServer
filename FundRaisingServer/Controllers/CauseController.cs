using Microsoft.AspNetCore.Mvc;
using FundRaisingServer.Dtos;
using Microsoft.EntityFrameworkCore;

namespace FundRaisingServer.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CauseController : ControllerBase
    {
        private readonly FundRaisingDbContext _context;

        public CauseController(FundRaisingDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Route("addCause")]
        public async Task<ActionResult<Cause>> AddCause([FromBody] CauseDto causeDto)
        {
            try
            {
                var newCause = new Cause
                {
                    CauseTitle = causeDto.CauseTitle,
                    Description = causeDto.Description,
                    ClosedStatus = false,
                    CollectedAmount = 0 // Assuming initial collected amount is zero
                };

                await _context.Causes.AddAsync(newCause);
                await _context.SaveChangesAsync();

                var latestCause = _context.Causes.OrderByDescending(c => c.CauseId).FirstOrDefault();

                var newCauseLog = new CauseLog
                {
                    LogType = "CREATED",
                    LogTimestamp = DateTime.UtcNow,
                    CauseTitle = causeDto.CauseTitle,
                    Description = causeDto.Description,
                    UserCnic = causeDto.UserCnic,
                    CauseId = newCause.CauseId
                };

                _context.CauseLogs.Add(newCauseLog);
                await _context.SaveChangesAsync();

                return Ok("Cause added successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // below method delete the Cause if and only if the Transaction are null for the associated causeId
        [HttpDelete]
        [Route("DeleteCause/{id:int}")]
        public async Task<ActionResult> DeleteCause([FromRoute] int id)
        {
            try
            {
                var cause = await _context.Causes.Where(c => c.CauseId == id).FirstOrDefaultAsync();
                if (cause == null) return NotFound();

                // we need to make sure there should be no cause Transactions othervise we will close the cause instead of deleting it
                var transactionList = await this._context.CauseTransactions.Where(t => t.CauseId == id).ToListAsync();
                if (cause.ClosedStatus) return BadRequest("Cannot delete, Cause is already closed");
                else if (transactionList == null) return BadRequest();
                else if (cause.CollectedAmount > 0 ) 
                    return BadRequest("Cannot delete cause with a non-zero collected amount.");
                // since the cause exist we need to delete the cause logs first...

                this._context.CauseLogs.RemoveRange(
                    await this._context.CauseLogs.Where(l => l.CauseId == id).ToListAsync()
                );
                await this._context.SaveChangesAsync();
                _context.Causes.Remove(cause);
                await _context.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut]
        [Route("CloseCause/{id:int}")]
        public async Task<ActionResult> CloseCause([FromRoute] int id,[FromBody] CauseDto causeDto)
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
                    Description = causeDto.Description,
                    CauseId = cause.CauseId,
                    CollectedAmount= cause.CollectedAmount
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
        [HttpPut]
        [Route("UpdateCause/{id:int}")]
        public async Task<ActionResult> UpdateCause([FromRoute] int id, [FromBody] CauseUpdateDto causeUpdateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var cause = await this._context.Causes.FindAsync(id);
                if (cause == null) return NotFound();

                cause.CauseTitle = causeUpdateDto.Title;
                cause.Description = causeUpdateDto.Description;
                await _context.SaveChangesAsync();
                var newCauseLog = new CauseLog
                {
                    LogType = "UPDATED",
                    LogTimestamp = DateTime.UtcNow,
                    CauseTitle = causeUpdateDto.Title,
                    UserCnic = causeUpdateDto.UserCnic,
                    Description = causeUpdateDto.Description,
                    CauseId = id,
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