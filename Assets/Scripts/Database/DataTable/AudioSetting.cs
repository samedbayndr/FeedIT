using System;
using UnityEngine;

public class AudioSetting : BaseTableClass, IBaseTable<AudioSetting>
{
    [NonSerialized]
    public string filePath = "JsonDataFiles/" + TableName.AudioSetting;

    public float musicLevel;
    public float environmentLevel;


    public void Init()
    {
        string jsonString;
        jsonString = FileOperation.LoadTextAsset(filePath, Extension.Json);
        var loadObj = JsonUtility.FromJson<AudioSetting>(jsonString);
        Create(loadObj);
    }


    public void Create(AudioSetting audioSetting)
    {
        if (audioSetting == null)
        {
            audioSetting = new AudioSetting();

        }

        this.musicLevel = audioSetting.musicLevel;
        this.environmentLevel = audioSetting.environmentLevel;
    }

}