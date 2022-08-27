using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    public Image[] starSilver, starGold;
    public Text starText;
    AudioSource aSource;
    public AudioClip ding, explosion, pop;

    int countStars;
    int lives, health, coins;
    bool gold = false;

    private void Start()
    {
        aSource = GetComponent<AudioSource>();

        lives = PlayerInfos.pi.playerLives;
        health = PlayerInfos.pi.playerHealth;
        coins = PlayerInfos.pi.nbCoin;

        if(lives == 3 && health == 3 && coins == 30)
        {
            gold = true;
            starText.text = "PERFECT !!";
        }

        else if(lives == 3 && health == 3 || lives == 3 && health == 2)
        {
            countStars = 3;
            starText.text = "Great !!";
        }

        else if(lives == 3 && health == 1 || lives == 2 && health == 3)
        {
            countStars = 2;
            starText.text = "Nice one !";
        }

        else if(lives == 2 && health == 2 || lives == 2 && health == 1)
        {
            countStars = 1;
            starText.text = "You can do better !";
        }

        else
        {
            countStars = 0;
            starText.text = "It will be better next time...";
        }

        StartCoroutine(SetStars());
    }

    IEnumerator SetStars()
    {
        yield return new WaitForSeconds(0.1f);

        if(gold)
        {
            foreach(Image star in starSilver)
            {
                star.enabled = true;
                aSource.PlayOneShot(ding, 0.5f);
                aSource.pitch += 0.1f;
                iTween.PunchScale(star.gameObject, Vector3.one * 1f, 0.3f);
                yield return new WaitForSeconds(0.12f);
            }
            yield return new WaitForSeconds(0.5f);

            aSource.PlayOneShot(explosion, 0.1f);
            foreach(Image star in starGold)
            {
                star.enabled = true;
                iTween.PunchScale(star.gameObject, Vector3.one * 1f, 0.2f);
            }
        }

        else
            for(int i = 0 ; i < countStars; i++)
            {
                starSilver[i].enabled = true;
                aSource.PlayOneShot(ding, 0.5f);
                aSource.pitch += 0.1f;
                iTween.PunchScale(starSilver[i].gameObject, Vector3.one * 1f, 0.3f);
                yield return new WaitForSeconds(0.12f);
            }

        yield return new WaitForSeconds(0.2f);
        iTween.ScaleTo(starText.gameObject, Vector3.one, 0.05f);
        yield return new WaitForSeconds(0.06f);
        iTween.PunchScale(starText.gameObject, Vector3.one * 0.3f, 0.4f);
        aSource.PlayOneShot(pop, 0.1f);
    }

}
