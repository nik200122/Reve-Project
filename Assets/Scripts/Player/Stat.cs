
public class Stat
{
    public float currentValue;
    public float baseValue;
    public float maxValue;
    public string statTag;

    public Stat() { }

    public Stat(float currentValue, float baseValue, float maxValue, string statTag)
    {
        this.currentValue = currentValue;
        this.baseValue = baseValue;
        this.maxValue = maxValue;
        this.statTag = statTag;
    }

    public float GetCurrentValue()
    {
        return currentValue;
    }

    public float GetBaseValue()
    {
        return baseValue;
    }

    public float GetMaxValue()
    {
        return maxValue;
    }

    public string GetStatTag()
    {
        return statTag;
    }

    public void SetCurrentValue(float newValue)
    {
        currentValue = newValue;
    }
    
    public void SetBaseValue(float newValue){
        baseValue = newValue;
    }
}
