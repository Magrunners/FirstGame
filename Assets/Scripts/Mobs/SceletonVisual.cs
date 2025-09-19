using UnityEngine;

[RequireComponent(typeof(Animator))]

public class SceletonVisual : MonoBehaviour
{
    [SerializeField] private EnemyAI _enemyAI;
    [SerializeField] private EnemyEntity _enemyEntity;

    private Animator _animator;
    private const string IS_ROAMING = "IsRoaming";
    private const string CHASING_SPEED_MULTYPLIER = "ChasingSpeedMultiplier";
    private const string ATTACK = "Attack";



    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        _enemyAI.OnEnemyAttack += _enemyAI_OnEnemyAttack;
    }

    private void OnDestroy()
    {
        _enemyAI.OnEnemyAttack -= _enemyAI_OnEnemyAttack;   
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
}
