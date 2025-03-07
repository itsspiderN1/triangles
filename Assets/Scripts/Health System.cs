using UnityEngine;
using UnityEngine.UI;


public class HealthSystem : MonoBehaviour
{
    public int health;
    public int numOfHearts;
    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;
    private Stats stats;

    void Start()
    {
        stats=GetComponent<Stats>();
    }
    void Update()
    {
        health=stats.Health;
        if(health>numOfHearts)
        health=numOfHearts;
        for(int i=0;i<hearts.Length;i++)
        {
            if(i<health)
            hearts[i].sprite=fullHeart;
            else
            hearts[i].sprite=emptyHeart;
            if(i<numOfHearts)
            hearts[i].enabled=true;
            else
            hearts[i].enabled=false;
        }
    }
}
