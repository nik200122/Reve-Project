using UnityEngine.UI;
using UnityEngine;


public class UIBar : MonoBehaviour{

    private Slider slider;
    private const float smoothSpeed = 0.04f; // Velocità del cambiamento

    // public void Start(){
    //     health = character.GetComponent<CharacterData>();
    // }

    private void Awake(){
        slider = GetComponent<Slider>();
    }

    public void SetMaxValue(float maxValue){
        slider.maxValue = maxValue;
        slider.value = maxValue;
    }
    public void SetValue(float value){
        slider.value = value;
    }

    public void UpdateSmooth(float currentValue, float maxValue){
        float newValue = currentValue/maxValue;
        float curValue = slider.fillRect.anchorMax.x;
        //float changeAmount = curValue - newValue;

        slider.value = Mathf.Lerp(currentValue, maxValue, smoothSpeed * Time.deltaTime);

        // while(curHP - newHP > Mathf.Epsilon){
        //     curHP -= changeAmount * Time.deltaTime;
        //     slider.fillRect.anchorMax = new Vector3 (curHP,1);
        //     yield return null;
        // }
        // slider.fillRect.anchorMax = new Vector3 (newHP,1);
    }
}
