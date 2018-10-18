using Nethereum.ABI.FunctionEncoding.Attributes;

namespace MHLab.Ethereum
{
    [FunctionOutput]
    public class TopScore
    {
        [Parameter("address", "player", 1)]
        public string PlayerAddress { get; set; }
        [Parameter("int", "score", 2)]
        public int Score { get; set; }
    }
}
