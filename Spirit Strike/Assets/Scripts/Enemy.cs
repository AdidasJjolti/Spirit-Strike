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
    [SerializeField] PlayerControl _player;   // 공격 목표물로 사용할 플레이어 위치 저장
    NavMeshAgent _agent;
    Rigidbody _rigid;
    Animator _animator;

    [SerializeField] float _attackSpeed = 1.0f;
    [SerializeField] float _attackDelay = 1.0f;
    [SerializeField] int _damage = 1;
    [SerializeField] ParticleSystem _atkEffect;
    [SerializeField] float _waitSec;

    [SerializeField] GameObject _hpBarPrefab;   // HP바 프리팹
    [SerializeField] Canvas _hpBarCanvas;       // HP바가 생성될 캔버스
    [SerializeField] Slider _hpBarSlider;       // HP바가 가진 슬라이더 컴포넌트
    GameObject _hpBarObj;                       // 적 생성될 때 함께 생성한 HP바 게임오브젝트
    UIHPBar _hpBar;                             // 대미지를 입을 때 슬라이더 값을 변경할 함수 연결용 변수

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
        // 플레이어 따라가기
        // 플레이어와 충분히 가까워지면 추적 정지
        // 플레이어 공격
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

    // 물리력이 NavAgent의 이동을 방해하지 않도록 속도를 0으로 설정
    void FreezeVelocity()
    {
        _rigid.velocity = Vector3.zero;
        _rigid.angularVelocity = Vector3.zero;
    }

    public void TakeDamage(int damage)
    {
        _HP -= damage;
        _hpBar.ChangeValue(HP);
        //Debug.Log($"현재 체력은 : {_HP}");

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

    // 실제 공격 모션과 이펙트 재생 시점을 맞추기 위해 _waitSec만큼 대기
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
