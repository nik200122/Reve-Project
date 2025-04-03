using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class AbilitiesPanel : MonoBehaviour
{
    [SerializeField] private AbilityUI passiveAbilityPrefab;
    [SerializeField] private GameObject scrollViewContent;
    [SerializeField] private RectTransform contentRectTrasform;
    [SerializeField] private List<AbilityUI> abilityUISlots;
    protected Scrollbar scrollbar;
    const int itemsInViewPort = 9;

    protected void Awake(){
        abilityUISlots = new List<AbilityUI>();
    }

    protected void DestroyGameObjects()
    {
        abilityUISlots.Clear();
        foreach (Transform child in scrollViewContent.transform)
        {
            Destroy(child.gameObject);
        }
    }

    public void SetData(AbilityList abilityList, List<AbilityRef> playerAbilityRef)
    {
        if (abilityList == null)
        {
            Debug.LogError("AbilityList non è stata inizializzata!");
            return;
        }

        DestroyGameObjects();

        foreach (var ability in abilityList.Abilities)
        {
            // Cerca l'abilità completa corrispondente all'ID
            AbilityRef abilityRef = playerAbilityRef.FirstOrDefault(a => a.Id == ability.id); // Risolve l'ID in un oggetto reale
            var slotUIobj = Instantiate(passiveAbilityPrefab, scrollViewContent.transform);
            slotUIobj.SetData(ability, abilityRef.IsActive);
            abilityUISlots.Add(slotUIobj);
        }
    }
    public void Select(int selectedItem){
        abilityUISlots[selectedItem].Select();
        HandleScrolling(selectedItem);
    }

    private void HandleScrolling(int selectedItem)
    {   
        //funzione utile a rendere lo scroll più smooth
        float scrollPos = Mathf.Clamp(selectedItem - itemsInViewPort/2, 0, selectedItem) * abilityUISlots[selectedItem].GetRectTransformHeight();
        contentRectTrasform.localPosition = new Vector2(contentRectTrasform.localPosition.x, scrollPos);
    }

    public void Deselect(int deselectedItem){
        abilityUISlots[deselectedItem].Deselect();
    }
    public void UpdateAbilityState(int index, bool isActive)
{
    abilityUISlots[index].UpdateActivationState(isActive);
}

    
}
