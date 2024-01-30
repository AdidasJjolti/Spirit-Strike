using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class PlayerControl : MonoBehaviour
{
    Rigidbody _rigid;
    [SerializeField] bool _isWalking;
    [SerializeField] bool _isIdle;
    [SerializeField] bool _isAttacking;

    float _hAxis;
    float _vAxis;
    Vector3 _moveDir;
    float _moveSpeed = 1f;

    Animator _animator;

    [SerializeField] float _attackSpeed = 1.0f;
    [SerializeField] float _attackDelay = 1.0f;
    [SerializeField] Enemy _targetEnemy;
    int _damage = 1;

    [SerializeField] float _rayDistance = 1.5f;

    public PlayerData playerData;

    public float attackSpeed
    {
        get
        {
            return _attackSpeed;
        }
    }

    void Awake()
    {
        _rigid = GetComponent<Rigidbody>();
        _isIdle = true;

        _animator = GetComponent<Animator>();
        _animator.SetBool("isIdle", true);
    }

    void Update()
    {
        _attackDelay += Time.deltaTime;

        _hAxis = Input.GetAxisRaw("Horizontal");
        _vAxis = Input.GetAxisRaw("Vertical");
        _moveDir = new Vector3(_hAxis, 0, _vAxis).normalized;

        transform.position += _moveDir * _moveSpeed * Time.deltaTime;

        if(_moveDir != Vector3.zero)
        {
            _isIdle = false;
            _isWalking = true;

            _animator.SetBool("isWalking", _isWalking);
            _animator.SetBool("isIdle", _isIdle);
        }
        else
        {
            _isIdle = true;
            _isWalking = false;

            _animator.SetBool("isWalking", _isWalking);
            _animator.SetBool("isIdle", _isIdle);
        }

        transform.LookAt(transform.position + _moveDir);

        if(Input.GetKey(KeyCode.Space))
        {
            Attack();
        }
        else
        {
            _isAttacking = false;
            _animator.SetBool("isAttacking", _isAttacking);
        }

        Debug.DrawRay(transform.position + new Vector3(0, 0.5f, 0), transform.forward * _rayDistance, Color.red);
    }

    void Attack()
    {
        if(_attackDelay < _attackSpeed)
        {
            return;
        }

        _isIdle = false;
        _isWalking = false;

        _animator.SetBool("isWalking", _isWalking);
        _animator.SetBool("isIdle", _isIdle);

        _isAttacking = true;
        _animator.SetBool("isAttacking", _isAttacking);

        _attackDelay = 0;

        Ray ray = new Ray(transform.position + new Vector3(0, 0.5f, 0), transform.forward);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, _rayDistance))
        {
            if(hit.collider.GetComponent<Enemy>() == _targetEnemy)
            {
                _targetEnemy.TakeDamage(_damage);
            }
        }
    }

    [ContextMenu("To Json Data")]
    void SavePlayerDataToJson()
    {
        string jsonData = JsonUtility.ToJson(playerData, true);
        string path = Path.Combine(Application.dataPath, "playerData.json");
        File.WriteAllText(path, jsonData);
    }

    [ContextMenu("From Json Data")]
    void LoadPlayerDataFromJson()
    {
        string path = Path.Combine(Application.dataPath, "playerData.json");
        string jsonData = File.ReadAllText(path);
        playerData = JsonUtility.FromJson<PlayerData>(jsonData);
    }
}

[System.Serializable]
public class PlayerData
{
    public string name;
    public int age;
    public int level;
    public bool isDead;
    public string[] items;
}
