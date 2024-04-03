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

    //[SerializeField] List<Skill> _skillList = new List<Skill>();
    Dictionary<eSkill, string> _skillDic = new Dictionary<eSkill, string>();

    public SkillManager()
    {
        // �� ��ų ������Ʈ�� ����
        // 1. ��ų ������(���ӿ�����Ʈ)�� ���� ��� �־�� �ϳ�?    ��ũ���ͺ� ������Ʈ�� �����ϱ� (�ĺ�)
        // 2. ���� �÷��̾ ��� �ִٸ� ��ų �Ŵ����� �ϴ� ���� �׳� � ��ų �� �� �ִ��� �����ϴ� ���ҹۿ� ���°� ����...
        // 3. ���࿡ ��ų �Ŵ����� ��� �ִٸ� SkillManager Ŭ���� ���� �ÿ� �� ��ų �����յ� �̸� �����ؾ� �ϴµ�, �̰� ��� �� �� ������?

        _skillDic.Add(eSkill.FIREBALL, "Prefabs/Fireball");
        _skillDic.Add(eSkill.FIRESLASH, "Prefabs/FireSlash");
    }

    public GameObject LoadPrefab(eSkill type)
    {
        GameObject prefab = Resources.Load<GameObject>(_skillDic[type]);
        return prefab;
    }
}
