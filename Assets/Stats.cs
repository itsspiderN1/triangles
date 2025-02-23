using UnityEngine;

public class Stats : MonoBehaviour
{
    private int MaxHealth=3;
    public int Health;

    public bool invincible = false;
    void Start()
    {
        Health=MaxHealth;
    }

   
    void Update()
    {
        if(Health<=0)
        Die();
    }
    void Die()
    {
        Destroy(gameObject);
    }

}
