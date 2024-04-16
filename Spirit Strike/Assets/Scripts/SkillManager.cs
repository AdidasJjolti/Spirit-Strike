using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eSkill
{
    NONE = 0,
    FIREBALL,
    FIRESLASH,
    POISON,
    HEAL,

    MAX
}

// ToDo : 미리 사용할 스킬 클래스를 생성하여 스킬 관리, 플레이어에게 정보를 넘기는 구조로 만들기
public class SkillManager
{
    Dictionary<eSkill, string> _skillDic = new Dictionary<eSkill, string>();        // 스킬 타입과 스킬 프리팹 경로를 저장할 딕셔너리
    Dictionary<eSkill, float> _skillCoolDownDic = new Dictionary<eSkill, float>();  // 스킬 타입과 해당 스킬의 쿨타임을 저장할 딕셔너리
    Dictionary<eSkill, bool> _skillReadyDic = new Dictionary<eSkill, bool>();       // 스킬 타입과 해당 스킬의 사용 가능 여부를 저장할 딕셔너리

    public Dictionary<eSkill, string> SkillDic
    {
        get
        {
            return _skillDic;
        }
    }

    public Dictionary<eSkill, float> SkillCoolDownDic
    {
        get
        {
            return _skillCoolDownDic;
        }
    }

    public Dictionary<eSkill, bool> SkillReadyDic
    {
        get
        {
            return _skillReadyDic;
        }
    }

    public SkillManager()
    {
        _skillDic.Add(eSkill.FIREBALL, "Prefabs/Fireball");
        _skillReadyDic.Add(eSkill.FIREBALL, true);

        _skillDic.Add(eSkill.FIRESLASH, "Prefabs/FireSlash");
        _skillReadyDic.Add(eSkill.FIRESLASH, true);

        _skillDic.Add(eSkill.POISON, "Prefabs/Poison");
        _skillReadyDic.Add(eSkill.POISON, true);
    }

    public GameObject LoadPrefab(eSkill type)
    {
        GameObject prefab = Resources.Load<GameObject>(_skillDic[type]);           // 스킬 프리팹 생성
        return prefab;
    }

    // PlayerControl에서 만들어진 스킬 프리팹의 쿨타임을 딕셔너리에 저장하는 메서드
    // 이미 등록된 스킬 타입인 경우 넘어감
    public void GetCoolDown(GameObject prefab, eSkill type)
    {
        if (!_skillCoolDownDic.ContainsKey(type))                                     // 각 스킬 타입별로 쿨타임 정보를 딕셔너리에 저장
        {
            float coolDown = prefab.GetComponent<Skill>().GetStat()._coolDown;         // 프리팹의 Skill 클래스가 가진 쿨타임을 지역 변수로 정의
            _skillCoolDownDic.Add(type, coolDown);
        }
    }

    // 사용한 스킬의 쿨타임 동안 사용 불가하도록 변경
    public IEnumerator CountCoolDown(eSkill type)
    {
        //Debug.Log("코루틴 실행 중");
        _skillReadyDic[type] = false;
        yield return new WaitForSeconds(_skillCoolDownDic[type]);
        _skillReadyDic[type] = true;
        yield break;
    }
}
