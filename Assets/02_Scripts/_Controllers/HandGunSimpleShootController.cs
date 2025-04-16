using UnityEngine;
using UnityEngine.Pool;
using System.Collections;

public class HandGunSimpleShootController : MonoBehaviour
{
    [Header("Prefab Refrences")]
    [SerializeField] private GameObject _bulletPrefab;
    public GameObject BulletPrefab { get { return _bulletPrefab; } }

    [SerializeField] private GameObject _casingPrefab;
    public GameObject CasingPrefab { get {return _casingPrefab; } }

    [SerializeField] private GameObject _muzzleFlashPrefab;
    public GameObject MuzzleFlashPrefab { get { return _muzzleFlashPrefab;} }

    [Header("Location Refrences")]
    [SerializeField] private Animator _gunAnimator;
    public Animator GunAnimator { get { return _gunAnimator; } }
    
    [SerializeField] private Transform _barrelLocation;
    public Transform BarrelLocat { get { return _barrelLocation; } }

    [SerializeField] private Transform _casingExitLocation;
    public Transform CasingExitLocation { get { return _casingExitLocation; } }

    [Header("Settings")]
    [Tooltip("Specify time to destroy the casing object")]
    [SerializeField] private float _destroyTimer; // 기본 2.0f
    public float DestroyTimer { get { return _destroyTimer; } }

    [Tooltip("Bullet Speed")]
    [SerializeField] private float _shotPower; // 기본 500.0f
    public float ShotPower { get { return _shotPower; } }

    [Tooltip("Casing Ejection Speed")]
    [SerializeField] private float _ejectPower; // 기본 150.0f
    public float EjectPower { get { return _ejectPower; } }

    [Tooltip("Bullet Lifetime (seconds) before returning to pool")]
    [SerializeField] private float _bulletLifetime; // 기본 5.0f
    public float BulletLifetime { get { return _bulletLifetime; } }

    // 총알 오브젝트 풀 (UnityEngine.Pool API 사용)

    [SerializeField] private ObjectPool<GameObject> _bulletPool;
    public ObjectPool<GameObject> BulletPool { get { return _bulletPool; } }

    void Start()
    {
        if (_barrelLocation == null) _barrelLocation = transform;

        if (_gunAnimator == null) _gunAnimator = GetComponentInChildren<Animator>();

        // 오브젝트 풀 초기화
        _bulletPool = new ObjectPool<GameObject>
        (
            CreateBullet,         // 새 총알 생성 함수
            OnTakeFromPool,       // 풀에서 꺼낼 때 호출
            OnReturnedToPool,     // 풀에 반납할 때 호출
            OnDestroyPoolObject,  // 풀에서 삭제될 때 호출
            collectionCheck: false,
            defaultCapacity: 10,
            maxSize: 20
        );
    }

    void Update()
    {
        // Fire 입력 (예: 좌클릭) 시 총 발사 애니메이션 트리거
        if (Input.GetMouseButtonDown(0))
        {
            _gunAnimator.SetTrigger("Fire");
        }
    }

    // Animation Event 등에서 호출하여 총 발사 처리
    void Shoot()
    {
        // Muzzle Flash 생성
        if (_muzzleFlashPrefab)
        {
            GameObject tempFlash = Instantiate(_muzzleFlashPrefab, _barrelLocation.position, _barrelLocation.rotation);
            Destroy(tempFlash, _destroyTimer);
        }

        // 총알 Prefab이 할당되지 않았다면 종료
        if (!_bulletPrefab) return;

        // 오브젝트 풀에서 총알 가져오기 대신 Instantiate하지 않음
        GameObject bullet = _bulletPool.Get();
        bullet.transform.position = _barrelLocation.position;
        bullet.transform.rotation = _barrelLocation.rotation;

        // Rigidbody 리셋 및 총구 방향으로 힘 가하기
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.AddForce(_barrelLocation.forward * _shotPower);
        }

        // bulletLifetime 후 총알을 풀에 반납 (반환하는 코루틴 실행)
        StartCoroutine(ReturnBulletAfterTime(bullet, _bulletLifetime));
    }

    // 총알 오브젝트 생성 함수 (초기 생성시 호출됨)
    GameObject CreateBullet()
    {
        GameObject bullet = Instantiate(_bulletPrefab, _barrelLocation.position, _barrelLocation.rotation);
        bullet.SetActive(false);
        return bullet;
    }

    // 오브젝트 풀에서 총알을 꺼낼 때 호출
    void OnTakeFromPool(GameObject bullet)
    {
        bullet.SetActive(true);
    }

    // 총알을 풀에 반납할 때 호출
    void OnReturnedToPool(GameObject bullet)
    {
        bullet.SetActive(false);
    }

    // 총알이 파괴될 때 호출 (풀의 maxSize 초과 등)
    void OnDestroyPoolObject(GameObject bullet)
    {
        Destroy(bullet);
    }

    // 일정 시간이 지난 후 총알을 다시 풀로 반납하는 코루틴
    IEnumerator ReturnBulletAfterTime(GameObject bullet, float time)
    {
        yield return new WaitForSeconds(time);
        _bulletPool.Release(bullet);
    }

    // 기존 CasingRelease 함수 (총알 풀링과 별개로 casings은 Instantiate/Destroy 방식)
    void CasingRelease()
    {
        if (!_casingExitLocation || !_casingPrefab) return;

        GameObject tempCasing = Instantiate(_casingPrefab, _casingExitLocation.position, _casingExitLocation.rotation);
        Rigidbody casingRb = tempCasing.GetComponent<Rigidbody>();

        if (casingRb != null)
        {
            casingRb.AddExplosionForce(Random.Range(_ejectPower * 0.7f, _ejectPower), _casingExitLocation.position - _casingExitLocation.right * 0.3f - _casingExitLocation.up * 0.6f, 1f);
            casingRb.AddTorque(new Vector3(0, Random.Range(100f, 500f), Random.Range(100f, 1000f)), ForceMode.Impulse);
        }
        Destroy(tempCasing, _destroyTimer);
    }
}
