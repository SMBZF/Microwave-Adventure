using UnityEngine;
using UnityEngine.UI;

public class SliderValueDisplay : MonoBehaviour
{
    public Slider targetSlider; // ��Ҫ��ʾ��ֵ�� Slider
    public Text valueText; // ��ʾ��ֵ�� UI Text

    private void Start()
    {
        if (targetSlider != null && valueText != null)
        {
            UpdateSliderValue(targetSlider.value); // ��ʼ��ʱ���� Text
            targetSlider.onValueChanged.AddListener(UpdateSliderValue); // ���� Slider �仯
        }
        else
        {
            Debug.LogError("���� Inspector ��� Slider �� Text �����");
        }
    }

    private void UpdateSliderValue(float value)
    {
        if (valueText != null)
        {
            valueText.text = $"Frequency: {value:F2} Hz"; // ��ʾ��λС��
        }
    }

    private void OnDestroy()
    {
        if (targetSlider != null)
        {
            targetSlider.onValueChanged.RemoveListener(UpdateSliderValue);
        }
    }
}
