using UnityEngine;

[RequireComponent(typeof(Animator))]
public class BlueSlimeVisual : MonoBehaviour
{
    [SerializeField] private EnemyAI enemyAI;
    [SerializeField] private EnemyEntity enemyEntity;
    [SerializeField] private GameObject enemyShadow;

    private Animator _animator;
    private SpriteRenderer _spriteRenderer;

    // Параметры аниматора
    private const string IsRoaming = "IsRoaming";
    private const string IsIdle = "IsIdle";
    private const string TakeDamage = "TakeDamage";
    private const string IsDie = "IsDie";

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        if (enemyEntity != null)
        {
            enemyEntity.OnTakeDamage += _enemyEntity_OnTakeDamage;
            enemyEntity.OnDeath += _enemyEntity_OnDeath;
        }

        if (enemyAI == null)
        {
            Debug.LogError("EnemyAI not assigned on " + gameObject.name);
        }
    }

    private void Update()
    {
        if (enemyAI == null || _animator == null) return;

        // Обновление состояний анимации
        if (HasParameter(IsRoaming))
        {
            _animator.SetBool(IsRoaming, enemyAI.IsRoaming);
        }

        if (HasParameter(IsIdle))
        {
            _animator.SetBool(IsIdle, !enemyAI.IsRoaming);
        }
    }

    // Проверка существования параметра
    private bool HasParameter(string paramName)
    {
        if (_animator == null) return false;

        foreach (AnimatorControllerParameter param in _animator.parameters)
        {
            if (param.name == paramName)
                return true;
        }
        return false;
    }

    // Анимация получения урона
    private void _enemyEntity_OnTakeDamage(object sender, System.EventArgs e)
    {
        if (_animator == null) return;

        if (HasParameter(TakeDamage))
        {
            _animator.SetTrigger(TakeDamage);
        }
    }

    // Анимация смерти
    private void _enemyEntity_OnDeath(object sender, System.EventArgs e)
    {
        if (_animator == null) return;

        if (HasParameter(IsDie))
        {
            _animator.SetBool(IsDie, true);
        }

        if (_spriteRenderer != null)
        {
            _spriteRenderer.sortingOrder = -1;
        }

        if (enemyShadow != null)
        {
            enemyShadow.SetActive(false);
        }
    }

    // Отписка от событий
    private void OnDestroy()
    {
        if (enemyEntity != null)
        {
            enemyEntity.OnTakeDamage -= _enemyEntity_OnTakeDamage;
            enemyEntity.OnDeath -= _enemyEntity_OnDeath;
        }
    }
}