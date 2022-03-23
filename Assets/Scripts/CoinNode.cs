using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinNode : MonoBehaviour
{
    public CoinController Coin;
    public bool Visited;

    public CoinNode(CoinController coin)
    {
        Coin = coin;
        Visited = false;
    }
}
