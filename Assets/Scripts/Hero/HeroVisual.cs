using UnityEngine;

public class HeroVisual : MonoBehaviour
{
    public Hero Hero; 

    private Animator animator;
    private const string IS_RUNNING = "IsRunning";
    private const string IS_DIE = "IsDie";
    SpriteRenderer spriteRenderer;
    
    private void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
       
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
    }

    private void RotateHero()
    {
        Vector3 mousePosition = GameInput.Instance.MousePosition();
        Vector3 heroPosition = Hero.HeroPosition();

        if (mousePosition.x < heroPosition.x)
        {
            spriteRenderer.flipX = true;
        }
        else
            spriteRenderer.flipX = false;
    }

    
}
