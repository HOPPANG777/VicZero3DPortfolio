using System;
using System.Collections.Generic;

/// <summary>
/// VATS UI 오버레이와 상호작용하기 위한 인터페이스
/// </summary>
public interface IVATSUIView
{
    /// <summary>
    /// UI를 표시한다.
    /// </summary>
    void Show();

    /// <summary>
    /// UI를 숨긴다.
    /// </summary>
    void Hide();

    /// <summary>
    /// 현재 타겟의 파츠 정보를 UI에 바인딩한다.
    /// </summary>
    /// <param name="parts">표시할 파츠 목록</param>
    /// <param name="onPartClicked">클릭 콜백</param>
    void BindParts(IReadOnlyList<IVATSPart> parts, Action<IVATSPart> onPartClicked);

    /// <summary>
    /// 현재 선택된 파츠 정보를 갱신한다.
    /// </summary>
    /// <param name="part">선택된 파츠</param>
    void UpdateSelectedPart(IVATSPart part);

    /// <summary>
    /// AP 게이지와 예약된 공격 정보를 갱신한다.
    /// </summary>
    /// <param name="current">현재 남은 AP</param>
    /// <param name="max">최대 AP</param>
    /// <param name="queuedParts">예약된 공격 파츠 목록</param>
    void UpdateAP(float current, float max, IReadOnlyList<IVATSPart> queuedParts);

    /// <summary>
    /// 공격 결과를 표시한다.
    /// </summary>
    /// <param name="part">공격한 파츠</param>
    /// <param name="isHit">명중 여부</param>
    /// <param name="roll">명중 판정을 위한 주사위 값</param>
    /// <param name="hitChance">파츠의 명중 확률</param>
    void ShowAttackResult(IVATSPart part, bool isHit, float roll, float hitChance);

    /// <summary>
    /// 표시 중인 파츠와 상태 정보를 모두 초기화한다.
    /// </summary>
    void Clear();
}
