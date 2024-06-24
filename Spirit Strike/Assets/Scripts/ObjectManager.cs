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
        base.Awake();       // Singleton Ŭ������ ����Ƽ ������ ���� �θ��� �κ��� �����Ƿ� base.Awake()�� ȣ���Ͽ� �θ� Ŭ������ �����ϵ��� ����
        _monsterList = new List<GameObject>();
        _enemyList = new List<Enemy>();
        _gameManager = FindObjectOfType<GameManager>();
    }

    public void FillEnemyList(GameObject obj)
    {
        Enemy enemy = obj.GetComponent<Enemy>();

        // �������� ���� ����Ʈ���� ���Ͱ� �����Ǵ� ��� foreach������ _monsterList�� �� �븶�� �ߺ� ����� �� �����Ƿ� �ߺ��� enemy�� �� ��� ��ŵ
        if (enemy != null && !_enemyList.Contains(enemy))
        {
            _enemyList.Add(enemy);
        }
    }
}
