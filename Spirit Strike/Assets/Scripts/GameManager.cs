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
        foreach(var point in _spawnPoints)
        {
            point.CallSpawn();
        }
    }

    public void AddSlayCount()
    {
        // �������Ͱ� �ƴ� ���� óġ ��
        if(_isBossMonster == false)
        {
            _slayCount++;

            if (_slayCount >= _spawnCounts[_stageCount - 1] * _spawnPoints.Length)
            {
                if (_stageCount >= _spawnCounts.Length)
                {
                    return;
                }

                // ToDo : �������� �������� ���� ���� ��ȯ�ϴ� ���� �߰��ϱ�
                _spawnPoints[0].SpawnBossMonster(_stageCount);
            }
        }
        else
        {
            GoToNextStage();
        }
    }

    void GoToNextStage()
    {
        _stageCount++;
        Debug.Log($"{_stageCount} �������� ����");

        foreach (var point in _spawnPoints)
        {
            point.CallSpawn();
        }

        _slayCount = 0;
    }
}
