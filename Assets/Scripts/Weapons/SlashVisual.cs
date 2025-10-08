using UnityEngine;
public class SlashVisual : MonoBehaviour
{
    [SerializeField] private Sword sword;

    private Animator _animator;
    private const string ATTACK = "Attack";
    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }
    private void Start()
    {
        sword.OnSwordSwing += Sword_OnSwordSwing;
    }
    private void Sword_OnSwordSwing(object sender, System.EventArgs e)
    {
        _animator.SetTrigger(ATTACK);
    }
    private void OnDestroy()
    {
        sword.OnSwordSwing -= Sword_OnSwordSwing;
    }
}

