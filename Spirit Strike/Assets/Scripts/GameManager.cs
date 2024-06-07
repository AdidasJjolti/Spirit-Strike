using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    int[] _spawnCounts = {5, 10, 15, 20};

    public int[] SpawnCounts
    {
        get
        {
            return _spawnCounts;
        }
    }

    int _slayCount;

    public int SlayCount
    {
        get
        {
            return _slayCount;
        }
    }

    int _stageCount;

    public int StageCount
    {
        get
        {
            return _stageCount;
        }
    }

    SpawnEnemy[] _spawnPoints;

    public override void Awake()
    {
        base.Awake();       // Singleton Ŭ������ ����Ƽ ������ ���� �θ��� �κ��� �����Ƿ� base.Awake()�� ȣ���Ͽ� �θ� Ŭ������ �����ϵ��� ����
        _spawnPoints = FindObjectsOfType<SpawnEnemy>();
    }

    void Start()
    {
        foreach(var point in _spawnPoints)
        {
            point.CallSpawn();
        }
    }
}
