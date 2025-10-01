using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// VATS UI를 제어하는 기본 구현체
/// </summary>
public class VATSUIView : MonoBehaviour, IVATSUIView
{
    [Header("Part Buttons")]
    [SerializeField] private RectTransform _partButtonParent;
    [SerializeField] private VATSPartButtonView _partButtonPrefab;
    [SerializeField] private Camera _worldCamera;
    [SerializeField] private Vector2 _partButtonScreenOffset = Vector2.zero;
    [SerializeField] private bool _hideButtonsWhenOffscreen = true;

    [Header("Selected Part Info")]
    [SerializeField] private Text _selectedPartName;
    [SerializeField] private Text _selectedPartChance;

    [Header("AP Gauge")]
    [SerializeField] private VATSAPGaugeView _apGaugeView;

    [Header("Queued Attacks")]
    [SerializeField] private Text _queuedShotsText;

    [Header("Attack Result")]
    [SerializeField] private Text _attackResultText;

    private readonly List<VATSPartButtonView> _spawnedButtons = new List<VATSPartButtonView>();
    private Action<IVATSPart> _onPartClicked;
    private Canvas _parentCanvas;

    void Awake()
    {
        if (_partButtonParent == null)
            _partButtonParent = transform as RectTransform;

        _parentCanvas = GetComponentInParent<Canvas>();

        if (_worldCamera == null)
            _worldCamera = Camera.main;
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    void LateUpdate()
    {
        UpdatePartButtonPositions();
    }

    public void BindParts(IReadOnlyList<IVATSPart> parts, Action<IVATSPart> onPartClicked)
    {
        ClearButtons();
        _onPartClicked = onPartClicked;

        if (parts == null || parts.Count == 0)
            return;

        if (_partButtonParent == null || _partButtonPrefab == null)
            return;

        for (int i = 0; i < parts.Count; i++)
        {
            VATSPartButtonView button = Instantiate(_partButtonPrefab, _partButtonParent);
            button.Initialize(parts[i], HandleButtonClicked);
            _spawnedButtons.Add(button);
        }
    }

    public void UpdateSelectedPart(IVATSPart part)
    {
        if (_selectedPartName != null)
            _selectedPartName.text = part != null ? part.PartName : string.Empty;

        if (_selectedPartChance != null)
            _selectedPartChance.text = part != null ? string.Format("{0:F0}%", part.HitChance) : string.Empty;

        for (int i = 0; i < _spawnedButtons.Count; i++)
        {
            bool isSelected = _spawnedButtons[i].Part == part;
            _spawnedButtons[i].SetSelected(isSelected);
        }
    }

    public void UpdateAP(float current, float max, IReadOnlyList<IVATSPart> queuedParts)
    {
        if (_apGaugeView != null)
            _apGaugeView.UpdateGauge(current, max);

        if (_queuedShotsText != null)
        {
            if (queuedParts != null && queuedParts.Count > 0)
            {
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < queuedParts.Count; i++)
                {
                    IVATSPart part = queuedParts[i];
                    if (part == null) continue;
                    builder.AppendFormat("{0}. {1} - AP {2:F1}\n", i + 1, part.PartName, part.APCost);
                }
                _queuedShotsText.text = builder.ToString().TrimEnd('\n');
            }
            else
            {
                _queuedShotsText.text = string.Empty;
            }
        }
    }

    public void ShowAttackResult(IVATSPart part, bool isHit, float roll, float hitChance)
    {
        if (_attackResultText == null)
            return;

        string partName = part != null ? part.PartName : "Unknown";
        _attackResultText.text = string.Format(
            "{0}: {1} (Roll {2:F1} / Chance {3:F0}%)",
            partName,
            isHit ? "Hit" : "Miss",
            roll,
            hitChance);
    }

    public void Clear()
    {
        ClearButtons();
        _onPartClicked = null;

        if (_selectedPartName != null) _selectedPartName.text = string.Empty;
        if (_selectedPartChance != null) _selectedPartChance.text = string.Empty;
        if (_queuedShotsText != null) _queuedShotsText.text = string.Empty;
        if (_attackResultText != null) _attackResultText.text = string.Empty;
        if (_apGaugeView != null) _apGaugeView.UpdateGauge(0f, 0f);
    }

    void ClearButtons()
    {
        for (int i = 0; i < _spawnedButtons.Count; i++)
        {
            if (_spawnedButtons[i] == null) continue;
            DestroyButton(_spawnedButtons[i].gameObject);
        }
        _spawnedButtons.Clear();
    }

    void DestroyButton(GameObject target)
    {
        if (target == null) return;
#if UNITY_EDITOR
        if (!Application.isPlaying)
        {
            DestroyImmediate(target);
            return;
        }
#endif
        Destroy(target);
    }

    void HandleButtonClicked(IVATSPart part)
    {
        _onPartClicked?.Invoke(part);
    }

    void UpdatePartButtonPositions()
    {
        if (_spawnedButtons.Count == 0)
            return;

        RectTransform parentRect = _partButtonParent != null ? _partButtonParent : transform as RectTransform;
        if (parentRect == null)
            return;

        Camera worldCamera = _worldCamera != null ? _worldCamera : Camera.main;
        if (worldCamera == null)
            return;

        Canvas canvas = _parentCanvas;
        if (canvas == null)
            canvas = parentRect.GetComponentInParent<Canvas>();

        Camera uiCamera = null;
        if (canvas != null)
        {
            if (canvas.renderMode == RenderMode.ScreenSpaceCamera || canvas.renderMode == RenderMode.WorldSpace)
            {
                uiCamera = canvas.worldCamera != null ? canvas.worldCamera : worldCamera;
            }
        }

        for (int i = 0; i < _spawnedButtons.Count; i++)
        {
            VATSPartButtonView button = _spawnedButtons[i];
            if (button == null)
                continue;

            IVATSPart part = button.Part;
            Transform aimPoint = part != null ? part.AimPoint : null;

            if (aimPoint == null)
            {
                if (_hideButtonsWhenOffscreen)
                    button.SetVisible(false);
                continue;
            }

            Vector3 screenPosition = worldCamera.WorldToScreenPoint(aimPoint.position);
            if (screenPosition.z < 0f)
            {
                if (_hideButtonsWhenOffscreen)
                    button.SetVisible(false);
                continue;
            }

            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRect, screenPosition, uiCamera, out Vector2 localPoint))
            {
                RectTransform buttonRect = button.RectTransform;
                if (buttonRect != null)
                {
                    buttonRect.anchoredPosition = localPoint + _partButtonScreenOffset;
                }

                if (!button.gameObject.activeSelf)
                    button.SetVisible(true);
            }
            else if (_hideButtonsWhenOffscreen)
            {
                button.SetVisible(false);
            }
        }
    }
}
