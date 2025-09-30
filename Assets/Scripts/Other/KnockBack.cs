using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
public class KnockBack : MonoBehaviour
{
    [SerializeField] private float _knockBackForce = 3f;
    [SerializeField] private float _knockBackovingTimerMax = 0.3f;

    private float _knockBackMovingTimer;
    private Rigidbody2D _rb;
    public bool IsGettingKnockBack { get; private set; }

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        _knockBackMovingTimer -= Time.deltaTime;
        if (_knockBackMovingTimer < 0)
            StopKnockBackMovement();
    }
    public void GetKnockBack(Transform damageSource)
    {
        IsGettingKnockBack = true;
        _knockBackMovingTimer = _knockBackovingTimerMax;
        Vector2 difference = (transform.position - damageSource.position).normalized *_knockBackForce / _rb.mass;
        _rb.AddForce(difference, ForceMode2D.Impulse);
    }
    public void StopKnockBackMovement()
    {
        IsGettingKnockBack = false;
        _rb.linearVelocity = Vector2.zero;
    }
}