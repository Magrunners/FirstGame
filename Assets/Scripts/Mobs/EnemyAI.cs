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
    [SerializeField] private bool _isAttackingEnemy = false;
    [SerializeField] private float _attackingDistance = 2f;
    [SerializeField] private float _chasingDistance = 4f;
    [SerializeField] private float _chasingSpeedMultyplier = 2f;
    [SerializeField] private float _attackRate = 2f;

    private float _nextAttackTimer = 0f;
    private NavMeshAgent _navMeshAgent;
    private State _currentState;
    private float _roamingTimer;
    private Vector3 _roamPosition;
    private Vector3 _startingPosition;
    private Hero _hero;
    private float _roamingSpeed;
    private float _chasingSpeed;
    public event EventHandler OnEnemyAttack;

    public bool IsRoaming
    {
        get
        {
            if (_navMeshAgent.velocity == Vector3.zero)
                return false;
            else
                return true;
        }
    }
    private enum State
    {
        Idle,
        Roaming,
        Chasing,
        Attaking,
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

    private void Update()
    {
        StateHandler();
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
            case State.Attaking:
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

    private State CheckCurrentState()
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
                newState = State.Attaking;
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
        else if( newState == State.Attaking)
        {
            _navMeshAgent.ResetPath();
        }


            return newState;
    }
    private void Roaming()
    {
        _startingPosition = transform.position;
        _roamPosition = GetRoamingPosition();
        _navMeshAgent.SetDestination(_roamPosition);
        ChangeFacingDirection(_startingPosition, _roamPosition);
    }
    private Vector3 GetRoamingPosition()
    {
        return _startingPosition + MobsUtils.GetRandomDir() * UnityEngine.Random.Range(_roamingDistanceMin, _roamingDistanceMax);

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

    private void ChasingTarget()
    {
        _navMeshAgent.SetDestination(_hero.transform.position);
        _startingPosition = _navMeshAgent.transform.position;
        ChangeFacingDirection(_startingPosition, _hero.transform.position);
    }

    private void AttakingTarget()
    {
        if(Time.time > _nextAttackTimer)
        {
            OnEnemyAttack?.Invoke(this, EventArgs.Empty);
            _nextAttackTimer = Time.time + _attackRate;
        }

    }

    public float GetRoamingAnimationSpeed()
    {
        return _navMeshAgent.speed / _roamingSpeed;
    }



}
