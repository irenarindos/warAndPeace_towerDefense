using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ProjectileBehavior : MonoBehaviour {

	private Creep target;
	private TowerBehavior source;
	private float damage;
	private IList<ImpactEffect> effects;

	public void init(Creep target, TowerBehavior source, float damage, IList<ImpactEffect> effects)
	{
		this.target = target;
		this.source = source;
		this.damage = damage;
		this.effects = effects;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (transform.localPosition.magnitude < 0.1)
		{
			foreach (ImpactEffect eff in effects)
			{
				eff.apply(target, source);
			}
			target.realizeDamage(damage);
			Destroy(gameObject);
		}
		else
		{
			transform.localPosition -= transform.localPosition.normalized*8.5f*Time.deltaTime*1/target.transform.localScale.magnitude;
		}

	}
}
