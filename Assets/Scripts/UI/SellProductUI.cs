
using TMPro;
using UnityEngine;

public class SellProductUI : MonoBehaviour
{
    #region Singleton Things
    private static SellProductUI _instance;
    public static SellProductUI Instance
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



    public GameObject ProductSellPanel;
    
    //Meat Category
    public TextMeshProUGUI sheepMeatStock;
    public TextMeshProUGUI ramMeatStock;
    public TextMeshProUGUI chickenMeatStock;
    public TextMeshProUGUI roosterMeatStock;
    
    //Animal Category
    public TextMeshProUGUI lambStock;

    //Animal's Product Category
    public TextMeshProUGUI eggStock;
    public TextMeshProUGUI woolStock;

    public void Start()
    {

    }


    public void openProductSellPanel()
    {

        updateSellableInventoryUI();

        if (FarmUI.Instance.openMenu(ProductSellPanel))
        {
            //--//
        }
        else
        {
            Debug.Log(NextGenDebug.NormalError("Menü yok!! "));
        }

    }

    public void updateSellableInventoryUI()
    {
        sheepMeatStock.SetText("x" + TableManager.Instance.inventoryTable.sheepMeat.ToString());
        ramMeatStock.SetText("x" + TableManager.Instance.inventoryTable.ramMeat.ToString());
        chickenMeatStock.SetText("x" + TableManager.Instance.inventoryTable.chickenMeat.ToString());
        roosterMeatStock.SetText("x" + TableManager.Instance.inventoryTable.roosterMeat.ToString());
        lambStock.SetText("x" + TableManager.Instance.inventoryTable.lamb.ToString());
        eggStock.SetText("x" + TableManager.Instance.inventoryTable.egg.ToString());
        woolStock.SetText("x" + TableManager.Instance.inventoryTable.wool.ToString());
    }
}
