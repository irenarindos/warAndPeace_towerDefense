using UnityEngine;
using System.Collections.Generic;

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
	public IList<TowerModule> modules;
	public TargetingStrategy targetingStrategy;
	public Creep target;
	public MapBehavior map;
	private GameObject rangeIndicator;

	// Use this for initialization
	void Start () {
		modules = new List<TowerModule>();
		addModule<BasicModule>();
		lastShot = Time.time;

	    rangeIndicator = new GameObject("range indicator");
		SpriteRenderer rend = rangeIndicator.AddComponent<SpriteRenderer>();
		rend.sprite = Sprite.Create(Utils.makeCircle(), Rect.MinMaxRect(0f, 0f, 128f, 128f), new Vector2(0f,0f));
		rangeIndicator.transform.parent = transform;
		rangeIndicator.transform.localPosition = new Vector3(-0.64f*getRange(), -0.64f*getRange(), 0f);
		rangeIndicator.transform.localScale = new Vector3(getRange(), getRange(), 1f);

	}

	void addModule<T>() where T: TowerModule, new()
	{
		T newModule = new T();
		int i = 0;
		for (; i < modules.Count && modules[i].getPriority() < newModule.getPriority(); ++i);
		modules.Insert(i, newModule);
		newModule.init(this);
	}
	
	// Update is called once per frame
	void Update () {
		if (target != null && ((target.transform.position - gameObject.transform.position).magnitude > getRange() || target.dead))
		{
			target = null;
		}
		acquireTarget();
		if (target != null)
		{
			if (Time.time >= lastShot + getShootDelay())
			{
			    target.damage(getDamage());
				lastShot = Time.time;
			}
		}
		if (Input.GetButtonDown("Jump") && map.resources >= ScopeModule.cost)
		{
			addModule<ScopeModule>();
			map.resources -= ScopeModule.cost;
		}
		else if (Input.GetButtonDown("Fire1") && map.resources >= BombModule.cost)
		{
			addModule<BombModule>();
			map.resources -= BombModule.cost;
		}
		else if (Input.GetButtonDown("Fire2") && map.resources >= GearModule.cost)
		{
			addModule<GearModule>();
			map.resources -= GearModule.cost;
		}
		rangeIndicator.transform.localPosition = new Vector3(-0.64f*getRange(), -0.64f*getRange(), 0f);
		rangeIndicator.transform.localScale = new Vector3(getRange(), getRange(), 1f);
		map.towertext.text = "Damage: " + getDamage() + "; Range: " + getRange() + "; Fire delay: " + getShootDelay();
	}

	void acquireTarget()
	{
		if (targetingStrategy == TargetingStrategy.FIRSTINRANGE)
		{
			if (target == null)
			{
				foreach (Creep c in map.creeps)
				{
					if ((c.transform.position - gameObject.transform.position).magnitude < getRange() && !c.dead)
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

	public float getShootDelay()
	{
		float delay = 10;
		foreach (TowerModule mod in modules)
		{
			delay = mod.getShootDelay(delay);
		}
		return delay;
	}
	
	public float getDamage()
	{
		float damage = 0;
		foreach (TowerModule mod in modules)
		{
			damage = mod.getDamage(damage);
		}
		return damage;
	}
	
	public float getRange()
	{
		float range = 0;
		foreach (TowerModule mod in modules)
		{
			range = mod.getRange(range);
		}
		return range;
	}
}
