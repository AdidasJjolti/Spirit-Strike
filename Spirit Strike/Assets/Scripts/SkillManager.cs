using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ToDo : 미리 사용할 스킬 클래스를 생성하여 스킬 관리, 플레이어에게 정보를 넘기는 구조로 만들기
public class SkillManager : MonoBehaviour
{
    [SerializeField] List<Skill> _skillList = new List<Skill>();

    void Start()
    {
        for(int i = 0; i < _skillList.Count; i++)
        {
            Debug.Log($"{_skillList[i].name}의 쿨타임은 {_skillList[i].GetStat()._coolDown}야");
        }
    }
}
