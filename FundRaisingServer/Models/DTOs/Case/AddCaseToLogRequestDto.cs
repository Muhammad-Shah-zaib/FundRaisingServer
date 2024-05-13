using FundRaisingServer.Models.DTOs.Case;

namespace FundRaisingServer.Models.DTOs.CaseLog
{
    public class AddCaseToLogRequestDto: CaseDto
    {
        public decimal CollectedDonations { get; set; }
        public decimal RemainingDonations { get; set; }
        public bool ResolvedStatus { get; set; }
    }
}