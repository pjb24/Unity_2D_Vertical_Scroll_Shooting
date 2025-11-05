using System.Collections.Generic;
using UnityEngine;

public class Follower : MonoBehaviour
{
    [SerializeField] private float _maxShotDelay = 2f;
    private float _curShotDelay;

    [SerializeField] private ObjectManager _objectManager;

    private Vector3 _followPos;
    [SerializeField] private int _followDelay = 12;
    [SerializeField] private Transform _parent;
    private Queue<Vector3> _parentPos;

    private void Awake()
    {
        _parentPos = new Queue<Vector3>();
    }

    private void Update()
    {
        Watch();
        Follow();
        Fire();
        Reload();
    }

    private void Watch()
    {
        // Queue = FIFO (First In, First Out)
        // Input Position
        if (!_parentPos.Contains(_parent.position))
        {
            _parentPos.Enqueue(_parent.position);
        }

        // Output Position
        if (_parentPos.Count > _followDelay)
        {
            _followPos = _parentPos.Dequeue();
        }
        else if (_parentPos.Count < _followDelay)
        {
            _followPos = _parent.position;
        }
    }

    private void Follow()
    {
        transform.position = _followPos;
    }

    private void Fire()
    {
        if (!Input.GetButton("Fire1"))
        {
            return;
        }

        if (_curShotDelay < _maxShotDelay)
        {
            return;
        }

        GameObject bullet = _objectManager.MakeObj("BulletFollower");
        bullet.transform.position = transform.position;

        Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
        rigid.AddForce(Vector2.up * 10f, ForceMode2D.Impulse);

        _curShotDelay = 0f;
    }

    private void Reload()
    {
        _curShotDelay += Time.deltaTime;
    }
}
