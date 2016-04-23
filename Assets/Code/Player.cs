using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
	[SerializeField]
	GameObject m_forcePointIndicator;

	[SerializeField]
	public GameObject m_hitEffectPrefab;

	[SerializeField]
	float m_maxJumpPower = 4f;

	[SerializeField]
	public float m_pushPower = 5f;

	public bool m_useKeyboard = false;

	Controller m_controller;
	Rigidbody2D m_body;
	Vector2 m_forceVector = Vector2.zero;

	float m_cooldown = 0f;

	public void Init(Controller controller)
	{
		m_controller = controller;
		m_body = GetComponent<Rigidbody2D>();
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
			jumpButton = m_controller.GetButtonDown(Xbox360ControllerButtonId.RB) && m_cooldown < 0f;
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

		m_forcePointIndicator.transform.position = transform.position + (Vector3)left * transform.localScale.x * 0.3f + Vector3.back;

		m_forceVector = right;

		if (jumpButton)
		{
			m_body.AddForceAtPosition(m_forceVector * m_maxJumpPower, (Vector2)m_forcePointIndicator.transform.position, ForceMode2D.Impulse);
			m_cooldown = 0.7f;
		}

		m_cooldown -= Time.deltaTime;
	}

	void OnDrawGizmos()
	{
		Gizmos.color = Color.blue;
		Gizmos.DrawLine(m_forcePointIndicator.transform.position, m_forcePointIndicator.transform.position + (Vector3)m_forceVector);
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
