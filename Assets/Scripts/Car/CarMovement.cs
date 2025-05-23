using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(CarSound))]
public class CarMovement : MonoBehaviour
{
    [SerializeField] private TargetsCar _targetCar;
    [SerializeField] private List<Wheel> _wheels;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _turnSpeed;

    private CarSound _carSound;
    private Rigidbody _rigidbody;
    private Transform _target;

    private const float MinSpeedEnableDrift = 5f;
    private const float MaxSpeedEnableDrift = 13f;

    public bool IsReady { get; private set; }

    private void Start()
    {
        IsReady = false;
        _rigidbody = GetComponent<Rigidbody>();
        _carSound = GetComponent<CarSound>();
        _targetCar.AssignTarget(ref _target);
    }

    private void FixedUpdate()
    {
        if (IsReady == true)
        {
            Move();
            Turn();

            if (_rigidbody.velocity.magnitude > MinSpeedEnableDrift && _rigidbody.velocity.magnitude < MaxSpeedEnableDrift)
                SetValueTraces(true);
            else
                SetValueTraces(false);
        }
        else
        {
            SetValueTraces(false);
            _rigidbody.velocity = Vector3.zero;
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.TryGetComponent<Checkpoint>(out Checkpoint checkpoint))
        {
            checkpoint.TransferTargetCar(ref _targetCar);
            _targetCar.AssignTarget(ref _target);
        }
    }

    private void Move()
    {
        _carSound.PlayMove();
        _rigidbody.AddRelativeForce(Vector3.forward * _moveSpeed, ForceMode.VelocityChange);
    }

    private void Turn()
    {
        Vector3 directionSteer = _target.position - transform.position;
        Quaternion rotationSteer = Quaternion.LookRotation(directionSteer);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotationSteer, _turnSpeed);
    }

    private void SetValueTraces(bool flag)
    {
        if (flag == true)
            _carSound.PlayTurn();

        for (int i = 0; i < _wheels.Count; i++)
            _wheels[i].SetFlag(flag);
    }

    public void StartMove()
    {
        IsReady = true;
    }

    public void StopMove()
    {
        IsReady = false;
    }
}
