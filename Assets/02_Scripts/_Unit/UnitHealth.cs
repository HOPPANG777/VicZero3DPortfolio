using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 공통 적 유닛의 체력 및 피해 처리 컴포넌트.
/// </summary>
public class UnitHealth : MonoBehaviour
{
    [Header("체력 설정")]
    [SerializeField] private float _maxHealth = 100f;
    [SerializeField] private float _startingHealth = 100f;
    [SerializeField] private bool _destroyOnDeath = true;

    [Header("피해/사망 이벤트")]
    [SerializeField] private UnityEvent<float> _onDamaged = new UnityEvent<float>();
    [SerializeField] private UnityEvent<float> _onHealthChanged = new UnityEvent<float>();
    [SerializeField] private UnityEvent _onDeath = new UnityEvent();

    public float MaxHealth => _maxHealth;
    public float CurrentHealth { get; private set; }
    public bool IsDead => CurrentHealth <= 0f;

    void Awake()
    {
        InitialiseHealth();
    }

    void OnValidate()
    {
        _maxHealth = Mathf.Max(1f, _maxHealth);
        _startingHealth = Mathf.Clamp(_startingHealth, 0f, _maxHealth);
    }

    /// <summary>
    /// 외부에서 초기 체력을 설정하거나 재설정할 때 호출한다.
    /// </summary>
    public void InitialiseHealth(float? overrideStartingHealth = null)
    {
        float startingHealth = overrideStartingHealth.HasValue
            ? Mathf.Clamp(overrideStartingHealth.Value, 0f, _maxHealth)
            : _startingHealth;

        CurrentHealth = startingHealth;
        _onHealthChanged?.Invoke(CurrentHealth);
    }

    /// <summary>
    /// 지정한 양만큼 피해를 적용한다.
    /// </summary>
    /// <param name="amount">적용할 피해량.</param>
    public void ApplyDamage(float amount)
    {
        if (amount <= 0f || IsDead)
            return;

        float previousHealth = CurrentHealth;
        CurrentHealth = Mathf.Max(0f, CurrentHealth - amount);

        float actualDamage = previousHealth - CurrentHealth;
        if (actualDamage > 0f)
        {
            _onDamaged?.Invoke(actualDamage);
            _onHealthChanged?.Invoke(CurrentHealth);
        }

        if (CurrentHealth <= 0f)
        {
            HandleDeath();
        }
    }

    void HandleDeath()
    {
        _onDeath?.Invoke();

        if (_destroyOnDeath)
        {
            Destroy(gameObject);
        }
    }
}
