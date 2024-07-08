using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;

public class GameManager : Singleton<GameManager>
{
    JsonData _data;

    int _spawnCount;

    public int SpawnCount
    {
        get
        {
            return _spawnCount;
        }
    }

    JsonData _bossData;

    int _bossSpawnCount;

    public int BossSpawnCount
    {
        get
        {
            return _bossSpawnCount;
        }
    }

    int _slayCount;

    int _type;

    public int Type
    {
        get
        {
            return _type;
        }
    }

    int _bossType;

    public int BossType
    {
        get
        {
            return _bossType;
        }
    }

    public int SlayCount
    {
        get
        {
            return _slayCount;
        }
    }

    int _stageCount = 1;

    public int StageCount
    {
        get
        {
            return _stageCount;
        }
    }

    SpawnEnemy[] _spawnPoints;

    bool _isBossMonster;

    public bool IsBossMonster
    {
        get
        {
            return _isBossMonster;
        }
    }

    public override void Awake()
    {
        base.Awake();       // Singleton 클래스는 유니티 씬에서 따로 부르는 부분이 없으므로 base.Awake()를 호출하여 부모 클래스를 실행하도록 유도
        _spawnPoints = FindObjectsOfType<SpawnEnemy>();
    }

    void Start()
    {
        LoadSpawnDataFromJson(_stageCount);
        foreach (var point in _spawnPoints)
        {
            point.CallSpawn((eMonster)_type);
        }
    }

    public void AddSlayCount()
    {
        // 보스몬스터가 아닌 몬스터 처치 시
        if(_isBossMonster == false)
        {
            _slayCount++;

            if (_slayCount >= _spawnCount * _spawnPoints.Length)
            {
                _isBossMonster = true;

                // ToDo : 스테이지 마지막에 보스 몬스터 소환하는 로직 추가하기
                LoadBossSpawnDataFromJson(_stageCount);
                _spawnPoints[0].SpawnBossMonster(_bossSpawnCount, (eMonster)_bossType);
            }
        }
        else if(_isBossMonster == true)
        {
            GoToNextStage();
        }
    }

    void GoToNextStage()
    {
        _stageCount++;
        Debug.Log($"{_stageCount} 스테이지 입장");
        LoadSpawnDataFromJson(_stageCount);

        foreach (var point in _spawnPoints)
        {
            point.CallSpawn((eMonster)_type);
        }

        _slayCount = 0;
    }

    void LoadSpawnDataFromJson(int stage)
    {
        string JsonString = File.ReadAllText(Application.dataPath + "/Resources/SpawnData.json");
        JsonData jsonData = JsonMapper.ToObject(JsonString);
        _data = jsonData;
        _spawnCount = (int)_data[stage - 1]["spawncount"];
        _type = (int)_data[stage - 1]["type"];
    }

    void LoadBossSpawnDataFromJson(int stage)
    {
        string JsonString = File.ReadAllText(Application.dataPath + "/Resources/BossSpawnData.json");
        JsonData jsonData = JsonMapper.ToObject(JsonString);
        _bossData = jsonData;
        _bossSpawnCount = (int)_bossData[stage - 1]["spawncount"];
        _bossType = (int)_bossData[stage - 1]["type"];
    }
}
