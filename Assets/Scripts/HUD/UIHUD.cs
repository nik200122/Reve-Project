using TMPro;
using UnityEngine;

public class UIHUD : MonoBehaviour
{
    [SerializeField] private UIBar healthBar;
    [SerializeField] private TextMeshProUGUI moneyQuantity;

    public void SetData(Player player)
    {   
        healthBar.SetMaxValue(player.GetStat("Hp").maxValue);
        healthBar.SetValue(player.GetStat("Hp").currentValue);
    }

    public void UpdateData(Player player)
    {
        healthBar.UpdateSmooth(player.GetStat("Hp").baseValue, player.GetStat("Hp").maxValue);
        moneyQuantity.text = player.GetStat("Money").currentValue.ToString() + "$";
    }
}
