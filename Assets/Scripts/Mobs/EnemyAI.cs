using UnityEngine;
using UnityEngine.AI;
using EnemyUtils;
using UnityEngine.Rendering;
using Unity.VisualScripting;
using System;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private State _startingState;
    [SerializeField] private float _roamingDistanceMax = 7f;
    [SerializeField] private float _roamingDistanceMin = 3f;
    [SerializeField] private float _roamingTimerMax = 2f;

    [SerializeField] private bool _isChasingEnemy = false;
    [SerializeField] private float _chasingDistance = 4f;
    [SerializeField] private float _chasingSpeedMultyplier = 2f;

    [SerializeField] private bool _isAttackingEnemy = false;
    [SerializeField] private float _attackingDistance = 2.5f;
    [SerializeField] private float _attackRate = 1f;

    private NavMeshAgent _navMeshAgent;
    private Hero _hero;
    private State _currentState;

    private Vector3 _startingPosition;
    private Vector3 _roamPosition;
    private float _roamingTimer;
    private float _roamingSpeed;
    private float _chasingSpeed;

    private float _nextAttackTimer = 0f;

    private float _nextCheckDirectionTime = 0f;
    private readonly float _checkDirectionDuration = 0.1f;
    private Vector3 _lastPosition;

    public event EventHandler OnEnemyAttack;

    public bool IsRoaming => _navMeshAgent.velocity != Vector3.zero;

    private enum State
    {
        Idle,
        Roaming,
        Chasing,
        Attacking,
        Death
    }
    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navMeshAgent.updateRotation = false;
        _navMeshAgent.updateUpAxis = false;
        _currentState = _startingState;
        _roamingSpeed = _navMeshAgent.speed;
        _chasingSpeed = _navMeshAgent.speed * _chasingSpeedMultyplier;
        _hero = FindAnyObjectByType<Hero>();
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        StateHandler();
        MovementDirectionHandler();
    }
    public void SetDeathState()
    {
        _navMeshAgent.ResetPath();
        _currentState = State.Death;
    }
    private void StateHandler()
    {
        switch (_currentState)
        {
            case State.Roaming:
                _roamingTimer -= Time.deltaTime;
                if (_roamingTimer < 0)
                {
                    Roaming();
                    _roamingTimer = _roamingTimerMax;
                }
                CheckCurrentState();
                break;
            case State.Chasing:
                ChasingTarget();
                CheckCurrentState();
                break;
            case State.Attacking:
                AttakingTarget();
                CheckCurrentState();
                break;
            case State.Death:
                break;
            default:
            case State.Idle:
                CheckCurrentState();
                break;

        }
    }
    private void CheckCurrentState()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, _hero.transform.position);
        State newState = State.Roaming;

        if (_isChasingEnemy)
        {
            if(distanceToPlayer <= _chasingDistance)
                newState = State.Chasing;
        }
        if (_isAttackingEnemy)
        {
            if (distanceToPlayer <= _attackingDistance)
                newState = State.Attacking;
        }
        if(newState != _currentState)
        {
            if (newState == State.Chasing)
            {
                _navMeshAgent.ResetPath();
                _navMeshAgent.speed = _chasingSpeed;
            }
            _currentState = newState;
        }
        else if(newState == State.Roaming)
        {
            _navMeshAgent.speed = _roamingSpeed;
        }
        else if( newState == State.Attacking)
        {
            _navMeshAgent.ResetPath();
        }


        _currentState = newState;
    }
    private void Roaming()
    {
        _startingPosition = transform.position;
        _roamPosition = GetRoamingPosition();
        _navMeshAgent.SetDestination(_roamPosition);
    }


    private Vector3 GetRoamingPosition()
    {
        return _startingPosition + MobsUtils.GetRandomDir() * UnityEngine.Random.Range(_roamingDistanceMin, _roamingDistanceMax);
    }
    private void ChasingTarget()
    {
        _navMeshAgent.SetDestination(_hero.transform.position);
    }
    private void AttakingTarget()
    {
        if (Time.time > _nextAttackTimer)
        {
            OnEnemyAttack?.Invoke(this, EventArgs.Empty);
            _nextAttackTimer = Time.time + _attackRate;
        }

    }
    private void MovementDirectionHandler()
    {
        if (Time.time > _nextCheckDirectionTime)
        {
            if (IsRoaming)
            {
                ChangeFacingDirection(_lastPosition, transform.position);
            }
            else if (_currentState == State.Attacking)
            {
                ChangeFacingDirection(transform.position, _hero.transform.position);
            }

            _lastPosition = transform.position;
            _nextCheckDirectionTime = Time.time + _checkDirectionDuration;
        }
    }
    private void ChangeFacingDirection(Vector3 sourcePosition, Vector3 targetPosition)
    {
        if (sourcePosition.x > targetPosition.x)
        {
            transform.rotation = Quaternion.Euler(0, -180, 0);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }
    public float GetRoamingAnimationSpeed()
    {
        return _navMeshAgent.speed / _roamingSpeed;
    }

    


}
