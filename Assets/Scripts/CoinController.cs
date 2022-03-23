using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinController : MonoBehaviour
{
    private GameManager m_GameManager;
    
    private int m_MoveDirection;
    private Vector2 m_Destination;
    private float m_Speed = 20.0f;

    public int Value;
    public int XIndex
    {
        get
        {
            return Mathf.FloorToInt(transform.position.x);
        }
    }
    public int YIndex
    {
        get
        {
            return Mathf.FloorToInt(14.0f - transform.position.y);
        }
    }

    public CoinNode Left
    {
        get
        {
            if (XIndex - 1 < 0)
            {
                return null;
            }
            else
            {
                return m_GameManager.FreeCoins[YIndex, XIndex - 1];
            }
        }
    }
    public CoinNode Right
    {
        get
        {
            if (XIndex + 1 >= m_GameManager.BoardWidth)
            {
                return null;
            }
            else
            {
                return m_GameManager.FreeCoins[YIndex, XIndex + 1];
            }
        }
    }
    public CoinNode Up
    {
        get
        {
            if (YIndex - 1 < 0)
            {
                return null;
            }
            else
            {
                return m_GameManager.FreeCoins[YIndex - 1, XIndex];
            }
        }
    }
    public CoinNode Down
    {
        get
        {
            if (YIndex + 1 >= m_GameManager.BoardHeight)
            {
                return null;
            }
            else
            {
                return m_GameManager.FreeCoins[YIndex + 1, XIndex];
            }
        }
    }

    public bool IsMoving
    {
        get
        {
            return m_MoveDirection != 0;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        m_GameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (m_MoveDirection != 0)
        {
            transform.position += Vector3.up * m_MoveDirection * m_Speed * Time.deltaTime;

            if((m_MoveDirection > 0 && transform.position.y > m_Destination.y)
                || (m_MoveDirection < 0 && transform.position.y < m_Destination.y))
            {
                transform.position = new Vector2(transform.position.x, m_Destination.y);
                m_MoveDirection = 0;
                // Change to only flag when coin was traveling up
                m_GameManager.NeedsUpdate = true;
            }
        }
    }

    public void SetDestination(Vector2 destination)
    {
        m_Destination = destination;
        if(m_Destination.y < transform.position.y)
        {
            m_MoveDirection = -1;
        }
        else
        {
            m_MoveDirection = 1;
        }
    }
}
