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
        Debug.Log("경험치 클래스 생성 완료!");
    }

    // PlayerLevel을 통해서 레벨업 관련 데이터를 모두 처리하는 방식으로 코드 수정
    public void GetExp(int exp)
    {
        _curExp += exp;
        _accExp += exp;

        // 누적 경험치가 현재 레벨에서 필요한 누적 경험치를 초과하면 레벨업
        while(_curExp >= _exp)
        {
            _level++;
            _exp = (int)_data[_level - 1]["Exp"];
            _prevExp = (int)_data[_level - 2]["AccExp"];
            _curExp = _accExp - _prevExp;

            Debug.Log($"현재 레벨은 {_level}이고 레벨업 필요 경험치는 {_exp}야.");
        }

        Debug.Log($"현재 경험치는 {_curExp}야.");
    }

    void LoadPlayerExperienceDataFromJson()
    {
        string JsonString = File.ReadAllText(Application.dataPath + "/Resources/PlayerExperienceData.json");
        JsonData jsonData = JsonMapper.ToObject(JsonString);
        _data = jsonData;
        _exp = (int)_data[_level - 1]["Exp"];
        Debug.Log($"현재 레벨은 {_level}이고 현재 경험치는 {_curExp}이고 필요 경험치는 {_exp}야.");
    }
}
