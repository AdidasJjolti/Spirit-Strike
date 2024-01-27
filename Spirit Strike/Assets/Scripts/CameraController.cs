using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform _player;
    [SerializeField] Vector3 _offset;

    void Update()
    {
        transform.position = _player.transform.position + _offset;
    }
}
