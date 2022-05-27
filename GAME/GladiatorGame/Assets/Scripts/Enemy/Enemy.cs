using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject floatingDamagePrefab;
    public GameObject floatingDamageParent;

    public HealthBar healthBar;
    private int maxHealth = 1000; //Defualt: 1000
    private int currentHealth;

    private string armState;

    // Start is called before the first frame update
    void Awake()
    {
        Rigidbody enemyBody = GetComponentInChildren<Rigidbody>();
        enemyBody.collisionDetectionMode = CollisionDetectionMode.Continuous;

        healthBar = GetComponentInChildren<HealthBar>();

        currentHealth = maxHealth;
    }

    private void UpdateCurrentHp()
    {
        healthBar.UpdateHealth((float)currentHealth / (float)maxHealth);
    }

    public int GetCurrentHp()
    {
        return currentHealth;
    }

    public int GetMaxHp()
    {
        return maxHealth;
    }

    public void SetMaxHp(int newMax)
    {
        maxHealth = newMax;
        currentHealth = maxHealth;
        UpdateCurrentHp();
    }

    public void ResetEnemyStat()
    {
        currentHealth = maxHealth;
        UpdateCurrentHp();
    }

    public void TakeDamage(int damage)
    {
        var parentTransform = floatingDamageParent.transform;

        if (floatingDamagePrefab) ShowFloatingDamage(damage, parentTransform);

        // Calculate current HP
        CheckHealth(damage);
    }

    // private void CheckArmState(string armState)
    // {
    //     if (armState is "stop") mqtt.Publish("/ar/", "S");
    //     else if (armState is "free") mqtt.Publish("/ar/", "F");
    // }

    // private void OnTriggerEnter(Collider other)
    // {
    //     // If it isn't a weapon
    //     if (!other.gameObject.CompareTag("Weapon")) return;

    //     // Debug.Log(other.gameObject.name);

    //     Rigidbody weaponBody = other.transform.parent.gameObject.GetComponent<Rigidbody>();

    //     TakeDamageO(weaponBody);

    //     CheckArmState("stop");
    // }

    // private void OnTriggerExit(Collider other)
    // {
    //     CheckArmState("free");
    // }

    // private void TakeDamageO(Rigidbody weaponBody)
    // {
    //     float speed = weaponBody.velocity.magnitude;

    //     float force = 0.5f * weaponBody.mass * Mathf.Pow(speed, 2);

    //     int damage = 0;

    //     // Calculate taken damage based on weapon force
    //     // if (force >= 0.03) damage = (int)(Random.Range(34, 50) * CheckCriticalHit());
    //     // else if (force >= 0.0001) damage = (int)(Random.Range(1, 5));
    //     // else damage = (int)(Random.Range(65, 100) * CheckCriticalHit());

    //     damage = (int)(Random.Range(34, 50) * CheckCriticalHit()); // Test

    //     // Debug.Log("force: " + force + ", damage: " + damage);
    //     // Debug.Log("damage: " + damage);

    //     var parentTransform = floatingDamageParent.transform;

    //     if (floatingDamagePrefab) ShowFloatingDamage(damage, parentTransform);

    //     // Calculate current HP
    //     CheckHealth(damage);
    // }

    // private int CheckCriticalHit()
    // {
    //     if (Random.Range(0, 20) == 2)
    //     {
    //         Debug.Log("CRITICAL !!!!!!");
    //         return 2;
    //     }
    //     return 1;
    // }

    private void ShowFloatingDamage(int damage, Transform parentTransform)
    {
        // Can be optimized by "Object pooling"
        var text = Instantiate(floatingDamagePrefab, parentTransform.position, Quaternion.identity, parentTransform);
        text.GetComponent<TextMesh>().text = damage.ToString();
    }

    private void CheckHealth(int damage)
    {
        currentHealth -= damage;
        UpdateCurrentHp();

        // Debug.Log("currentHealth: " + currentHealth);
    }

    // Update is called once per frame
    void FixedUpdate()
    {

    }
}
