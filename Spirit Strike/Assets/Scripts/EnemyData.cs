using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyData
{
    int _maxHP;

    public int MaxHP
    {
        get
        {
            return _maxHP;
        }

        set
        {
            _maxHP = value;
        }
    }

    int _hp;

    public int Hp
    {
        get
        {
            return _hp;
        }
        set
        {
            _hp = value;
        }
    }

    int _attack;

    public int Attack
    {
        get
        {
            return _attack;
        }
        set
        {
            _attack = value;
        }
    }
    int _defence;

    public int Defence
    {
        get
        {
            return _defence;
        }
        set
        {
            _defence = value;
        }
    }

    int _dodge;

    public int Dodge
    {
        get
        {
            return _dodge;
        }
        set
        {
            _dodge = value;
        }
    }

    int _critical;

    public int Critical
    {
        get
        {
            return _critical;
        }
        set
        {
            _critical = value;
        }
    }

    int _atkSpeed;

    public int AtkSpeed
    {
        get
        {
            return _atkSpeed;
        }
        set
        {
            _atkSpeed = value;
        }
    }
}
