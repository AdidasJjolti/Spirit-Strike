using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [SerializeField] int _MaxHP = 20;
    [SerializeField] int _HP = 20;
    [SerializeField] PlayerControl _player;   // ���� ��ǥ���� ����� �÷��̾� ��ġ ����
    NavMeshAgent _agent;
    Rigidbody _rigid;
    Animator _animator;

    [SerializeField] float _attackSpeed = 1.0f;
    [SerializeField] float _attackDelay = 1.0f;
    [SerializeField] int _damage = 1;
    [SerializeField] ParticleSystem _atkEffect;
    [SerializeField] float _waitSec;

    [SerializeField] GameObject _hpBarPrefab;   // HP�� ������
    [SerializeField] Canvas _hpBarCanvas;       // HP�ٰ� ������ ĵ����
    [SerializeField] Slider _hpBarSlider;       // HP�ٰ� ���� �����̴� ������Ʈ
    GameObject _hpBarObj;                       // �� ������ �� �Բ� ������ HP�� ���ӿ�����Ʈ
    UIHPBar _hpBar;                             // ������� ���� �� �����̴� ���� ������ �Լ� ����� ����

    public int HP
    {
        get
        {
            return _HP;
        }
    }

    bool _isPoisonous;

    public bool IsPoisonous
    {
        get
        {
            return _isPoisonous;
        }

        set
        {
            _isPoisonous = value;
        }
    }

    void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _rigid = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();

        SetHPBar();
    }

    void OnEnable()
    {
        if (_player == null)
        {
            _player = FindObjectOfType<PlayerControl>();
        }

        _hpBarSlider.gameObject.SetActive(true);
    }

    void Update()
    {
        // �÷��̾� ���󰡱�
        // �÷��̾�� ����� ��������� ���� ����
        // �÷��̾� ����
        if (Vector3.Distance(transform.position, _player.transform.position) <= 1.2f)
        {
            if (_player != null)
            {
                _agent.isStopped = true;
                AttackPlayer();
            }
            else
            {
                _player = FindObjectOfType<PlayerControl>();
            }
        }
        else
        {
            Move();
        }

        _attackDelay += Time.deltaTime;
    }

    void FixedUpdate()
    {
        FreezeVelocity();
    }

    // �������� NavAgent�� �̵��� �������� �ʵ��� �ӵ��� 0���� ����
    void FreezeVelocity()
    {
        _rigid.velocity = Vector3.zero;
        _rigid.angularVelocity = Vector3.zero;
    }

    public void TakeDamage(int damage)
    {
        _HP -= damage;
        _hpBar.ChangeValue(HP);
        //Debug.Log($"���� ü���� : {_HP}");

        if (_HP <= 0)
        {
            StartCoroutine("Die");
        }
    }

    IEnumerator Die()
    {
        _animator.SetTrigger("Die");
        Destroy(_hpBarObj);
        yield return new WaitForSecondsRealtime(1.0f);
        Destroy(this.gameObject);
        yield return null;
    }

    void Move()
    {
        _agent.SetDestination(_player.transform.position);
        _animator.SetBool("isMoving", true);
        _animator.SetBool("isAttacking", false);
    }

    void AttackPlayer()
    {
        if(_animator == null)
        {
            Debug.LogError("Animator is Null");
            return;
        }

        if (_player == null)
        {
            Debug.LogError("Player is Null");
            return;
        }

        if (_attackDelay < _attackSpeed)
        {
            _animator.SetBool("isAttacking", false);
            return;
        }

        transform.LookAt(_player.transform);
        _animator.SetBool("isMoving", false);
        _animator.SetBool("isAttacking", true);

        _player.TakeDamage(_damage);

        StartCoroutine("PlayEffect");
        _attackDelay = 0;
    }


    IEnumerator PlayEffect()
    {
        _atkEffect.gameObject.SetActive(true);
        yield return new WaitUntil(CheckAnimationState);

        _atkEffect.Play();

        yield break;
    }

    // ���� ���� ��ǰ� ����Ʈ ��� ������ ���߱� ���� _waitSec��ŭ ���
    bool CheckAnimationState()
    {
        return _animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= _waitSec;
    }

    void SetHPBar()
    {
        _hpBarCanvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        GameObject hpBar = Instantiate<GameObject>(_hpBarPrefab, _hpBarCanvas.transform);
        _hpBarObj = hpBar;

        _hpBarSlider = hpBar.GetComponent<Slider>();
        _hpBarSlider.maxValue = _MaxHP;
        _hpBarSlider.value = _MaxHP;

        var bar = hpBar.GetComponent<UIHPBar>();
        bar._enemyTransform = this.gameObject.transform;
        _hpBar = bar;
    }
}
