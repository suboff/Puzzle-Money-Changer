using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool NeedsUpdate
    { 
        get; set; 
    }
    public int BoardWidth
    {
        get
        {
            return m_FreeCoins.GetLength(1);
        }
    }
    public int BoardHeight
    {
        get
        {
            return m_FreeCoins.GetLength(0);
        }
    }

    private GameObject[] m_CoinObjects;
    private CoinNode[] m_CoinNodes;
    private CoinNode[,] m_FreeCoins;
    public CoinNode[,] FreeCoins
    {
        get
        {
            return m_FreeCoins;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        RebuildCoinList();
    }

    // Update is called once per frame
    void Update()
    {
        if(NeedsUpdate)
        {
            ScanForChains();
            NeedsUpdate = false;
        }
    }

    private void ScanForChains()
    {
        // Be smarter, only rebuild dirty sections
        RebuildCoinList();

        Dictionary<int, List<CoinNode>> coinNodeValueDictionary = new Dictionary<int, List<CoinNode>>()
        {
            { 1, new List<CoinNode>() },
            { 5, new List<CoinNode>() },
            { 10, new List<CoinNode>() },
            { 50, new List<CoinNode>() },
            { 100, new List<CoinNode>() },
            { 500, new List<CoinNode>() }
        };

        // Build the dictionary
        foreach(CoinNode coinNode in m_CoinNodes)
        {
            if(coinNode.Coin.Value != 0)
            {
                coinNodeValueDictionary[coinNode.Coin.Value].Add(coinNode);
            }
        }

        // For each vaule in the dictionary (i.e. for each value of coin)
        foreach(KeyValuePair<int, List<CoinNode>> coinsValuePair in coinNodeValueDictionary)
        {
            int currentValue = coinsValuePair.Key;

            foreach (CoinNode coin in coinsValuePair.Value)
            {
                List<CoinNode> potentialChain = new List<CoinNode>();
                Stack<CoinNode> coinNodes = new Stack<CoinNode>();
                if (!coin.Visited)
                {
                    coinNodes.Push(coin);
                }

                while (coinNodes.Count > 0)
                {
                    CoinNode currentNode = coinNodes.Pop();
                    currentNode.Visited = true;
                    potentialChain.Add(currentNode);

                    if (currentNode.Coin.Up != null && currentNode.Coin.Up.Coin.Value == currentValue && !currentNode.Coin.Up.Visited)
                    {
                        coinNodes.Push(currentNode.Coin.Up);
                    }
                    if (currentNode.Coin.Down != null && currentNode.Coin.Down.Coin.Value == currentValue && !currentNode.Coin.Down.Visited)
                    {
                        coinNodes.Push(currentNode.Coin.Down);
                    }
                    if (currentNode.Coin.Left != null && currentNode.Coin.Left.Coin.Value == currentValue && !currentNode.Coin.Left.Visited)
                    {
                        coinNodes.Push(currentNode.Coin.Left);
                    }
                    if (currentNode.Coin.Right != null && currentNode.Coin.Right.Coin.Value == currentValue && !currentNode.Coin.Right.Visited)
                    {
                        coinNodes.Push(currentNode.Coin.Right);
                    }
                }

                // Stack empty --> searched every connecting node from starting node of the same value
                // If potentialChain length greater than threshold, valid chain found
                if (potentialChain.Count >= GetMinimumChainLengthForValue(currentValue))
                {
                    Debug.Log("Found chain");
                    foreach (CoinNode chainCoin in potentialChain)
                    {
                        Destroy(chainCoin.Coin.gameObject);
                    }
                }
            }
        }
    }

    private void CoinPositionToIndex(Vector2 position, out int x, out int y)
    {
        y = Mathf.FloorToInt(14.0f - position.y);
        x = Mathf.FloorToInt(position.x);
    }

    private int GetMinimumChainLengthForValue(int value)
    {
        switch(value)
        {
            case 1:
                return 5;
            case 5:
                return 2;
            case 10:
                return 5;
            case 50:
                return 2;
            case 100:
                return 5;
            case 500:
                return 2;
            default:
                return 999;
        }
    }

    private void RebuildCoinList()
    {
        m_FreeCoins = new CoinNode[12, 7];

        m_CoinObjects = GameObject.FindGameObjectsWithTag("FreeCoin");
        m_CoinNodes = new CoinNode[m_CoinObjects.Length];
        int yIndex, xIndex;

        for (int i = 0; i < m_CoinObjects.Length; i++)
        {
            m_CoinNodes[i] = new CoinNode(m_CoinObjects[i].GetComponent<CoinController>());
            CoinPositionToIndex(m_CoinObjects[i].transform.position, out xIndex, out yIndex);
            Debug.Log("Trying Y: " + yIndex + ", X: " + xIndex);
            m_FreeCoins[yIndex, xIndex] = m_CoinNodes[i];
        }
    }
}
