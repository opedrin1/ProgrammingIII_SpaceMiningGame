using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class PlayerProjectile : MonoBehaviour
{
    [SerializeField] private float speed = 12f;
    [SerializeField] private float lifetime = 3f;
    [SerializeField] private int damage = 1;

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        transform.position += transform.up * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out EnemyHealth enemyHealth))
        {
            enemyHealth.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}