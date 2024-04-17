using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;

public class PlayerDataManager
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

    JsonData _data;

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

    int _exp;

    public int Exp
    {
        get
        {
            return _exp;
        }
        set
        {
            _exp = value;
        }
    }

    int _accExp;

    public int AccExp
    {
        get
        {
            return _accExp;
        }
        set
        {
            _accExp = value;
        }
    }

    int _curExp;

    public int CurExp
    {
        get
        {
            return _curExp;
        }
        set
        {
            _curExp = value;
        }
    }

    int _prevExp;

    public int PrevExp
    {
        get
        {
            return _prevExp;
        }
        set
        {
            _prevExp = value;
        }
    }

    JsonData _expData;


    public PlayerDataManager()
    {
        Level = 1;
        LoadPlayerExperienceDataFromJson();
        LoadPlayerDataFromJson(Level);
    }

    void LoadPlayerDataFromJson(int level)
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

        _maxHP = _hp;

        UnityEngine.Debug.Log($"���ݷ��� {_attack}�̾�.");
    }

    // PlayerDataManager�� ���ؼ� ������ ���� �����͸� ��� ó���ϴ� ������� �ڵ� ����
    public void GetExp(int exp)
    {
        _curExp += exp;
        _accExp += exp;

        // ���� ����ġ�� ���� �������� �ʿ��� ���� ����ġ�� �ʰ��ϸ� ������
        while (_curExp >= _exp)
        {
            _level++;
            _exp = (int)_expData[_level - 1]["Exp"];
            _prevExp = (int)_expData[_level - 2]["AccExp"];
            _curExp = _accExp - _prevExp;

            Debug.Log($"���� ������ {_level}�̰� ������ �ʿ� ����ġ�� {_exp}��.");
        }

        Debug.Log($"���� ����ġ�� {_curExp}��.");

        // ������ �߻� �� �÷��̾� ���� ����
        LoadPlayerDataFromJson(Level);
    }

    void LoadPlayerExperienceDataFromJson()
    {
        string JsonString = File.ReadAllText(Application.dataPath + "/Resources/PlayerExperienceData.json");
        JsonData jsonData = JsonMapper.ToObject(JsonString);
        _expData = jsonData;
        _exp = (int)_expData[_level - 1]["Exp"];
        Debug.Log($"���� ������ {_level}�̰� ���� ����ġ�� {_curExp}�̰� �ʿ� ����ġ�� {_exp}��.");
    }
}
