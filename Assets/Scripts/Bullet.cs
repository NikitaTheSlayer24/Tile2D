using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float bulletSpeed = 20f;

    private Rigidbody2D _rb;
    private PlayerMovement _player;
    private GameSession _gameSession;

    private float _xSpeed;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _player = FindObjectOfType<PlayerMovement>();
        _gameSession = FindObjectOfType<GameSession>();
        _xSpeed = _player.transform.localScale.x * bulletSpeed;
    }

    private void Update()
    {
        _rb.velocity = new Vector2(_xSpeed, 0f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Destroy(collision.gameObject);
            _gameSession.AddToScore(50);
        }

        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }
}
