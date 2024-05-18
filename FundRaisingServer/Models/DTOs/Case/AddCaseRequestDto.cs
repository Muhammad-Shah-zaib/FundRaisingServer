namespace FundRaisingServer.Models.DTOs.Case
{
    public class AddCaseRequestDto    {
        public string Title { get; set; } = string.Empty;
        public string CauseName { get; set; } = string.Empty;
        public decimal RequiredDonations { get; set; }
        public bool VerifiedStatus { get; set; }
        public int UserCnic { get; set; }
        public string Description { get; set; } = null!;
    }
}