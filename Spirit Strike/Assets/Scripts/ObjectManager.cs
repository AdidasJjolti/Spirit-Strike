using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : Singleton<ObjectManager>
{
    [SerializeField] List<GameObject> _monsterList;

    public List<GameObject> MonsterList
    {
        get
        {
            return _monsterList;
        }
    }

    [SerializeField] List<Enemy> _enemyList;

    public List<Enemy> EnemyList
    {
        get
        {
            return _enemyList;
        }
    }

    GameManager _gameManager;

    public override void Awake()
    {
        base.Awake();       // Singleton 클래스는 유니티 씬에서 따로 부르는 부분이 없으므로 base.Awake()를 호출하여 부모 클래스를 실행하도록 유도
        _monsterList = new List<GameObject>();
        _enemyList = new List<Enemy>();
        _gameManager = FindObjectOfType<GameManager>();
    }

    public void FillEnemyList(GameObject obj)
    {
        Enemy enemy = obj.GetComponent<Enemy>();

        // 여러개의 스폰 포인트에서 몬스터가 생성되는 경우 foreach문에서 _monsterList를 돌 대마다 중복 계산할 수 있으므로 중복된 enemy가 들어간 경우 스킵
        if (enemy != null && !_enemyList.Contains(enemy))
        {
            _enemyList.Add(enemy);
        }
    }
}
