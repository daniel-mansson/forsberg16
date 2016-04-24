using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
	[SerializeField]
	GameObject m_forcePointIndicator;

	public GameObject m_hitEffectPrefab;

	[SerializeField]
	float m_maxJumpPower = 4f;

	public float m_bigPushPower = 5f;
	public float m_smallPushPower = 2f; // Can I have two SerializeField like this?

	public bool m_useKeyboard = false;

	public List<GroundDetection> m_groundDetectors = new List<GroundDetection>();
	public List<GameObject> m_corners = new List<GameObject>();

	public Material m_groundMat;
	public Material m_airMat;

	public int m_numAirJumps = 1;

	int m_airJumps = 1;
	Controller m_controller;
	Rigidbody2D m_body;
	Renderer m_renderer;
	Vector2 m_forceVector = Vector2.zero;

	float m_cooldown = 0f;

	public void Init(Controller controller)
	{
		m_controller = controller;
		m_body = GetComponent<Rigidbody2D>();
		m_renderer = GetComponent<MeshRenderer>();
	}

	void Start ()
	{
	
	}
	
	void Update ()
	{
		var left = Vector2.zero;
		var right = Vector2.zero;
		bool jumpButton = false;

		if (!m_useKeyboard)
		{
			left = m_controller.GetJoystick(Xbox360ControllerJoystickId.Left);
			right = m_controller.GetJoystick(Xbox360ControllerJoystickId.Right);
			jumpButton = m_controller.GetButtonDown(Xbox360ControllerButtonId.RB);
		}
		else
		{
			if (Input.GetKey(KeyCode.W))
				left += Vector2.up;
			if (Input.GetKey(KeyCode.S))
				left += Vector2.down;
			if (Input.GetKey(KeyCode.A))
				left += Vector2.left;
			if (Input.GetKey(KeyCode.D))
				left += Vector2.right;

			if (left.magnitude > 0.1f)
				left.Normalize();

			if (Input.GetKey(KeyCode.UpArrow))
				right += Vector2.up;
			if (Input.GetKey(KeyCode.DownArrow))
				right += Vector2.down;
			if (Input.GetKey(KeyCode.LeftArrow))
				right += Vector2.left;
			if (Input.GetKey(KeyCode.RightArrow))
				right += Vector2.right;

			if (right.magnitude > 0.1f)
				right.Normalize();

			jumpButton = Input.GetKeyDown(KeyCode.Space) && m_cooldown < 0f;
		}

		bool onGround = false;

		foreach (var gd in m_groundDetectors)
		{
			if (gd.count > 0)
			{
				onGround = true;
				break;
			}
		}

		m_forcePointIndicator.transform.position = transform.position + (Vector3)left * transform.localScale.x * 0.3f + Vector3.back;

		m_forceVector = right;

		if (jumpButton)
		{
			if ((onGround && m_cooldown < 0f) || (!onGround && m_airJumps > 0))
			{
				Debug.Log("Yes " + onGround + " " + m_airJumps + " " + m_cooldown);

				float factor = onGround ? 1f : 0.7f;
				m_body.AddForceAtPosition(factor * m_forceVector * m_maxJumpPower, (Vector2)m_forcePointIndicator.transform.position, ForceMode2D.Impulse);
				m_cooldown = 0.7f;
				//foreach (var gd in m_groundDetectors)
				//	gd.count = 0;
				if (!onGround)
					m_airJumps--;
				//			Debug.Log("test");
			}
			else
			{
				Debug.Log("No " + onGround + " " + m_airJumps + " " + m_cooldown);
			}
		}

		if(onGround)
			m_airJumps = m_numAirJumps;

		if (onGround || m_airJumps > 0)
		{
			m_renderer.material = m_groundMat;
		}
		else
		{
			m_renderer.material = m_airMat;
		}

		m_cooldown -= Time.deltaTime;


	}

	void OnDrawGizmos()
	{
		Gizmos.color = Color.blue;
		Gizmos.DrawLine(m_forcePointIndicator.transform.position, m_forcePointIndicator.transform.position + (Vector3)m_forceVector);
	}

	public float ClosestDistanceToHit(Player other)
	{
		float minDist = float.PositiveInfinity;

		foreach (var c in m_corners)
		{
			foreach (var g in other.m_groundDetectors)
			{
				float dist = Vector2.Distance(c.transform.position, g.transform.position);

				if (dist < minDist)
					minDist = dist;
			}
		}

		return minDist;
	}

	void FixedUpdate()
	{

	}
	/*
	void OnCollisionEnter2D(Collision2D collision)
	{
		var otherPlayer = collision.gameObject.GetComponent<Player>();
		if (otherPlayer != null)
		{
			
		}
	}

	void OnTriggerEnter2D(Collider2D collider)
	{
		var otherPlayer = collider.gameObject.GetComponent<Player>();
		if (otherPlayer != null)
		{
			var otherBody = otherPlayer.GetComponent<Rigidbody2D>();

			m_body.AddForce((m_body.position - otherBody.position).normalized * 6f, ForceMode2D.Impulse);
		}
	}*/
}
