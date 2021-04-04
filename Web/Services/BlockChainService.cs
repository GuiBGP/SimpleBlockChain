using Ledger;

namespace Web.Services
{
    public class BlockChainService : IBlockChainService
    {
        public TransactionPool TransactionPool { get; private set; }
        public BlockMiner BlockMiner { get; private set; }


        public BlockChainService()
        {
            TransactionPool = new TransactionPool();
            BlockMiner = new BlockMiner(TransactionPool);
        }

        public BlockChainService(TransactionPool transactionPool, BlockMiner blockMiner)
        {
            TransactionPool = transactionPool;
            BlockMiner = blockMiner;
        }
    }
}
