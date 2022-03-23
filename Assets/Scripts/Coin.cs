using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
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

    public Coin()
    {
        Value = 5;
    }
}
