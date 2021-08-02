using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayCycle : MonoBehaviour
{
    public Light DirectionalLight;
    public LightningPreset Preset;
    [Range(0, 24)]
    public float tTime;
    public void Start()
    {
        if (RenderSettings.sun != null)
        {
            DirectionalLight = RenderSettings.sun;
        }
        else
        {

        }


    }

    private float timePercent;
    public void Update()
    {
        if (GameTimeManager.Instance.moveTimeFlag == true)
        {
            timePercent = (GameTimeManager.Instance.getHour() / 24f) +
                          ((GameTimeManager.Instance.getMinute() / 60f) / 24f);
            updateLightning(timePercent);
        }


    }
    public void updateLightning(float pTimePercent)
    {
        RenderSettings.ambientLight = Preset.AmbientColor.Evaluate(pTimePercent);
        RenderSettings.fogColor = Preset.FogColor.Evaluate(pTimePercent);
        if (DirectionalLight != null)
        {
            DirectionalLight.color = Preset.DirectionalColor.Evaluate(pTimePercent);

            DirectionalLight.transform.localRotation =
                Quaternion.Euler(new Vector3((pTimePercent * 360f) - 90f, -170f, 0));
        }
    }

}
