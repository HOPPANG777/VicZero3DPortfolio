using UnityEngine;

/// <summary>
/// VATS에서 조준 가능한 대상
/// </summary>
public class VATSTarget : MonoBehaviour, IVATSTarget
{
    [Header("타격 부위 목록")]
    [SerializeField] private VATSPart[] _parts;
    public IVATSPart[] Parts { get { return _parts; } }

    [Header("체력 컴포넌트")]
    [SerializeField] private UnitHealth _unitHealth;
    public UnitHealth Health => _unitHealth;

    private int _highlightIndex = -1;

    void Awake()
    {
        CacheUnitHealth();
        CachePartHealthReferences();
    }

    void OnValidate()
    {
        CacheUnitHealth();
        CachePartHealthReferences();
    }

    void CacheUnitHealth()
    {
        if (_unitHealth == null)
        {
            _unitHealth = GetComponent<UnitHealth>();
        }
    }

    void CachePartHealthReferences()
    {
        if (_parts == null)
            return;

        for (int i = 0; i < _parts.Length; i++)
        {
            VATSPart part = _parts[i];
            if (part == null)
                continue;

            part.SetOwnerTarget(this);
        }
    }

    public void HighlightPart(int index)
    {
        ClearHighlight();
        if (index < 0 || index >= _parts.Length) return;
        _highlightIndex = index;
        _parts[index].Highlight(true);
    }

    public void ClearHighlight()
    {
        if (_highlightIndex < 0) return;
        _parts[_highlightIndex].Highlight(false);
        _highlightIndex = -1;
    }
}
