using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class OptionMenuUI : MonoBehaviour
{

    #region Singleton Things
    private static OptionMenuUI _instance;
    public static OptionMenuUI Instance
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

    public GameObject OptionMenuUIParentGO;

    public Slider musicLevelSlider;
    public Slider environmentLevelSlider;

    public void openOptionMenu()
    {
        musicLevelSlider.value = TableManager.Instance.audioSettingTable.musicLevel;
        environmentLevelSlider.value = TableManager.Instance.audioSettingTable.environmentLevel;

        musicLevelSlider.onValueChanged.AddListener(delegate { updateMusicLevel();});
        environmentLevelSlider.onValueChanged.AddListener(delegate { updateEnvironmentLevel();});

        OptionMenuUIParentGO.SetActive(true);
    }
    public void returnBack()
    {
        OptionMenuUIParentGO.SetActive(false);
    }

    public void updateMusicLevel()
    {
        MusicManager.Instance.changeMusicVolume(musicLevelSlider.value);
        
    }

    public void resetGame()
    {

        //string[] filePaths = Directory.GetFiles(Application.persistentDataPath+ "/JsonDataFiles");
        //foreach (string filePath in filePaths)
        //{
        //    File.Delete(filePath);
        //}
        if (Directory.Exists(Application.persistentDataPath + "/JsonDataFiles"))
        {
            Directory.Delete(Application.persistentDataPath + "/JsonDataFiles", true);
          
        }
        ApplicationManager.Instance.quitGame();
    }
    
    public void updateEnvironmentLevel()
    {
        //MusicManager.Instance.changeMusicVolume(musicLevelSlider.value);
        EnvironmentSoundManager.Instance.changeMusicVolume(environmentLevelSlider.value);
    }

    void Start()
    {
        
    }

}
