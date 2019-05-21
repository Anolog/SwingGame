using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private static float EPSILON = 0.001f;
    public static PlayerController Instance;
    public GameObject m_DeathPlatform;
    public GameObject m_ShotLocation;
    public Text m_ScoreText;

    private SpringJoint2D m_Rope;
    public int m_MaxRopeFrameCount;
    private int m_RopeFrameCount;

    public LineRenderer m_LineRender;
    public TrailRenderer m_Trail;

    private bool m_InputHeld = false;
    private Vector3 m_RopePosition;
    private Vector3 m_FiringAngle;

    private Rigidbody2D m_RigidBody;
    private Vector2 m_ForceAmount = new Vector2(5f, 0);

    private const float MAX_ROPE_DISTANCE = 4.0f;
    private const float MAX_X_VELOCITY = 30.0f;

    public CircleCollider2D m_Collider;

    public GameObject m_SpawnPoint;

    private const float MAX_DEATH_COUNTER = 3.0f;
    private float m_DeathCounter = MAX_DEATH_COUNTER;

    private int m_Score = 0;

    private bool m_bDidTriggerCollectionItem = false;
    private GameObject m_CollectionItemTriggered;

	// Use this for initialization
	void Start ()
    {
        Instance = this;
        m_RigidBody = this.GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        m_RopePosition = this.transform.position;

        if (System.Math.Abs(m_RigidBody.velocity.y) < EPSILON)
        {

            m_DeathCounter -= Time.deltaTime;

            if (m_DeathCounter <= 0)
            {
                m_DeathCounter = MAX_DEATH_COUNTER;
                KillPlayer();
            }
        }

        else if (System.Math.Abs(m_RigidBody.velocity.y) > EPSILON && m_DeathCounter != MAX_DEATH_COUNTER)
        {
            m_DeathCounter = MAX_DEATH_COUNTER;
        }
	}

    private void FixedUpdate()
    {
        if (m_RigidBody.velocity.x > MAX_X_VELOCITY)
        {
            m_RigidBody.velocity = new Vector2(MAX_X_VELOCITY, 0);
        }

        if (m_InputHeld == true && m_Rope != null)
        {
            //m_RigidBody.AddForce(m_ForceAmount, ForceMode2D.Force);
            //m_RigidBody.AddTorque(10);

            m_RigidBody.velocity += new Vector2(0.1f, 0);

        }

#if UNITY_STANDALONE || UNITY_EDITOR
        if (GameManager.Instance.GetCurrentGameState() == GameManager.GAMESTATE.IN_GAME_MODE_ENDLESS ||
            GameManager.Instance.GetCurrentGameState() == GameManager.GAMESTATE.IN_GAME_MODE_LEVELS)
        {
            if (Input.GetMouseButtonDown(0) == true && m_InputHeld == false)
            {
                HandleInputBegan();
            }

            else if (Input.GetMouseButtonUp(0) == true && m_InputHeld == true)
            {
                HandleInputEnded();
            }
        }

#elif UNITY_IOS || UNITY_ANDROID || UNITY_WP8 || UNITY_IPHONE
        if (GameManager.Instance.GetCurrentGameState() == GameManager.GAMESTATE.IN_GAME_MODE_ENDLESS_MODE_ENDLESS)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                HandleInputBegan();
            }

            else if (Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                HandleInputEnded();
            }
        }
#endif

    }

    private void LateUpdate()
    {
        if (m_Rope != null)
        {
            m_LineRender.enabled = true;
            m_LineRender.positionCount = 2;
            m_LineRender.SetPosition(0, this.transform.position);
            m_LineRender.SetPosition(1, m_Rope.connectedAnchor);

        }

        else
        {
            m_LineRender.enabled = false;
        }

        m_ScoreText.text = m_Score.ToString();

        //TODO: Try moving this code to the collection item itself, to see if it handles it better.
        if (m_bDidTriggerCollectionItem == true)
        {
            if (GameManager.Instance.GetCurrentGameState() == GameManager.GAMESTATE.IN_GAME_MODE_ENDLESS)
            {
                GameObject.Destroy(m_CollectionItemTriggered, 0.1f);

                m_bDidTriggerCollectionItem = false;
                m_CollectionItemTriggered = null;
                //Add to points or whatever, depending on the gamemode?

            }
        }
    }

    private void HandleInputBegan()
    {
        m_InputHeld = true;
        m_ForceAmount.x *= m_RigidBody.velocity.x;
        m_RigidBody.AddForce(m_ForceAmount, ForceMode2D.Impulse);
        ActivateLine();
        //Debug.Log("Current Vel:" + m_RigidBody.velocity.x);
    }

    private void HandleInputEnded()
    {
        m_InputHeld = false;

        //Delete the line
        GameObject.DestroyImmediate(m_Rope);
    }

    public int GetScore()
    {
        return m_Score;
    }

    public void SetScore(int aScore)
    {
        m_Score = aScore;
    }

    public void AddToScore()
    {
        m_Score++;
    }

    public void AddToScore(int aScore)
    {
        m_Score += aScore;
    }

    private void ActivateLine()
    {
        m_RopePosition =  this.transform.position;
        m_FiringAngle = m_ShotLocation.transform.position - m_RopePosition;

        RaycastHit2D hit = Physics2D.Raycast(m_RopePosition, m_FiringAngle, MAX_ROPE_DISTANCE);

        if (hit.collider != null )
        {
            if (hit.transform.tag == "Platform")
            {
                SpringJoint2D newRope = this.gameObject.AddComponent<SpringJoint2D>();
                newRope.enableCollision = false;
                newRope.frequency = 0;
                newRope.connectedAnchor = hit.point;
                newRope.distance = MAX_ROPE_DISTANCE;
                newRope.enabled = true;
                //newRope.maxDistanceOnly = false;
                newRope.autoConfigureConnectedAnchor = false;

                GameObject.DestroyImmediate(m_Rope);
                m_Rope = newRope;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Death")
        {
            KillPlayer();
        }

        if (collision.gameObject.tag == "Platform")
        {
            RaycastHit2D hit = Physics2D.Raycast(this.transform.position, Vector2.up);

            if (hit.collider != null && hit.transform.tag == "Platform")
            {
                KillPlayer();
            }
        }
    }

	private void OnTriggerEnter2D(Collider2D collision)
	{
        if (collision.gameObject.tag == "CollectionItem")
        {
            Debug.Log("Colliding with CollectionItem");

            m_bDidTriggerCollectionItem = true;
            m_CollectionItemTriggered = collision.gameObject;

        }
	}

	private void KillPlayer()
    {
        //Debug.Log("Player died");
        m_RigidBody.velocity = Vector2.zero;
        m_DeathCounter = MAX_DEATH_COUNTER;

        if (m_Rope != null)
        {
            GameObject.Destroy(m_Rope);
            m_LineRender.enabled = false;
            m_InputHeld = false;
        }

        InGameUIManager.Instance.ShowUI();

        GameManager.Instance.SetGameState(GameManager.GAMESTATE.GAME_OVER);
    }

    public void RespawnPlayer()
    {
        m_Trail.Clear();
        m_DeathCounter = MAX_DEATH_COUNTER;
        m_DeathPlatform.GetComponent<DeathPlatform>().ResetPlatformPos();
        transform.position = m_SpawnPoint.transform.position;
        m_RigidBody.velocity = Vector2.zero;

        m_Score = 0;
    }
}
