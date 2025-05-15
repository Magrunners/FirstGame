using JetBrains.Annotations;
using UnityEngine;

public class Hero : MonoBehaviour
{
   
    [SerializeField] public float movingSpeed = 5f;

    private Rigidbody2D rb;

    private float heroMinMovingSpeed = 0.1f;
    private bool isRunning = false;
    Vector2 inputVector;

    //Вариант с singletone
    //public static Hero Instance { get; private set; }

    private void Awake()
    {   
        //Инициализация при singletone
        //Instance = this
        rb = GetComponent<Rigidbody2D>();        
    }
    private void Start()
    {
        GameInput.Instance.OnHeroAttack += Hero_OnHeroAttack;
    }

    private void Hero_OnHeroAttack(object sender, System.EventArgs e)
    {
        ActiveWeapon.Instance.GetActiveWeapon().Attack();
    }

    private void Update()
    {
        inputVector = GameInput.Instance.GetMovementVector();
    }

    private void FixedUpdate()
    {
        HandleMovement();
        
    }


    private void HandleMovement()
    {
        
        rb.MovePosition(rb.position + inputVector* (movingSpeed* Time.fixedDeltaTime));
        if(Mathf.Abs(inputVector.x) > heroMinMovingSpeed || Mathf.Abs(inputVector.y) > heroMinMovingSpeed)
            isRunning = true;
        else
            isRunning = false;

    }

    public bool IsRunning()
    {
        return isRunning;
    }
    public Vector3 HeroPosition()
    {
        Vector3 heroPosition = Camera.main.WorldToScreenPoint(transform.position);

        return heroPosition;
    }
}



