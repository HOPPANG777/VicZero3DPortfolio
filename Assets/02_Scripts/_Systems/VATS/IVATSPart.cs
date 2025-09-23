using UnityEngine;

/// <summary>
/// VATS에서 선택 가능한 부위에 대한 인터페이스
/// </summary>
public interface IVATSPart
{
    string PartName { get; }
    float HitChance { get; }
    Transform AimPoint { get; }
    float APCost { get; }
    void Highlight(bool enable);
    void OnHit();
}
