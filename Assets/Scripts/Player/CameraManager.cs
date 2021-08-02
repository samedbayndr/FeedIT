using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;


//Kameraların kontrolü için ise CameraController scriptine göz atabilirsin.



public static class GameCameraName
{
    public const string FreeCamera = "FreeCamera";
    public const string BuildingCamera = "BuildingCamera";
    public const string ShepherdFollowCamera = "ShepherdFollowCamera";

}

[Serializable]
public class GameCamera
{
    public string cameraName;
    public CinemachineVirtualCamera cinemachineCamera;
    private GameCamera() { }

    public CinemachineVirtualCamera getCamera()
    {
        return this.cinemachineCamera;
    }
}

public class CameraManager : MonoBehaviour
{
    private static CameraManager _instance;

    public static CameraManager Instance
    {
        get
        {
            return _instance;
        }
    }

    public void Awake()
    {
        if (_instance != null || _instance != this)
        {
            Destroy(_instance);
        }

        _instance = this;
    }

    [HideInInspector]
    public GameCamera currentCamera;

    public List<GameCamera> allCameras;

    // Kameraya geçiş yapıldığında geçiş yapılan kameranın priority değeri 15 olacaktır.
    // Diğer kameralar 10 değerinde kalacaktır.
    public int defaultPriority = 10;
    public int increasePriority = 5;
    public int decreasePriority = 5;

    private int _cameraValue = 0;

    public int CameraValue
    {
        get
        {
            return _cameraValue;
        }
        set
        {
            //Buraya dikkat et! allCameras. Count eğer hata veriyorsa veya sıfır geliyorsa önce CameraValue initiliaze oluyor!!!! 
            //if (allCameras.Count == 0)
            _cameraValue = value % allCameras.Count;
            //_cameraValue = value;
            if (_cameraValue == 0)
                changeCameraToFree();
            else if (_cameraValue == 1)
            {
                if (!WorkerManager.Instance.currentShepherd)
                    _cameraValue++;
                else if (WorkerManager.Instance.currentShepherd.workerLocation == WorkerLocation.onFarm)
                    _cameraValue++;
                else
                    changeCameraToShepherdFollow();

            }
            //else if ile devam etmiyor çünkü eğer currentShepherd yoksa doğrudan building cameraya geçiyor..
            if (_cameraValue == 2)
                changeCameraToBuilding();

        }
    }
    public void Start()
    {
        initCamera();
    }

    public void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
            CameraValue++;
    }

    public void initCamera()
    {
        foreach (var gameCamera in allCameras)
        {
            if (gameCamera.cameraName == GameCameraName.FreeCamera)
            {
                gameCamera.cinemachineCamera.Priority = defaultPriority + increasePriority;
                currentCamera = gameCamera;
            }
            else
                gameCamera.cinemachineCamera.Priority = defaultPriority;
        }
    }


    public void changeCameraToFree()
    {
        currentCamera.cinemachineCamera.Priority = defaultPriority;
        currentCamera = allCameras.Find(a => a.cameraName == GameCameraName.FreeCamera);

        if (currentCamera != null)
            currentCamera.cinemachineCamera.Priority = defaultPriority + increasePriority;
    }

    public void changeCameraToShepherdFollow()
    {

        currentCamera.cinemachineCamera.Priority = defaultPriority;
        currentCamera = allCameras.Find(a => a.cameraName == GameCameraName.ShepherdFollowCamera);
        currentCamera.cinemachineCamera.Follow = WorkerManager.Instance.currentShepherd.transform;
        currentCamera.cinemachineCamera.LookAt = WorkerManager.Instance.currentShepherd.transform;

        if (currentCamera != null)
            currentCamera.cinemachineCamera.Priority = defaultPriority + increasePriority;
    }

    public void changeCameraToBuilding()
    {
        currentCamera.cinemachineCamera.Priority = defaultPriority;
        currentCamera = allCameras.Find(a => a.cameraName == GameCameraName.BuildingCamera);
        if (currentCamera != null)
            currentCamera.cinemachineCamera.Priority = defaultPriority + increasePriority;
    }

}
