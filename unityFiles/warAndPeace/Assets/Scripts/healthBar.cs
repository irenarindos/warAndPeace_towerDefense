using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class healthBar : MonoBehaviour {
	public GameObject greenHealthbar;
	private float health = 1.0F; //Initialize to 100%
	private float lastHealthValue;


	// Use this for initialization
	void Start () {
		lastHealthValue = health;
	}

	public void setHealthPercent(float dmg){
		health = dmg;
	}
	
	// Update is called once per frame
	void Update () {
		if (health <= 0.0F)
			return;

		float healthChange = lastHealthValue - health;
		greenHealthbar.transform.localScale -= new Vector3(healthChange, 0, 0);

		greenHealthbar.transform.Translate (-healthChange/2.0F, 0F, 0F);//= greenHealthbar.transform.position.x -xScale;
		lastHealthValue = health;
	}
}
