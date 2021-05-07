using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TreasureController : MonoBehaviour
{
    public Animator anim;
    public Image TreasureBoxImage;
    public Sprite ClosedTreasureBox;
    public Sprite OpenedTreasureBox;
    public Sprite Test;

    public GameObject[] money;
    public Animator moneyAnim;

    void OnEnable()
    {
        //TreasureBoxImage.sprite = OpenedTreasureBox;
        anim.Play("ScaleUp");
        foreach (GameObject g in money)
        {
            g.SetActive(false);
        }
    }

    void Start()
    {
        TreasureBoxImage.sprite = Test;
    }

    public void OnTreasureButtonPressed()
    {
        StartCoroutine(TreasureAnimation());
    }

    IEnumerator TreasureAnimation()
    {
        anim.Play("Idle");
        TreasureBoxImage.sprite = ClosedTreasureBox;
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


    }

}
