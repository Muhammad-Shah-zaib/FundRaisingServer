using FundRaisingServer.Models.DTOs.CaseLog;

namespace FundRaisingServer.Models.DTOs
{
    public class CaseResponseDto : CaseDto
    {
        public int CaseId { get; set; }
        public double TotalDonations { get; set; }
        public decimal CollectedDonations { get; set; }
        public List<CaseLogDto> CaseLogs { get; set; }
    }

}

