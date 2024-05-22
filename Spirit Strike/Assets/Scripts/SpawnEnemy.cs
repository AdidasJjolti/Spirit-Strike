using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    [SerializeField] GameObject _enemy;
    [SerializeField] ObjectManager _objManager;

    void Start()
    {
        int i = 1;
        while (i <= 5)
        {
            _objManager.MonsterList.Add(Instantiate(_enemy, transform));
            i++;
        }

        _objManager.FillEnemyList();
    }
}
