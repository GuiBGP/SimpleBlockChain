using System;
using System.Collections.Generic;
using System.Linq;

namespace Ledger
{
    public class TransactionPool
    {
        private readonly object _lockObj;
        private readonly ICollection<Transaction> _transactions;

        public TransactionPool()
        {
            _lockObj = new object();
            _transactions = new List<Transaction>();
        }

        public void Add(Transaction transaction) 
        {
            lock (_lockObj) 
            {
                _transactions.Add(transaction);
            }
        }

        public IEnumerable<Transaction> TakeAll() 
        {
            lock (_lockObj) 
            {
                var pendingTransactions = _transactions.ToList();
                _transactions.Clear();
                return pendingTransactions;
            }
        }
    }
}
