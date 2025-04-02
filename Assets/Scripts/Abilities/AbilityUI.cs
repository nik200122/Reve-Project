using UnityEngine;
using UnityEngine.UI;

public class AbilityUI : MonoBehaviour
{
    [SerializeField] protected TMPro.TextMeshProUGUI nameText;
    
    [SerializeField] protected TMPro.TextMeshProUGUI descriptionText;
    [SerializeField] protected Image icon;

    protected Ability ability;
    public void Deselect(){
        nameText.color = Color.white;
    }

    public void Select(){
        nameText.color = Color.blue;
    }

    public void SetData(Ability ability){
        this.ability = ability;
        nameText.text = ability.name;
        descriptionText.text = ability.description;
        icon.sprite = ability.sprite;

    }
}
