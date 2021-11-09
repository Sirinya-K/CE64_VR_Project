using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public MqttProtocol mqtt;

    public GameObject floatingDamagePrefab;
    public GameObject floatingDamageParent;

    public HealthBar healthBar;
    [SerializeField] private int maxHealth = 1000;
    private int currentHealth;

    private string armState;

    // Start is called before the first frame update
    void Start()
    {
        Rigidbody enemyBody = GetComponent<Rigidbody>();
        enemyBody.collisionDetectionMode = CollisionDetectionMode.Continuous;

        currentHealth = maxHealth;
    }

    private void CheckArmState(string armState)
    {
        if (armState is "stop") mqtt.Publish("/un/out/", "ar:S");
        else if (armState is "free") mqtt.Publish("/un/out/", "ar:F");
    }

    private void OnTriggerEnter(Collider other)
    {
        // If it isn't a weapon
        if (!other.gameObject.CompareTag("Weapon")) return;

        // Debug.Log(other.gameObject.name);

        Rigidbody weaponBody = other.transform.parent.gameObject.GetComponent<Rigidbody>();

        TakeDamage(weaponBody);

        CheckArmState("stop");
    }

    private void OnTriggerExit(Collider other)
    {
        CheckArmState("free");
    }

    private void TakeDamage(Rigidbody weaponBody)
    {
        float speed = weaponBody.velocity.magnitude;

        float force = 0.5f * weaponBody.mass * Mathf.Pow(speed, 2);

        int damage = 0;

        // Calculate taken damage based on weapon force
        if (force >= 0.03) damage = (int)(Random.Range(34, 50) * CheckCriticalHit());
        else if (force >= 0.0001) damage = (int)(Random.Range(1, 5));
        else damage = (int)(Random.Range(65, 100) * CheckCriticalHit());

        // Debug.Log("force: " + force + ", damage: " + damage);

        var parentTransform = floatingDamageParent.transform;

        if (floatingDamagePrefab) ShowFloatingDamage(damage, parentTransform);

        // Calculate current HP
        CheckHealth(damage);
    }

    private int CheckCriticalHit()
    {
        if (Random.Range(0, 20) == 2)
        {
            Debug.Log("CRITICAL !!!!!!");
            return 2;
        }
        return 1;
    }

    private void ShowFloatingDamage(int damage, Transform parentTransform)
    {
        // Can be optimized by "Object pooling"
        var text = Instantiate(floatingDamagePrefab, parentTransform.position, Quaternion.identity, parentTransform);
        text.GetComponent<TextMesh>().text = damage.ToString();
    }

    private void CheckHealth(int damage)
    {
        currentHealth -= damage;
        healthBar.UpdateHealth((float)currentHealth / (float)maxHealth);
    }

    // Update is called once per frame
    void FixedUpdate()
    {

    }
}
