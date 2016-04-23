using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraMovement : MonoBehaviour
{
	public float m_speed = 5f;
	public float m_zoomSpeed = 5f;
	public float m_timeSpeed = 5f;

	Camera m_camera;

	List<GameObject> m_targets = new List<GameObject>();
	Vector2 m_target = Vector2.zero;

	float m_zoomTarget = 5f;

	float m_timeScaleTarget = 1f;

	public bool m_action = false;

	public void AddTarget(GameObject target)
	{
		m_targets.Add(target);
	}

	void Awake ()
	{
		m_camera = GetComponent<Camera>();
	}
	
	void Update ()
	{
		for (int i = m_targets.Count - 1; i >= 0; i--)
		{
			if (m_targets[i] == null)
				m_targets.RemoveAt(i);
		}

		if (m_targets.Count != 0)
		{
			m_target = Vector2.zero;

			foreach (var t in m_targets)
			{
				m_target += (Vector2)t.transform.position;
			}

			m_target *= 1f / (float)m_targets.Count;
		}

		if (m_targets.Count == 2)
		{
			float d = Vector2.Distance(m_targets[0].transform.position, m_targets[1].transform.position);
			m_zoomTarget = d * 0.3f + (m_action ? 1f : 3f);
		}
		else
		{
			m_zoomTarget = 5f;
		}

		m_timeScaleTarget = m_action ? 0.1f : 1f;

		Vector3 pos = Vector3.Lerp(m_camera.transform.position, (Vector3)m_target, Time.unscaledDeltaTime * m_speed);
		pos.z = m_camera.transform.position.z;
		m_camera.transform.position = pos;

		m_camera.orthographicSize = Mathf.Lerp(m_camera.orthographicSize, m_zoomTarget, Time.unscaledDeltaTime * m_zoomSpeed);

		Time.timeScale = Mathf.Lerp(Time.timeScale, m_timeScaleTarget, Time.unscaledDeltaTime * m_timeSpeed);
		Time.fixedDeltaTime = Time.timeScale * (1f / 60f);
	}
}
