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

    public OrbManagement theOrb;
    public Image currentOrbDisplay;

    [SerializeField] private int maxHealth = 500, maxStamina = 150;
    [SerializeField] private float increaseStaminaPoint = 0.02f;
    [HideInInspector] public int currentHealth;
    [HideInInspector] public float currentStamina;
    [HideInInspector] public bool regenable;

    // [HideInInspector] public string theItem;
    [HideInInspector] public bool readey, win, fail;

    private int playerLevel = 0;

    private GameObject currentWeapon;
    private int currentOrb = 6;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        totalHp.text = "/" + maxHealth.ToString();
        currentHp.text = currentHealth.ToString();

        currentStamina = maxStamina;

        ShowCurrentOrb();
    }

    public int GetCurrentHp()
    {
        return currentHealth;
    }

    public void SetCurrentHp(int newHp)
    {
        currentHealth = newHp;
    }

    public int GetMaxHp()
    {
        return maxHealth;
    }

    public void SetMaxHp(int newHp)
    {
        maxHealth = newHp;
    }

    public int GetMaxStamina()
    {
        return maxStamina;
    }

    public void SetMaxStamina(int newStamina)
    {
        maxStamina = newStamina;
    }

    public GameObject GetCurrentWeapon()
    {
        return currentWeapon;
    }

    public void SetCurrentWeapon(GameObject obj)
    {
        currentWeapon = obj;
    }

    // return orb ปัจจุบันให้ arena ทำการ implement effect
    public int GetCurrentOrb()
    {
        return currentOrb;
    }

    // กระเป๋าใส่ orb ปัจจุบันที่ผู้เล่นเก็บ
    public void SetCurrentOrb(int orbNum)
    {
        currentOrb = orbNum;
    }

    public void ShowCurrentOrb()
    {
        currentOrbDisplay.color = theOrb.GetColor(currentOrb);
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

    // public void GrabbedItem(string grabbedItem)
    // {
    //     theItem = grabbedItem;
    // }

    // Note: ควรทำ script PlayerArmController แยกออกมาเลย
    // Note: ควรเปลี่ยนชื่อเป็น PublishArmStateToMqtt
    public void PublishArmStateToMqtt(string arm, string state)
    {
        if (arm is "Left")
        {
            if (state is "Stop") mqtt.Publish("/al/", "S");
            else if (state is "Free") mqtt.Publish("/al/", "F");
            else if (state is "In") mqtt.Publish("/al/", "I");
        }
        else if (arm is "Right")
        {
            if (state is "Stop") mqtt.Publish("/ar/", "S");
            else if (state is "Free") mqtt.Publish("/ar/", "F");
            else if (state is "In") mqtt.Publish("/ar/", "I");
        }
    }

    // public void CheckArmState(string armState)
    // {
    //     if (armState is "stop") mqtt.Publish("/ar/", "S");
    //     else if (armState is "free") mqtt.Publish("/ar/", "F");
    //     else if (armState is "in") mqtt.Publish("/ar/", "I");
    // }

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

    public void ReduceStamina(int damage)
    {
        if (currentStamina - damage <= 0) currentStamina = 0;
        else currentStamina -= damage;

        staminaBar.UpdateHealth((float)currentStamina / (float)maxStamina);
    }

    // public void CheckStamina()
    // {
    //     if (currentStamina - reduceStaminaPoint <= 0) currentStamina = 0;
    //     else currentStamina -= reduceStaminaPoint;

    //     staminaBar.UpdateHealth((float)currentStamina / (float)maxStamina);
    // }

    private void RegenerateStamina()
    {
        if (currentStamina < maxStamina && regenable) currentStamina += increaseStaminaPoint;
        if (currentStamina > maxStamina) currentStamina = maxStamina;

        staminaBar.UpdateHealth((float)currentStamina / (float)maxStamina);
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
