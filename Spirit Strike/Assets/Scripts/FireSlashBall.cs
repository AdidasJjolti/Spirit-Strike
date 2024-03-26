using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireSlashBall : FireSlash
{
    [SerializeField] Vector3 _startPosition;

    protected override void OnEnable()
    {
        Debug.Log("자식 OnEnable");
        //base.OnEnable();              // 부모 클래스가 오브젝트로 이미 존재하기 때문에 부모의 OnEnable은 자식의 OnEnable 이전에 이미 호출하므로 사용하지 않음
        _startPosition = transform.position;
        Debug.Log($"시작 지점은 {_startPosition}");
    }

    void Update()
    {
        if(Vector3.Distance(transform.position, _startPosition) >= 25.0f)
        {
            Debug.Log($"거리는 {Vector3.Distance(transform.position, _startPosition)}야.");
            _isDistanceLimit = true;
            RemoveFireSlash();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<Enemy>().TakeDamage(_stat._damage);
            gameObject.SetActive(false);
        }
    }
}
