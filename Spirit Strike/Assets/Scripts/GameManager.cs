using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    int[] _spawnCounts = {1, 1, 1, 1};

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

    int _stageCount = 1;

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

    public void AddSlayCount()
    {
        _slayCount++;

        if(_slayCount >= _spawnCounts[_stageCount - 1] * _spawnPoints.Length)
        {
            if(_stageCount >= _spawnCounts.Length)
            {
                return;
            }

            GoToNextStage();
        }
    }

    void GoToNextStage()
    {
        _stageCount++;
        Debug.Log($"{_stageCount} 스테이지 입장");

        foreach (var point in _spawnPoints)
        {
            point.CallSpawn();
        }

        _slayCount = 0;
    }
}
