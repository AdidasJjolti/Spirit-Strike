using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHPBar : MonoBehaviour
{
    //Camera _camera;
    //Canvas _canvas;
    //RectTransform _rectParent;
    //RectTransform _rectHP;

    //public Vector3 _offset = Vector3.zero;
    public Transform _transform;        // HP바 UI를 붙일 오브젝트의 좌표
    Slider _slider;
    Vector3 _monsterOffset = new Vector3(0.0f, 1.2f, 0.0f);
    Vector3 _playerOffset = new Vector3(0.0f, 2.0f, 0.0f);

    bool _isPlayer;

    void Start()
    {
        _slider = GetComponent<Slider>();
        CheckPlayer();
    }

    void LateUpdate()
    {
        if (_transform == null)
        {
            return;
        }

        // HP바 UI를 붙인 오브젝트의 머리 위 위치를 따라가면서 표시
        if (_isPlayer)
        {
            transform.position = Camera.main.WorldToScreenPoint(_transform.position + _playerOffset);
        }
        else
        {
            transform.position = Camera.main.WorldToScreenPoint(_transform.position + _monsterOffset);
        }
    }

    public void ChangeValue(int hp)
    {
        _slider.value = hp;
    }

    public void ChangeMaxValue(int maxHP)
    {
        _slider.maxValue = maxHP;
    }

    bool CheckPlayer()
    {
        return _isPlayer = _transform.GetComponent<PlayerControl>();
    }
}
