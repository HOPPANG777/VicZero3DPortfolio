using UnityEngine;

/// <summary>
/// VATS에서 조준 가능한 대상
/// </summary>
public class VATSTarget : MonoBehaviour, IVATSTarget
{
    [Header("타격 부위 목록")]
    [SerializeField] private VATSPart[] _parts;
    public IVATSPart[] Parts { get { return _parts; } }

    private int _highlightIndex = -1;

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
