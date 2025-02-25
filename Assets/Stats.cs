using UnityEngine;
using TMPro;
public class Stats : MonoBehaviour
{
    private int MaxHealth=3;
    public int Health;

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
    }
    void Die()
    {
        Destroy(gameObject);
    }

}
