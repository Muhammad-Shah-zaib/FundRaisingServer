using FundRaisingServer.Models.DTOs.Case;

namespace FundRaisingServer.Models.DTOs.CaseLog
{
    public class AddCaseToLogRequestDto: CaseDto
    {
        public decimal RemainingDonations { get; set; }
    }
}