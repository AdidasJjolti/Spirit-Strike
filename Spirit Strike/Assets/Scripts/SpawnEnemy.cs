using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    [SerializeField] GameObject _enemy;
    [SerializeField] ObjectManager _objManager;
    GameManager _gameManager;

    void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();
    }

    public void CallSpawn()
    {
        StartCoroutine(Spawn());
    }

    IEnumerator Spawn()
    {
        if(_gameManager == null)
        {
            _gameManager = FindObjectOfType<GameManager>();
        }

        int i = 1;
        GameObject obj;
        while (i <= _gameManager.SpawnCounts[_gameManager.StageCount])
        {
            obj = Instantiate(_enemy, transform);
            _objManager.MonsterList.Add(obj);
            _objManager.FillEnemyList(obj);
            yield return new WaitForSeconds(3.0f);
            i++;
        }

        yield return null;
    }

    public void SpawnBossMonster(int stageCount)
    {
        Debug.Log("보스몬스터 소환");
    }
}
