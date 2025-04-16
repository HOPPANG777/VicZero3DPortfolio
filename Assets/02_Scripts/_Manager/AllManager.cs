using UnityEngine;

//==============================================================================
// AllManager
//==============================================================================
// AllManager 싱글톤 매니저는 이거 하나고 나머지는 모두 컨트롤러로 
// 그리고 해당 컨트롤러를 접근하는것은 이 AllManager를 통해서만 가능하도록 한다.
// 
//==============================================================================

public class AllManager : Singleton<AllManager>
{
    [Header("Controller")]
    [SerializeField] private GameModeController _gameModeController;
    public GameModeController GameModeController {  get { return _gameModeController; } set { _gameModeController = value; } }

    private void Start()
    {
        Screen.SetResolution(1920, 1080, true);
        Application.targetFrameRate = 60;

        if (_gameModeController == null) Debug.Log("GameMode Controller가 없습니다.");
    }
}
