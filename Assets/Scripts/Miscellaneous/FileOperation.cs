using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class FileOperation
{

    public static string LoadTextAsset(string filePath, string extension)
    {
        try
        {
            string textAss="";
            string fullPath;

            string persistentPath;


            string jsonDataFilesPath = Path.Combine(Application.persistentDataPath + "/JsonDataFiles");
            if (!File.Exists(jsonDataFilesPath))
            {
                Directory.CreateDirectory(jsonDataFilesPath);
            }

            //fullPath = Path.Combine(Application.streamingAssetsPath + "/" + filePath + extension);
            //if (Application.platform == RuntimePlatform.Android)
            //{
            //    UnityWebRequest www = UnityWebRequest.Get(fullPath);
            //    www.SendWebRequest();
            //    while (!www.isDone) ;
            //    textAss = www.downloadHandler.text;
            //}
            //else textAss = File.ReadAllText(fullPath);


            string loadedJson;
            persistentPath = Path.Combine(Application.persistentDataPath + "/" + filePath + extension);
            if (!File.Exists(persistentPath))
            {

                using (FileStream fs = File.Create(persistentPath))
                {
                    //Debug.Log(persistentPath + " YARATILDI");
                    Debug.Log("TextAss: "+ textAss);
                    
                    //Eğer Streaming Asset pathinde aranan datatable varsa içeriği okunup dönülüyor.. 
                    string streamingAssetPath = Path.Combine(Application.streamingAssetsPath + "/" + filePath + extension);
                    if (File.Exists(streamingAssetPath))
                    {
                        
                        loadedJson = File.ReadAllText(streamingAssetPath);
                        //Asıl pathdeki(Appdata yolundaki) dosyaya streamingasset klasöründeki datatable dosyasının içeriği kaydediliyor. 
                        Byte[] streaminAssetTextByte = new UTF8Encoding(true).GetBytes(loadedJson);
                        fs.Write(streaminAssetTextByte, 0, streaminAssetTextByte.Length);
                        return loadedJson;

                    }

                    Byte[] textAssByte = new UTF8Encoding(true).GetBytes(textAss);
                    fs.Write(textAssByte, 0, textAssByte.Length);
                    return textAss;
                }
            }
            else
            {
                loadedJson = File.ReadAllText(persistentPath);
                //Debug.Log("LoadedJson: " + loadedJson);
                return loadedJson;
            }




            return textAss;
        }
        catch (System.Exception m)
        {
            Debug.Log(m.Message + " hatasını aldınız. Muhtemelen filePath hatalı!");

            throw;
        }

    }

    public static bool SaveTextAsset<T>(T saveObj, string filePath, string extension)
    {
        try
        {
            string textAss = JsonUtility.ToJson(saveObj, true);
            //string realPath = Path.Combine(Application.streamingAssetsPath + "/" + filePath + extension);
            string realPath = Path.Combine(Application.persistentDataPath + "/" + filePath + extension);
            //Debug.Log(realPath);
            using (StreamWriter writer = new StreamWriter(realPath, false))
            {
                writer.Write(textAss);
            }
            //Debug.Log("<color=blue>" + typeof(T) + " başarıyla kaydedildi.</color>");

            return true;
        }
        catch (System.Exception m)
        {
            Debug.Log(m.Message);

            return false;
        }

    }


    public static bool SaveTextAssetWithBaseTableClass(BaseTableClass saveObj, string filePath, string extension)
    {
        try
        {
            string textAss = JsonUtility.ToJson(saveObj, true);
            //string realPath = Path.Combine(Application.streamingAssetsPath + "/" + filePath + extension);
            string realPath = Path.Combine(Application.persistentDataPath + "/" + filePath + extension);
            using (StreamWriter writer = new StreamWriter(realPath, false))
            {
                writer.Write(textAss);
            }
            //Debug.Log("<color=red>" + saveObj.GetType() + " başarıyla kaydedildi.</color>");

            return true;
        }
        catch (System.Exception m)
        {
            //Debug.Log(m.Message);

            return false;
        }

    }
}
