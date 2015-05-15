using UnityEngine;
using System.Collections;

public class HealthBarManager : MonoBehaviour {

	public GameObject red;
	public GameObject green;
	Vector3 transitionPoint;
	float health = 1.0F;
	private float lastHealthValue;
	Vector3 objPos;
	private bool isBeingDead = false;

	// Use this for initialization
	void Start () {
		transitionPoint = new Vector3 (1.0F, 0, -1);
		lastHealthValue = health;
	
	}

	public void setHealthPercent(float dmg){
		health = dmg;
	}

	public void updatePos(Vector3 p){
		objPos = p;
		//transitionPoint.y = p.y;
		//transitionPoint.z = -1;
	}

	public bool getIsDead(){
		return isBeingDead;
	}
	
	// Update is called once per frame
	void Update () {
		if(health <= 0){ 
			green.GetComponent<LineRenderer> ().enabled = false;
			red.GetComponent<LineRenderer> ().enabled = false;
			isBeingDead = true;
			return;
		};
		health -= .01F;
		float healthChange = lastHealthValue - health;

		transitionPoint.x -= healthChange;
		red.GetComponent<LineRenderer> ().SetPosition (1, transitionPoint);
		green.GetComponent<LineRenderer> ().SetPosition (0, transitionPoint);

		lastHealthValue = health;
	
	}
}
