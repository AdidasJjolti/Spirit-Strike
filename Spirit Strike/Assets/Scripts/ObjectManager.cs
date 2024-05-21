using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : Singleton<ObjectManager>
{
    public List<GameObject> _monsterList;
    List<Enemy> _enemyList;

    public List<Enemy> EnemyList
    {
        get
        {
            return _enemyList;
        }
    }

    public override void Awake()
    {
        base.Awake();       // Singleton 클래스는 유니티 씬에서 따로 부르는 부분이 없으므로 base.Awake()를 호출하여 부모 클래스를 실행하도록 유도
        _monsterList = new List<GameObject>();
        _enemyList = new List<Enemy>();
    }

    public void GetEnemy()
    {
        foreach(var monster in _monsterList)
        {
            Enemy enemy = monster.GetComponent<Enemy>();

            if (enemy != null)
            {
                _enemyList.Add(enemy);
            }
        }
    }
}
