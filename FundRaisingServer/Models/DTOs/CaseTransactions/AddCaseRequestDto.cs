namespace FundRaisingServer.Models.DTOs.CaseTransactions
{
    public class AddCaseTransactionRequestDto
    {
        public int CaseId { get; set; }
        public int DonorCnic { get; set; }
        public decimal TransactionAmount { get; set; }
    }
}