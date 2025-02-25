using UnityEngine;
using TMPro;
public class Stats : MonoBehaviour
{
    [Header("Damage")]

    public int bulletSpeedAmplifier;
    public int bulletDamageAmplifier;
    public int bulletRateAmplifier;

    [Header("HP")]

    public int Health;
    private int MaxHealth=3;
    public bool invincible = false;

    public TMP_Text moneyText;

    public int money;
    void Start()
    {
        Health=MaxHealth;
    }

   
    void Update()
    {
        if(Health<=0)
        Die();
        moneyText.text=money.ToString();
        PlayerPrefs.SetInt("Money",money);
    }
    void Die()
    {
        Destroy(gameObject);
    }

}
