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
        base.Awake();       // Singleton Ŭ������ ����Ƽ ������ ���� �θ��� �κ��� �����Ƿ� base.Awake()�� ȣ���Ͽ� �θ� Ŭ������ �����ϵ��� ����
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
        // �������Ͱ� �ƴ� ���� óġ ��
        if(_isBossMonster == false)
        {
            _slayCount++;

            if (_slayCount >= _spawnCount * _spawnPoints.Length)
            {
                _isBossMonster = true;

                // ToDo : �������� �������� ���� ���� ��ȯ�ϴ� ���� �߰��ϱ�
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
        Debug.Log($"{_stageCount} �������� ����");
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
