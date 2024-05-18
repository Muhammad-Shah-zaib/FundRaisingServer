namespace FundRaisingServer.Models.DTOs.Case
{
    public class AddCaseToLogRequestDto: CaseDto
    {
        public decimal RemainingDonations { get; set; }
    }
}