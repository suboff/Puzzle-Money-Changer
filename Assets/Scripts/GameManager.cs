using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool NeedsUpdate
    { 
        get; set; 
    }

    private CoinController[,] m_FreeCoins;

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
        }
    }

    private void ScanForChains()
    {
        // Be smarter, only rebuild dirty sections
        RebuildCoinList();

        for (int y = 0; y < m_FreeCoins.GetLength(0); y++)
        {
            string row = "";
            for (int x = 0; x < m_FreeCoins.GetLength(1); x++)
            {
                string value;
                if (m_FreeCoins[y, x] == null)
                {
                    value = "x";
                }
                else
                {
                    value = "c";
                }
                row += value;
            }
            Debug.Log(row);
        }
    }

    private void CoinPositionToIndex(Vector2 position, out int x, out int y)
    {
        y = Mathf.FloorToInt(14.0f - position.y);
        x = Mathf.FloorToInt(position.x);
    }

    private void RebuildCoinList()
    {
        m_FreeCoins = new CoinController[12, 7];

        GameObject[] coins = GameObject.FindGameObjectsWithTag("FreeCoin");
        int yIndex, xIndex;

        foreach (GameObject coin in coins)
        {
            CoinPositionToIndex(coin.transform.position, out xIndex, out yIndex);
            m_FreeCoins[yIndex, xIndex] = coin.GetComponent<CoinController>();
        }
    }
}
