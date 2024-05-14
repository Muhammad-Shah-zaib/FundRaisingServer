using FundRaisingServer.Models.DTOs.Case;
using FundRaisingServer.Models.DTOs.CaseLog;

namespace FundRaisingServer.Models.DTOs
{
    public class CaseResponseDto : CaseDto
    {
        //public int CaseId { get; set; }
        //public decimal CollectedDonations { get; set; }
        public decimal? RemainingDonations { get; set; }
        public bool ResolveStatus { get; set; }

        public List<CaseLogDto> CaseLogs { get; set; } = new List<CaseLogDto>(); 
    }
}

