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

			otherBody.AddForceAtPosition((otherBody.position - (Vector2)transform.position).normalized * m_player.m_pushPower, transform.position, ForceMode2D.Impulse);

			if(m_player.m_hitEffectPrefab != null)
				Instantiate(m_player.m_hitEffectPrefab, transform.position, Quaternion.identity);
		}
	}
}
