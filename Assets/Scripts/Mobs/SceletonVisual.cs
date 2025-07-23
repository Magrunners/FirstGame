using UnityEngine;

public class SceletonVisual : MonoBehaviour
{
    [SerializeField] private EnemyAI _enemyAI;

    private Animator _animator;
    private const string IS_ROAMING = "IsRoaming";
    private const string CHASING_SPEED_MULTYPLIER = "ChasingSpeedMultiplier";


    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        _animator.SetBool(IS_ROAMING, _enemyAI.IsRoaming);
        _animator.SetFloat(CHASING_SPEED_MULTYPLIER, _enemyAI.GetRoamingAnimationSpeed());
    }


}
