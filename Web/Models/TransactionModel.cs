using Ledger;

namespace Web.Models
{
    public class TransactionModel
    {
        public string FromAgent { get; set; }
        public string ToAgent { get; set; }
        public decimal Amount { get; set; }

        public Transaction ToTransaction() 
        {
            return new Transaction(FromAgent, ToAgent, Amount);
        }
    }
}
