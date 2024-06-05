using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager inst;

    [SerializeField] AudioSource sound;
    [SerializeField] AudioSource music;

    public Sound[] clips;


    void Awake()
    {
        inst = this;
    }

    public void PlaySound(SoundName name)
    {
        foreach (var item in clips)
        {
            if (item.name == name)
            {
                sound.PlayOneShot(item.clip);
                break;
            }
        }
    }


    public void MusicMute(bool value)
    {
        music.mute = value;
    }

    public void SoundMute(bool value)
    {
        sound.mute = value;
    }

    public void MusicController(float value)
    {
        music.volume = value;
    }

    public void SoundController(float value)
    {
        sound.volume = value;
    }
}


[System.Serializable]
public class Sound
{
    public SoundName name;
    public AudioClip clip;
}
public enum SoundName
{
    BtnClick,
    GameDraw,
    GameWin,
    Ball,
    FourConnectedBall,
}



