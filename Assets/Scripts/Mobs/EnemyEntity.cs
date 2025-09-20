using System;
using UnityEngine;

[RequireComponent(typeof(PolygonCollider2D))]


public class EnemyEntity : MonoBehaviour
{
    public event EventHandler OnTakeDamage;

    [SerializeField] private int _maxHealth;
    private int _currentHealth;

    private PolygonCollider2D _polygonCollider2D;

    private void Awake()
    {
        _polygonCollider2D = GetComponent<PolygonCollider2D>();
    }

    private void Start()
    {
        _currentHealth = _maxHealth;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Attack");
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

    private void DetectDeath()
    {
        if (_currentHealth <= 0)
            Destroy(gameObject);
    }


}


