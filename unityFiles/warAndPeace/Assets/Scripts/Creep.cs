using UnityEngine;
using System.Collections.Generic;

public class Creep : MonoBehaviour {

	public enum CreepType 
	{
		NORMAL,
		LARGE,
		BOSS,
		FAST,
		SHIELDED
	}

	private const float TARGETDIST = 0.05f;
	public MapBehavior map;
	private IList<Vector2> path;
	private bool valid;
	public bool dead;
	private int waypoint;
	private int pathpoint;
	private IList<CreepModule> modules;
	public float health
	{
		get { return getHealth(); }
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
		healthIndicator = (GameObject)Instantiate(Resources.Load("healthBar"));
		healthIndicator.transform.position = gameObject.transform.position;
		healthIndicator.transform.SetParent (gameObject.transform);//this;//gameObject;
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

	void getWaypoint()
	{
		path = map.getPathToWaypoint(waypoint, gameObject.transform.position);
		pathpoint = 0;
		valid = true;
		if (path.Count == 0)
		{
			dead = true;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (dead)
		{
			if (health <= 0)
			    map.resources += value;
		    else
				map.lives -= 1;
			map.removeCreep(this);
			gameObject.SetActive(false);
			Destroy (gameObject, 1);
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
		healthBar hb = (healthBar)healthIndicator.GetComponent<healthBar> ();
		hb.setHealthPercent(damage);
	}

	public void damage(float dmg)
	{
		foreach (CreepModule mod in modules)
		{
			dmg = mod.damage(dmg);
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

	public void setType(CreepType t, int wave)
	{
		addModule(new NormalCreep(wave));

	}
}
