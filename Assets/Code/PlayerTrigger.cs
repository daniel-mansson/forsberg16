using UnityEngine;
using System.Collections;

public class PlayerTrigger : MonoBehaviour
{
	Player m_player;

	void Awake()
	{
		m_player = transform.parent.GetComponent<Player>();
	}

	void OnTriggerEnter2D(Collider2D collider)
	{
		var otherPlayer = collider.transform.parent.GetComponent<Player>();
		if (otherPlayer != null && otherPlayer != m_player)
		{
			var otherBody = otherPlayer.GetComponent<Rigidbody2D>();

			var thisBody = m_player.GetComponent<Rigidbody2D>();
			Vector2 otherAngleVector = collider.transform.position - otherBody.transform.position;
			Vector2 thisAngleVector = thisBody.transform.position - this.transform.position;
			var angle = Vector2.Angle(otherAngleVector, thisAngleVector);

			if (angle < 30f)
			{
				Debug.Log("Good hit! ANGLE:" + angle);
			// This logic only works for collision of cubes, not future "ground corner" hits
				otherBody.AddForceAtPosition((otherBody.position - (Vector2)transform.position).normalized * m_player.m_bigPushPower, transform.position, ForceMode2D.Impulse);
			}
			else
			{
				Debug.Log("Bad hit! ANGLE:" + angle);
				otherBody.AddForceAtPosition((otherBody.position - (Vector2)transform.position).normalized * m_player.m_smallPushPower, transform.position, ForceMode2D.Impulse);
			}  // It would be nice with different hits

			if(m_player.m_hitEffectPrefab != null)
				Instantiate(m_player.m_hitEffectPrefab, transform.position, Quaternion.identity);
		}
	}
}
