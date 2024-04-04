using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eSkill
{
    NONE = 0,
    FIREBALL,
    FIRESLASH,

    MAX
}

// ToDo : 미리 사용할 스킬 클래스를 생성하여 스킬 관리, 플레이어에게 정보를 넘기는 구조로 만들기
public class SkillManager
{

    //[SerializeField] List<Skill> _skillList = new List<Skill>();
    Dictionary<eSkill, string> _skillDic = new Dictionary<eSkill, string>();

    public SkillManager()
    {
        _skillDic.Add(eSkill.FIREBALL, "Prefabs/Fireball");
        _skillDic.Add(eSkill.FIRESLASH, "Prefabs/FireSlash");
    }

    public GameObject LoadPrefab(eSkill type)
    {
        GameObject prefab = Resources.Load<GameObject>(_skillDic[type]);
        float coolDown = prefab.GetComponent<Skill>().GetStat()._coolDown;
        return prefab;
    }
}
