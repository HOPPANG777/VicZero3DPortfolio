using UnityEngine;
using UnityEngine.UI;

//==============================================================================
// VATSController
//==============================================================================
// 폴아웃에 나온 VATS 시스템 구현
// 
//==============================================================================

public class VATSController : MonoBehaviour
{
    [Header("VATS Settings")]
    [Tooltip("Time scale during VATS mode (e.g., 0.2 for slow motion)")]
    [SerializeField] private float _vatsTimeScale;
    public float VATSTimeScale { get { return _vatsTimeScale; } set { _vatsTimeScale = value; } }

    [Tooltip("Key to toggle VATS mode")]
    [SerializeField] private KeyCode _vatsToggleKey = KeyCode.V;
    public KeyCode VATSToggleKey { get { return _vatsToggleKey; } }

    [Header("UI")]
    [Tooltip("VATS UI overlay (e.g., targeting reticle & enemy highlights)")]
    [SerializeField] private GameObject _vatsUI;
    public GameObject VatsUI { get { return _vatsUI; } } 

    [SerializeField] private bool _isVATSActive; // false
    public bool isVATSActive { get { return _isVATSActive; } }

    [SerializeField] private float originalTimeScale; // 1f
    public float OriginalTimeScale { get { return originalTimeScale; } }

    // 타깃 검출(예: Raycast 또는 SphereCast를 통한 간단한 구현)
    // 추후 세부 타깃(적의 부위) 선택 로직을 추가할 수 있음.
    void Update()
    {
        // VATS 모드 토글 입력 감지 (위 코드와 중복됨)
        if (Input.GetKeyDown(_vatsToggleKey))
        {
            ToggleVATS();
        }

        // VATS 모드일 때 타깃 검출 예시
        if (isVATSActive)
        {
            RaycastHit hit;
            // 카메라 중앙에서 Raycast
            Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            if (Physics.Raycast(ray, out hit, 100f))
            {
                // 예: hit.collider가 적의 태그("Enemy")를 가진 경우
                if (hit.collider.CompareTag("Enemy"))
                {
                    // UI에서 해당 적을 하이라이트하거나, 조준 인디케이터 업데이트
                    // Debug.Log("타깃 후보: " + hit.collider.gameObject.name);
                }
            }
        }
    }

    void ToggleVATS()
    {
        _isVATSActive = !_isVATSActive;

        if (_isVATSActive)
        {
            // VATS 모드 진입: 시간 느리게, UI 표시
            originalTimeScale = Time.timeScale;
            Time.timeScale = _vatsTimeScale;
            if (_vatsUI != null)
            {
                _vatsUI.SetActive(true);
            }
        }
        else
        {
            // VATS 모드 해제: 원래 시간 복귀, UI 숨김
            Time.timeScale = originalTimeScale;
            if (_vatsUI != null)
            {
                _vatsUI.SetActive(false);
            }
        }
    }
}
