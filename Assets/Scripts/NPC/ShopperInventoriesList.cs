// Root class for the XML
using System.Collections.Generic;
using System.Xml.Serialization;
[XmlRoot("ShopperInventoriesList")]
public class ShopperInventoriesList
{
    [XmlElement("ShopperInventory")]
    public List<ShopperInventoryData> ShopperInventories;
}

// Represents data for a single shopper
public class ShopperInventoryData
{
    [XmlAttribute("id")]
    public string id;

    public Inventory inventory; // Initialize for safety
}