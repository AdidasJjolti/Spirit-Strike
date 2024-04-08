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

// ToDo : �̸� ����� ��ų Ŭ������ �����Ͽ� ��ų ����, �÷��̾�� ������ �ѱ�� ������ �����
public class SkillManager
{
    Dictionary<eSkill, string> _skillDic = new Dictionary<eSkill, string>();        // ��ų Ÿ�԰� ��ų ������ ��θ� ������ ��ųʸ�
    Dictionary<eSkill, float> _skillCoolDownDic = new Dictionary<eSkill, float>();  // ��ų Ÿ�԰� �ش� ��ų�� ��Ÿ���� ������ ��ųʸ�
    Dictionary<eSkill, bool> _skillReadyDic = new Dictionary<eSkill, bool>();       // ��ų Ÿ�԰� �ش� ��ų�� ��� ���� ���θ� ������ ��ųʸ�

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
    }

    public GameObject LoadPrefab(eSkill type)
    {
        GameObject prefab = Resources.Load<GameObject>(_skillDic[type]);           // ��ų ������ ����
        return prefab;
    }

    public void GetCoolDown(GameObject prefab, eSkill type)
    {
        float coolDown = prefab.GetComponent<Skill>().GetStat()._coolDown;         // �������� Skill Ŭ������ ���� ��Ÿ���� ���� ������ ����
        if (!_skillCoolDownDic.ContainsKey(type))                                  // �� ��ų Ÿ�Ժ��� ��Ÿ�� ������ ��ųʸ��� ����
        {
            _skillCoolDownDic.Add(type, coolDown);
        }
    }

    // ����� ��ų�� ��Ÿ�� ���� ��� �Ұ��ϵ��� ����
    public IEnumerator CountCoolDown(eSkill type)
    {
        _skillReadyDic[type] = false;
        yield return new WaitForSeconds(_skillCoolDownDic[type]);
        _skillReadyDic[type] = true;
        yield break;
    }
}
