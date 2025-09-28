using System;
using System.Collections;
using UnityEngine;

public class Hero : MonoBehaviour
{
   
    [SerializeField] private float _movingSpeed = 5f;
    [SerializeField] private int _maxHealth = 30;
    [SerializeField] private float _damageRecoveryTime = 0.5f;

    private Rigidbody2D _rb;
    private KnockBack _knockBack;

    public EventHandler OnHeroDeath;

    private float _heroMinMovingSpeed = 0.1f;
    private int _currentHealth;
    private bool _isRunning = false;
    Vector2 inputVector;
    private bool _canTakeDamage;
    private bool _isAlive = true;
    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();      
        _knockBack = GetComponent<KnockBack>();
    }
    private void Start()
    {
        _currentHealth = _maxHealth;
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

            StartCoroutine(DemageRecoveryRoutine());
        }
        DetectDeath();
    }

    private void DetectDeath()
    {
        if(_currentHealth ==  0 && _isAlive)
        {
            _isAlive = false;
            _knockBack.StopKnockBackMovement();
            GameInput.Instance.DisableMovement();
            OnHeroDeath.Invoke(this, EventArgs.Empty);
        }
    }

    private IEnumerator DemageRecoveryRoutine()
    {
        yield return new WaitForSeconds(_damageRecoveryTime);
        _canTakeDamage = true;
    }

    public Vector3 HeroPosition()
    {
        Vector3 heroPosition = Camera.main.WorldToScreenPoint(transform.position);

        return heroPosition;
    }
    private void Hero_OnHeroAttack(object sender, System.EventArgs e)
    {
        ActiveWeapon.Instance.GetActiveWeapon().Attack();
    }
    private void HandleMovement()
    {
        
        _rb.MovePosition(_rb.position + inputVector* (_movingSpeed* Time.fixedDeltaTime));
        if(Mathf.Abs(inputVector.x) > _heroMinMovingSpeed || Mathf.Abs(inputVector.y) > _heroMinMovingSpeed)
            _isRunning = true;
        else
            _isRunning = false;
    }

}



