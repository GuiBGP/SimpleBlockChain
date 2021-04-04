using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ledger
{
    public class BlockMiner
    {
        private const int STATIC_MINING_PERIOD = 10000;
        private const int STATIC_MINING_FACTOR = 4;
        private CancellationTokenSource _cancellationToken;
        private readonly TransactionPool _transactionPool;

        public IList<Block> BlockChain { get; private set; }

        public BlockMiner() 
        {
            BlockChain = new List<Block>();
        }

        public BlockMiner(TransactionPool transactionPool) : this()
        {
            _transactionPool = transactionPool;
        }

        public void Start() 
        {
            if (_cancellationToken != null)
                return;

            _cancellationToken = new CancellationTokenSource();
            Task.Run(() => LoopMining(), _cancellationToken.Token);
            Console.WriteLine("Mining has started");
        }

        public void Stop() 
        {
            _cancellationToken.Cancel();
            Console.WriteLine($"{nameof(BlockMiner)} stopping...");
        }

        private void LoopMining() 
        {
            while(true)
            {
                TakeRequiredTime(() => MineBlock());
            }
        }

        private void MineBlock() 
        {
            var lastBlock = BlockChain.LastOrDefault();
            var block = new Block()
            {
                Index = lastBlock?.Index + 1 ?? 0,
                CreatedAt = DateTime.UtcNow,
                PreviousBlockHash = lastBlock?.BlockHash ?? string.Empty,
                Transactions = _transactionPool.TakeAll()
            };

            MineBlock(block);
            BlockChain.Add(block);
        }

        private void MineBlock(Block block)
        {
            var nonce = -1;
            var blockHash = string.Empty;
            var rootHash = GetHash(block.Transactions);

            do
            {
                nonce++;
                var rawBlock = $"{block.Index}{block.CreatedAt}{block.PreviousBlockHash}{rootHash}{nonce}";
                blockHash = GetHash(GetHash(rawBlock));
            }
            while (!blockHash.StartsWith(new string('0', STATIC_MINING_FACTOR)));
            /* 
             * Disclaimer:
             * STATIC_MINING_FACTOR is the number of times 0's are seek at beginig of the blockHash.
             * It could variate in a function of the number of Miners in the network(as Bitcoin does) to make it harder if the total of Miners is larger.
             * In this example we have an Static definition in order to simplefy the implementation.
             */

            block.Nonce = nonce;
            block.BlockHash = blockHash;
        }

        /// <summary>
        /// Gets the hashcode of the hash of all elements of the collection.
        /// </summary>
        /// <param name="transactions"></param>
        /// <returns>A hash code from the collection</returns>
        private static string GetHash(IEnumerable<Transaction> transactions) 
        {
            var hashedTransactions = transactions.Select(t => GetHash(GetHash($"{t.FromAgent}{t.ToAgent}{t.Amount}")));
            return GetHashByMerkleTree(hashedTransactions.ToArray());
        }

        private static string GetHash(string rawData) 
        {
            using (var sha256 = SHA256.Create()) 
            {
                var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                var stringBuilder = new StringBuilder();
                foreach (var row in bytes)
                {
                    stringBuilder.Append(row.ToString("x2")); //x2 defines a Hexadecimal string format
                }
                return stringBuilder.ToString();
            }
        }

        /// <summary>
        /// Transforms a collection into a tree and extract the Hash based on Merkle tree.
        /// </summary>
        /// <param name="leaves">Which collection item represents a leave</param>
        /// <returns>A hash from the tree</returns>
        /// <remarks>https://brilliant.org/wiki/merkle-tree/</remarks>
        private static string GetHashByMerkleTree(ICollection<string> leaves) 
        {
            if (leaves == null || !leaves.Any()) 
            {
                return string.Empty;
            }

            if (leaves.Count == 1) 
            {
                return leaves.First();
            }

            // This algoritm uses a Binary Mekle tree, so if the number to items is odd it needs to normalized
            if (leaves.Count % 2 > 0) 
            {
                leaves.Add(leaves.Last());
            }

            ICollection<string> branch = new List<string>();

            for (int i = 0; i < leaves.Count; i += 2)
            {
                var leafPair = string.Concat(leaves.ElementAt(i), leaves.ElementAt(i + 1));
                branch.Add(GetHash(GetHash(leafPair)));
            }

            // Recurrent call from the Leaves to the Main Branch
            return GetHashByMerkleTree(branch);
        }

        private void TakeRequiredTime(Action action)
        {
            var started = DateTime.Now.Millisecond;

            action();

            var ended = DateTime.Now.Millisecond;
            var leftOver = STATIC_MINING_PERIOD - (ended - started);
            Thread.Sleep(Math.Max(leftOver, 0));
        }

    }
}
