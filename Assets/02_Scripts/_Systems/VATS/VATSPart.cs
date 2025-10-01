using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// VATS에서 타격 가능한 신체 부위
/// </summary>
public class VATSPart : MonoBehaviour, IVATSPart
{
    [Header("부위 이름")]
    [SerializeField] private string _partName;
    public string PartName { get { return _partName; } }

    [Header("명중 확률(%)")]
    [SerializeField] private float _hitChance = 100f;
    public float HitChance { get { return _hitChance; } }

    [Header("필요 AP")]
    [SerializeField] private float _apCost = 1f;
    public float APCost { get { return Mathf.Max(0f, _apCost); } }

    [Header("조준 지점")]
    [SerializeField] private Transform _aimPoint;
    public Transform AimPoint { get { return _aimPoint ? _aimPoint : this.transform; } }

    [Header("하이라이트 색상")]
    [SerializeField] private Color _highlightColor = Color.yellow;

    [Header("피해 처리")]
    [SerializeField] private float _damageAmount = 10f;
    [SerializeField] private UnitHealth _unitHealth;

    [Header("피드백 트리거")]
    [SerializeField] private UnityEvent _onHitFeedback = new UnityEvent();
    [SerializeField] private UnityEvent _onMissFeedback = new UnityEvent();

    private Renderer _renderer;
    private Color _originalColor;
    private VATSTarget _ownerTarget;

    void Awake()
    {
        _renderer = GetComponent<Renderer>();
        if (_renderer != null)
        {
            _originalColor = _renderer.material.color;
        }

        CacheUnitHealth();
    }

    void OnValidate()
    {
        _hitChance = Mathf.Clamp(_hitChance, 0f, 100f);
        _damageAmount = Mathf.Max(0f, _damageAmount);

        if (_renderer == null)
        {
            _renderer = GetComponent<Renderer>();
            if (_renderer != null)
            {
                _originalColor = _renderer.sharedMaterial != null
                    ? _renderer.sharedMaterial.color
                    : Color.white;
            }
        }

        CacheUnitHealth();
    }

    public void SetOwnerTarget(VATSTarget target)
    {
        _ownerTarget = target;
        if (_unitHealth == null && _ownerTarget != null)
        {
            _unitHealth = _ownerTarget.Health;
        }

        CacheUnitHealth();
    }

    void CacheUnitHealth()
    {
        if (_unitHealth == null)
        {
            _unitHealth = GetComponentInParent<UnitHealth>();
        }
    }

    public void Highlight(bool enable)
    {
        if (_renderer == null) return;
        _renderer.material.color = enable ? _highlightColor : _originalColor;
    }

    public void OnHit()
    {
        if (_unitHealth == null)
        {
            CacheUnitHealth();
        }

        if (_unitHealth != null)
        {
            _unitHealth.ApplyDamage(_damageAmount);
        }
        else
        {
            Debug.LogWarning($"{name}에서 UnitHealth를 찾을 수 없습니다.");
        }

        _onHitFeedback?.Invoke();
    }

    public void OnMiss()
    {
        _onMissFeedback?.Invoke();
    }
}
