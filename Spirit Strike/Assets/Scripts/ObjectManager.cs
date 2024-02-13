using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : Singleton<ObjectManager>
{
    public List<GameObject> _monsterList;

    public override void Awake()
    {
        base.Awake();
        _monsterList = new List<GameObject>();
    }
}
