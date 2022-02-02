using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public MqttProtocol mqtt;
    public GameObject floatingDamagePrefab;
    public GameObject floatingDamageParent;

    public HealthBar healthBar, staminaBar;

    public Text totalHp, currentHp;

    [SerializeField] private int maxHealth = 500, maxStamina = 100, reduceStaminaPoint = 20;
    [SerializeField] private float increaseStaminaPoint = 0.02f;
    [HideInInspector] public int currentHealth;
    [HideInInspector] public float currentStamina;
    [HideInInspector] public bool regenable;

    [HideInInspector] public string theItem;
    [HideInInspector] public bool readey, win, fail;

    private int playerLevel;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        totalHp.text = "/" + maxHealth.ToString();
        currentHp.text = currentHealth.ToString();

        currentStamina = maxStamina;
    }

    public void levelUp()
    {
        playerLevel++;
    }

    public int getLevel()
    {
        return playerLevel;
    }

    public void ResetPlayerStat()
    {
        currentHealth = maxHealth;
        currentStamina = maxStamina;
    }

    public void GrabbedItem(string grabbedItem)
    {
        theItem = grabbedItem;
    }

    // Note: ควรทำ script PlayerArmController แยกออกมาเลย
    // Note: ควรเปลี่ยนชื่อเป็น PublishArmStateToMqtt
    public void CheckArmState(string armState)
    {
        if (armState is "stop") mqtt.Publish("/ar/", "S");
        else if (armState is "free") mqtt.Publish("/ar/", "F");
        else if (armState is "in") mqtt.Publish("/ar/", "I");
    }

    public void TakeDamage(int damage)
    {
        // Debug.Log("damage: " + damage);

        var parentTransform = floatingDamageParent.transform;

        if (floatingDamagePrefab) ShowFloatingDamage(damage, parentTransform);

        // เช็คว่า HP เหลือ 0 หรือยัง ถ้ายังก็ลด HP ต่อ
        if (currentHealth == 0) return;
        CheckHealth(damage);
    }

    private void ShowFloatingDamage(int damage, Transform parentTransform)
    {
        // Can be optimized by "Object pooling"
        var text = Instantiate(floatingDamagePrefab, parentTransform.position, Quaternion.identity, parentTransform);
        text.GetComponent<TextMesh>().text = damage.ToString();
    }

    private void CheckHealth(int damage)
    {
        if (currentHealth - damage <= 0) currentHealth = 0;
        else currentHealth -= damage;

        currentHp.text = currentHealth.ToString();
        healthBar.UpdateHealth((float)currentHealth / (float)maxHealth);
    }

    public void CheckStamina()
    {
        if (currentStamina - reduceStaminaPoint <= 0) currentStamina = 0;
        else currentStamina -= reduceStaminaPoint;

        staminaBar.UpdateHealth((float)currentStamina / (float)maxStamina);
    }

    private void RegenerateStamina()
    {
        if (currentStamina < maxStamina && regenable) currentStamina += increaseStaminaPoint;
        if (currentStamina > maxStamina) currentStamina = maxStamina;

        staminaBar.UpdateHealth((float)currentStamina / (float)maxStamina);
    }

    public int getHealth()
    {
        return currentHealth;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        RegenerateStamina();

        // Debug.Log(currentStamina);
    }

    public IEnumerator DelayRegenerateStamina()
    {
        yield return new WaitForSeconds(5);
        regenable = true;
    }
}
