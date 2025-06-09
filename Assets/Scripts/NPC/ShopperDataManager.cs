using UnityEngine;
using System.Collections.Generic;
// using System.IO; // Needed if not using XMLHelper and loading directly
// using System.Xml.Serialization; // Needed if not using XMLHelper

public class ShopperDataManager : MonoBehaviour
{
    public static ShopperDataManager Instance { get; private set; }

    // Path to the XML file within a "Resources" folder (e.g., "XML/ShopperInventories")
    // The .xml extension is usually not included when using Resources.Load or XMLHelper if it abstracts it.
    private string shopperDataFilePath = "XML/shopper01InventoryData"; 

    // Stores all loaded shopper data, keyed by shopper ID
    private Dictionary<string, ShopperInventoryData> shopperDataDictionary = new Dictionary<string, ShopperInventoryData>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject); // Make it persistent across scenes
        LoadShopperData();
    }

    private void LoadShopperData()
    {
        // Assuming XMLHelper.LoadFromXml<T>(path) handles loading from Resources
        // and deserialization. 'path' should be relative to a Resources folder.
        ShopperInventoriesList dataList = XMLHelper.LoadFromXml<ShopperInventoriesList>(shopperDataFilePath); 
        
        if (dataList != null && dataList.ShopperInventories != null)
        {
            foreach (ShopperInventoryData shopperData in dataList.ShopperInventories)
            {
                if (!string.IsNullOrEmpty(shopperData.id))
                {
                    if (!shopperDataDictionary.ContainsKey(shopperData.id))
                    {
                        shopperDataDictionary.Add(shopperData.id, shopperData);
                        if (shopperData.inventory != null)
                        {
                             Debug.Log($"[ShopperDataManager] Data loaded for shopper '{shopperData.id}' with {shopperData.inventory.itemList.Count} kinds of items in stock.");
                        }
                        else
                        {
                            Debug.LogWarning($"[ShopperDataManager] Data loaded for shopper '{shopperData.id}' but their stock or itemList is null.");
                        }
                    }
                    else
                    {
                        Debug.LogWarning($"[ShopperDataManager] Duplicate shopper ID found: '{shopperData.id}'. Ignoring subsequent entry.");
                    }
                }
                else
                {
                    Debug.LogWarning("[ShopperDataManager] Found ShopperInventoryData with null or empty ID in XML. Entry ignored.");
                }
            }
            Debug.Log($"[ShopperDataManager] Shopper data loading complete. Total unique shopper entries: {shopperDataDictionary.Count}");
        }
        else
        {
            Debug.LogError($"[ShopperDataManager] Error loading shopper data from '{shopperDataFilePath}'. Check XMLHelper or file path.");
        }
    }

    
    

    /// <summary>
    /// Retrieves the raw ShopperInventoryData for a given shopper ID.
    /// This includes their ID and their full Inventory (Stock).
    /// </summary>
    /// <returns>ShopperInventoryData or null if not found.</returns>
    public ShopperInventoryData GetShopperData(string shopperId)
    {
        if (shopperDataDictionary.TryGetValue(shopperId, out ShopperInventoryData data))
        {
            return data;
        }
        Debug.LogWarning($"[ShopperDataManager] No data found for shopper ID: '{shopperId}'.");
        return null;
    }

    /// <summary>
    /// Retrieves the actual Inventory (stock) for a given shopper ID.
    /// </summary>
    /// <returns>The Inventory object or null if the shopper or their stock is not found.</returns>
    public Inventory GetShopperInventory(string shopperId)
    {
        ShopperInventoryData data = GetShopperData(shopperId);
        if (data != null)
        {
            return data.inventory;
        }
        return null; // Or return new Inventory() if you prefer an empty inventory over null
    }
}