using System.Runtime.Serialization;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private ObjectManager _objectManager;
    public void SetObjectManager(ObjectManager objectManager) { _objectManager = objectManager; }

    [SerializeField] private string _enemyName;

    [SerializeField] private float _speed = 3f;
    public float Speed => _speed;
    [SerializeField] private int _health = 3;
    [SerializeField] private Sprite[] _sprites;
    [SerializeField] private float _delayReturnSprite = 0.1f;
    [SerializeField] private float _maxShotDelay = 3f;
    [SerializeField] private int _enemyScore = 50;

    private string _bulletObjectA;
    private string _bulletObjectB;
    private string _itemCoin;
    private string _itemPower;
    private string _itemBoom;

    private int _curHealth;

    private float _curShotDelay;
    private SpriteRenderer _spriteRenderer;

    private GameObject _player;
    public GameObject Player { set { _player = value; } }

    private Animator _animator;

    private int _patternIndex = -1;
    private int _curPatternCount;
    [SerializeField] private int[] _maxPatternCount;

    private GameManager _gameManager;
    public void SetGameManager(GameManager gameManager) { _gameManager = gameManager; }

    void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();

        if (_enemyName == "B")
        {
            _animator = GetComponent<Animator>();
        }

        _bulletObjectA = "BulletEnemyA";
        _bulletObjectB = "BulletEnemyB";
        _itemCoin = "ItemCoin";
        _itemPower = "ItemPower";
        _itemBoom = "ItemBoom";
    }

    private void OnEnable()
    {
        _curHealth = _health;

        if (_enemyName == "B")
        {
            Invoke("Stop", 2f);
        }
    }

    private void Stop()
    {
        if (!gameObject.activeSelf)
        {
            return;
        }

        Rigidbody2D rigid = GetComponent<Rigidbody2D>();
        rigid.linearVelocity = Vector2.zero;

        Invoke("Think", 2f);
    }

    private void Think()
    {
        _patternIndex = _patternIndex == 3 ? 0 : _patternIndex + 1;
        _curPatternCount = 0;

        switch (_patternIndex)
        {
            case 0:
                {
                    FireForward();
                    break;
                }
            case 1:
                {
                    FireShot();
                    break;
                }
            case 2:
                {
                    FireArc();
                    break;
                }
            case 3:
                {
                    FireAround();
                    break;
                }
            default:
                {
                    Debug.LogError("Requested Non Setted Parameter: " + _patternIndex);
                    break;
                }
        }


    }

    private void FireForward()
    {
        if (_curHealth <= 0)
        {
            return;
        }

        Debug.Log("앞으로 4발 발사.");

        // Fire 4 Bullet Forward
        GameObject bulletR = _objectManager.MakeObj("BulletBossA");
        bulletR.transform.position = transform.position + Vector3.right * 0.3f;
        GameObject bulletRR = _objectManager.MakeObj("BulletBossA");
        bulletRR.transform.position = transform.position + Vector3.right * 0.45f;
        GameObject bulletL = _objectManager.MakeObj("BulletBossA");
        bulletL.transform.position = transform.position + Vector3.left * 0.3f;
        GameObject bulletLL = _objectManager.MakeObj("BulletBossA");
        bulletLL.transform.position = transform.position + Vector3.left * 0.45f;

        Rigidbody2D rigidR = bulletR.GetComponent<Rigidbody2D>();
        Rigidbody2D rigidRR = bulletRR.GetComponent<Rigidbody2D>();
        Rigidbody2D rigidL = bulletL.GetComponent<Rigidbody2D>();
        Rigidbody2D rigidLL = bulletLL.GetComponent<Rigidbody2D>();

        rigidR.AddForce(Vector2.down * 8f, ForceMode2D.Impulse);
        rigidRR.AddForce(Vector2.down * 8f, ForceMode2D.Impulse);
        rigidL.AddForce(Vector2.down * 8f, ForceMode2D.Impulse);
        rigidLL.AddForce(Vector2.down * 8f, ForceMode2D.Impulse);

        // Patttern Counting
        _curPatternCount++;

        if (_curPatternCount < _maxPatternCount[_patternIndex])
        {
            Invoke("FireForward", 2f);
        }
        else
        {
            Invoke("Think", 3f);
        }
    }

    private void FireShot()
    {
        if (_curHealth <= 0)
        {
            return;
        }

        Debug.Log("플레이어 방향으로 샷건.");

        // Fire 5 Random Shotgun Bullet to Player
        for (int index = 0; index < 5; index++)
        {
            GameObject bullet = _objectManager.MakeObj("BulletEnemyB");
            bullet.transform.position = transform.position;

            Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();

            Vector2 dirVec = _player.transform.position - transform.position;
            Vector2 ranVec = new Vector2(Random.Range(-0.5f, 0.5f), Random.Range(0f, 2f));
            dirVec += ranVec;
            rigid.AddForce(dirVec.normalized * 5f, ForceMode2D.Impulse);
        }

        // Pattern Counting
        _curPatternCount++;

        if (_curPatternCount < _maxPatternCount[_patternIndex])
        {
            Invoke("FireShot", 3.5f);
        }
        else
        {
            Invoke("Think", 3f);
        }
    }

    private void FireArc()
    {
        if (_curHealth <= 0)
        {
            return;
        }

        Debug.Log("부채모양으로 발사.");

        // Fire Arc Continue Fire
        GameObject bullet = _objectManager.MakeObj("BulletEnemyA");
        bullet.transform.position = transform.position;

        Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();

        Vector2 dirVec = new Vector2(Mathf.Cos(Mathf.PI * 10 * _curPatternCount / _maxPatternCount[_patternIndex]), -1);
        rigid.AddForce(dirVec.normalized * 5f, ForceMode2D.Impulse);

        // Pattern Counting
        _curPatternCount++;

        if (_curPatternCount < _maxPatternCount[_patternIndex])
        {
            Invoke("FireArc", 0.15f);
        }
        else
        {
            Invoke("Think", 3f);
        }
    }

    private void FireAround()
    {
        if (_curHealth <= 0)
        {
            return;
        }

        Debug.Log("원 형태로 전체 공격.");

        // Fire Around
        int roundNumA = 50;
        int roundNumB = 40;
        int roundNum = _curPatternCount % 2 == 0 ? roundNumA : roundNumB;

        for (int index = 0; index < roundNum; index++)
        {
            GameObject bullet = _objectManager.MakeObj("BulletBossB");
            bullet.transform.position = transform.position;
            
            Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();

            Vector2 dirVec = new Vector2(Mathf.Cos(Mathf.PI * 2 * index / roundNum)
                , Mathf.Sin(Mathf.PI * 2 * index / roundNum));
            rigid.AddForce(dirVec.normalized * 2f, ForceMode2D.Impulse);

            Vector3 rotVec = Vector3.forward * 360 * index / roundNum + Vector3.forward * 90;
            bullet.transform.Rotate(rotVec);
        }

        _curPatternCount++;

        if (_curPatternCount < _maxPatternCount[_patternIndex])
        {
            Invoke("FireAround", 0.7f);
        }
        else
        {
            Invoke("Think", 3f);
        }
    }

    private void Update()
    {
        if (_enemyName == "B")
        {
            return;
        }

        Fire();
        Reload();
    }

    private void Fire()
    {
        if (_curShotDelay < _maxShotDelay)
        {
            return;
        }

        if (_enemyName == "S")
        {
            GameObject bullet = _objectManager.MakeObj(_bulletObjectA);
            bullet.transform.position = transform.position;

            Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();

            Vector3 dirVec = _player.transform.position - transform.position;
            rigid.AddForce(dirVec.normalized * 3f, ForceMode2D.Impulse);
        }
        else if (_enemyName == "L")
        {
            GameObject bulletR = _objectManager.MakeObj(_bulletObjectB);
            bulletR.transform.position = transform.position + Vector3.right * 0.3f;

            GameObject bulletL = _objectManager.MakeObj(_bulletObjectB);
            bulletL.transform.position = transform.position + Vector3.left * 0.3f;

            Rigidbody2D rigidR = bulletR.GetComponent<Rigidbody2D>();
            Rigidbody2D rigidL = bulletL.GetComponent<Rigidbody2D>();

            Vector3 dirVecR = _player.transform.position - (transform.position + Vector3.right * 0.3f);
            Vector3 dirVecL = _player.transform.position - (transform.position + Vector3.left * 0.3f);

            rigidR.AddForce(dirVecR.normalized * 4f, ForceMode2D.Impulse);
            rigidL.AddForce(dirVecL.normalized * 4f, ForceMode2D.Impulse);
        }

        _curShotDelay = 0f;
    }

    private void Reload()
    {
        _curShotDelay += Time.deltaTime;
    }

    public void OnHit(int damage)
    {
        if (_curHealth <= 0)
        {
            return;
        }

        _curHealth -= damage;

        if (_enemyName == "B")
        {
            _animator.SetTrigger("OnHit");
        }
        else
        {
            _spriteRenderer.sprite = _sprites[1];
            Invoke("ReturnSprite", _delayReturnSprite);
        }

        if (_curHealth <= 0)
        {
            Player playerLogic = _player.GetComponent<Player>();
            playerLogic.AddScore(_enemyScore);

            // Random Ratio Item Drop
            int random = _enemyName == "B" ? 0 : Random.Range(0, 10);
            // Not Item, 50%
            if (random < 5)
            {
                Debug.Log("Not Item");
            }
            else
            {
                string itemName = "";

                // Coin, 30%
                if (random < 8)
                {
                    itemName = _itemCoin;
                }
                // Power, 10%
                else if (random < 9)
                {
                    itemName = _itemPower;
                }
                // Boom, 10%
                else if (random < 10)
                {
                    itemName = _itemBoom;
                }

                if (itemName != "")
                {
                    GameObject obj = _objectManager.MakeObj(itemName);
                    obj.transform.position = transform.position;
                }
            }

            _gameManager.CallExplosion(transform.position, _enemyName);

            transform.rotation = Quaternion.identity;

            // Boss Kill
            if (_enemyName == "B")
            {
                _objectManager.DeleteAllEnemyBullets();
                CancelInvoke();
                _gameManager.StageEnd();
            }

            gameObject.SetActive(false);
        }
    }

    void ReturnSprite()
    {
        _spriteRenderer.sprite = _sprites[0];
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "BorderBullet" && _enemyName != "B")
        {
            gameObject.SetActive(false);
            transform.rotation = Quaternion.identity;
        }
        else if (collision.tag == "PlayerBullet")
        {
            Bullet bullet = collision.GetComponent<Bullet>();
            OnHit(bullet.Damage);

            collision.gameObject.SetActive(false);
        }
    }
}
