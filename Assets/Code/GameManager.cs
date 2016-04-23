using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
	[SerializeField]
	Player m_playerPrefab;

	Player m_player;
	InputManager m_input;


	void Start ()
	{
		m_input = Game.Instance.Input;

		m_player = (Player)Instantiate(m_playerPrefab, Vector3.zero, Quaternion.identity);
		m_player.Init(m_input.GetController(0));

		m_player = (Player)Instantiate(m_playerPrefab, Vector3.right * 1.5f, Quaternion.identity);
		m_player.Init(m_input.GetController(1));
	}
	
	void Update () {
	
	}
}
