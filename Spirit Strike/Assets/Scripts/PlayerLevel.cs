using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLevel : MonoBehaviour
{
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

    int _playerLv;

    public int PlayerLv
    {
        get
        {
            return _playerLv;
        }
    }

    // ���� ���� �� ����ġ�� 0, �÷��̾� ������ 1�� ������
    public PlayerLevel()
    {
        _curExp = 0;
        _accExp = 0;
        _playerLv = 1;
    }
}
