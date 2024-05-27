using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    [SerializeField] GameObject _enemy;
    [SerializeField] ObjectManager _objManager;

    void Start()
    {
        StartCoroutine(Spawn());
    }

    IEnumerator Spawn()
    {
        int i = 1;
        GameObject obj;
        while (i <= 5)
        {
            obj = Instantiate(_enemy, transform);
            _objManager.MonsterList.Add(obj);
            _objManager.FillEnemyList(obj);
            yield return new WaitForSeconds(3.0f);
            i++;
        }


        yield return null;
    }
}
