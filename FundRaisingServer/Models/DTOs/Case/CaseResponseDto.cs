namespace FundRaisingServer.Models.DTOs
{
    public class CaseResponseDto : CaseDto
    {
        public int CaseId { get; set; }
        public DateTime CreatedDate { get; set; }
        public decimal TotalDonations { get; set; }
        public decimal CollectedDonations { get; set; }
    }
}