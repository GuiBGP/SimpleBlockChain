using Ledger;

namespace Web.Services
{
    public interface IBlockChainService
    {
        BlockMiner BlockMiner { get; }
        TransactionPool TransactionPool { get; }
    }
}