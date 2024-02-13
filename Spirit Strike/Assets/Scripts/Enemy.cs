using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] int _HP = 2;
    [SerializeField] PlayerControl _player;   // 공격 목표물로 사용할 플레이어 위치 저장
    NavMeshAgent _agent;
    Rigidbody _rigid;
    Animator _animator;

    [SerializeField] float _attackSpeed = 1.0f;
    [SerializeField] float _attackDelay = 1.0f;
    [SerializeField] int _damage = 1;
    [SerializeField] ParticleSystem _atkEffect; 

    public int HP
    {
        get
        {
            return _HP;
        }
    }

    void OnEnable()
    {
        if(_player == null)
        {
            _player = FindObjectOfType<PlayerControl>();
        }
    }

    void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _rigid = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
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
        Debug.Log($"현재 체력은 : {_HP}");


        if(_HP <= 0)
        {
            StartCoroutine("Die");
        }
    }

    IEnumerator Die()
    {
        _animator.SetTrigger("Die");
        yield return new WaitForSecondsRealtime(1.0f);
        //gameObject.SetActive(false);
        Destroy(this.gameObject);
        yield return null;
    }

    void Move()
    {
        _agent.SetDestination(_player.transform.position);
        _animator.SetBool("isMoving", true);
    }

    void AttackPlayer()
    {
        if (_attackDelay < _attackSpeed)
        {
            return;
        }

        transform.LookAt(_player.transform);
        _animator.SetBool("isMoving", false);
        _animator.SetTrigger("isAttacking");

        _player.TakeDamage(_damage);

        // ToDo : 공격 애니메이션 재생 진행도에 맞춰서 이펙트를 재생하도록 수정

        if (!_atkEffect.isPlaying)
        {
            _atkEffect.Play(true);
            Debug.Log("이펙트 재생");
        }
        //StartCoroutine("PlayEffect");

        _attackDelay = 0;
    }

    IEnumerator PlayEffect()
    {
        if(!_atkEffect.isPlaying)
        {
            _atkEffect.Play(true);
            Debug.Log("이펙트 재생");
        }
        //yield return new WaitForSeconds(0.5f);
        //_atkEffect.Stop(true);
        yield break;
    }
}
