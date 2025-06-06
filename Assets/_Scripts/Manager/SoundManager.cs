using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    #region Atributes
    public static SoundManager Instance;
    [field: SerializeField] public AudioSource MusicSrc { get; private set; }
    [field: SerializeField] public AudioSource SfxSrc { get; private set; }
    [SerializeField] private SoundData sd;
    #endregion

    #region Startup
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void Start()
    {

    }
    #endregion

    #region Music 
    public void PlayMusic(string name)
    {
        Sound s = sd.Music.Find(m => m.Name.Equals(name));

        if (s != null)
        {
            MusicSrc.clip = s.Clip;
            MusicSrc.Play();
        }
    }

    public void StopMusic(string name)
    {
        Sound s = sd.Music.Find(m => m.Name.Equals(name));

        if (s != null)
        {
            MusicSrc.clip = s.Clip;
            MusicSrc.Stop();
        }
    }

    public void AdjustMusic(float value)
    {
        MusicSrc.volume = value;
        if (MusicSrc.volume == 0)
            MusicSrc.mute = true;
        else
            MusicSrc.mute = false;
    }
    #endregion

    #region SFX
    public void PlaySfx(string name)
    {
        Sound s = sd.Sfx.Find(m => m.Name.Equals(name));

        if (s != null)
            SfxSrc.PlayOneShot(s.Clip);
    }

    public void AdjustSfx(float value)
    {
        SfxSrc.volume = value;
        if (MusicSrc.volume == 0)
            MusicSrc.mute = true;
        else
            MusicSrc.mute = false;
    }
    #endregion
}
