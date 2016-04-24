using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class AudioEvent : EventArgs
{
	public AudioEvent(string key, Vector3 pos)
	{
		Key = key;
		Pos = pos;
	}

	public string Key;
	public Vector3 Pos;
}

public class AudioManager : MonoBehaviour
{
	[System.Serializable]
	public class AudioEntry
	{
		public string key;
		public List<AudioClip> clips;
	}

	public List<AudioEntry> m_audio;

	public AudioClip m_music;

	void Start()
	{
		EventManager.Instance.RegisterEvent<AudioEvent>(OnAudio);

		var go = new GameObject();
		var src = go.AddComponent<AudioSource>();
		src.clip = m_music;
		src.Play();
		go.transform.position = Vector3.zero;
	}

	void OnDestroy()
	{
		EventManager.Instance.UnregisterEvent<AudioEvent>(OnAudio);
	}

	void Update ()
	{
	
	}

	void OnAudio(AudioEvent args)
	{
		var audio = m_audio.FirstOrDefault(a => a.key == args.Key);
		if (audio != null)
		{
			var go = new GameObject();
			var src =go.AddComponent<AudioSource>();
			var dst = go.AddComponent<DestroyInTime>();
			dst.time = 4f;
			src.PlayOneShot(audio.clips[UnityEngine.Random.Range(0, audio.clips.Count)]);
			go.transform.position = args.Pos;
		}
	}

	[ContextMenu("SDFG")]
	void test()
	{
		EventManager.Instance.SendEvent(new AudioEvent("hit", Vector3.zero));
	}
}
