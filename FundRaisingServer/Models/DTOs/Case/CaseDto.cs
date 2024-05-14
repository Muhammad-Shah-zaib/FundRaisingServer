namespace FundRaisingServer.Models.DTOs.Case
{
    public class CaseDto
    {
        public string Title { get; set; } = string.Empty;
        public int CaseId { get; set; }
        public string CauseName { get; set; } = string.Empty;
        public decimal RequiredDonations { get; set; }
        public bool VerifiedStatus { get; set; }
        public decimal CollectedDonations { get; set; } 
        public int DonorCnic { get; set; }
        public string Description { get; set; } = null!;

        public bool ResolvedStatus { get; set; }
    }
}