using UnityEngine;

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

    [Header("조준 지점")]
    [SerializeField] private Transform _aimPoint;
    public Transform AimPoint { get { return _aimPoint ? _aimPoint : this.transform; } }

    [Header("하이라이트 색상")]
    [SerializeField] private Color _highlightColor = Color.yellow;

    private Renderer _renderer;
    private Color _originalColor;

    void Awake()
    {
        _renderer = GetComponent<Renderer>();
        if (_renderer != null)
        {
            _originalColor = _renderer.material.color;
        }
    }

    public void Highlight(bool enable)
    {
        if (_renderer == null) return;
        _renderer.material.color = enable ? _highlightColor : _originalColor;
    }

    public void OnHit()
    {
        // 맞았을 때의 기본 동작 : 여기서는 단순 로그 처리
        Debug.Log($"{_partName} 부위가 명중됨");
    }
}
