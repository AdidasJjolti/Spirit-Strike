using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ToDo : �̸� ����� ��ų Ŭ������ �����Ͽ� ��ų ����, �÷��̾�� ������ �ѱ�� ������ �����
public class SkillManager : MonoBehaviour
{
    [SerializeField] List<Skill> _skillList = new List<Skill>();

    void Start()
    {
        for(int i = 0; i < _skillList.Count; i++)
        {
            Debug.Log($"{_skillList[i].name}�� ��Ÿ���� {_skillList[i].GetStat()._coolDown}��");
        }
    }
}
