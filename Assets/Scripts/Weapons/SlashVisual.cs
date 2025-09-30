using UnityEngine;
public class SlashVisual : MonoBehaviour
{
    [SerializeField] private Sword _sword;

    private Animator animator;
    private const string ATTACK = "Attack";
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    private void Start()
    {
        _sword.OnSwordSwing += Sword_OnSwordSwing;
    }
    private void Sword_OnSwordSwing(object sender, System.EventArgs e)
    {
        animator.SetTrigger(ATTACK);
    }
    private void OnDestroy()
    {
        _sword.OnSwordSwing -= Sword_OnSwordSwing;
    }
}

