using UnityEngine;
using UnityEngine.UI;

public class PlayerInfos : MonoBehaviour
{
    public static PlayerInfos pi;

    public int playerHealth = 3;
    public int playerLives = 3;
    public int nbPoussin = 0;
    public int nbCoin = 0;
    public bool gotKey;

    public Image[] hearts, poussins, poussinsOFF;
    public Image keyOn, keyOff;

    public Text[] messages;

    public Text coinText, scoreFinalText, nbLives;



    private void Awake() => pi = this;



    public void SetHeatlth(int val)
    {
        playerHealth += val;

        if(playerHealth > 3)
            playerHealth = 3;
        
        if(playerHealth <= 0)
            playerHealth = 0;

        SetHealthBar();
    }


    public void LoseLives()
    {
        playerLives -= 1;

        for(int i = 0; i < playerLives + 1; i ++)
            nbLives.text = i.ToString();

        if(playerLives > 3)
            playerLives = 3;
        
        if(playerLives <= 0)
            playerLives = 0;
    }



    public void GetCoin()
    {
        nbCoin++;
        coinText.text = nbCoin.ToString();
    }
    


    public void GetKey()
    {
        gotKey = true;
        keyOn.enabled = true;
        keyOff.enabled = false;
        iTween.PunchScale(keyOn.gameObject, new Vector2(.3f, .3f), .5f);
    }



    public void SetHealthBar()
    {
        //On vide la bar de vie
        foreach(Image img in hearts)
            img.enabled =false;

        //On met le bon nombre de coeurs
        for(int i = 0 ; i < playerHealth; i++)
            hearts[i].enabled = true;
    }



    public void SetPoussinBar()
    {
        poussins[nbPoussin].enabled = true;
        poussinsOFF[nbPoussin].enabled = false;
        iTween.PunchScale(poussins[nbPoussin].gameObject, new Vector2(.3f, .3f), .5f);
    }
}
