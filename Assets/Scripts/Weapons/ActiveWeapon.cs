using UnityEngine;

public class ActiveWeapon : MonoBehaviour
{
    public Hero Hero;
    public static ActiveWeapon Instance {  get; private set; }

    [SerializeField] private Sword sword;


    private void Awake()
    {
        Instance = this;
    }
    public Sword GetActiveWeapon()
    {
        return sword;
    }

    private void Update()
    {
        if(Hero.IsAlive())
        RotateSword();
    }
    private void RotateSword()
    {
        Vector3 mousePosition = GameInput.Instance.MousePosition();
        Vector3 heroPosition = Hero.HeroPosition();

        if (mousePosition.x < heroPosition.x)
            transform.rotation = Quaternion.Euler(0, 180, 0);
        else
            transform.rotation = Quaternion.Euler(0, 0, 0);
    }



}
