namespace FundRaisingServer.Models.DTOs
{
    public class CaseResponseDto : CaseDto
    {
        public int CaseId { get; set; }
        public DateTime CreatedDate { get; set; }
        public double TotalDonations { get; set; }
        public double CollectedDonations { get; set; }
    }
}