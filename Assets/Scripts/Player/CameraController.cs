using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float cameraSpeed = 200;
    //public bool isCameraOnLimitUpper = true;
    //public bool isCameraOnLimitLower = true;
    //public bool isCameraOnLimitLeft = true;
    //public bool isCameraOnLimitRight = true;
    //public bool isCameraOnLimitForward = true;
    //public bool isCameraOnLimitBackward = true;
    //public bool isCameraOnLimitBackwardLeft = true;
    //public float mapBoundary = new Rect(-1320,350,,350)

    // 0:Left, 1:Right, 2:Forward, 3:Backward, 4:Lower, 5:Upper
    private float[] mapBoundary = new float[] {-1320, 390, 1700, -850, 20, 350};
    // 0:Left, 1:Right, 2:Forward, 3:Backward
    private float[] buildBoundary = new float[] {45, 410, -300, -600, 40, 210};

    // Start is called before the first frame update
    //void Start()
    //{

    //}
    

    void Update()
    {
        //Free Camera
        if (CameraManager.Instance.currentCamera.cameraName == GameCameraName.FreeCamera)
        {
            if (Input.GetKey(KeyCode.A))
            {
                if (CameraManager.Instance.currentCamera.cinemachineCamera.transform.position.x > mapBoundary[0])
                    CameraManager.Instance.currentCamera.cinemachineCamera.transform.position += Vector3.left * (Time.deltaTime * cameraSpeed);
            }
            if (Input.GetKey(KeyCode.D))
            {
                if (CameraManager.Instance.currentCamera.cinemachineCamera.transform.position.x < mapBoundary[1])
                    CameraManager.Instance.currentCamera.cinemachineCamera.transform.position += Vector3.right * (Time.deltaTime * cameraSpeed);
            }
            if (Input.GetKey(KeyCode.W))
            {
                if (CameraManager.Instance.currentCamera.cinemachineCamera.transform.position.z < mapBoundary[2])
                    CameraManager.Instance.currentCamera.cinemachineCamera.transform.position += Vector3.forward * (Time.deltaTime * cameraSpeed);
            }
            if (Input.GetKey(KeyCode.S))
            {
                if (CameraManager.Instance.currentCamera.cinemachineCamera.transform.position.z > mapBoundary[3])
                    CameraManager.Instance.currentCamera.cinemachineCamera.transform.position += Vector3.back * (Time.deltaTime * cameraSpeed);
            }

            var wheelAxis = Input.GetAxis("Mouse ScrollWheel");
            if (wheelAxis > 0f)
            {
                if (CameraManager.Instance.currentCamera.cinemachineCamera.transform.position.y < mapBoundary[4])
                    return; 
                
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit point; 
                Physics.Raycast(ray, out point, 25);
                Vector3 Scrolldirection = ray.GetPoint(5);

                float step = 7500 * Time.deltaTime;
                CameraManager.Instance.currentCamera.cinemachineCamera.transform.position = Vector3.MoveTowards(CameraManager.Instance.currentCamera.cinemachineCamera.transform.position, Scrolldirection, wheelAxis * step);
                
            }
            else if (wheelAxis < 0f)
            {
                if (CameraManager.Instance.currentCamera.cinemachineCamera.transform.position.y > mapBoundary[5])
                    return;

                CameraManager.Instance.currentCamera.cinemachineCamera.transform.position += Vector3.up;
                //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                //RaycastHit point;
                //Physics.Raycast(ray, out point, 25);
                //Vector3 Scrolldirection = -ray.GetPoint(5);
                //
                //float step = 7500 * Time.deltaTime;
                //CameraManager.Instance.currentCamera.cinemachineCamera.transform.position = Vector3.MoveTowards(CameraManager.Instance.currentCamera.cinemachineCamera.transform.position, Scrolldirection, (wheelAxis * step));

            }

        }
        //Build Camera
        else if (CameraManager.Instance.currentCamera.cameraName == GameCameraName.BuildingCamera)
        {
            if (Input.GetKey(KeyCode.A))
            {
                if (CameraManager.Instance.currentCamera.cinemachineCamera.transform.position.x > buildBoundary[0])
                    CameraManager.Instance.currentCamera.cinemachineCamera.transform.position += Vector3.left * (Time.deltaTime * cameraSpeed);
            }
            if (Input.GetKey(KeyCode.D))
            {
                if (CameraManager.Instance.currentCamera.cinemachineCamera.transform.position.x < buildBoundary[1])
                    CameraManager.Instance.currentCamera.cinemachineCamera.transform.position += Vector3.right * (Time.deltaTime * cameraSpeed);
            }
            if (Input.GetKey(KeyCode.W))
            {
                if (CameraManager.Instance.currentCamera.cinemachineCamera.transform.position.z < buildBoundary[2])
                    CameraManager.Instance.currentCamera.cinemachineCamera.transform.position += Vector3.forward * (Time.deltaTime * cameraSpeed);
            }
            if (Input.GetKey(KeyCode.S))
            {
                if (CameraManager.Instance.currentCamera.cinemachineCamera.transform.position.z > buildBoundary[3])
                    CameraManager.Instance.currentCamera.cinemachineCamera.transform.position += Vector3.back * (Time.deltaTime * cameraSpeed);
            }

            var wheelAxis = Input.GetAxis("Mouse ScrollWheel");
            if (wheelAxis > 0f)
            {
                if (CameraManager.Instance.currentCamera.cinemachineCamera.transform.position.y < buildBoundary[4])
                    return;

                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit point;
                Physics.Raycast(ray, out point, 25);
                Vector3 Scrolldirection = ray.GetPoint(5);

                float step = 7500 * Time.deltaTime;
                CameraManager.Instance.currentCamera.cinemachineCamera.transform.position = Vector3.MoveTowards(CameraManager.Instance.currentCamera.cinemachineCamera.transform.position, Scrolldirection, wheelAxis * step);

            }
            else if (wheelAxis < 0f)
            {
                if (CameraManager.Instance.currentCamera.cinemachineCamera.transform.position.y > buildBoundary[5])
                    return;

                CameraManager.Instance.currentCamera.cinemachineCamera.transform.position += Vector3.up;
                //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                //RaycastHit point;
                //Physics.Raycast(ray, out point, 25);
                //Vector3 Scrolldirection = -ray.GetPoint(5);
                //
                //float step = 7500 * Time.deltaTime;
                //CameraManager.Instance.currentCamera.cinemachineCamera.transform.position = Vector3.MoveTowards(CameraManager.Instance.currentCamera.cinemachineCamera.transform.position, Scrolldirection, (wheelAxis * step));

            }

            if (Input.GetKey(KeyCode.Q))
            {
                //CameraManager.Instance.currentCamera.cinemachineCamera.transform.RotateAround(BuildingPlacementHelper.Instance.CurrentBuildPaGO.transform.position, new Vector3(0, 1f, 0), 30 * Time.deltaTime);
                CameraManager.Instance.currentCamera.cinemachineCamera.transform.Rotate(0,-50*Time.deltaTime,0,Space.World);
            }
            if (Input.GetKey(KeyCode.E))
            {
                CameraManager.Instance.currentCamera.cinemachineCamera.transform.Rotate(0,50 * Time.deltaTime, 0,Space.World);
                //CameraManager.Instance.currentCamera.cinemachineCamera.transform.RotateAround(BuildingPlacementHelper.Instance.CurrentBuildPaGO.transform.position, new Vector3(0, -1f, 0), 30 * Time.deltaTime);
            }
        }
        //Follow Camera
        else if (CameraManager.Instance.currentCamera.cameraName == GameCameraName.ShepherdFollowCamera)
        {

        }




    }

}
