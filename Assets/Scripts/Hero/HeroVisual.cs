using UnityEngine;

public class HeroVisual : MonoBehaviour
{
    private static readonly int Die = Animator.StringToHash("IsDie");
    private static readonly int Running = Animator.StringToHash("IsRunning");

    public Hero Hero; 
    private FlashBlink _flashBlink;
    private Animator animator;
    private const string IsRunning = "IsRunning";
    private const string IsDie = "IsDie";
    private SpriteRenderer _spriteRenderer;
    
    private void Awake()
    {
        animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
       _flashBlink = GetComponent<FlashBlink>();
    }

    private void Start()
    {
        Hero.OnHeroDeath += Hero_OnHeroDeath;
    }
    private void Update()
    {
        if (Hero.IsAlive())
            RotateHero();

        animator.SetBool(Running, Hero.IsRunning());       
    }
    // Подписка на событие смерти героя
    private void Hero_OnHeroDeath(object sender, System.EventArgs e)
    {
        animator.SetBool(Die, true);
        _flashBlink.StopBlinking();
    }
    // Поворот героя в сторону курсора мыши
    private void RotateHero()
    {
        Vector3 mousePosition = GameInput.Instance.MousePosition();
        Vector3 heroPosition = Hero.HeroPosition();

        _spriteRenderer.flipX = mousePosition.x < heroPosition.x;
    }

    private void OnDestroy()
    {
        Hero.OnHeroDeath -= Hero_OnHeroDeath;
    }
}
