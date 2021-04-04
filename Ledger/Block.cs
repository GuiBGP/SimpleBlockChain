using System;
using System.Collections.Generic;
using System.Text;

namespace Ledger
{
    public class Block
    {
        /// <summary>
        /// Number that indentifies the sequence of Blocks
        /// </summary>
        public long Index { get; set; }

        /// <summary>
        /// Hash that indentifies the Block
        /// </summary>
        public string BlockHash { get; set; }

        /// <summary>
        /// Hash that indentifies the Previous Hash Block
        /// </summary>
        public string PreviousBlockHash { get; set; }

        /// <summary>
        /// Number that added to this Block creates the desired BlockHash
        /// </summary>
        public long Nonce { get; set; }

        /// <summary>
        /// Time which the Block was created
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// List of Transactions Processed in this Block
        /// </summary>
        public IEnumerable<Transaction> Transactions { get; set; }
    }
}
