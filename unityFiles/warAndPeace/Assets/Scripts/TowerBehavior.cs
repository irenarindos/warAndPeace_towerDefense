using UnityEngine;
using System.Collections;

public class TowerBehavior : MonoBehaviour {

	public float SHOOTDELAY;
	public float DAMAGE;
	public float RANGE;
	public float lastShot;
	public enum TargetingStrategy
	{
		FIRSTINRANGE,
		FIRST,
		CLOSEST,
		FURTHEST,
		HIGHESTHP,
		LOWESTHP
	};
	public TargetingStrategy targetingStrategy;
	public Creep target;
	public MapBehavior map;

	// Use this for initialization
	void Start () {
		lastShot = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
		if (target != null && ((target.transform.position - gameObject.transform.position).magnitude > RANGE || target.dead))
		{
			target = null;
		}
		acquireTarget();
		if (target != null)
		{
			if (Time.time >= lastShot + SHOOTDELAY)
			{
			    target.damage(DAMAGE);
				lastShot = Time.time;
			}
		}
	}

	void acquireTarget()
	{
		if (targetingStrategy == TargetingStrategy.FIRSTINRANGE)
		{
			if (target == null)
			{
				foreach (Creep c in map.creeps)
				{
					if ((c.transform.position - gameObject.transform.position).magnitude < RANGE && !c.dead)
					{
						target = c;
					}
				}
			}
		}
		else
		{
			Debug.Log("Only 'first in range' targeting strategy implemented yet");
		}
	}
}
