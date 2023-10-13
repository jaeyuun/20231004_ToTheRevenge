using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    [SerializeField] private AudioSource bgm;
    [SerializeField] private AudioSource effect;
    [SerializeField] private AudioClip[] musics;

    private static AudioController Instance;
    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        } else
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
    }

    public void PlayBGM()
    {
        bgm.Play();
    }

    public void StopBGM()
    {
        bgm.Stop();
    }

    public void PlayEffect(int index)
    {
        effect.clip = musics[index];
        effect.Play();
    }

    public void StopEffect(int index)
    {
        effect.clip = musics[index];
        effect.Stop();
    }
}
