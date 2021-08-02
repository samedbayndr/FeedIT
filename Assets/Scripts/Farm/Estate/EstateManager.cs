using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;

public static class AreaMagicString
{
    public const string Area = "Area";
    public const string Fence = "Fence";
    public const string Garbage = "Garbage";
    public const string Farm = "Farm";
    public const string Estate = "Estate";

}

public class Estate
{
    public GameObject AreaParent;
    public GameObject AreaFence;
    public GameObject AreaGarbage;

}
public class EstateManager : MonoBehaviour
{
    private static EstateManager _instance;

    public static EstateManager Instance
    {
        get
        {
            return _instance;
        }
    }

    public void Awake()
    {
        if (_instance != null & _instance != this)
        {
            Destroy(_instance);
        }

        _instance = this;
    }
    public void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            if (EventSystem.current.IsPointerOverGameObject())
                return;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit raycastHit;
            if (Physics.Raycast(ray, out raycastHit))
            {
                if (raycastHit.collider.tag == "ForSale")
                {
                    raycastHit.collider.GetComponent<ForSale>().openOfferPanel();
                }
            }
        }
    }

    public GameObject ExpensionAreas;
    public List<Estate> allEstates = new List<Estate>();
    public List<GameObject> allAreasParent = new List<GameObject>();
    private EstateAsset ownAsset;
    
    private bool[] allAssetStatus;
    void Start()
    {
        ownAsset = TableManager.Instance.estateAssetTable;
        allAssetStatus = new bool[] {ownAsset.Area1, ownAsset.Area2, ownAsset.Area3};
        allAreasParent = ChildOperation.GetFirstLevelChildren(ExpensionAreas); //ExpensionAreas.transform.GetComponentsInChildren<Transform>(true).ToList().FindAll(a => a.name.Contains(AreaMagicString.Area));
        if (allAreasParent.Count != allAssetStatus.Length)
            Debug.Log(NextGenDebug.HeavyError("Yeni area var ama burada güncelleme yapılmamış!!!!!!!"));
        
        configureEstate();
    }

    private void configureEstate()
    {
        for (int i = 0; i < allAssetStatus.Length; i++)
        {
            Estate curEstate = new Estate();
            curEstate.AreaParent = allAreasParent[i].gameObject;
            List<GameObject> areaChildren = ChildOperation.GetFirstLevelChildren(allAreasParent[i]); // allAreasParent[i].GetComponentsInChildren<Transform>().ToList();
            curEstate.AreaFence = areaChildren.Find(a => a.name == AreaMagicString.Fence).gameObject;
            curEstate.AreaGarbage = areaChildren.Find(a => a.name == AreaMagicString.Garbage).gameObject;
            allEstates.Add(curEstate);

            if (allAssetStatus[i] == true)
            {
                curEstate.AreaParent.tag = AreaMagicString.Farm;
                curEstate.AreaFence.SetActive(false);
                curEstate.AreaGarbage.SetActive(false);
            }
            else
            {
                curEstate.AreaParent.tag = AreaMagicString.Estate;
                curEstate.AreaFence.SetActive(true);
                curEstate.AreaGarbage.SetActive(true);
            }
        }
    }

    public bool buyEstate(string areaName, double price)
    {
        Estate estate = allEstates.Find(a => a.AreaParent.name == areaName);
        if (estate != null)
        {
            if (Player.Instance.getMoney() >= price)
            {
                Player.Instance.decreaseMoney(price);
                FieldInfo fInfo = TableManager.Instance.estateAssetTable.GetType().GetField(areaName);
                fInfo.SetValue(TableManager.Instance.estateAssetTable, true);

                estate.AreaFence.SetActive(false); 
                estate.AreaGarbage.SetActive(false);
                estate.AreaParent.tag = AreaMagicString.Farm;
                return true;
            }
            else
            {
                MessageSentenceThrower.Instance.fireMessage("You have not enough money!");
            }

        }

        return false;
    }
    
}
