/// <summary>
/// VATS 타겟 오브젝트 인터페이스
/// </summary>
public interface IVATSTarget
{
    IVATSPart[] Parts { get; }
    void HighlightPart(int index);
    void ClearHighlight();
}
