using UnityEngine;

public class HeroVisual : MonoBehaviour
{
    public Hero Hero; 

    private Animator animator;
    private const string IS_RUNNING = "IsRunning";
    SpriteRenderer spriteRenderer;
    
    private void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
       
    }

    private void Update()
    {
        animator.SetBool(IS_RUNNING, Hero.IsRunning());
        RotateHero();
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
