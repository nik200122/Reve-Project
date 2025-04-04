
public class EquipmentRule
{
    public EquipableItemCategory equipableItemCategory;
    public int maxEquipableNumber;

    public EquipmentRule(){}
    public EquipmentRule(EquipableItemCategory equipableItemCategory, int maxEquipableNumber){
        this.equipableItemCategory = equipableItemCategory;
        this.maxEquipableNumber = maxEquipableNumber;
    }
}
