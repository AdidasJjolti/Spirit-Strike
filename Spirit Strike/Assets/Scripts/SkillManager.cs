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
        // 각 스킬 오브젝트를 생성
        // 1. 스킬 프리팹(게임오브젝트)는 누가 들고 있어야 하나?    스크립터블 오브젝트로 관리하기 (후보)
        // 2. 만약 플레이어가 들고 있다면 스킬 매니저가 하는 일은 그냥 어떤 스킬 쓸 수 있는지 전달하는 역할밖에 없는거 같고...
        // 3. 만약에 스킬 매니저가 들고 있다면 SkillManager 클래스 생성 시에 각 스킬 프리팹도 미리 생성해야 하는데, 이걸 어떻게 할 수 있을까?

        _skillDic.Add(eSkill.FIREBALL, "Prefabs/Fireball");
        _skillDic.Add(eSkill.FIRESLASH, "Prefabs/FireSlash");
    }

    public GameObject LoadPrefab(eSkill type)
    {
        GameObject prefab = Resources.Load<GameObject>(_skillDic[type]);
        return prefab;
    }
}
