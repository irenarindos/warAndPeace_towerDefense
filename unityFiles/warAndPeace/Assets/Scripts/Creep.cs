using UnityEngine;
using System.Collections.Generic;

public class Creep : MonoBehaviour {

	public enum CreepType 
	{
		NORMAL,
		LARGE,
		BOSS
	}

	public enum CreepTrait
	{
		FAST,
		SHIELDED,
		ARMORED
	}

	private const float TARGETDIST = 0.05f;
	public MapBehavior map;
	private IList<Vector2> path;
	private bool valid;
	public bool dead;
	public bool dying;
	private int waypoint;
	private int pathpoint;
	private IList<CreepModule> modules;
	//private HealthBarManager healthBar;
	private GameObject healthBar;

	public float health
	{
		get { return getHealth(); }
	}

	public float prospectiveHealth
	{
		get { return getProspectiveHealth(); }
	}
	public float maxHealth
	{
		get { return getMaxHealth(); }
	}
	public int value 
	{
		get { return getValue(); }
	}

	private GameObject healthIndicator;
	/// Use this for initialization
	void Start () {
		valid = false;
		dead = false;
		waypoint = 0;
		getWaypoint();
		if (modules == null) modules = new List<CreepModule>();
		healthBar = (GameObject)Instantiate(Resources.Load("healthBarManager"));
		healthBar.transform.position = gameObject.transform.position + new Vector3(0F, .1F, 0F);
		healthBar.transform.SetParent (gameObject.transform);//this;//gameObject;
	}

	void addModule<T>() where T: CreepModule, new()
	{
		T newModule = new T();
		int i = 0;
		for (; i < modules.Count && modules[i].getPriority() < newModule.getPriority(); ++i);
		modules.Insert(i, newModule);
		newModule.init(this);
	}

	void addModule(CreepModule newModule) 
	{
		int i = 0;
		if (modules == null)
		{
			modules = new List<CreepModule>();
		}
		for (; i < modules.Count && modules[i].getPriority() < newModule.getPriority(); ++i);
		modules.Insert(i, newModule);
		newModule.init(this);
	}

	public T getModule<T>() where T : CreepModule, new()
	{
		if (!hasModule<T>())
		{
			addModule<T>();
		}
		foreach (CreepModule mod in modules)
		{
			if (mod is T)
			{
				return (T)mod;
			}
		}
		return null;
	}

	public bool hasModule<T>() where T : CreepModule, new()
	{
		foreach (CreepModule mod in modules)
		{
			if (mod is T)
			{
				return true;
			}
		}
		return false;
	}

	void getWaypoint()
	{
		path = map.getPathToWaypoint(waypoint, gameObject.transform.position);
		pathpoint = 0;
		valid = true;
		if (path.Count == 0)
		{
			dying = true;
			dead = true;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (dying)
		{
			map.removeCreep(this);
		}
		if (dead)
		{
			if (health <= 0)
			    map.resources += value;
		    else
				map.lives -= 1;
			gameObject.SetActive(false);
			Destroy (gameObject, 1);
			map.spawnedCreeps--;
			return;
		}
		if (path == null) return;
		float v = getSpeed(); 
		v *= Time.deltaTime;
		if ((((Vector2)gameObject.transform.position - path[pathpoint]).magnitude < TARGETDIST) || !valid)
		{
			if (pathpoint + 1 == path.Count)
			{
				waypoint++;
				getWaypoint ();
			}
			else
			{
				pathpoint++;
			}
		}
		if (pathpoint < path.Count)
		{
			Vector2 dir = path[pathpoint] - (Vector2)gameObject.transform.position;
			gameObject.transform.position += (Vector3)dir.normalized*v;
		}
		drawHealthBar ();
	}

	//Draw a health bar above the creep
	//Red is damage taken, green is health left
	private void drawHealthBar(){
		float damage = health / maxHealth;
		HealthBarManager hb = (HealthBarManager)healthBar.GetComponent<HealthBarManager> ();
		hb.setHealthPercent(damage);
	}

	public void damage(float dmg)
	{
		foreach (CreepModule mod in modules)
		{
			dmg = mod.damage(dmg);
		}
		if (prospectiveHealth <= 0)
		{
			dying = true;
		}
	}

	public void realizeDamage(float dmg)
	{
		foreach (CreepModule mod in modules)
		{
			dmg = mod.realizeDamage(dmg);
		}
		//gameObject.GetComponent<SpriteRenderer>().color.r = (int)(255*health/100.0);
		gameObject.GetComponent<SpriteRenderer>().color = Color.Lerp(Color.red, Color.white, health/maxHealth);
		if (health <= 0)
		{
			dead = true;
		}
	}

	public float getSpeed()
	{
		float result = 0;
		foreach (CreepModule mod in modules)
		{
			result = mod.getSpeed(result);
		}
		return result;
	}

	public float getHealth()
	{
		float result = 0;
		foreach (CreepModule mod in modules)
		{
			result = mod.getHealth(result);
		}
		return result;
	}

	public float getProspectiveHealth()
	{
		float result = 0;
		foreach (CreepModule mod in modules)
		{
			result = mod.getProspectiveHealth(result);
		}
		return result;
	}

	public float getMaxHealth()
	{
		float result = 0;
		foreach (CreepModule mod in modules)
		{
			result = mod.getMaxHealth(result);
		}
		return result;
	}

	public int getValue()
	{
		int result = 0;
		foreach (CreepModule mod in modules)
		{
			result = mod.getValue(result);
		}
		return result;
	}

	public void setType(CreepType t, CreepTrait[] traits, int wave)
	{
		switch (t)
		{
		case CreepType.NORMAL: addModule(new NormalCreep(wave)); break;
		case CreepType.LARGE:  addModule(new LargeCreep(wave)); 
			transform.localScale *= 1.8f;
			 break;
		case CreepType.BOSS: addModule(new BossCreep(wave)); 
			transform.localScale *= 3f;
			break;
			
		}
		foreach (CreepTrait trait in traits)
		{
			switch (trait)
			{
			case CreepTrait.FAST: addModule<FastCreep>(); break;
			case CreepTrait.SHIELDED: addModule<CreepShield>(); break;
			case CreepTrait.ARMORED: addModule<CreepArmor>(); break;
			}

		}


	}
}
