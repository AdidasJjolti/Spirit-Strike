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
    public Transform _enemyTransform;
    Slider _slider;

    void Start()
    {
        //_canvas = GetComponentInParent<Canvas>();
        //_camera = _canvas.worldCamera;
        //_rectParent = _canvas.GetComponent<RectTransform>();
        //_rectHP = this.gameObject.GetComponent<RectTransform>();
        _slider = GetComponent<Slider>();
    }

    void LateUpdate()
    {
        //var screenPos = Camera.main.WorldToScreenPoint(_enemyTransform.position + _offset);
        //var localPos = Vector2.zero;
        //RectTransformUtility.ScreenPointToLocalPointInRectangle(_rectParent, screenPos, _camera, out localPos);
        //_rectHP.localPosition = localPos;

        if(_enemyTransform == null)
        {
            return;
        }
        transform.position = Camera.main.WorldToScreenPoint(_enemyTransform.position + new Vector3(0, 1.0f, 0));
    }

    public void ChangeValue(int hp)
    {
        _slider.value = hp;
    }
}
