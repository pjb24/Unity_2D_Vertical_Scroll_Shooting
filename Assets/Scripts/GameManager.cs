using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;

public class GameManager : MonoBehaviour
{
    private int _stage = 1;
    private int _maxStageCount = 0;

    [SerializeField] private Animator _stageAnimator;
    [SerializeField] private Animator _clearAnimator;
    [SerializeField] private Animator _fadeAnimator;

    [SerializeField] private ObjectManager _objectManager;

    [SerializeField] private GameObject _player;

    [SerializeField] private string[] _enemyNames;
    [SerializeField] private Transform[] _spawnPoints;

    [SerializeField] private Text _scoreText;
    [SerializeField] private Image[] _lifeImage;
    [SerializeField] private GameObject _gameOverSet;
    [SerializeField] private Image[] _boomImage;

    private float _nextSpawnDelay = 0f;
    private float _curSpawnDelay = 0f;

    private Vector3 _playerSpawnPosition;

    private List<Spawn> _spawnList;
    private int _spawnIndex = 0;
    private bool _spawnEnd = false;

    private void Awake()
    {
        ReadStageFileCount();

        _playerSpawnPosition = _player.transform.position;
        _gameOverSet.SetActive(false);

        _spawnList = new List<Spawn>();

        StageStart();
    }

    public void StageStart()
    {
        // Stage UI Load
        _stageAnimator.SetTrigger("On");
        _stageAnimator.GetComponent<Text>().text = "Stage " + _stage + "\nStart";
        _clearAnimator.GetComponent<Text>().text = "Stage " + _stage + "\nClear!";

        // Enemy Spawn File Read
        ReadSpawnFile();

        // Fade In
        _fadeAnimator.SetTrigger("In");
    }

    public void StageEnd()
    {
        // Clear UI Load
        _clearAnimator.SetTrigger("On");

        // Fade Out
        _fadeAnimator.SetTrigger("Out");

        // Player Reposition
        _player.transform.position = _playerSpawnPosition;

        // Stage Increment
        _stage++;
        if (_stage > _maxStageCount)
        {
            Invoke("GameOver", 6f);
        }
        else
        {
            Invoke("StageStart", 5f);
        }
    }

    private void ReadStageFileCount()
    {
        TextAsset[] files = Resources.LoadAll<TextAsset>("");

        int count = 0;
        foreach (var file in files)
        {
            if (file.name.StartsWith("Stage"))
                count++;
        }

        _maxStageCount = count;
    }

    private void ReadSpawnFile()
    {
        // 변수 초기화.
        _spawnList.Clear();
        _spawnIndex = 0;
        _spawnEnd = false;

        // 스폰 파일 읽기.
        TextAsset textFile = Resources.Load("Stage " + _stage) as TextAsset;
        StringReader stringReader = new StringReader(textFile.text);

        while(stringReader != null)
        {
            string line = stringReader.ReadLine();
            Debug.Log(line);

            if (line == null)
            {
                break;
            }

            // 스폰 데이터 생성.
            Spawn spawnData = new Spawn();
            string[] splitedStrings = line.Split(',');
            spawnData._delay = float.Parse(splitedStrings[0]);
            spawnData._type = splitedStrings[1];
            spawnData._point = int.Parse(splitedStrings[2]);

            _spawnList.Add(spawnData);
        }

        // 텍스트 파일 닫기.
        stringReader.Close();

        // 첫번째 스폰 딜레이 적용.
        _nextSpawnDelay = _spawnList[0]._delay;
    }

    private void Update()
    {
        _curSpawnDelay += Time.deltaTime;

        if (_curSpawnDelay > _nextSpawnDelay && !_spawnEnd)
        {
            SpawnEnemy();
            _curSpawnDelay = 0f;
        }

        Player playerLogic = _player.GetComponent<Player>();
        _scoreText.text = playerLogic.Score.ToString("N0");
    }

    private void SpawnEnemy()
    {
        int enemyIndex = 0;
        switch (_spawnList[_spawnIndex]._type)
        {
            case "S":
                {
                    enemyIndex = 0;
                    break;
                }
            case "M":
                {
                    enemyIndex = 1;
                    break;
                }
            case "L":
                {
                    enemyIndex = 2;
                    break;
                }
            case "B":
                {
                    enemyIndex = 3;
                    break;
                }
            default:
                {
                    Debug.LogError("Requested Non Setted Parameter: " + _spawnList[_spawnIndex]._type);
                    break;
                }
        }

        int enemyPoint = _spawnList[_spawnIndex]._point;

        GameObject enemy = _objectManager.MakeObj(_enemyNames[enemyIndex]);
        enemy.transform.position = _spawnPoints[enemyPoint].position;

        Rigidbody2D rigid = enemy.GetComponent<Rigidbody2D>();
        Enemy enemyLogic = enemy.GetComponent<Enemy>();
        enemyLogic.Player = _player;
        enemyLogic.SetObjectManager(_objectManager);
        enemyLogic.SetGameManager(this);

        // Right Spawn
        if (enemyPoint == 5 || enemyPoint == 6)
        {
            enemy.transform.Rotate(Vector3.back * 90);
            rigid.linearVelocity = new Vector2(enemyLogic.Speed * (-1), -1);
        }
        // Left Spawn
        else if (enemyPoint == 7 || enemyPoint == 8)
        {
            enemy.transform.Rotate(Vector3.forward * 90);
            rigid.linearVelocity = new Vector2(enemyLogic.Speed * 1, -1);
        }
        // Forward Spawn
        else
        {
            rigid.linearVelocity = new Vector2(0, enemyLogic.Speed * (-1));
        }

        // 스폰 인덱스 증가.
        _spawnIndex++;
        if (_spawnIndex >= _spawnList.Count)
        {
            _spawnEnd = true;
            return;
        }

        // 다음 스폰 딜레이 갱신.
        _nextSpawnDelay = _spawnList[_spawnIndex]._delay;
    }

    public void RespawnPlayer()
    {
        Player playerLogic = _player.GetComponent<Player>();
        Invoke("RespawnPlayerExe", playerLogic.RespawnDelay);
    }

    private void RespawnPlayerExe()
    {
        _player.transform.position = _playerSpawnPosition;
        _player.SetActive(true);

        Player playerLogic = _player.GetComponent<Player>();
        playerLogic.IsHit = false;
    }

    public void UpdateLifeIcon(int life)
    {
        // UI Life Init Disable
        for (int index = 0; index < _lifeImage.Length; index++)
        {
            _lifeImage[index].color = new Color(1, 1, 1, 0);
        }

        // UI Life Active
        for (int index = 0; index < life; index++)
        {
            _lifeImage[index].color = new Color(1, 1, 1, 1);
        }
    }

    public void UpdateBoomIcon(int boom)
    {
        // UI Boom Init Disable
        for (int index = 0; index < _boomImage.Length; index++)
        {
            _boomImage[index].color = new Color(1, 1, 1, 0);
        }

        // UI Boom Active
        for (int index = 0; index < boom; index++)
        {
            _boomImage[index].color = new Color(1, 1, 1, 1);
        }
    }

    public void CallExplosion(Vector3 pos, string type)
    {
        GameObject explosion = _objectManager.MakeObj("Explosion");
        Explosion explosionLogic = explosion.GetComponent<Explosion>();

        explosion.transform.position = pos;
        explosionLogic.StartExplosion(type);
    }

    public void GameOver()
    {
        _gameOverSet.SetActive(true);
    }

    public void GameRetry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
