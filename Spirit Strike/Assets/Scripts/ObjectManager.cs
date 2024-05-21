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
        base.Awake();       // Singleton Ŭ������ ����Ƽ ������ ���� �θ��� �κ��� �����Ƿ� base.Awake()�� ȣ���Ͽ� �θ� Ŭ������ �����ϵ��� ����
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
