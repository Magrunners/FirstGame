using UnityEngine;
[RequireComponent(typeof(Animator))]
public class SceletonVisual : MonoBehaviour
{
    [SerializeField] private EnemyAI enemyAI;
    [SerializeField] private EnemyEntity enemyEntity;
    [SerializeField] private GameObject enemyShadow;

    private Animator _animator;
    private const string IsRoaming = "IsRoaming";
    private const string ChasingSpeedMultiplier = "ChasingSpeedMultiplier";
    private const string Attack = "Attack";
    private const string TakeDamage = "TakeDamage";
    private const string IsDie = "IsDie";
    private const string IsHeap = "IsHeap";
    private const string Rising = "Rising";
    private const string EndRising = "EndRising";

    SpriteRenderer _spriteRenderer;
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void Start()
    {
        enemyAI.OnEnemyAttack += _enemyAI_OnEnemyAttack;
        enemyEntity.OnTakeDamage += _enemyEntity_OnTakeDamage;
        enemyEntity.OnDeath += _enemyEntity_OnDeath;
        enemyAI.OnStartingRising += _enemyAI_OnStartingRising;

        if (enemyAI.IsHeapEnemy() == true)
        {
            _animator.SetBool(IsHeap, true);
        }
    }
    private void Update()
    {
        if (!_animator.GetBool(IsHeap))
        {
            _animator.SetBool(IsRoaming, enemyAI.IsRoaming);
            _animator.SetFloat(ChasingSpeedMultiplier, enemyAI.GetRoamingAnimationSpeed());
        }
        
    }

    private void _enemyAI_OnStartingRising(object sender, System.EventArgs e)
    {
        _animator.SetBool(IsHeap, false);
        _animator.SetTrigger(Rising);
    }
    public void OnRisingAnimationComplete()
    {
        _animator.SetTrigger(EndRising);
        enemyAI.EndRising();
        enemyEntity.EnableBoxCollider();
        enemyShadow.SetActive(true);
    }
    // Включает коллайдер атаки в начале анимации атаки
    public void TriggerAttackAnimationTurnOn()
    {
        enemyEntity.PolygonColliderTurnOn();
    }
    // Выключает коллайдер атаки в конце анимации атаки
    public void TriggerAttackAnimationTurnOff()
    {
        enemyEntity.PolygonColliderTurnOff();
    }
    // Анимация атаки врагом
    private void _enemyAI_OnEnemyAttack(object sender, System.EventArgs e)
    {
        _animator.SetTrigger(Attack);
    }
    // Анимация получения урона врагом
    private void _enemyEntity_OnTakeDamage(object sender, System.EventArgs e)
    {
        _animator.SetTrigger(TakeDamage);
    }
    // Анимация смерти врага
    private void _enemyEntity_OnDeath(object sender, System.EventArgs e)
    {
        _animator.SetBool(IsDie, true);
        _spriteRenderer.sortingOrder = -1;
        enemyShadow.SetActive(false);
    }
    // Отписка от событий при уничтожении объекта
    private void OnDestroy()
    {
        enemyAI.OnEnemyAttack -= _enemyAI_OnEnemyAttack;
        enemyEntity.OnTakeDamage -= _enemyEntity_OnTakeDamage;
        enemyAI.OnStartingRising -= _enemyAI_OnStartingRising;
        enemyEntity.OnDeath -= _enemyEntity_OnDeath;
    }
}
