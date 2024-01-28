using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] int _HP = 100;

    public int HP
    {
        get
        {
            return _HP;
        }
    }

    public void TakeDamage(int damage)
    {
        _HP -= damage;
        Debug.Log($"HP remaining : {_HP}");
    }
}
