using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinNode
{
    public CoinController Coin;
    public bool Visited;

    public CoinNode()
    {
        // Empty, simplifies null checks in matching
        Coin.Value = 0;
    }

    public CoinNode(CoinController coin)
    {
        Coin = coin;
        Visited = false;
    }
}
