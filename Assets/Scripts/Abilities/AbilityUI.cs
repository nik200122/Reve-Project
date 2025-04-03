using UnityEngine;
using UnityEngine.UI;

public class AbilityUI : MonoBehaviour
{
    [SerializeField] protected TMPro.TextMeshProUGUI nameText;
    [SerializeField] protected GameObject activePanel;
    
    [SerializeField] protected TMPro.TextMeshProUGUI descriptionText;
    [SerializeField] protected Image icon;
    private RectTransform rectTransform;
    public float GetRectTransformHeight(){
        return rectTransform.rect.height;
    }

    private void Awake(){
        rectTransform = GetComponent<RectTransform>();
    }

    protected Ability ability;
    public void Deselect(){
        nameText.color = Color.white;
    }

    public void Select(){
        nameText.color = Color.blue;
    }

    private void Activate(bool isActive){
       activePanel.SetActive(isActive);
    }

    public void UpdateActivationState(bool isActive)
    {
        Activate(isActive); // Attiva/disattiva il pannello visivo
    }


    public void SetData(Ability ability, bool isActive){
        this.ability = ability;
        nameText.text = ability.name+"  "+ ability.abilityType;
        descriptionText.text = ability.description;
        icon.sprite = ability.sprite;
        Activate(isActive);
    }
}
