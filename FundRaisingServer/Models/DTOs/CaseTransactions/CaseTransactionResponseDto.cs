using FundRaisingServer.Models;

namespace FundRaisingServer.Models.DTOs.CaseTransactions
{
    public class CaseTransactionResponseDto
    {
        // transaction information
        public decimal TransactionAmount { get; set; }
        public int CaseTransactionId { get; set; }
        public string TransacntionLogDate { get; set; } = string.Empty;
        public TimeSpan TransactionLogTime { get; set; }
        // case information
        public string CaseTitle { get; set; } = string.Empty;
        public int CaseId { get; set; }
        // donor infomration
        public int DonorCnic { get; set; }
        public string DonorFirstName { get; set; } = string.Empty;
        public string DonorLastName { get; set; } = string.Empty;
    }
}