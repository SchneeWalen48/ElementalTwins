using UnityEngine;

public class AudioManager : MonoBehaviour
{
  public AudioClip bgm;
  void Start()
  {
    AudioSource audio = gameObject.AddComponent<AudioSource>();
    audio.clip = bgm;
    audio.loop = true;
    audio.volume = 0.7f;
    audio.Play();
  }
}
