using System;
using UnityEngine;

[RequireComponent(typeof(PolygonCollider2D))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(EnemyAI))]
public class EnemyEntity : MonoBehaviour
{
    [SerializeField] private EnemySO _enemySO;
    public event EventHandler OnTakeDamage;
    public event EventHandler OnDeath;

    private PolygonCollider2D _polygonCollider2D;
    private BoxCollider2D _boxCollider2D;
    private EnemyAI _enemyAI;

    private int _currentHealth;
    private void Awake()
    {
        _polygonCollider2D = GetComponent<PolygonCollider2D>();
        _boxCollider2D = GetComponent<BoxCollider2D>();
        _enemyAI = GetComponent<EnemyAI>();
    }
    private void Start()
    {
        _currentHealth = _enemySO.enemyHealth;
    }
    
    public void TakeDamage(int damage)
    {
        _currentHealth -= damage;
        OnTakeDamage?.Invoke(this, EventArgs.Empty);
        DetectDeath();
    }
    public void PolygonColliderTurnOn()
    {
        _polygonCollider2D.enabled = true;
    }
    public void PolygonColliderTurnOff()
    {
        _polygonCollider2D.enabled = false;
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.transform.TryGetComponent(out Hero hero))
            hero.TakeDamage(transform, _enemySO.enemyDamageAmount);
    }
    private void DetectDeath()
    {
        if (_currentHealth <= 0)
        {
            _boxCollider2D.enabled = false;
            _polygonCollider2D.enabled = false;
            _enemyAI.SetDeathState();
            OnDeath?.Invoke(this, EventArgs.Empty);
        }
    }
}


