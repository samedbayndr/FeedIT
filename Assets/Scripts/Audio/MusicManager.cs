using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;


public class MusicManager : MonoBehaviour
{
    #region Singleton Things
    private static MusicManager _instance;
    public static MusicManager Instance
    {
        get
        {
            return _instance;
        }
    }

    public void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }

        _instance = this;
    }
    #endregion


    public AudioClip startMenuAudioClip;
    public List<AudioClip> inGameMusicList;
    public AudioSource musicAudioSource;
    public bool isGameStarted;
    public int inGameClipCounter;

    void Start()
    {
        inGameClipCounter = 0;
        nextMusicClip();
        StartMenuUI.Instance.PlayGameEvent.AddListener(passInGameMusicList);
    }

    // Update is called once per frame
    void Update()
    {
        if (isGameStarted == true && !musicAudioSource.isPlaying)
        {
            nextMusicClip();
        }
    }

    public void passInGameMusicList()
    {
        isGameStarted = true;
        musicAudioSource.Stop();
    }

    public void nextMusicClip()
    {
        musicAudioSource.volume = TableManager.Instance.audioSettingTable.musicLevel / 100f;
        if (isGameStarted == true)
        {
            musicAudioSource.clip = inGameMusicList[inGameClipCounter];
            musicAudioSource.Play();
            inGameClipCounter = (inGameClipCounter + 1) % inGameMusicList.Count;

        }
        else
        {
            musicAudioSource.clip = startMenuAudioClip;
            musicAudioSource.Play();
        }

    }

    public void changeMusicVolume(float newVolume)
    {
        musicAudioSource.volume = newVolume / 100;
        Debug.Log("newVolume: "+newVolume+ "//// Current Audio Source Volume: "+ musicAudioSource.volume);
        TableManager.Instance.audioSettingTable.musicLevel = newVolume;
    }


}
