using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ActualPlayer
{
	public int id;
	public Vector3 original_startposition;
	public int deaths = 0;
	public Player m_player;
}

public class GameManager : MonoBehaviour
{
	[SerializeField]
	Player m_playerPrefab;

	[SerializeField]
	float kill_height = -3f;

	ActualPlayer m_actualPlayer1;
	ActualPlayer m_actualPlayer2;
	InputManager m_input;

	void Start ()
	{
		m_input = Game.Instance.Input;

		m_actualPlayer1 = new ActualPlayer (){id=0, original_startposition=Vector3.zero};
		m_actualPlayer2 = new ActualPlayer (){id=1, original_startposition=Vector3.right * 1.5f};

		SpawnPlayer(m_actualPlayer1, Vector3.zero);
		SpawnPlayer(m_actualPlayer2, Vector3.zero);
	}

	void SpawnPlayer (ActualPlayer actualPlayer, Vector3 startposition_offset) {
		actualPlayer.m_player = (Player)Instantiate(m_playerPrefab, actualPlayer.original_startposition+startposition_offset, Quaternion.identity);
		actualPlayer.m_player.Init(m_input.GetController(actualPlayer.id));
	}
	
	void Update () {
		if (m_actualPlayer1.m_player != null && m_actualPlayer1.m_player.transform.position.y < kill_height)
		{
			m_actualPlayer1.deaths += 1;  // or is it better with m_actualPlayer2.score += 1 ?
			Destroy(m_actualPlayer1.m_player.gameObject);
			SpawnPlayer(m_actualPlayer1, Vector3.up * 3f);
		}
		if (m_actualPlayer2.m_player != null && m_actualPlayer2.m_player.transform.position.y < kill_height)
		{
			m_actualPlayer2.deaths += 1;
			Destroy(m_actualPlayer2.m_player.gameObject);
			SpawnPlayer(m_actualPlayer2, Vector3.up * 3f);
		}
	}
}
