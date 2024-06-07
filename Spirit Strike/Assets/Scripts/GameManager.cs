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
        base.Awake();       // Singleton 클래스는 유니티 씬에서 따로 부르는 부분이 없으므로 base.Awake()를 호출하여 부모 클래스를 실행하도록 유도
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
