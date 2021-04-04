using System;

namespace Ledger
{
    public class Transaction
    {
        public string FromAgent { get; set; }
        public string ToAgent { get; set; }
        public decimal Amount { get; set; }
        public decimal Fee { get; set; }

        public Transaction() { }

        public Transaction(string fromAgent, string toAgent, decimal amount) : this(fromAgent, toAgent, amount, 0){ }

        public Transaction(string fromAgent, string toAgent, decimal amount, decimal fee)
        {
            FromAgent = fromAgent;
            ToAgent = toAgent;
            Amount = amount;
            Fee = fee;
        }
    }
}
