using System;
using UnityEngine;
public class Sword : MonoBehaviour
{
    [SerializeField] private int swordDamage = 1;

    public event EventHandler OnSwordSwing;
    public PolygonCollider2D _polygonCollider2D;
    public void Awake()
    {
        _polygonCollider2D = GetComponent<PolygonCollider2D>();
    }
    private void Start()
    {
        _polygonCollider2D.enabled = false;
    }
    public void Attack()
    {
        AttackColliderTurnOffOn();
        OnSwordSwing?.Invoke(this, EventArgs.Empty);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.TryGetComponent(out EnemyEntity enemyEntity))
            enemyEntity.TakeDamage(swordDamage);
    }
    private void AttackColliderTurnOffOn()
    {
        AttackColliderTurnOff();
        AttackColliderTurnOn();
    }
    public void AttackColliderTurnOff()
    {
        _polygonCollider2D.enabled = false;
    }
    private void AttackColliderTurnOn()
    {
        _polygonCollider2D.enabled = true;
    }
}
