using UnityEngine;

public class ActiveWeapon : MonoBehaviour
{
    public static ActiveWeapon Instance {  get; private set; }
    
    [SerializeField] private Sword _sword;

    public Hero _hero;

    private void Awake()
    {
        Instance = this;
    }
    public Sword GetActiveWeapon()
    {
        return _sword;
    }
    private void Update()
    {
        if(_hero.IsAlive())
        RotateSword();
    }
    private void RotateSword()
    {
        Vector3 mousePosition = GameInput.Instance.MousePosition();
        Vector3 heroPosition = _hero.HeroPosition();

        if (mousePosition.x < heroPosition.x)
            transform.rotation = Quaternion.Euler(0, 180, 0);
        else
            transform.rotation = Quaternion.Euler(0, 0, 0);
    }



}
