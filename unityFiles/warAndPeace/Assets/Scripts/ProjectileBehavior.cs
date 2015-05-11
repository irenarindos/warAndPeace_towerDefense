using UnityEngine;
using System.Collections;

public class ProjectileBehavior : MonoBehaviour {

	private Creep target;
	private TowerBehavior source;
	private float damage;

	public void init(Creep target, TowerBehavior source, float damage)
	{
		this.target = target;
		this.source = source;
		this.damage = damage;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (transform.localPosition.magnitude < 0.1)
		{
			target.damage(damage);
			Destroy(gameObject);
		}
		else
		{
			transform.localPosition -= transform.localPosition.normalized*2.5f*Time.deltaTime;
		}

	}
}
