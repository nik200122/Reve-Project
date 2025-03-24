using Unity.VisualScripting;
using UnityEngine;

public class PlayerStat
{   
    //REMINDER ANGELONE ACCEDE COSI AI DATI IN UIMANAGER
        // hungerSlider.value = cat.getStat(CatTag.SAZIETA).currentValue;
        // enjoymentSlider.value = cat.getStat(CatTag.DIVERTIMENTO).currentValue;
        // happinessSlider.value = cat.getStat(CatTag.FELICITA).currentValue;

    public float currentValue;
    public float maxValue;
    public string statTag;

    public PlayerStat(){}

    public PlayerStat(float currentValue, float maxValue, string statTag){
        this.currentValue = currentValue;
        this.maxValue = maxValue;
        this.statTag = statTag;
    }

    public float GetCurrentValue(){
        return currentValue;
    }

    public float GetMaxValue(){
        return maxValue;
    }

    public string GetStatTag(){
        return statTag;
    }

    public void SetCurrentValue(float newValue){
        currentValue = newValue;
    }
}
