using UnityEngine;
public class FlashBlink : MonoBehaviour
{
    [SerializeField] private float _blinkDuration = 0.2f;
    [SerializeField] private MonoBehaviour _damagableObject;
    [SerializeField] private Material _blinkMaterial;

    private Material _defaultMaterial;
    private float _blinkTimer;
    private SpriteRenderer _spriteRenderer;
    private bool _isBlinking;
    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _defaultMaterial = _spriteRenderer.material;
        _isBlinking = true;                
    }
    private void DamagebleObject_OnFlashBlink(object sender, System.EventArgs e)
    {
        SetBlinkingMaterial();
        _isBlinking = true;
    }
    private void Start()
    {
        if (_damagableObject is Hero)
        {
            (_damagableObject as Hero).OnFlashBlink += DamagebleObject_OnFlashBlink;
        }
    }
    private void Update()
    {
        if(_isBlinking)
        {
            _blinkTimer -= Time.deltaTime;
            if (_blinkTimer <= 0f)
            {
                SetDefaultMaterial();
            }
        }
    }
    private void SetBlinkingMaterial()
    {
        _blinkTimer = _blinkDuration;
        _spriteRenderer.material = _blinkMaterial;
    }
    private void SetDefaultMaterial()
    {
        _spriteRenderer.material = _defaultMaterial;
    }
    public void StopBlinking()
    {
        SetDefaultMaterial();
        _isBlinking = false;
    }
    private void OnDestroy()
    {
        if (_damagableObject is Hero)
        {
            (_damagableObject as Hero).OnFlashBlink -= DamagebleObject_OnFlashBlink;
        }
    }
}
