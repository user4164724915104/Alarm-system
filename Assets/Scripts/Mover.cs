using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Mover : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private Renderer _body;
    [SerializeField] private Transform[] _waypoints;
    [SerializeField] private float _delay;
    private int _currentWaypoint = 0;
    private bool _isMoving = true;
    private Rigidbody _rigidbody;

    private void Start()
    {
        _body.material.color = Color.red;
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.freezeRotation = true;
    }

    private void Update()
    {
        if (transform.position == _waypoints[_currentWaypoint].position)
        {
            _currentWaypoint = (_currentWaypoint + 1) % _waypoints.Length;
            StartCoroutine(Looting());
        }

        if (_isMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, _waypoints[_currentWaypoint].position, _moveSpeed * Time.deltaTime);
        }
    }

    private IEnumerator Looting()
    {
        _isMoving = false;
        yield return new WaitForSeconds(_delay);
        _isMoving = true;
    }
}
