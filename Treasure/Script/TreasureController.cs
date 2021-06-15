using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TreasureController : MonoBehaviour
{
    public GameObject TreasurePannel;
    public Animator anim;
    public Image TreasureBoxImage;
    public Sprite ClosedTreasureBox;
    public Sprite OpenedTreasureBox;

    public GameObject[] money;
    public Animator moneyAnim;

    public Text MoneyText;

    void Start()
    {
        TreasureBoxImage.sprite = ClosedTreasureBox;
        anim.Play("ScaleUp");
        foreach (GameObject g in money)
        {
            g.SetActive(false);
        }
        TreasurePannel.SetActive(false);
    }

    public void OnTreasureButtonPressed()
    {
        StartCoroutine(TreasureAnimation());
    }

    IEnumerator TreasureAnimation()
    {
        anim.Play("Idle");
        MoneyText.text=saveload.money.ToString();
        TreasureBoxImage.sprite = OpenedTreasureBox;
        foreach (GameObject g in money)
        {
            g.SetActive(true);
        }
        yield return new WaitForSeconds(1);
        TreasureBoxImage.gameObject.SetActive(false);
        
        moneyAnim.Play("money");
        
        yield return new WaitForSeconds(1);
        foreach (GameObject g in money)
        {
            g.SetActive(false);
        }
        //start money increase counter
        //show close button
        MoneyText.text=saveload.money.ToString();
        TreasurePannel.SetActive(false);
    }

}
