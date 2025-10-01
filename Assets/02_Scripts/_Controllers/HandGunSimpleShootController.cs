using UnityEngine;
using UnityEngine.Pool;
using System.Collections;

public class HandGunSimpleShootController : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private VATSController _vatsController;
    public VATSController VatsController { get { return _vatsController; } set { _vatsController = value; } }

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
    [SerializeField] private float _destroyTimer; // ⺻ 2.0f
    public float DestroyTimer { get { return _destroyTimer; } }

    [Tooltip("Bullet Speed")]
    [SerializeField] private float _shotPower; // ⺻ 500.0f
    public float ShotPower { get { return _shotPower; } }

    [Tooltip("Casing Ejection Speed")]
    [SerializeField] private float _ejectPower; // ⺻ 150.0f
    public float EjectPower { get { return _ejectPower; } }

    [Tooltip("Bullet Lifetime (seconds) before returning to pool")]
    [SerializeField] private float _bulletLifetime; // ⺻ 5.0f
    public float BulletLifetime { get { return _bulletLifetime; } }

    // Ѿ Ʈ Ǯ (UnityEngine.Pool API )

    [SerializeField] private ObjectPool<GameObject> _bulletPool;
    public ObjectPool<GameObject> BulletPool { get { return _bulletPool; } }

    void Start()
    {
        if (_vatsController == null)
            _vatsController = FindFirstObjectByType<VATSController>();

        if (_barrelLocation == null) _barrelLocation = transform;

        if (_gunAnimator == null) _gunAnimator = GetComponentInChildren<Animator>();

        // Ʈ Ǯ ʱȭ
        _bulletPool = new ObjectPool<GameObject>
        (
            CreateBullet,         //  Ѿ  Լ
            OnTakeFromPool,       // Ǯ   ȣ
            OnReturnedToPool,     // Ǯ ݳ  ȣ
            OnDestroyPoolObject,  // Ǯ   ȣ
            collectionCheck: false,
            defaultCapacity: 10,
            maxSize: 20
        );
    }

    void Update()
    {
        if (_vatsController != null && _vatsController.IsVATSActive)
            return;

        // Fire Է (: Ŭ)   ߻ ִϸ̼ Ʈ
        if (Input.GetMouseButtonDown(0))
        {
            _gunAnimator.SetTrigger("Fire");
        }
    }

    // Animation Event  ȣϿ  ߻ ó
    void Shoot()
    {
        // Muzzle Flash 
        if (_muzzleFlashPrefab)
        {
            GameObject tempFlash = Instantiate(_muzzleFlashPrefab, _barrelLocation.position, _barrelLocation.rotation);
            Destroy(tempFlash, _destroyTimer);
        }

        // Ѿ Prefab Ҵ ʾҴٸ 
        if (!_bulletPrefab) return;

        // Ʈ Ǯ Ѿ   Instantiate 
        GameObject bullet = _bulletPool.Get();
        bullet.transform.position = _barrelLocation.position;
        bullet.transform.rotation = _barrelLocation.rotation;

        // Rigidbody   ѱ   ϱ
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.AddForce(_barrelLocation.forward * _shotPower);
        }

        // bulletLifetime  Ѿ Ǯ ݳ (ȯϴ ڷƾ )
        StartCoroutine(ReturnBulletAfterTime(bullet, _bulletLifetime));
    }

    // Ѿ Ʈ  Լ (ʱ  ȣ)
    GameObject CreateBullet()
    {
        GameObject bullet = Instantiate(_bulletPrefab, _barrelLocation.position, _barrelLocation.rotation);
        bullet.SetActive(false);
        return bullet;
    }

    // Ʈ Ǯ Ѿ   ȣ
    void OnTakeFromPool(GameObject bullet)
    {
        bullet.SetActive(true);
    }

    // Ѿ Ǯ ݳ  ȣ
    void OnReturnedToPool(GameObject bullet)
    {
        bullet.SetActive(false);
    }

    // Ѿ ı  ȣ (Ǯ maxSize ʰ )
    void OnDestroyPoolObject(GameObject bullet)
    {
        Destroy(bullet);
    }

    //  ð   Ѿ ٽ Ǯ ݳϴ ڷƾ
    IEnumerator ReturnBulletAfterTime(GameObject bullet, float time)
    {
        yield return new WaitForSeconds(time);
        _bulletPool.Release(bullet);
    }

    //  CasingRelease Լ (Ѿ Ǯ  casings Instantiate/Destroy )
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
