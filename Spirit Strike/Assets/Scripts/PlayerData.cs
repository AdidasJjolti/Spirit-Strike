using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;

[System.Serializable]
public class PlayerData
{
    int _level;

    public int Level
    {
        get
        {
            return _level;
        }
        set
        {
            _level = value;
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

    JsonData _data;

    public PlayerData()
    {
        Level = 1;
        Hp = 0;
        Attack = 0;
        Defence = 0;
        Dodge = 0;
        Critical = 0;
        AtkSpeed = 0;

        LoadPlayerDataFromJson();
        Debug.Log("플레이어 데이터 생성 완료");
    }

    void LoadPlayerDataFromJson(int level = 1)
    {
        string JsonString = File.ReadAllText(Application.dataPath + "/Resources/PlayerData.json");
        JsonData jsonData = JsonMapper.ToObject(JsonString);
        _data = jsonData;
        _hp = (int)_data[level - 1]["hp"];
        _attack = (int)_data[level - 1]["attack"];
        _defence = (int)_data[level - 1]["defence"];
        _dodge = (int)_data[level - 1]["dodge"];
        _critical = (int)_data[level - 1]["critical"];
        _atkSpeed = (int)_data[level - 1]["atk_speed"];

        UnityEngine.Debug.Log($"공격력은 {_attack}이야.");
    }

    // 레벨업할 때 PlayerDataManager에서 레벨에 맞는 능력치를 다시 부르도록 수정
    public void LevelUp(int level)
    {
        LoadPlayerDataFromJson(level);
    }
}
