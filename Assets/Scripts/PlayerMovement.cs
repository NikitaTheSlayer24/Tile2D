using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float runSpeed = 10f;
    [SerializeField] private float climbSpeed = 5f;
    [SerializeField] private float jumpPower = 10f;
    [SerializeField] private Vector2 deathKIck = new Vector2(0f, 20f);
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform gun;

    private Rigidbody2D _rb;
    private Animator _animator;
    private Vector2 _moveInput;
    private CapsuleCollider2D _capsuleCollider;
    private BoxCollider2D _boxCollider;
    private GameSession _gameSession;

    private bool _playerHasHorizontalSpeed;
    private bool _isAlive = true;
    private float _gravityScaleAtStart;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _capsuleCollider = GetComponent<CapsuleCollider2D>();
        _boxCollider = GetComponent<BoxCollider2D>();
        _gravityScaleAtStart = _rb.gravityScale;
        _gameSession = FindObjectOfType<GameSession>();
    }

    private void Update()
    {
        if (_isAlive)
        {
            Run();
            FlipSprite();
            ClimbLadder();
            Die();
        }        
    }

    private void OnFire(InputValue value)
    {
        if (_isAlive)
        {
            Instantiate(bullet, gun.position, transform.rotation);
        }
    }

    private void OnMove(InputValue value)
    {
        if (_isAlive)
        {
            _moveInput = value.Get<Vector2>();
        }
    }

    private void OnJump(InputValue value)
    {
        if (value.isPressed && _boxCollider.IsTouchingLayers(LayerMask.GetMask("Ground")) && _isAlive)
        {
            _rb.velocity += new Vector2(0f, jumpPower);
        }
    }
    
    private bool HasPlayerSpeed(float direction)
    {
        return _playerHasHorizontalSpeed = Mathf.Abs(direction) > Mathf.Epsilon;
    }

    private void Run()
    {
        Vector2 playerVelocity = new Vector2(_moveInput.x * runSpeed, _rb.velocity.y);
        _rb.velocity = playerVelocity;
        _animator.SetBool("isRunning", _playerHasHorizontalSpeed);
    }

    private void FlipSprite()
    {
        if (HasPlayerSpeed(_rb.velocity.x))
        {
            transform.localScale = new Vector2(Mathf.Sign(_rb.velocity.x), 1f);
        }
    }

    private void ClimbLadder()
    {
        if (_boxCollider.IsTouchingLayers(LayerMask.GetMask("Climbing")))
        {
            Vector2 climbVelocity = new Vector2(_rb.velocity.x, _moveInput.y * climbSpeed);
            _rb.velocity = climbVelocity;
            _rb.gravityScale = 0;
            _animator.SetBool("isClimbing", HasPlayerSpeed(_rb.velocity.y));
        }
        else
        {
            _rb.gravityScale = _gravityScaleAtStart;
        }
    }

    private void Die()
    {
        if (_capsuleCollider.IsTouchingLayers(LayerMask.GetMask("Enemy", "Hazards")))
        {
            _isAlive = false;
            _animator.SetTrigger("Dying");
            _rb.velocity = deathKIck;
            _gameSession.ProcessPlayerDeath();
        }
    }
}
