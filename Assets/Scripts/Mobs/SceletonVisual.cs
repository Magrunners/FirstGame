using UnityEngine;
[RequireComponent(typeof(Animator))]
public class SceletonVisual : MonoBehaviour
{
    [SerializeField] private EnemyAI _enemyAI;
    [SerializeField] private EnemyEntity _enemyEntity;
    [SerializeField] private GameObject _enemyShadow;

    private Animator _animator;
    private const string IS_ROAMING = "IsRoaming";
    private const string CHASING_SPEED_MULTYPLIER = "ChasingSpeedMultiplier";
    private const string ATTACK = "Attack";
    private const string TAKEDAMAGE = "TakeDamage";
    private const string ISDIE = "IsDie";
    SpriteRenderer _spriteRenderer;
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void Start()
    {
        _enemyAI.OnEnemyAttack += _enemyAI_OnEnemyAttack;
        _enemyEntity.OnTakeDamage += _enemyEntity_OnTakeDamage;
        _enemyEntity.OnDeath += _enemyEntity_OnDeath;       
    }
    private void Update()
    {
        _animator.SetBool(IS_ROAMING, _enemyAI.IsRoaming);
        _animator.SetFloat(CHASING_SPEED_MULTYPLIER, _enemyAI.GetRoamingAnimationSpeed());
    }
    public void TriggerAttackAnimationTurnOn()
    {
        _enemyEntity.PolygonColliderTurnOn();
    }
    public void TriggerAttackAnimationTurnOff()
    {
        _enemyEntity.PolygonColliderTurnOff();
    }
    private void _enemyAI_OnEnemyAttack(object sender, System.EventArgs e)
    {
        _animator.SetTrigger(ATTACK);
    }
    private void _enemyEntity_OnTakeDamage(object sender, System.EventArgs e)
    {
        _animator.SetTrigger(TAKEDAMAGE);
    }
    private void _enemyEntity_OnDeath(object sender, System.EventArgs e)
    {
        _animator.SetBool(ISDIE, true);
        _spriteRenderer.sortingOrder = -1;
        _enemyShadow.SetActive(false);
    }
    private void OnDestroy()
    {
        _enemyAI.OnEnemyAttack -= _enemyAI_OnEnemyAttack;
        _enemyEntity.OnTakeDamage -= _enemyEntity_OnTakeDamage;
        _enemyEntity.OnDeath -= _enemyEntity_OnDeath;
    }
}
