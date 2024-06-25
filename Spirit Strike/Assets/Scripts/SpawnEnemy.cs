using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    Dictionary<eMonster, string> _monsterDic = new Dictionary<eMonster, string>();        // 몬스터 타입과 몬스터 프리팹 경로를 저장할 딕셔너리

    //[SerializeField] GameObject _enemy;
    [SerializeField] ObjectManager _objManager;
    GameManager _gameManager;

    void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();

        _monsterDic.Add(eMonster.GHOST, "Prefabs/Monsters/GHOST");
        _monsterDic.Add(eMonster.SLIME, "Prefabs/Monsters/SLIME");
    }

    public void CallSpawn(eMonster type)
    {
        StartCoroutine(Spawn(type));
    }

    IEnumerator Spawn(eMonster type)
    {
        if(_gameManager == null)
        {
            _gameManager = FindObjectOfType<GameManager>();
        }

        int i = 1;
        GameObject obj = LoadPrefab(type);
        while (i <= _gameManager.SpawnCount)
        {
            GameObject enemy = Instantiate(obj, transform);
            _objManager.MonsterList.Add(enemy);
            _objManager.FillEnemyList(enemy);
            yield return new WaitForSeconds(3.0f);
            i++;
        }

        yield return null;
    }

    public GameObject LoadPrefab(eMonster type)
    {
        GameObject prefab = Resources.Load<GameObject>(_monsterDic[type]);           // 몬스터 프리팹 생성
        return prefab;
    }

    public void SpawnBossMonster(int stageCount)
    {
        Debug.Log("보스몬스터 소환");
    }
}
