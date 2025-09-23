using System;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// VATS 파츠 선택 버튼 UI
/// </summary>
[DisallowMultipleComponent]
public class VATSPartButtonView : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private Text _label;
    [SerializeField] private Image _background;
    [SerializeField] private Color _selectedColor = new Color(0.2f, 0.8f, 0.2f, 1f);
    [SerializeField] private Color _normalColor = Color.white;

    public IVATSPart Part { get; private set; }

    private Action<IVATSPart> _onClicked;

    void Awake()
    {
        if (_button == null)
            _button = GetComponent<Button>();
    }

    public void Initialize(IVATSPart part, Action<IVATSPart> onClicked)
    {
        Part = part;
        _onClicked = onClicked;

        if (_button != null)
        {
            _button.onClick.RemoveListener(OnButtonClicked);
            _button.onClick.AddListener(OnButtonClicked);
        }

        UpdateLabel();
        SetSelected(false);
    }

    public void SetSelected(bool selected)
    {
        if (_background != null)
        {
            _background.color = selected ? _selectedColor : _normalColor;
        }
    }

    void UpdateLabel()
    {
        if (_label == null)
            return;

        if (Part == null)
        {
            _label.text = string.Empty;
            return;
        }

        _label.text = string.Format("{0}\nHit {1:F0}% | AP {2:F1}", Part.PartName, Part.HitChance, Part.APCost);
    }

    void OnButtonClicked()
    {
        if (Part == null)
            return;

        _onClicked?.Invoke(Part);
    }
}
