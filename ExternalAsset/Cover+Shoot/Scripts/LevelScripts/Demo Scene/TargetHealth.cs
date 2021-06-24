using UnityEngine;

// This class is created for the example scene. There is no support for this script.
public class TargetHealth : HealthManager
{
	public bool boss;
	public AudioClip toggleSound;

	private Vector3 targetRotation;
	private float health, totalHealth = 150;
	private RectTransform healthBar;
	private float originalBarScale;

	void Awake ()
	{
		//targetRotation = this.transform.localEulerAngles;
		//targetRotation.x = -90;
		if (boss)
		{
			//healthBar = this.transform.Find("Health/Bar").GetComponent<RectTransform>();
			//healthBar.parent.gameObject.SetActive(false);
			//originalBarScale = healthBar.sizeDelta.x;
		}
		dead = true;
		health = totalHealth;
        Revive();
	}

    public void SetHealth(float temphealth)
    {
        totalHealth=temphealth;
        health = totalHealth;
    }

	void Update ()
	{
		//this.transform.localRotation = Quaternion.Slerp(this.transform.localRotation, Quaternion.Euler(targetRotation), 10 * Time.deltaTime);
	}

	public bool IsDead { get { return dead; } }

    public void TakeDamageMy(float nu)
    {
        health -= nu;
        
        if (health <= 0)
        {
            Kill();
        }
    }

	public override void TakeDamage(Vector3 location, Vector3 direction, float damage, Collider bodyPart=null, GameObject origin=null)
	{
        
		if (boss)
		{
			health -= damage;
			UpdateHealthBar();
			if (health <= 0)
			{
				Kill();
			}
		}
		else if ( !dead)
		{
            print("tets");
			Kill();
		}
	}

	public void Kill()
	{
		if(boss)
			healthBar.parent.gameObject.SetActive(false);
		dead = true;
		//targetRotation.x = -90;
		AudioSource.PlayClipAtPoint(toggleSound, transform.position);
        
        //show death animationn of this bot
        if (gameObject.GetComponent<GaurdController>())
        gameObject.GetComponent<GaurdController>().SetDead();

		int random=Random.Range(1,6);
		string temp="Yell"+random;
		FindObjectOfType<AudioManager>().Play(temp);
	}

	public void Revive()
	{
		if (boss)
		{
			health = totalHealth;
			healthBar.parent.gameObject.SetActive(true);
			UpdateHealthBar();
		}
		dead = false;
		//targetRotation.x = 0;
		AudioSource.PlayClipAtPoint(toggleSound, transform.position);
	}

	private void UpdateHealthBar()
	{
		float scaleFactor = health / totalHealth;

		healthBar.sizeDelta = new Vector2(scaleFactor * originalBarScale, healthBar.sizeDelta.y);
	}
}
