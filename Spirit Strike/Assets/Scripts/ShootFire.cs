using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootFire : MonoBehaviour
{
    Vector3 _player;
    Vector3 _target;
    Rigidbody _rigid;

    void OnEnable()
    {
        _player = FindObjectOfType<PlayerControl>().transform.position;
        _target = FindObjectOfType<PlayerControl>().TargetEnemy.transform.position;
        _rigid = GetComponent<Rigidbody>();

        Vector3 dir = (_target - _player).normalized;
        _rigid.AddForce(dir * 10f, ForceMode.Impulse);
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy"))
        {
            Destroy(this.gameObject);
        }
    }
}
