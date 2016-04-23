using UnityEngine;
using System.Collections;

public class GroundDetection : MonoBehaviour
{
	Player m_player;
	public int count = 0;

	void Awake()
	{
		m_player = transform.parent.GetComponent<Player>();
	}

	void Update()
	{

	}

	void OnTriggerEnter2D(Collider2D collider)
	{
		count++;
	}

	void OnTriggerExit2D(Collider2D collider)
	{
		if(count > 0)
			count--;
	}
}
