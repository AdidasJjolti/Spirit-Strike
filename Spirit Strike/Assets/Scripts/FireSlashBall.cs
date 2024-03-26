using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireSlashBall : FireSlash
{
    [SerializeField] Vector3 _startPosition;

    protected override void OnEnable()
    {
        Debug.Log("�ڽ� OnEnable");
        //base.OnEnable();              // �θ� Ŭ������ ������Ʈ�� �̹� �����ϱ� ������ �θ��� OnEnable�� �ڽ��� OnEnable ������ �̹� ȣ���ϹǷ� ������� ����
        _startPosition = transform.position;
        Debug.Log($"���� ������ {_startPosition}");
    }

    void Update()
    {
        if(Vector3.Distance(transform.position, _startPosition) >= 25.0f)
        {
            Debug.Log($"�Ÿ��� {Vector3.Distance(transform.position, _startPosition)}��.");
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
