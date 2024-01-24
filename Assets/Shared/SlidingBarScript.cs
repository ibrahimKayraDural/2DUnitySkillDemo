using UnityEngine;
using UnityEngine.UI;

public class SlidingBarScript : MonoBehaviour
{
    [SerializeField] Slider slider;
    [SerializeField, Range(0, 1)] float normalizedStartValue = 1;

    void Awake()
    {
        if(slider == null)
        {
            Debug.LogError("No Slider found in " + gameObject.name + ", destroying self");
            Destroy(this);
        }

        SetNormalizedValue(normalizedStartValue);
    }

    public Slider GetSlider() => slider;
    public void SetValue(float setTo) => slider.value = Mathf.Clamp(setTo, slider.minValue, slider.maxValue);
    public void SetNormalizedValue(float setTo) => slider.normalizedValue = Mathf.Clamp(setTo, 0, 1);
    public void AddValue(float amount) => SetValue(Mathf.Clamp(slider.value + amount, slider.minValue, slider.maxValue));
    public void SubtractValue(float amount) => SetValue(Mathf.Clamp(slider.value - amount, slider.minValue, slider.maxValue));
    public void SetMaxValue(float setTo) => SetMinMaxValues(slider.minValue, setTo);
    public void SetMinValue(float setTo) => SetMinMaxValues(setTo, slider.maxValue);
    public void SetMinMaxValues(float min, float max)
    {
        if (min >= max) max = min + .1f;

        slider.maxValue = max;
        slider.minValue = min;
    }
    public void SetSliderDireciton(Slider.Direction direction) => slider.direction = direction;
    public void SetImagecolor(Color color) => slider.image.color = color;
    public void SetUseWholeNumbers(bool setTo) => slider.wholeNumbers = setTo;
    public void SetSliderEnabled(bool setTo) => slider.enabled = setTo;
    public float GetNormalizedValue() => slider.normalizedValue;
    public float GetMinValue() => slider.minValue;
    public float GetMaxValue() => slider.maxValue;
    public float GetValue() => slider.value;
}
