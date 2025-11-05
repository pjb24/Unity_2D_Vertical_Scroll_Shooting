using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    [SerializeField] private GameObject _enemyLPrefab;
    [SerializeField] private GameObject _enemyMPrefab;
    [SerializeField] private GameObject _enemySPrefab;
    [SerializeField] private GameObject _enemyBPrefab;

    [SerializeField] private GameObject _itemCoinPrefab;
    [SerializeField] private GameObject _itemPowerPrefab;
    [SerializeField] private GameObject _itemBoomPrefab;

    [SerializeField] private GameObject _bulletPlayerAPrefab;
    [SerializeField] private GameObject _bulletPlayerBPrefab;
    [SerializeField] private GameObject _bulletEnemyAPrefab;
    [SerializeField] private GameObject _bulletEnemyBPrefab;
    [SerializeField] private GameObject _bulletBossAPrefab;
    [SerializeField] private GameObject _bulletBossBPrefab;
    [SerializeField] private GameObject _bulletFollowerPrefab;
    
    [SerializeField] private GameObject _explosionPrefab;

    private GameObject[] _enemyL;
    private GameObject[] _enemyM;
    private GameObject[] _enemyS;
    private GameObject[] _enemyB;

    private GameObject[] _itemCoin;
    private GameObject[] _itemPower;
    private GameObject[] _itemBoom;

    private GameObject[] _bulletPlayerA;
    private GameObject[] _bulletPlayerB;
    private GameObject[] _bulletEnemyA;
    private GameObject[] _bulletEnemyB;
    private GameObject[] _bulletBossA;
    private GameObject[] _bulletBossB;
    private GameObject[] _bulletFollower;

    private GameObject[] _explosion;

    private GameObject[] _targetPool;

    private void Awake()
    {
        _enemyL = new GameObject[10];
        _enemyM = new GameObject[10];
        _enemyS = new GameObject[20];
        _enemyB = new GameObject[1];

        _itemCoin = new GameObject[20];
        _itemPower = new GameObject[10];
        _itemBoom = new GameObject[10];

        _bulletPlayerA = new GameObject[100];
        _bulletPlayerB = new GameObject[100];
        _bulletEnemyA = new GameObject[100];
        _bulletEnemyB = new GameObject[100];
        _bulletBossA = new GameObject[100];
        _bulletBossB = new GameObject[500];
        _bulletFollower = new GameObject[100];

        _explosion = new GameObject[20];

        Generate();
    }

    void Generate()
    {
        // Enemy
        for (int index = 0; index < _enemyL.Length; index++)
        {
            GameObject obj = Instantiate(_enemyLPrefab);
            obj.SetActive(false);
            _enemyL[index] = obj;
        }
        for (int index = 0; index < _enemyM.Length; index++)
        {
            GameObject obj = Instantiate(_enemyMPrefab);
            obj.SetActive(false);
            _enemyM[index] = obj;
        }
        for (int index = 0; index < _enemyS.Length; index++)
        {
            GameObject obj = Instantiate(_enemySPrefab);
            obj.SetActive(false);
            _enemyS[index] = obj;
        }
        for (int index = 0; index < _enemyB.Length; index++)
        {
            GameObject obj = Instantiate(_enemyBPrefab);
            obj.SetActive(false);
            _enemyB[index] = obj;
        }

        // Item
        for (int index = 0; index < _itemCoin.Length; index++)
        {
            GameObject obj = Instantiate(_itemCoinPrefab);
            obj.SetActive(false);
            _itemCoin[index] = obj;
        }
        for (int index = 0; index < _itemPower.Length; index++)
        {
            GameObject obj = Instantiate(_itemPowerPrefab);
            obj.SetActive(false);
            _itemPower[index] = obj;
        }
        for (int index = 0; index < _itemBoom.Length; index++)
        {
            GameObject obj = Instantiate(_itemBoomPrefab);
            obj.SetActive(false);
            _itemBoom[index] = obj;
        }

        // Bullet
        for (int index = 0; index < _bulletPlayerA.Length; index++)
        {
            GameObject obj = Instantiate(_bulletPlayerAPrefab);
            obj.SetActive(false);
            _bulletPlayerA[index] = obj;
        }
        for (int index = 0; index < _bulletPlayerB.Length; index++)
        {
            GameObject obj = Instantiate(_bulletPlayerBPrefab);
            obj.SetActive(false);
            _bulletPlayerB[index] = obj;
        }
        for (int index = 0; index < _bulletEnemyA.Length; index++)
        {
            GameObject obj = Instantiate(_bulletEnemyAPrefab);
            obj.SetActive(false);
            _bulletEnemyA[index] = obj;
        }
        for (int index = 0; index < _bulletEnemyB.Length; index++)
        {
            GameObject obj = Instantiate(_bulletEnemyBPrefab);
            obj.SetActive(false);
            _bulletEnemyB[index] = obj;
        }
        for (int index = 0; index < _bulletBossA.Length; index++)
        {
            GameObject obj = Instantiate(_bulletBossAPrefab);
            obj.SetActive(false);
            _bulletBossA[index] = obj;
        }
        for (int index = 0; index < _bulletBossB.Length; index++)
        {
            GameObject obj = Instantiate(_bulletBossBPrefab);
            obj.SetActive(false);
            _bulletBossB[index] = obj;
        }
        for (int index = 0; index < _bulletFollower.Length; index++)
        {
            GameObject obj = Instantiate(_bulletFollowerPrefab);
            obj.SetActive(false);
            _bulletFollower[index] = obj;
        }

        for (int index = 0; index < _explosion.Length; index++)
        {
            GameObject obj = Instantiate(_explosionPrefab);
            obj.SetActive(false);
            _explosion[index] = obj;
        }
    }

    public GameObject MakeObj(string type)
    {
        switch (type)
        {
            case "EnemyL":
                {
                    _targetPool = _enemyL;
                    break;
                }
            case "EnemyM":
                {
                    _targetPool = _enemyM;
                    break;
                }
            case "EnemyS":
                {
                    _targetPool = _enemyS;
                    break;
                }
            case "EnemyB":
                {
                    _targetPool = _enemyB;
                    break;
                }

            case "ItemCoin":
                {
                    _targetPool = _itemCoin;
                    break;
                }
            case "ItemPower":
                {
                    _targetPool = _itemPower;
                    break;
                }
            case "ItemBoom":
                {
                    _targetPool = _itemBoom;
                    break;
                }

            case "BulletPlayerA":
                {
                    _targetPool = _bulletPlayerA;
                    break;
                }
            case "BulletPlayerB":
                {
                    _targetPool = _bulletPlayerB;
                    break;
                }
            case "BulletEnemyA":
                {
                    _targetPool = _bulletEnemyA;
                    break;
                }
            case "BulletEnemyB":
                {
                    _targetPool = _bulletEnemyB;
                    break;
                }
            case "BulletBossA":
                {
                    _targetPool = _bulletBossA;
                    break;
                }
            case "BulletBossB":
                {
                    _targetPool = _bulletBossB;
                    break;
                }
            case "BulletFollower":
                {
                    _targetPool = _bulletFollower;
                    break;
                }

            case "Explosion":
                {
                    _targetPool = _explosion;
                    break;
                }

            default:
                {
                    Debug.LogError("Requested Non Setted Parameter: " + type);
                    break;
                }
        }

        for (int index = 0; index < _targetPool.Length; index++)
        {
            if (!_targetPool[index].gameObject.activeSelf)
            {
                _targetPool[index].SetActive(true);
                return _targetPool[index];
            }
        }

        return null;
    }

    public GameObject[] GetPool(string type)
    {
        switch (type)
        {
            case "EnemyL":
                {
                    _targetPool = _enemyL;
                    break;
                }
            case "EnemyM":
                {
                    _targetPool = _enemyM;
                    break;
                }
            case "EnemyS":
                {
                    _targetPool = _enemyS;
                    break;
                }
            case "EnemyB":
                {
                    _targetPool = _enemyB;
                    break;
                }

            case "ItemCoin":
                {
                    _targetPool = _itemCoin;
                    break;
                }
            case "ItemPower":
                {
                    _targetPool = _itemPower;
                    break;
                }
            case "ItemBoom":
                {
                    _targetPool = _itemBoom;
                    break;
                }

            case "BulletPlayerA":
                {
                    _targetPool = _bulletPlayerA;
                    break;
                }
            case "BulletPlayerB":
                {
                    _targetPool = _bulletPlayerB;
                    break;
                }
            case "BulletEnemyA":
                {
                    _targetPool = _bulletEnemyA;
                    break;
                }
            case "BulletEnemyB":
                {
                    _targetPool = _bulletEnemyB;
                    break;
                }
            case "BulletBossA":
                {
                    _targetPool = _bulletBossA;
                    break;
                }
            case "BulletBossB":
                {
                    _targetPool = _bulletBossB;
                    break;
                }
            case "BulletFollower":
                {
                    _targetPool = _bulletFollower;
                    break;
                }

            case "Explosion":
                {
                    _targetPool = _explosion;
                    break;
                }

            default:
                {
                    Debug.LogError("Requested Non Setted Parameter: " + type);
                    break;
                }
        }

        return _targetPool;
    }

    public void DeleteAllEnemyBullets()
    {
        foreach (var obj in GetPool("BulletEnemyA"))
        {
            obj.SetActive(false);
        }
        foreach (var obj in GetPool("BulletEnemyB"))
        {
            obj.SetActive(false);
        }
        foreach (var obj in GetPool("BulletBossA"))
        {
            obj.SetActive(false);
        }
        foreach (var obj in GetPool("BulletBossB"))
        {
            obj.SetActive(false);
        }
    }
}
