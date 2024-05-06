using System;
using System.Net.Http.Headers;

namespace FundRaisingServer.Models.DTOs
{
    public class CaseFundResponseDto
    {
        public int CaseFundId { get; set; }
        public int CaseId { get; set; }
        public decimal RequiredAmount { get; set; }
        public decimal CollectedAmount { get; set; }
        public decimal RemainingAmount { get; set; }
        public DateTime CreatedDate { get; set; }
        public CaseResponseDto Case { get; set; } = new CaseResponseDto();
    }
}