using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;

public class PlayerExperienceData
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

    JsonData _data;

    public PlayerExperienceData()
    {
        Level = 1;
        Exp = 0;
        AccExp = 0;
        CurExp = 0;
        PrevExp = 0;

        LoadPlayerExperienceDataFromJson();
        Debug.Log("����ġ Ŭ���� ���� �Ϸ�!");
    }

    // PlayerLevel�� ���ؼ� ������ ���� �����͸� ��� ó���ϴ� ������� �ڵ� ����
    public void GetExp(int exp)
    {
        _curExp += exp;
        _accExp += exp;

        // ���� ����ġ�� ���� �������� �ʿ��� ���� ����ġ�� �ʰ��ϸ� ������
        while(_curExp >= _exp)
        {
            _level++;
            _exp = (int)_data[_level - 1]["Exp"];
            _prevExp = (int)_data[_level - 2]["AccExp"];
            _curExp = _accExp - _prevExp;

            Debug.Log($"���� ������ {_level}�̰� ������ �ʿ� ����ġ�� {_exp}��.");
        }

        Debug.Log($"���� ����ġ�� {_curExp}��.");
    }

    void LoadPlayerExperienceDataFromJson()
    {
        string JsonString = File.ReadAllText(Application.dataPath + "/Resources/PlayerExperienceData.json");
        JsonData jsonData = JsonMapper.ToObject(JsonString);
        _data = jsonData;
        _exp = (int)_data[_level - 1]["Exp"];
        Debug.Log($"���� ������ {_level}�̰� ���� ����ġ�� {_curExp}�̰� �ʿ� ����ġ�� {_exp}��.");
    }
}
