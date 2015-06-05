using UnityEngine;
using System.Collections.Generic;

public class TowerBehavior : MonoBehaviour {

	public float SHOOTDELAY;
	public float DAMAGE;
	public float RANGE;
	public float lastShot;
	public bool selected = false;
	public bool isBuilt = false;
	public Material boltmat;
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
	public GameObject rangeIndicator;
	public Sprite shotsprite;

	// Use this for initialization
	void Start () {
		modules = new List<TowerModule>();
		PlayerState s = MainMenu.instance;
		addModule(new BasicModule(s.getResearch("BasicLevel")));
		addModule(new EnergyDrainModule(s.getResearch("EnergyDrainLevel"), s.getResearch("EnergyDrainMax")));
		addModule(new DynamiteModule(s.getResearch("DynamiteLevel"), s.getResearch("DynamiteMax")));
		addModule(new SteamModule(s.getResearch("SteamLevel"), s.getResearch("SteamMax")));
		addModule(new ScopeModule(s.getResearch("ScopeLevel"), s.getResearch("ScopeMax")));
		addModule(new GearModule(s.getResearch("GearLevel"), s.getResearch("GearMax")));
		addModule(new BombModule(s.getResearch("BombLevel"), s.getResearch("BombMax")));
		lastShot = Time.time;

	    rangeIndicator = new GameObject("range indicator");
		SpriteRenderer rend = rangeIndicator.AddComponent<SpriteRenderer>();
		rend.sprite = Sprite.Create(Utils.makeCircle(), Rect.MinMaxRect(0f, 0f, 128f, 128f), new Vector2(0f,0f));
		rangeIndicator.transform.parent = transform;
		rangeIndicator.transform.localPosition = new Vector3(-0.64f*getRange()/transform.localScale.x, -0.64f*getRange()/transform.localScale.x, 0f);
		rangeIndicator.transform.localScale = new Vector3(getRange()/transform.localScale.x, getRange()/transform.localScale.x, 1f);

	}

	public void upgrade(int what)
	{
		if (map.resources >= modules[what].getUpgradeCost() && modules[what].canUpgrade())
		{
			map.resources -= modules[what].getUpgradeCost();
			modules[what].upgrade();
		}
	}

	public string getTooltip(int what)
	{
		string result = modules[what].getName();
		if (modules[what].canUpgrade())
			result += "; Upgrade cost: " + modules[what].getUpgradeCost();
		return result;
	}

	T addModule<T>() where T: TowerModule, new()
	{
		T newModule = new T();
		int i = 0;
		for (; i < modules.Count && modules[i].getPriority() < newModule.getPriority(); ++i);
		modules.Insert(i, newModule);
		newModule.init(this);
		return newModule;
	}

	void addModule(TowerModule newModule)
	{
		int i = 0;
		for (; i < modules.Count && modules[i].getPriority() < newModule.getPriority(); ++i);
		modules.Insert(i, newModule);
		newModule.init(this);
	}
	
	// Update is called once per frame
	void Update () {
		if (selected || !isBuilt)
		{
			rangeIndicator.transform.localPosition = new Vector3(-getRange()/transform.localScale.x, -getRange()/transform.localScale.y, 0f);
			rangeIndicator.transform.localScale = new Vector3(getRange()/(0.64f*transform.localScale.x), getRange()/(0.64f*transform.localScale.y), 1f);
		}
		rangeIndicator.SetActive(selected);
		

		if (!isBuilt) return;
		if (target != null && ((target.transform.position - gameObject.transform.position).magnitude > getRange() || target.dead))
		{
			target = null;
		}
		acquireTarget();
		if (target != null)
		{
			if (Time.time >= lastShot + getShootDelay())
			{
				fireShot();
				lastShot = Time.time;
			}
		}
	}

	public void showStats()
	{
		map.towertext.text = "Damage: " + getDamage() + "; Range: " + getRange() + "; Fire delay: " + getShootDelay() + "; DPS: " + getDamage()/getShootDelay();
	}

	public void fireShot()
	{
		IList<ImpactEffect> effects = new List<ImpactEffect>();
		foreach(TowerModule mod in modules)
		{
			mod.getImpactEffect(effects);
		}
		target.damage(getDamage());

		/*ProjectileBehavior shot;
		GameObject go = new GameObject();
		SpriteRenderer rend = go.AddComponent<SpriteRenderer>();
		rend.sprite = shotsprite; 
		rend.color = Color.red;
		shot = go.AddComponent<ProjectileBehavior>();
		shot.init(target, this, getDamage(), effects);
		shot.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
		shot.transform.position = transform.position;
		shot.transform.parent = target.transform;*/

		GameObject bolt = new GameObject();
		bolt.AddComponent<LineRenderer>();
		BoltCleanup cleanup = bolt.AddComponent<BoltCleanup>();
		cleanup.expiration = Time.time + getShootDelay()/2.0f;
		cleanup.refresh = 0.05f;
		cleanup.source = (Vector2)gameObject.transform.position;
		cleanup.target = target;
		cleanup.boltmat = boltmat;
		cleanup.damage = getDamage ();
		cleanup.effects = effects;
		cleanup.tower = this;
	}

	void acquireTarget()
	{
		if (target != null && target.dying) target = null;
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

	public void OnMouseDown()
	{
		if (isBuilt)
		{
			select ();
		}
	}

	public void select()
	{
		if (map.selectedTower)
		{
			map.selectedTower.selected = false;
		}
		map.selectedTower = this;
		this.selected = true;
	}

	public void unselect()
	{
		this.selected = false;
		map.selectedTower = null;
	}
}
