using Ledger;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Linq;
using Web.Models;
using Web.Services;

namespace Web.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route("[controller]")]
    public class BlockChainController : Controller
    {
        private readonly IBlockChainService _blockChainService;

        public BlockChainController(IBlockChainService blockChainService)
        {
            _blockChainService = blockChainService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("blocks")]
        public string Get()
        {
            return JsonConvert.SerializeObject(_blockChainService.BlockMiner.BlockChain);
        }

        [HttpGet("block/{index}")]
        public string Get(int index)
        {
            Block block = null;
            if (index < _blockChainService.BlockMiner.BlockChain.Count)
                block = _blockChainService.BlockMiner.BlockChain.ElementAt(index);
            return JsonConvert.SerializeObject(block);
        }

        [HttpGet("block/last")]
        public string GetLast() 
        {
            return JsonConvert.SerializeObject(_blockChainService.BlockMiner.BlockChain.LastOrDefault());
        }

        [HttpPost("transaction/add")]
        public void Add([FromBody] TransactionModel model) 
        {
            if (model != null)
                _blockChainService.TransactionPool.Add(model.ToTransaction());
        }
    }
}
