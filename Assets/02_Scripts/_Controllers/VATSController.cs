using System.Collections.Generic;
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

    [Tooltip("IVATSUIView를 구현한 UI 컴포넌트. 비워두면 VATS UI 오브젝트에서 자동으로 찾습니다.")]
    [SerializeField] private MonoBehaviour _vatsUIViewBehaviour;
    private IVATSUIView _vatsUIView;
    public IVATSUIView VatsUIView { get { return _vatsUIView; } }

    [Tooltip("선택된 부위 이름 표시")]
    [SerializeField] private Text _partNameText;
    public Text PartNameText { get { return _partNameText; } }

    [Header("Action Points")]
    [Tooltip("VATS 모드에서 사용할 최대 AP")]
    [SerializeField] private float _maxAP = 6f;
    public float MaxAP { get { return _maxAP; } set { _maxAP = Mathf.Max(0f, value); } }

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

    private float _currentAP;
    public float CurrentAP { get { return _currentAP; } }

    private readonly List<IVATSPart> _queuedParts = new List<IVATSPart>();
    public IReadOnlyList<IVATSPart> QueuedParts { get { return _queuedParts; } }

    void Awake()
    {
        ResolveUIViewReference();

        _currentAP = Mathf.Max(0f, _maxAP);
        UpdateAPUI();
    }

    void ResolveUIViewReference()
    {
        if (_vatsUIViewBehaviour != null)
        {
            _vatsUIView = _vatsUIViewBehaviour as IVATSUIView;
            if (_vatsUIView != null)
                return;

            Debug.LogError("할당된 UI 컴포넌트가 IVATSUIView를 구현하지 않습니다.", _vatsUIViewBehaviour);
        }

        if (_vatsUI == null)
            return;

        MonoBehaviour[] uiBehaviours = _vatsUI.GetComponentsInChildren<MonoBehaviour>(true);
        for (int i = 0; i < uiBehaviours.Length; i++)
        {
            IVATSUIView candidate = uiBehaviours[i] as IVATSUIView;
            if (candidate == null)
                continue;

            _vatsUIViewBehaviour = uiBehaviours[i];
            _vatsUIView = candidate;
            return;
        }

        if (_vatsUIView == null)
        {
            Debug.LogError("VATS UI에서 IVATSUIView 구현체를 찾지 못했습니다. VATSUIView(또는 인터페이스를 구현한 스크립트)를 추가하고 참조를 연결하세요.", _vatsUI);
        }
    }

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
                if (_currentTarget != null)
                    _currentTarget.ClearHighlight();

                CancelQueue(true);
                _currentTarget = target;
                _currentPartIndex = 0;
                _currentTarget.HighlightPart(_currentPartIndex);
                BindCurrentTargetToUI();
                UpdateSelectedPartDisplay();
            }
        }
        else if (_currentTarget != null)
        {
            _currentTarget.ClearHighlight();
            _currentTarget = null;
            CancelQueue(true);
            if (_partNameText != null) _partNameText.text = string.Empty;
            if (_vatsUIView != null) _vatsUIView.Clear();
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
            UpdateSelectedPartDisplay();
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            _currentPartIndex = (_currentPartIndex + 1) % partCount;
            _currentTarget.HighlightPart(_currentPartIndex);
            UpdateSelectedPartDisplay();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            QueueSelectedPart();
        }

        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            CancelLastQueuedAttack();
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            ExecuteQueuedAttacks();
        }
    }

    void UpdateSelectedPartDisplay()
    {
        IVATSPart part = GetCurrentPart();

        if (_partNameText != null)
        {
            _partNameText.text = part != null ? string.Format("{0} ({1:F0}%)", part.PartName, part.HitChance) : string.Empty;
        }

        if (_vatsUIView != null)
        {
            _vatsUIView.UpdateSelectedPart(part);
        }
    }

    IVATSPart GetCurrentPart()
    {
        if (_currentTarget == null)
            return null;

        IVATSPart[] parts = _currentTarget.Parts;
        if (parts == null || parts.Length == 0)
            return null;

        if (_currentPartIndex < 0 || _currentPartIndex >= parts.Length)
        {
            _currentPartIndex = Mathf.Clamp(_currentPartIndex, 0, parts.Length - 1);
        }

        return parts[_currentPartIndex];
    }

    void BindCurrentTargetToUI()
    {
        if (_vatsUIView == null)
            return;

        if (_currentTarget == null)
        {
            _vatsUIView.Clear();
            return;
        }

        _vatsUIView.BindParts(_currentTarget.Parts, OnPartClickedFromUI);
        _vatsUIView.UpdateSelectedPart(GetCurrentPart());
        _vatsUIView.UpdateAP(_currentAP, _maxAP, _queuedParts);
    }

    void OnPartClickedFromUI(IVATSPart part)
    {
        if (part == null || _currentTarget == null)
            return;

        int index = IndexOfPart(part);
        if (index >= 0)
        {
            SelectPart(index);
        }

        QueueAttack(part);
    }

    void SelectPart(int index)
    {
        if (_currentTarget == null)
            return;

        IVATSPart[] parts = _currentTarget.Parts;
        if (parts == null || parts.Length == 0)
            return;

        _currentPartIndex = Mathf.Clamp(index, 0, parts.Length - 1);
        _currentTarget.HighlightPart(_currentPartIndex);
        UpdateSelectedPartDisplay();
    }

    void QueueSelectedPart()
    {
        QueueAttack(GetCurrentPart());
    }

    bool QueueAttack(IVATSPart part)
    {
        if (part == null || _currentTarget == null)
            return false;

        if (IndexOfPart(part) < 0)
            return false;

        float cost = Mathf.Max(0f, part.APCost);
        if (cost > 0f && _currentAP < cost)
        {
            Debug.LogWarning("AP가 부족하여 공격을 예약할 수 없습니다.");
            UpdateAPUI();
            return false;
        }

        if (cost > 0f)
        {
            _currentAP = Mathf.Max(0f, _currentAP - cost);
        }

        _queuedParts.Add(part);
        UpdateAPUI();
        return true;
    }

    void CancelLastQueuedAttack()
    {
        if (_queuedParts.Count == 0)
            return;

        IVATSPart part = _queuedParts[_queuedParts.Count - 1];
        _queuedParts.RemoveAt(_queuedParts.Count - 1);

        if (part != null)
        {
            float refund = Mathf.Max(0f, part.APCost);
            _currentAP = Mathf.Min(_currentAP + refund, _maxAP);
        }

        UpdateAPUI();
    }

    void CancelQueue(bool refundAll)
    {
        if (_queuedParts.Count == 0)
        {
            UpdateAPUI();
            return;
        }

        if (refundAll)
        {
            for (int i = 0; i < _queuedParts.Count; i++)
            {
                IVATSPart part = _queuedParts[i];
                if (part == null) continue;
                float refund = Mathf.Max(0f, part.APCost);
                _currentAP = Mathf.Min(_currentAP + refund, _maxAP);
            }
        }

        _queuedParts.Clear();
        UpdateAPUI();
    }

    void ExecuteQueuedAttacks()
    {
        if (_queuedParts.Count == 0)
            return;

        for (int i = 0; i < _queuedParts.Count; i++)
        {
            IVATSPart part = _queuedParts[i];
            if (part == null)
                continue;

            float hitChance = Mathf.Clamp(part.HitChance, 0f, 100f);
            float roll = Random.Range(0f, 100f);
            bool isHit = roll <= hitChance;

            if (isHit)
            {
                part.OnHit();
            }
            else
            {
                part.OnMiss();
            }

            _vatsUIView?.ShowAttackResult(part, isHit, roll, hitChance);
        }

        _queuedParts.Clear();
        UpdateAPUI();
    }

    public void ExecuteQueuedAttacksFromUI()
    {
        ExecuteQueuedAttacks();
    }

    public void ClearQueuedAttacksFromUI()
    {
        CancelQueue(true);
    }

    void UpdateAPUI()
    {
        if (_vatsUIView != null)
        {
            _vatsUIView.UpdateAP(_currentAP, _maxAP, _queuedParts);
        }
    }

    int IndexOfPart(IVATSPart part)
    {
        if (_currentTarget == null || part == null)
            return -1;

        IVATSPart[] parts = _currentTarget.Parts;
        if (parts == null)
            return -1;

        for (int i = 0; i < parts.Length; i++)
        {
            if (parts[i] == part)
                return i;
        }

        return -1;
    }

    void ToggleVATS()
    {
        _isVATSActive = !_isVATSActive;

        if (_isVATSActive)
        {
            _originalTimeScale = Time.timeScale;
            Time.timeScale = _vatsTimeScale;
            if (_vatsUI != null) _vatsUI.SetActive(true);
            if (_vatsUIView != null)
            {
                _vatsUIView.Show();
                BindCurrentTargetToUI();
                UpdateSelectedPartDisplay();
            }
        }
        else
        {
            Time.timeScale = _originalTimeScale;
            if (_vatsUI != null) _vatsUI.SetActive(false);
            if (_vatsUIView != null)
            {
                _vatsUIView.Hide();
                _vatsUIView.Clear();
            }

            if (_currentTarget != null)
            {
                _currentTarget.ClearHighlight();
                _currentTarget = null;
            }

            CancelQueue(true);
            if (_partNameText != null) _partNameText.text = string.Empty;
        }
    }

}
