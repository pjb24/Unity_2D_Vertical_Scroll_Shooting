using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] private ObjectManager _objectManager;

    [SerializeField] private int _life = 3;
    [SerializeField] private int _score = 0;
    public int Score => _score;
    [SerializeField] private float _speed = 5f;
    [SerializeField] private int _maxPower = 6;
    [SerializeField] private int _curPower = 1;
    [SerializeField] private int _maxBoom = 3;
    [SerializeField] private int _curBoom = 1;
    [SerializeField] private GameObject _boomEffect;
    [SerializeField] private float _maxShotDelay = 0.2f;
    [SerializeField] private GameManager _manager;
    [SerializeField] private float _respawnDelay = 2f;
    public float RespawnDelay => _respawnDelay;
    [SerializeField] private int _boomDamage = 1_000;
    [SerializeField] private float _boomEffectOffDelay = 4f;

    [SerializeField] private GameObject[] _followers;

    private string _bulletObjectA;
    private string _bulletObjectB;

    private float _curShotDelay;

    private Animator _animator;
    private SpriteRenderer _spriteRenderer;

    private bool _isTouchTop = false;
    private bool _isTouchBottom = false;
    private bool _isTouchLeft = false;
    private bool _isTouchRight = false;

    private bool _isHit = false;
    public bool IsHit { set { _isHit = value; } }

    private bool _isBoomTime = false;

    private bool _isRespawnTime = false;

    private bool[] _joyControl;
    private bool _isControl = false;

    private bool _isButtonA = false;
    private bool _isButtonB = false;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();

        _boomEffect.SetActive(false);

        _manager.UpdateBoomIcon(_curBoom);

        _bulletObjectA = "BulletPlayerA";
        _bulletObjectB = "BulletPlayerB";

        foreach (GameObject follower in _followers)
        {
            follower.SetActive(false);
        }

        _joyControl = new bool[9];
    }

    private void OnEnable()
    {
        Unbeatable();

        Invoke("Unbeatable", 3f);
    }

    private void Unbeatable()
    {
        _isRespawnTime = !_isRespawnTime;

        if (_isRespawnTime)
        {
            // 무적 시간 이펙트, 투명화.
            _spriteRenderer.color = new Color(1, 1, 1, 0.5f);

            foreach (GameObject follower in _followers)
            {
                follower.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.5f);
            }
        }
        else
        {
            // 무적 시간 종료, 투명화 해제.
            _spriteRenderer.color = new Color(1, 1, 1, 1);

            foreach (GameObject follower in _followers)
            {
                follower.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
            }
        }
    }

    private void Update()
    {
        Move();
        Fire();
        Boom();
        Reload();
    }

    public void JoyPanel(int type)
    {
        for (int index = 0; index < 9; index++)
        {
            _joyControl[index] = index == type;
        }
    }

    public void JoyDown()
    {
        _isControl = true;
    }

    public void JoyUp()
    {
        _isControl = false;
    }

    private void Move()
    {
        // Keyboard Control Value
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        bool isKeyInput = false;
        if (h != 0 || v != 0)
        {
            isKeyInput = true;
        }

        if (!isKeyInput)
        {
            // Joy Control Value
            if (_joyControl[0]) { h = -1; v = 1; }
            if (_joyControl[1]) { h = 0; v = 1; }
            if (_joyControl[2]) { h = 1; v = 1; }
            if (_joyControl[3]) { h = -1; v = 0; }
            if (_joyControl[4]) { h = 0; v = 0; }
            if (_joyControl[5]) { h = 1; v = 0; }
            if (_joyControl[6]) { h = -1; v = -1; }
            if (_joyControl[7]) { h = 0; v = -1; }
            if (_joyControl[8]) { h = 1; v = -1; }

            if (!_isControl)
            {
                v = 0;
                h = 0;
            }
        }

        if (_isTouchTop && v == 1 || _isTouchBottom && v == -1)
        {
            v = 0;
        }
        if (_isTouchLeft && h == -1 || _isTouchRight && h == 1)
        {
            h = 0;
        }

        Vector3 curPos = transform.position;
        Vector3 nextPos = new Vector3(h, v, 0) * _speed * Time.deltaTime;

        transform.position = curPos + nextPos;

        // Animation
        if (Input.GetButtonDown("Horizontal")
            || Input.GetButtonUp("Horizontal"))
        {
            _animator.SetInteger("Input", (int)h);
        }
    }

    public void ButtonADown()
    {
        _isButtonA = true;
    }

    public void ButtonAUp()
    {
        _isButtonA = false;
    }

    public void ButtonBDown()
    {
        _isButtonB = true;
    }

    public void OnAttack2(InputValue value)
    {
        if (value.isPressed)
        {
            _isButtonA = true;
        }
        else
        {
            _isButtonA = false;
        }
    }

    private void Fire()
    {
        if (!_isButtonA)
        {
            return;
        }

        if (_curShotDelay < _maxShotDelay)
        {
            return;
        }

        if (_curPower < 1)
        {
            _curPower = 1;
        }

        switch (_curPower)
        {
            case 1:
                {
                    // Power One
                    GameObject bullet = _objectManager.MakeObj(_bulletObjectA);
                    bullet.transform.position = transform.position;

                    Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
                    rigid.AddForce(Vector2.up * 10f, ForceMode2D.Impulse);

                    break;
                }
            case 2:
                {
                    GameObject bulletR = _objectManager.MakeObj(_bulletObjectA);
                    bulletR.transform.position = transform.position + Vector3.right * 0.1f;
                    GameObject bulletL = _objectManager.MakeObj(_bulletObjectA);
                    bulletL.transform.position = transform.position + Vector3.left * 0.1f;

                    Rigidbody2D rigidR = bulletR.GetComponent<Rigidbody2D>();
                    Rigidbody2D rigidL = bulletL.GetComponent<Rigidbody2D>();

                    rigidR.AddForce(Vector2.up * 10f, ForceMode2D.Impulse);
                    rigidL.AddForce(Vector2.up * 10f, ForceMode2D.Impulse);

                    break;
                }
            case 3:
            default:
                {
                    GameObject bulletRR = _objectManager.MakeObj(_bulletObjectA);
                    bulletRR.transform.position = transform.position + Vector3.right * 0.35f;
                    GameObject bulletCC = _objectManager.MakeObj(_bulletObjectB);
                    bulletCC.transform.position = transform.position;
                    GameObject bulletLL = _objectManager.MakeObj(_bulletObjectA);
                    bulletLL.transform.position = transform.position + Vector3.left * 0.35f;

                    Rigidbody2D rigidRR = bulletRR.GetComponent<Rigidbody2D>();
                    Rigidbody2D rigidCC = bulletCC.GetComponent<Rigidbody2D>();
                    Rigidbody2D rigidLL = bulletLL.GetComponent<Rigidbody2D>();

                    rigidRR.AddForce(Vector2.up * 10f, ForceMode2D.Impulse);
                    rigidCC.AddForce(Vector2.up * 10f, ForceMode2D.Impulse);
                    rigidLL.AddForce(Vector2.up * 10f, ForceMode2D.Impulse);

                    break;
                }
        }

        _curShotDelay = 0f;
    }

    public void OnBoom(InputValue value)
    {
        _isButtonB = true;
    }

    private void Boom()
    {
        if (!_isButtonB)
        {
            return;
        }

        _isButtonB = false;

        if (_isBoomTime)
        {
            return;
        }

        if (_curBoom <= 0)
        {
            return;
        }

        _curBoom--;
        _manager.UpdateBoomIcon(_curBoom);
        _isBoomTime = true;

        // Effect visible
        _boomEffect.SetActive(true);
        Invoke("OffBoomEffect", _boomEffectOffDelay);

        // Remove Enemy
        GameObject[] enemiesL = _objectManager.GetPool("EnemyL");
        GameObject[] enemiesM = _objectManager.GetPool("EnemyM");
        GameObject[] enemiesS = _objectManager.GetPool("EnemyS");
        GameObject[] enemiesB = _objectManager.GetPool("EnemyB");

        for (int index = 0; index < enemiesL.Length; index++)
        {
            if (enemiesL[index].activeSelf)
            {
                Enemy enemyLogic = enemiesL[index].GetComponent<Enemy>();
                enemyLogic.OnHit(_boomDamage);
            }
        }
        for (int index = 0; index < enemiesM.Length; index++)
        {
            if (enemiesM[index].activeSelf)
            {
                Enemy enemyLogic = enemiesM[index].GetComponent<Enemy>();
                enemyLogic.OnHit(_boomDamage);
            }
        }
        for (int index = 0; index < enemiesS.Length; index++)
        {
            if (enemiesS[index].activeSelf)
            {
                Enemy enemyLogic = enemiesS[index].GetComponent<Enemy>();
                enemyLogic.OnHit(_boomDamage);
            }
        }
        for (int index = 0; index < enemiesB.Length; index++)
        {
            if (enemiesB[index].activeSelf)
            {
                Enemy enemyLogic = enemiesB[index].GetComponent<Enemy>();
                enemyLogic.OnHit(_boomDamage);
            }
        }

        // Remove Enemy Bullet
        GameObject[] enemyBulletsA = _objectManager.GetPool("BulletEnemyA");
        GameObject[] enemyBulletsB = _objectManager.GetPool("BulletEnemyB");
        GameObject[] bossBulletsA = _objectManager.GetPool("BulletBossA");
        GameObject[] bossBulletsB = _objectManager.GetPool("BulletBossB");
        for (int index = 0; index < enemyBulletsA.Length; index++)
        {
            if (enemyBulletsA[index].activeSelf)
            {
                enemyBulletsA[index].SetActive(false);
            }
        }
        for (int index = 0; index < enemyBulletsB.Length; index++)
        {
            if (enemyBulletsB[index].activeSelf)
            {
                enemyBulletsB[index].SetActive(false);
            }
        }
        for (int index = 0; index < bossBulletsA.Length; index++)
        {
            if (bossBulletsA[index].activeSelf)
            {
                bossBulletsA[index].SetActive(false);
            }
        }
        for (int index = 0; index < bossBulletsB.Length; index++)
        {
            if (bossBulletsB[index].activeSelf)
            {
                bossBulletsB[index].SetActive(false);
            }
        }
    }

    private void Reload()
    {
        _curShotDelay += Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Border")
        {
            switch (collision.name)
            {
                case "Top":
                    {
                        _isTouchTop = true;
                        break;
                    }
                case "Bottom":
                    {
                        _isTouchBottom = true;
                        break;
                    }
                case "Left":
                    {
                        _isTouchLeft = true;
                        break;
                    }
                case "Right":
                    {
                        _isTouchRight = true;
                        break;
                    }
                default:
                    {
                        Debug.LogError("Requested Non Setted Parameter: " + collision.name);

                        break;
                    }
            }
        }
        else if (collision.tag == "Enemy" || collision.tag == "EnemyBullet")
        {
            if (_isRespawnTime)
            {
                return;
            }

            if (_isHit)
            {
                return;
            }

            _isHit = true;
            _life--;
            _manager.UpdateLifeIcon(_life);
            _manager.CallExplosion(transform.position, "P");

            if (_life <= 0)
            {
                _manager.GameOver();
            }
            else
            {
                _manager.RespawnPlayer();
            }

            gameObject.SetActive(false);
            collision.gameObject.SetActive(false);
        }
        else if (collision.tag == "Item")
        {
            Item item = collision.GetComponent<Item>();
            switch (item.Type)
            {
                case "Coin":
                    {
                        _score += 1_000;
                        break;
                    }
                case "Power":
                    {
                        if (_curPower >= _maxPower)
                        {
                            _score += 500;
                        }
                        else
                        {
                            _curPower++;
                            AddFollower();
                        }
                        break;
                    }
                case "Boom":
                    {
                        if (_curBoom >= _maxBoom)
                        {
                            _score += 500;
                        }
                        else
                        {
                            _curBoom++;
                            _manager.UpdateBoomIcon(_curBoom);
                        }
                        break;
                    }
                default:
                    {
                        Debug.LogError("Requested Non Setted Parameter: " + item.Type);

                        break;
                    }
            }

            collision.gameObject.SetActive(false);
        }
    }

    private void AddFollower()
    {
        if (_curPower == 4)
        {
            _followers[0].SetActive(true);
        }
        else if (_curPower == 5)
        {
            _followers[1].SetActive(true);
        }
        else if (_curPower == 6)
        {
            _followers[2].SetActive(true);
        }
    }

    private void OffBoomEffect()
    {
        _boomEffect.SetActive(false);
        _isBoomTime = false;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Border")
        {
            switch (collision.name)
            {
                case "Top":
                    {
                        _isTouchTop = false;
                        break;
                    }
                case "Bottom":
                    {
                        _isTouchBottom = false;
                        break;
                    }
                case "Left":
                    {
                        _isTouchLeft = false;
                        break;
                    }
                case "Right":
                    {
                        _isTouchRight = false;
                        break;
                    }
                default:
                    {
                        Debug.LogError("Requested Non Setted Parameter: " + collision.name);

                        break;
                    }
            }
        }
    }

    public void AddScore(int score)
    {
        _score += score;
    }
}
