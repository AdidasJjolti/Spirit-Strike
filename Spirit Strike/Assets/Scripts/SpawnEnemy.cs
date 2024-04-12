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
        while (i <= 10)
        {
            _objManager._monsterList.Add(Instantiate(_enemy, transform));
            i++;
        }
    }
}
