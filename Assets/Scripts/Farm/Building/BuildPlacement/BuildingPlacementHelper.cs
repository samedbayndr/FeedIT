using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;




public class BuildingPlacementHelper : MonoBehaviour
{


    private static BuildingPlacementHelper _instance;
    public static BuildingPlacementHelper Instance
    {
        get
        {
            return _instance;
        }
    }

    private int buildLayer;
    private int fenceLayer;

    public void Awake()
    {
        if (_instance != null || _instance != this)
        {
            Destroy(_instance);
        }

        _instance = this;
    }

    public void Start()
    {
        buildLayer = LayerMask.GetMask("Build");
        fenceLayer = LayerMask.GetMask("Fence");
        mainCamera = Camera.main.transform;
    }



    private Transform mainCamera;

    private BuildingSO currentBuildSO;


    private GameObject _currentBuild;

    private GameObject currentBuild
    {
        get
        {
            return _currentBuild;
        }
        set
        {
            setMainCamera();
            _currentBuild = value;
        }
    }

    //public Vector3 buildSpawnPoint = new Vector3(-28, -40, 0);

    public GameObject FarmBuilds;
    public GameObject CurrentBuildPaGO;


    public void setMainCamera()
    {
        if (Camera.main.transform.parent != null)
        {
            Camera.main.transform.parent = null;
            return;
        }
        else
        {
            Camera.main.transform.parent = CurrentBuildPaGO.transform;
            return;
        }
    }


    // Update is called once per frame
    void Update()
    {


        if (currentBuild != null)
        {
            //Dummy MainCamera parent changing



            if (Input.GetKey(KeyCode.Q))
            {
                Camera.main.transform.RotateAround(Camera.main.transform.parent.transform.position, new Vector3(0, 1f, 0), 30 * Time.deltaTime);
                //Camera.main.transform.Rotate(0.0f, 3.0f , 0.0f);
            }
            if (Input.GetKey(KeyCode.E))
            {
                Camera.main.transform.RotateAround(Camera.main.transform.parent.transform.position, new Vector3(0, -1f, 0), 30 * Time.deltaTime);
            }


            if (Input.GetMouseButton(0))
            {
                if (EventSystem.current.IsPointerOverGameObject())
                    return;

                var wheelAxis = Input.GetAxis("Mouse ScrollWheel");
                if (wheelAxis > 0f)
                {
                    currentBuild.gameObject.transform.Rotate(0.0f, 45.0f, 0.0f, Space.Self);
                }
                else if (wheelAxis < 0f)
                {
                    currentBuild.gameObject.transform.Rotate(0.0f, -45.0f, 0.0f, Space.Self);
                }

                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hitInfo;

                if (Physics.Raycast(ray, out hitInfo, float.MaxValue, ~buildLayer))
                {
                    if (hitInfo.transform.tag == "Farm")
                    {
                        //currentBuild.transform.localScale.y/2 binanın yarısı yerin altında olduğu için ekleniyor.
                        //currentBuild.transform.position = new Vector3(hitInfo.point.x, currentBuild.transform.position.y + currentBuildSO.altitude, hitInfo.point.z);

                        currentBuild.transform.position = new Vector3(hitInfo.point.x, hitInfo.point.y + (currentBuild.transform.localScale.y / 2) + 1, hitInfo.point.z);

                        //Vector3 currentBuildLocalTransform = currentBuild.transform.localPosition;
                        //currentBuildLocalTransform.Set(hitInfo.point.x, hitInfo.point.y + currentBuildSO.altitude, hitInfo.point.z);
                        //currentBuild.transform.localPosition = currentBuildLocalTransform;
                    }
                }
            }
        }
    }

    public void spawnBuild(BuildingSO buildingSo)
    {
        if (buildingSo != null)
        {
            currentBuildSO = buildingSo;
            currentBuild = Instantiate(buildingSo.buildingPrefab, CurrentBuildPaGO.transform);
            CurrentBuildPaGO.transform.localScale = currentBuild.transform.localScale;
            BuildUI.Instance.buildingDecisionPanel.SetActive(true);
            //build.transform.parent = FarmBuilds.transform;

        }
    }
    public void placeIt()
    {
        if (currentBuild != null)
        {

            if (!isPlacementAvailable())
                return;

            // Para vs. gibi koşulları da buraya ekle
            // Parayı tekrar kontrol et ve güncelle

            Player.Instance.decreaseMoney(currentBuildSO.cost);

            // ------------------Diğer koşulların Bitişi ---------------------



            Destroy(currentBuild.GetComponent<ObstacleDetection>().HighlightGameObject);


            Vector3 currentBuildLocalTransform = currentBuild.transform.localPosition;
            currentBuildLocalTransform.Set(currentBuild.transform.localPosition.x, currentBuildSO.altitude, currentBuild.transform.localPosition.z);
            currentBuild.transform.localPosition = currentBuildLocalTransform;

            FarmBuilds.transform.localScale = currentBuild.transform.localScale;
            currentBuild.transform.parent = FarmBuilds.transform;

            BuildUI.Instance.buildingDecisionPanel.SetActive(false);

            //Database recording...
            string buildAvailableName = currentBuildSO.objectName.Replace(" ", "");
            BuildEntity newBuild = new BuildEntity(buildAvailableName, IdGenerator.generateGUID(), 0,currentBuildSO.cost, currentBuild.transform.position.x,
                currentBuild.transform.position.y, currentBuild.transform.position.z, currentBuild.transform.rotation.x,
                currentBuild.transform.rotation.y, currentBuild.transform.rotation.z,
                currentBuild.transform.rotation.w);
            TableManager.Instance.ownedBuildTable.AllBoughtBuilds.Add(newBuild);
            FileOperation.SaveTextAsset(TableManager.Instance.ownedBuildTable,
                TableManager.Instance.ownedBuildTable.filePath, Extension.Json);


            //Yeni inşa edilen binaya id ve maintenance cost üzerindeki "Build" adlı componente atanıyor.
            Build currentBuildComponent = currentBuild.GetComponent<Build>();
            currentBuildComponent.id = newBuild.id;
            currentBuildComponent.maintenanceCost = newBuild.maintenanceCost;
            currentBuildComponent.buildCost = newBuild.buildCost;


            //Update building capacity;
            BuildManager.Instance.updateBuildCapacity(buildAvailableName);

            BuildUI.Instance.buyBuildMenuBtn.interactable = true;
            currentBuild = null;
            currentBuildSO = null;
        }
    }

    public void cancelPlace()
    {
        if (currentBuild != null)
        {
            Destroy(currentBuild);

            BuildUI.Instance.buildingDecisionPanel.SetActive(false);

            BuildUI.Instance.buyBuildMenuBtn.interactable = true;
            currentBuild = null;
            currentBuildSO = null;

            //TODO: Burada state değiştir ve tekrar normal akışa dön
        }
    }

    public bool isPlacementAvailable()
    {
        if (currentBuild == null)
            return false;

        //İlk satın alındığında bina pozisyon CurrentBuildPaGO pozisyonuna atanıyor.
        //Eğer pozisyonu değiştirmeden yerleştirmeye çalışırsa izin verilmiyor.
        //Collider bugunu düzeltmek için...
        if (currentBuild.transform.position == CurrentBuildPaGO.transform.position)
            return false;


        if (currentBuild.GetComponent<ObstacleDetection>().collidedObjects.Count > 0)
            return false;
        else
            return true;

    }
}
