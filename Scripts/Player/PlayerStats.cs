using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerStats : MonoBehaviour
{
    public int MaxHealth = 100;
    public int Health=100;
    public GameObject DamageBloodUI;
    public GameObject FullDamageBloodUI;
    public Animator anim;
    public GameObject ControlUI;
    Transform StartingPosition;
    public GameObject GameOverPannel;
    public Image HealthBar;

    void Start()
    {
        StartingPosition = GameObject.FindGameObjectWithTag("PlayerSpawnPoint").transform;
        HealthBar.fillAmount = (float)Health / (float)MaxHealth;
        DamageBloodUI.SetActive(false);
        FullDamageBloodUI.SetActive(false);
        OnContinueButtonPressed();
    }
   
    public void TakeDamage(int dam,float impact, string ranDeathAnim)
    {
        Health -= dam;
        HealthBar.fillAmount = (float)Health / (float)MaxHealth;
        var rb = GetComponent<Rigidbody>();
        rb.AddForce(0, 0, impact, ForceMode.Impulse);
        StartCoroutine(DamageImpact());
        if (Health < 0)
        {
            //Dead
            FullDamageBloodUI.SetActive(true);
            //gameObject.GetComponent<CapsuleCollider>().enabled = false;
            DisableAllComponents();
            anim.enabled = true;
            ControlUI.SetActive(false);
            gameObject.tag = "Untagged";
            anim.Play(ranDeathAnim);
            GameOverPannel.SetActive(true);
        }
        
    }

    IEnumerator DamageImpact()
    {
       
        DamageBloodUI.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        
        DamageBloodUI.SetActive(false);
    }


    public void OnContinueButtonPressed()
    {
        InitializePlayerFromStarting();
        FullDamageBloodUI.SetActive(false);
        //gameObject.GetComponent<CapsuleCollider>().enabled = false;
        EnableAllComponents();
        GameOverPannel.SetActive(false);
        anim.enabled = false;
        ControlUI.SetActive(true);
        gameObject.tag = "Player";
        Health = 100;
    }

    void DisableAllComponents()
    {
        gameObject.GetComponent<BasicBehaviour>().enabled = false;
        gameObject.GetComponent<Footsteps>().enabled = false;
        gameObject.GetComponent<CoverBehaviour>().enabled = false;
        gameObject.GetComponent<ShootBehaviour>().enabled = false;
        gameObject.GetComponent<MoveBehaviour>().enabled = false;
        gameObject.GetComponent<AimBehaviour>().enabled = false;
        gameObject.GetComponent<Animator>().enabled = false;
    }

    void EnableAllComponents()
    {
        gameObject.GetComponent<BasicBehaviour>().enabled = true;
        gameObject.GetComponent<Footsteps>().enabled = true;
        gameObject.GetComponent<CoverBehaviour>().enabled = true;
        gameObject.GetComponent<ShootBehaviour>().enabled = true;
        gameObject.GetComponent<MoveBehaviour>().enabled = true;
        gameObject.GetComponent<AimBehaviour>().enabled = true;
        gameObject.GetComponent<Animator>().enabled = true;
    }

    void InitializePlayerFromStarting()
    {
        gameObject.transform.position = StartingPosition.transform.position;
        gameObject.transform.rotation = StartingPosition.transform.rotation;
    }

    public void OnRestartButtonPressed()
    {
        SceneManager.LoadScene(0);
    }
}