using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public TMP_Text moneyText;
    public int money;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        money=PlayerPrefs.GetInt("Money");
        moneyText.text=money.ToString();
    }
}
