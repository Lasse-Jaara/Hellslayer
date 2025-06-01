using UnityEngine;
using System.Collections.Generic;

public class Rocket : MonoBehaviour
{
    [SerializeField] private float lifetime = 3f;
    [SerializeField] private float turnSpeed = 5f;
    [SerializeField] private float rocketSpeed = 50f;
    [SerializeField] private float explosionRadius = 5f;
    [SerializeField] private int damageValue = 20;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private GameObject explosionEffectPrefab;

    private Transform target;
    private Rigidbody rb;
    private bool hasExploded = false;

    public void AssignTarget(Transform targetTransform)
    {
        target = targetTransform;
    }

    private void Start()
    {
        Invoke(nameof(Explode), lifetime);

        rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = transform.forward * rocketSpeed;
        }

        // Ignore player collision
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            Collider rocketCollider = GetComponent<Collider>();
            if (rocketCollider != null)
            {
                Collider[] playerColliders = player.GetComponentsInChildren<Collider>();
                foreach (Collider playerCol in playerColliders)
                {
                    Physics.IgnoreCollision(rocketCollider, playerCol);
                }
            }
        }

        // Ignore other rockets
        Collider myCollider = GetComponent<Collider>();
        Rocket[] otherRockets = FindObjectsByType<Rocket>(FindObjectsSortMode.None);
        foreach (Rocket otherRocket in otherRockets)
        {
            if (otherRocket != this)
            {
                Collider otherCol = otherRocket.GetComponent<Collider>();
                if (myCollider != null && otherCol != null)
                {
                    Physics.IgnoreCollision(myCollider, otherCol);
                }
            }
        }
    }

    private void FixedUpdate()
    {
        if (target == null || rb == null) return;

        Vector3 direction = (target.position - transform.position).normalized;
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, direction, turnSpeed * Time.fixedDeltaTime, 0f);
        rb.linearVelocity = newDirection * rocketSpeed;
        transform.rotation = Quaternion.LookRotation(newDirection);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player")) return;
        Explode();
    }

    private void Explode()
    {
        if (hasExploded) return;
        hasExploded = true;
        CancelInvoke();

        if (explosionEffectPrefab != null)
        {
            explosionEffectPrefab.transform.localScale = Vector3.one * explosionRadius;
            Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
        }

        // Damage enemies in explosion radius only once
        Collider[] hits = Physics.OverlapSphere(transform.position, explosionRadius, enemyLayer);
        HashSet<EnemyHealth> damagedEnemies = new HashSet<EnemyHealth>();

        foreach (Collider hit in hits)
        {
            EnemyHealth enemyHealth = hit.GetComponentInParent<EnemyHealth>();
            if (enemyHealth != null && damagedEnemies.Add(enemyHealth))
            {
                enemyHealth.TakeDamage(damageValue);
                Debug.Log($"Rocket hit {hit.name}, dealing {damageValue} damage.");
            }
        }

        Destroy(gameObject);
    }
}
