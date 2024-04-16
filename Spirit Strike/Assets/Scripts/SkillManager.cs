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

// ToDo : �̸� ����� ��ų Ŭ������ �����Ͽ� ��ų ����, �÷��̾�� ������ �ѱ�� ������ �����
public class SkillManager
{
    Dictionary<eSkill, string> _skillDic = new Dictionary<eSkill, string>();        // ��ų Ÿ�԰� ��ų ������ ��θ� ������ ��ųʸ�
    Dictionary<eSkill, float> _skillCoolDownDic = new Dictionary<eSkill, float>();  // ��ų Ÿ�԰� �ش� ��ų�� ��Ÿ���� ������ ��ųʸ�
    Dictionary<eSkill, bool> _skillReadyDic = new Dictionary<eSkill, bool>();       // ��ų Ÿ�԰� �ش� ��ų�� ��� ���� ���θ� ������ ��ųʸ�

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
        GameObject prefab = Resources.Load<GameObject>(_skillDic[type]);           // ��ų ������ ����
        return prefab;
    }

    // PlayerControl���� ������� ��ų �������� ��Ÿ���� ��ųʸ��� �����ϴ� �޼���
    // �̹� ��ϵ� ��ų Ÿ���� ��� �Ѿ
    public void GetCoolDown(GameObject prefab, eSkill type)
    {
        if (!_skillCoolDownDic.ContainsKey(type))                                     // �� ��ų Ÿ�Ժ��� ��Ÿ�� ������ ��ųʸ��� ����
        {
            float coolDown = prefab.GetComponent<Skill>().GetStat()._coolDown;         // �������� Skill Ŭ������ ���� ��Ÿ���� ���� ������ ����
            _skillCoolDownDic.Add(type, coolDown);
        }
    }

    // ����� ��ų�� ��Ÿ�� ���� ��� �Ұ��ϵ��� ����
    public IEnumerator CountCoolDown(eSkill type)
    {
        //Debug.Log("�ڷ�ƾ ���� ��");
        _skillReadyDic[type] = false;
        yield return new WaitForSeconds(_skillCoolDownDic[type]);
        _skillReadyDic[type] = true;
        yield break;
    }
}
