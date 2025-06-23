using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// VATS 시스템을 제어하는 컴포넌트
/// </summary>
public class VATSController : MonoBehaviour
{
    [Header("VATS Settings")]
    [Tooltip("VATS 모드에서 사용할 타임 스케일 (예: 0.2f)")]
    [SerializeField] private float _vatsTimeScale = 0.2f;
    public float VATSTimeScale { get { return _vatsTimeScale; } set { _vatsTimeScale = value; } }

    [Tooltip("VATS 모드를 토글할 키")]
    [SerializeField] private KeyCode _vatsToggleKey = KeyCode.V;
    public KeyCode VATSToggleKey { get { return _vatsToggleKey; } }

    [Header("UI")]
    [Tooltip("VATS UI 오버레이")]
    [SerializeField] private GameObject _vatsUI;
    public GameObject VatsUI { get { return _vatsUI; } }

    [Tooltip("선택된 부위 이름 표시")]
    [SerializeField] private Text _partNameText;
    public Text PartNameText { get { return _partNameText; } }

    [Header("Targeting")]
    [Tooltip("타겟 레이어 마스크")]
    [SerializeField] private LayerMask _targetLayerMask = ~0;
    public LayerMask TargetLayerMask { get { return _targetLayerMask; } set { _targetLayerMask = value; } }

    [SerializeField] private bool _isVATSActive;
    public bool IsVATSActive { get { return _isVATSActive; } }

    private float _originalTimeScale = 1f;
    public float OriginalTimeScale { get { return _originalTimeScale; } }

    private IVATSTarget _currentTarget;
    public IVATSTarget CurrentTarget { get { return _currentTarget; } }

    private int _currentPartIndex;
    public int CurrentPartIndex { get { return _currentPartIndex; } }

    void Update()
    {
        if (Input.GetKeyDown(_vatsToggleKey))
        {
            ToggleVATS();
        }

        if (_isVATSActive)
        {
            UpdateTargeting();
            HandlePartInput();
        }
    }

    void UpdateTargeting()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        if (Physics.Raycast(ray, out RaycastHit hit, 100f, _targetLayerMask))
        {
            IVATSTarget target = hit.collider.GetComponentInParent<IVATSTarget>();
            if (target != null && target != _currentTarget)
            {
                if (_currentTarget != null) _currentTarget.ClearHighlight();
                _currentTarget = target;
                _currentPartIndex = 0;
                _currentTarget.HighlightPart(_currentPartIndex);
                UpdatePartName();
            }
        }
        else
        {
            if (_currentTarget != null)
            {
                _currentTarget.ClearHighlight();
                _currentTarget = null;
                if (_partNameText != null) _partNameText.text = string.Empty;
            }
        }
    }

    void HandlePartInput()
    {
        if (_currentTarget == null) return;
        int partCount = _currentTarget.Parts.Length;
        if (partCount == 0) return;

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            _currentPartIndex = (_currentPartIndex - 1 + partCount) % partCount;
            _currentTarget.HighlightPart(_currentPartIndex);
            UpdatePartName();
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            _currentPartIndex = (_currentPartIndex + 1) % partCount;
            _currentTarget.HighlightPart(_currentPartIndex);
            UpdatePartName();
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            IVATSPart part = _currentTarget.Parts[_currentPartIndex];
            part.OnHit();
        }
    }

    void UpdatePartName()
    {
        if (_partNameText != null && _currentTarget != null)
        {
            IVATSPart part = _currentTarget.Parts[_currentPartIndex];
            _partNameText.text = string.Format("{0} ({1:F0}%)", part.PartName, part.HitChance);
        }
    }

    void ToggleVATS()
    {
        _isVATSActive = !_isVATSActive;

        if (_isVATSActive)
        {
            _originalTimeScale = Time.timeScale;
            Time.timeScale = _vatsTimeScale;
            if (_vatsUI != null) _vatsUI.SetActive(true);
        }
        else
        {
            Time.timeScale = _originalTimeScale;
            if (_vatsUI != null) _vatsUI.SetActive(false);

            if (_currentTarget != null)
            {
                _currentTarget.ClearHighlight();
                _currentTarget = null;
            }

            if (_partNameText != null) _partNameText.text = string.Empty;
        }
    }
}
