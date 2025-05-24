
public class Stat
{   
    public float currentValue;
    public float maxValue;
    public string statTag;

    public Stat(){}

    public Stat(float currentValue, float maxValue, string statTag){
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
