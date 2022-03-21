using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CoinController m_HeldCoin;
    
    private bool m_IsMoving;
    private bool m_IsGrabbing;
    private float m_Speed = 5.0f;
    private float m_Direction = 0.0f;
    private float m_DestinationX;
    private float m_LeftXBound = 1.0f;
    private float m_RightXBound = 6.0f;

    private bool m_CanMoveLeft 
    { 
        get
        {
            return transform.position.x > m_LeftXBound && !m_IsGrabbing;
        }
    }
    private bool m_CanMoveRight
    {
        get
        {
            return transform.position.x < m_RightXBound && !m_IsGrabbing;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovement();

        if(Input.GetKeyDown(KeyCode.DownArrow) && !m_IsGrabbing)
        {
            StartCoroutine(GrabCoin());
        }

        if(Input.GetKeyDown(KeyCode.UpArrow) && !m_IsGrabbing)
        {
            ShootCoin();
        }
    }

    private void HandleMovement()
    {
        if (!m_IsMoving)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow) && m_CanMoveLeft)
            {
                m_IsMoving = true;
                m_Direction = -1.0f;
                m_DestinationX = transform.position.x - 1.0f;
            }

            if (Input.GetKeyDown(KeyCode.RightArrow) && m_CanMoveRight)
            {
                m_IsMoving = true;
                m_Direction = 1.0f;
                m_DestinationX = transform.position.x + 1.0f;
            }
        }

        if (m_IsMoving)
        {
            transform.position += Vector3.right * m_Direction * m_Speed * Time.deltaTime;

            if ((m_Direction == -1.0f && transform.position.x <= m_DestinationX)
                || (m_Direction == 1.0f && transform.position.x >= m_DestinationX))
            {
                transform.position = new Vector2(m_DestinationX, transform.position.y);
                m_IsMoving = false;
            }
        }
    }

    private IEnumerator GrabCoin()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.up, 100.0f, LayerMask.GetMask("FreeCoin"));

        if(hit.collider != null)
        {
            m_IsGrabbing = true;
            CoinController coinController = hit.collider.GetComponent<CoinController>();
            coinController.SetDestination(transform.position);

            while(coinController.IsMoving)
            {
                yield return null;
            }

            coinController.gameObject.transform.SetParent(transform);
            m_IsGrabbing = false;
            m_HeldCoin = coinController;
            coinController.gameObject.layer = 10;
        }
    }

    private void ShootCoin()
    {
        if(m_HeldCoin != null)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.up, 100.0f, LayerMask.GetMask("FreeCoin", "Wall"));
            Vector2 destination = new Vector2(transform.position.x, hit.collider.transform.position.y - 1.0f);
            m_HeldCoin.SetDestination(destination);
            m_HeldCoin.gameObject.layer = 11;
            m_HeldCoin.transform.parent = null;
            m_HeldCoin = null;
        }
    }
}
