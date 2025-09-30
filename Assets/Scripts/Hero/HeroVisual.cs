using UnityEngine;

public class HeroVisual : MonoBehaviour
{
    public Hero Hero; 
    private FlashBlink _flashBlink;
    private Animator animator;
    private const string IS_RUNNING = "IsRunning";
    private const string IS_DIE = "IsDie";
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

        animator.SetBool(IS_RUNNING, Hero.IsRunning());
       
    }
    
    private void Hero_OnHeroDeath(object sender, System.EventArgs e)
    {
        animator.SetBool(IS_DIE, true);
        _flashBlink.StopBlinking();
    }

    private void RotateHero()
    {
        Vector3 mousePosition = GameInput.Instance.MousePosition();
        Vector3 heroPosition = Hero.HeroPosition();

        if (mousePosition.x < heroPosition.x)
        {
            _spriteRenderer.flipX = true;
        }
        else
            _spriteRenderer.flipX = false;
    }

    
}
