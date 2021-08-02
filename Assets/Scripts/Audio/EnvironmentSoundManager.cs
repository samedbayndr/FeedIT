using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentSoundManager : MonoBehaviour
{
    #region Singleton Things
    private static EnvironmentSoundManager _instance;
    public static EnvironmentSoundManager Instance
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

    public AudioSource roosterCrowingSource;


    public List<AudioSource> environmentAudioSources;
    

    void Start()
    {
        environmentAudioSources = new List<AudioSource>()
        {
            roosterCrowingSource
        };


        setInitVolumeLevel();

        GameTimeManager.MorningSixEvent.AddListener(awakeRoosters);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void changeMusicVolume(float newVolume)
    {
        for (int i = 0; i < environmentAudioSources.Count; i++)
        {
            environmentAudioSources[i].volume = newVolume / 100;
        }
        TableManager.Instance.audioSettingTable.environmentLevel = newVolume;
    }

    public void setInitVolumeLevel()
    {
        changeMusicVolume(TableManager.Instance.audioSettingTable.environmentLevel);
    }


    public void awakeRoosters()
    {
        if (TableManager.Instance.inventoryTable.rooster != 0)
            roosterCrowingSource.Play();
    }
}
