using FundRaisingServer.Models.DTOs;
using FundRaisingServer.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FundRaisingServer.Services
{
    public class CasesService : ICasesRepository
    {
        private readonly FundRaisingDbContext _context;

        public CasesService(FundRaisingDbContext context)
        {
            _context = context;
        }

        public async Task<List<CasesDto>> GetAllCasesAsync()
        {
            try
            {
                var cases = await _context.Cases
                    .Include(c => c.Cause)
                    .Select(c => new CasesDto()
                    {
                        CaseId = c.CaseId,
                        Title = c.Title!,
                        Description = c.Description!,
                        CreatedDate = c.CreatedDate,
                        CauseName = c.Cause.Title,
                        CauseId = c.CauseId
                    })
                    .ToListAsync();

                return cases;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<CasesDto> GetCaseByIdAsync(int id) // Add this method
        {
            try
            {
                var singleCase = await _context.Cases
                    .Include(c => c.Cause)
                    .SingleOrDefaultAsync(c => c.CaseId == id);

                if (singleCase == null)
                {
                    return null; // Case not found
                }

                var caseDto = new CasesDto()
                {
                    CaseId = singleCase.CaseId,
                    Title = singleCase.Title!,
                    Description = singleCase.Description!,
                    CreatedDate = singleCase.CreatedDate,
                    CauseName = singleCase.Cause.Title,
                    CauseId = singleCase.CauseId
                };

                return caseDto;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task AddCaseAsync(CasesDto caseDto)
        {
            try
            {
                var newCase = new Case
                {
                    Title = caseDto.Title,
                    Description = caseDto.Description,
                    CreatedDate = caseDto.CreatedDate,
                    CauseId = caseDto.CauseId
                };

                _context.Cases.Add(newCase);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task UpdateCaseAsync(int id, CasesDto caseDto)
        {
            try
            {
                var existingCase = await _context.Cases.FindAsync(id);

                if (existingCase == null)
                {
                    throw new ArgumentException("Case not found");
                }

                existingCase.Title = caseDto.Title;
                existingCase.Description = caseDto.Description;
                existingCase.CreatedDate = caseDto.CreatedDate;
                existingCase.CauseId = caseDto.CauseId;

                _context.Update(existingCase);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
