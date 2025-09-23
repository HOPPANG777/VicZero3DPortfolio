using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// VATS AP 게이지 UI
/// </summary>
public class VATSAPGaugeView : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private Text _valueLabel;

    public void UpdateGauge(float current, float max)
    {
        float clampedMax = Mathf.Max(0f, max);
        float clampedCurrent = Mathf.Clamp(current, 0f, clampedMax);

        if (_slider != null)
        {
            _slider.maxValue = clampedMax;
            _slider.value = clampedCurrent;
        }

        if (_valueLabel != null)
        {
            if (clampedMax > 0f)
                _valueLabel.text = string.Format("{0:F1}/{1:F1}", clampedCurrent, clampedMax);
            else
                _valueLabel.text = "0/0";
        }
    }
}
