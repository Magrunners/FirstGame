using UnityEngine;
using UnityEngine.AI;
using EnemyUtils;
using System;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private State startingState;
    [SerializeField] private float roamingDistanceMax = 7f;
    [SerializeField] private float roamingDistanceMin = 3f;
    [SerializeField] private float roamingTimerMax = 2f;

    [SerializeField] private bool isChasingEnemy;
    [SerializeField] private float chasingDistance = 4f;
    [SerializeField] private float chasingSpeedMultyplier = 2f;

    [SerializeField] private bool isAttackingEnemy;
    [SerializeField] private float attackingDistance = 2.5f;
    [SerializeField] private float attackRate = 1f;

    [SerializeField] private bool isHeapEnemy;
    [SerializeField] private float risingDistance = 5f;

    private NavMeshAgent _navMeshAgent;
    private Hero _hero;
    private State _currentState;

    private Vector3 _startingPosition;
    private Vector3 _roamPosition;
    private float _roamingTimer;
    private float _roamingSpeed;
    private float _chasingSpeed;

    private float _nextAttackTimer;

    private float _nextCheckDirectionTime;
    private readonly float _checkDirectionDuration = 0.1f;
    private Vector3 _lastPosition;

    public event EventHandler OnEnemyAttack;
    public event EventHandler OnStartingRising;

    public bool IsRoaming => _navMeshAgent.velocity != Vector3.zero;

    private enum State
    {
        Idle,
        Roaming,
        Chasing,
        Attacking,
        Heap,
        Death
    }
    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navMeshAgent.updateRotation = false;
        _navMeshAgent.updateUpAxis = false;
        if (isHeapEnemy == true)
        {
            _navMeshAgent.enabled = false;
            _currentState = State.Heap;
        }
        else
        {
            _currentState = State.Idle;
        }
        _roamingSpeed = _navMeshAgent.speed;
        _chasingSpeed = _navMeshAgent.speed * chasingSpeedMultyplier;
        _hero = FindAnyObjectByType<Hero>();
    }
    private void Update()
    {
        StateHandler();
        MovementDirectionHandler();
    }
    public bool IsHeapEnemy()
    {
        return isHeapEnemy;
    }
    // Установка состояния смерти, остановка движения персонажа
    public void SetDeathState()
    {
        if (!IsNavMeshAgentReady())
            return;
        _chasingSpeed = 0;
        _navMeshAgent.ResetPath();
        _navMeshAgent.enabled = false;
        _currentState = State.Death;
    }
    // Проверка условий для подъёма из состояния "Heap"
    private void CheckForRising()
    {
        float _distanceToPlayer = Vector3.Distance(transform.position, _hero.transform.position);
        if (_distanceToPlayer <= risingDistance && _hero.IsAlive())
        {
            StartRising();
        }
    }
    // Логика подъёма из состояния "Heap"
    private void StartRising()
    {
        _currentState = State.Idle;

        OnStartingRising?.Invoke(this, EventArgs.Empty);
    }
    public void EndRising()
    {
        _navMeshAgent.enabled = true;
        _currentState = startingState;
    }
    // Обработка текущего состояния персонажа и выполнение соответствующих действий
    private void StateHandler()
    {
        switch (_currentState)
        {
            case State.Roaming:
                _roamingTimer -= Time.deltaTime;
                if (_roamingTimer < 0)
                {
                    Roaming();
                    _roamingTimer = roamingTimerMax;
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
            case State.Heap:
                CheckForRising();
                break;
            case State.Death:
                break;
            default:
            case State.Idle:
                CheckCurrentState();
                break;
        }
    }
    // Проверка и смена текущего состояния в зависимости от расстояния до цели и настроек преследования/атаки
    private void CheckCurrentState()
    {
        if(!IsNavMeshAgentReady())
            return;

        float distanceToPlayer = Vector3.Distance(transform.position, _hero.transform.position);
        State newState = State.Roaming;

        if (_currentState == State.Heap)
        {
            newState = State.Heap;
            return;
        }

        if (isChasingEnemy)
        {
            if (distanceToPlayer <= chasingDistance)
                newState = State.Chasing;
        }
        if (isAttackingEnemy && distanceToPlayer <= attackingDistance)
                newState = _hero.IsAlive() ? State.Attacking : State.Roaming;

        if (newState != _currentState)
        {
            if (newState == State.Chasing)
            {
                if (!IsNavMeshAgentReady())
                    return;
                _navMeshAgent.ResetPath();
                _navMeshAgent.speed = _chasingSpeed;
            }
            _currentState = newState;
        }
        else if (newState == State.Roaming)
            _navMeshAgent.speed = _roamingSpeed;
        else if (newState == State.Attacking)
            _navMeshAgent.ResetPath();

        _currentState = newState;
    }
    // Перемещение к случайной точке в пределах заданного радиуса от начальной позиции с определённым интервалом времени
    private void Roaming()
    {
        if (!IsNavMeshAgentReady())
            return;

        _startingPosition = transform.position;
        _roamPosition = GetRoamingPosition();
        _navMeshAgent.SetDestination(_roamPosition);
    }
    // Получение случайной точки для перемещения в пределах заданного радиуса от начальной позиции
    private Vector3 GetRoamingPosition()
    {
        return _startingPosition + MobsUtils.GetRandomDir() * UnityEngine.Random.Range(roamingDistanceMin, roamingDistanceMax);
    }
    private void ChasingTarget()
    {
        if (!IsNavMeshAgentReady())
            return;
        _navMeshAgent.SetDestination(_hero.transform.position);
    }
    // Атака цели с определённым интервалом
    private void AttakingTarget()
    {
        if (Time.time > _nextAttackTimer)
        {
            OnEnemyAttack?.Invoke(this, EventArgs.Empty);
            _nextAttackTimer = Time.time + attackRate;
        }
    }
    // Определение направления движения персонажа
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
    // Выбор направления персонажа в зависимости от положения цели
    private void ChangeFacingDirection(Vector3 sourcePosition, Vector3 targetPosition)
    {
        transform.rotation = sourcePosition.x > targetPosition.x ? Quaternion.Euler(0, -180, 0) : Quaternion.Euler(0, 0, 0);

    }
    public float GetRoamingAnimationSpeed()
    {
        return _navMeshAgent.speed / _roamingSpeed;
    }
    private bool IsNavMeshAgentReady()
    {
        if (_navMeshAgent == null)
        {
            return false;
        }

        if (!_navMeshAgent.isActiveAndEnabled)
        {
            return false;
        }

        return true;
    }
}
