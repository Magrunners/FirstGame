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
    private const string IS_HEAP = "IsHeap";
    private const string RISING = "Rising";
    private const string END_RISING = "EndRising";

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
        _enemyAI.OnStartingRising += _enemyAI_OnStartingRising;

        if (_enemyAI.IsHeapEnemy() == true)
        {
            _animator.SetBool(IS_HEAP, true);
        }
    }
    private void Update()
    {
        if (!_animator.GetBool(IS_HEAP))
        {
            _animator.SetBool(IS_ROAMING, _enemyAI.IsRoaming);
            _animator.SetFloat(CHASING_SPEED_MULTYPLIER, _enemyAI.GetRoamingAnimationSpeed());
        }
        
    }

    private void _enemyAI_OnStartingRising(object sender, System.EventArgs e)
    {
        _animator.SetBool(IS_HEAP, false);
        _animator.SetTrigger(RISING);
    }
    public void OnRisingAnimationComplete()
    {
        _animator.SetTrigger(END_RISING);
        _enemyAI.EndRising();
        _enemyEntity.EnableBoxCollider();
        _enemyShadow.SetActive(true);
    }
    // Включает коллайдер атаки в начале анимации атаки
    public void TriggerAttackAnimationTurnOn()
    {
        _enemyEntity.PolygonColliderTurnOn();
    }
    // Выключает коллайдер атаки в конце анимации атаки
    public void TriggerAttackAnimationTurnOff()
    {
        _enemyEntity.PolygonColliderTurnOff();
    }
    // Анимация атаки врагом
    private void _enemyAI_OnEnemyAttack(object sender, System.EventArgs e)
    {
        _animator.SetTrigger(ATTACK);
    }
    // Анимация получения урона врагом
    private void _enemyEntity_OnTakeDamage(object sender, System.EventArgs e)
    {
        _animator.SetTrigger(TAKEDAMAGE);
    }
    // Анимация смерти врага
    private void _enemyEntity_OnDeath(object sender, System.EventArgs e)
    {
        _animator.SetBool(ISDIE, true);
        _spriteRenderer.sortingOrder = -1;
        _enemyShadow.SetActive(false);
    }
    // Отписка от событий при уничтожении объекта
    private void OnDestroy()
    {
        _enemyAI.OnEnemyAttack -= _enemyAI_OnEnemyAttack;
        _enemyEntity.OnTakeDamage -= _enemyEntity_OnTakeDamage;
        _enemyAI.OnStartingRising -= _enemyAI_OnStartingRising;
        _enemyEntity.OnDeath -= _enemyEntity_OnDeath;
    }
}
