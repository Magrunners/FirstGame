using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider2D))]
public class Hero : MonoBehaviour
{

    [SerializeField] private float movingSpeed = 5f;
    [SerializeField] private int maxHealth = 30;
    [SerializeField] private float damageRecoveryTime = 0.5f;

    private Rigidbody2D _rb;
    private CapsuleCollider2D _capsuleCollider;
    private KnockBack _knockBack;

    public EventHandler OnHeroDeath;
    public EventHandler OnFlashBlink;

    private readonly float _heroMinMovingSpeed = 0.1f;
    private int _currentHealth;
    private bool _isRunning = false;
    Vector2 inputVector;
    private bool _canTakeDamage;
    private bool _isAlive = true;

    private Camera _mainCamera;
    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _capsuleCollider = GetComponent<CapsuleCollider2D>();
        _knockBack = GetComponent<KnockBack>();
        _mainCamera = Camera.main;
    }
    private void Start()
    {
        _currentHealth = maxHealth;
        _canTakeDamage = true;
        GameInput.Instance.OnHeroAttack += Hero_OnHeroAttack;
    }
    private void Update()
    {
        inputVector = GameInput.Instance.GetMovementVector();
    }
    private void FixedUpdate()
    {
        if (_knockBack.IsGettingKnockBack)
            return;
        HandleMovement();

    }
    public bool IsAlive()
    {
        return _isAlive;
    }
    public bool IsRunning()
    {
        return _isRunning;
    }
    public void TakeDamage(Transform damageSource, int damage)
    {
        if (_canTakeDamage && _isAlive)
        {

            _canTakeDamage = false;
            _currentHealth = Mathf.Max(0, _currentHealth -= damage);
            _knockBack.GetKnockBack(damageSource);

            OnFlashBlink.Invoke(this, EventArgs.Empty);

            StartCoroutine(DemageRecoveryRoutine());
        }
        DetectDeath();
    }
    // Проверка смерти героя
    private void DetectDeath()
    {
        if (_currentHealth == 0 && _isAlive)
        {
            _isAlive = false;
            _capsuleCollider.enabled = false;
            _knockBack.StopKnockBackMovement();
            GameInput.Instance.DisableMovement();
            OnHeroDeath.Invoke(this, EventArgs.Empty);
        }
    }
    // Восстановление возможности получать урон после времени неуязвимости
    private IEnumerator DemageRecoveryRoutine()
    {
        yield return new WaitForSeconds(damageRecoveryTime);
        _canTakeDamage = true;
    }
    // Позиция героя на экране
    public Vector3 HeroPosition()
    {
        Vector3 heroPosition = _mainCamera.WorldToScreenPoint(transform.position);
        return heroPosition;
    }
    // Атака героя
    private void Hero_OnHeroAttack(object sender, System.EventArgs e)
    {
        ActiveWeapon.Instance.GetActiveWeapon().Attack();
    }
    // Поворот героя в сторону курсора мыши
    private void HandleMovement()
    {
        _rb.MovePosition(_rb.position + inputVector * (movingSpeed * Time.fixedDeltaTime));
        if (Mathf.Abs(inputVector.x) > _heroMinMovingSpeed || Mathf.Abs(inputVector.y) > _heroMinMovingSpeed)
            _isRunning = true;
        else
            _isRunning = false;
    }
    private void OnDestroy()
    {
        GameInput.Instance.OnHeroAttack -= Hero_OnHeroAttack;
    }
}



