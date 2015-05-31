using UnityEngine;
using System.Collections.Generic;

public class TowerModule  {
	protected TowerBehavior tower;

	virtual public void init(TowerBehavior tower)
	{
		this.tower = tower;
	}

	virtual public void upgrade()
	{

	}

	virtual public string getName()
	{
		return "";
	}

	virtual public bool canUpgrade()
	{
		return false;
	}

	virtual public int getUpgradeCost()
	{
		return 0;
	}

	virtual public int getPriority()
	{
		return 1;
	}

	virtual public float getShootDelay(float delay)
	{
		return delay;
	}

	virtual public float getDamage(float damage)
	{
		return damage;
	}

	virtual public float getRange(float range)
	{
		return range;
	}

	virtual public void getImpactEffect(IList<ImpactEffect> effects)
	{
	}
}

public class BasicModule : TowerModule  {
	float level;
	public BasicModule(float level)
	{
		if (level <= 0) this.level = 1;
		else this.level = level;
	}

	override public int getPriority()
	{
		return 0;
	}
	
	override public float getShootDelay(float delay)
	{
		return tower.SHOOTDELAY;
	}
	
	override public float getDamage(float damage)
	{
		return tower.DAMAGE*this.level;
	}
	
	override public float getRange(float range)
	{
		return tower.RANGE;
	}
}

// increase range
public class ScopeModule : TowerModule  {

	private float percent = 1.0f;
	private int level = 0;
	private int maxlevel;
	public ScopeModule(float level, float maxlevel)
	{
		this.level = Mathf.RoundToInt(level);
		this.maxlevel = Mathf.RoundToInt(maxlevel);
	}

	override public float getRange(float range)
	{
		return range*percent;
	}
	

	override public void upgrade()
	{
		level++;
		percent = Mathf.Pow(1.15f, level) - level*0.1f;
	}
	
	override public bool canUpgrade()
	{
		return (this.level < this.maxlevel);
	}
	
	override public int getUpgradeCost()
	{
		return Mathf.RoundToInt((Mathf.Pow(1.65f, level) - 0.5f*level)*20);
	}

	override public string getName()
	{
		if (canUpgrade ())
		    return "Scope (range; upgrade to " + Mathf.Round(10*tower.RANGE*(Mathf.Pow(1.15f, level+1) - (level+1)*0.1f))/10  +  " units)";
		return "Scope (range; maximum level reached)";
	}
}

// increase damage
public class BombModule : TowerModule  {
	
	private float percent = 1.0f;
	private int level = 0;
	private int maxlevel;
	public BombModule(float level, float maxlevel)
	{
		this.level = Mathf.RoundToInt(level);
		this.maxlevel = Mathf.RoundToInt(maxlevel);
	}

	override public float getDamage(float damage)
	{
		return damage*percent;
	}
	

	override public void upgrade()
	{
		level++;
		percent = 0.5f + Mathf.Pow (1.4f, level) - level*0.15f;

	}
	
	override public bool canUpgrade()
	{
		return (level < maxlevel);
	}
	
	override public int getUpgradeCost()
	{
		return Mathf.RoundToInt((Mathf.Pow(1.8f, level) - 0.75f*level)*30);
	}

	override public string getName()
	{
		if (canUpgrade())
		    return "Bomb (damage; upgrade to " + Mathf.Round(tower.DAMAGE*(0.5f + Mathf.Pow (1.4f, level+1) - (level+1)*0.15f )) + " dmg)";
		return "Bomb (damage; maximum level reached)";
	}
}

// decrease fire delay
public class GearModule : TowerModule  {
	
	private float percent = 1f;
	private int level = 0;
	private int maxlevel;
	public GearModule(float level, float maxlevel)
	{
		this.level = Mathf.RoundToInt(level);
		this.maxlevel = Mathf.RoundToInt(maxlevel);
	}
	
	override public float getShootDelay(float delay)
	{
		return delay*percent;
	}

	override public void upgrade()
	{
		level++;
		percent = Mathf.Pow (0.7f, level/7.0f);
	}
	
	override public bool canUpgrade()
	{
		return (level < maxlevel);
	}
	
	override public int getUpgradeCost()
	{
		return Mathf.RoundToInt((Mathf.Pow(1.9f, level) - 0.65f*level)*25);
	}

	override public string getName()
	{
		if (canUpgrade())
		    return "Gear (attack delay; upgrade to: " + Mathf.Round(100*tower.SHOOTDELAY * Mathf.Pow (0.7f, (level + 1)/7.0f))/100 + "s delay)";
		return "Gear (attack delay; maximum level " + level + " of " + maxlevel + " reached)";
	}
}

// decrease creep speed
public class SteamModule : TowerModule  {
	
	private float percent = 1f;
	
	private int level = 0;
	private int maxlevel;
	public SteamModule(float level, float maxlevel)
	{
		this.level = Mathf.RoundToInt(level);
		this.maxlevel = Mathf.RoundToInt(maxlevel);
	}
	
	override public void upgrade()
	{
		level++;
		percent = Mathf.Pow (0.75f, level/7.0f) - 0.1f;
	}
	
	override public bool canUpgrade()
	{
		return (level < maxlevel);
	}
	
	override public int getUpgradeCost()
	{
		return Mathf.RoundToInt((Mathf.Pow(1.9f, level) - 0.45f*level)*45);
	}

	override public void getImpactEffect(IList<ImpactEffect> effects)
	{
		if (level > 0)
		    effects.Add(new SlowEffect(3, percent));
	}

	override public string getName()
	{
		if (canUpgrade())
		    return "Steam (slows creeps; currently by " + Mathf.Round((1-percent)*100) + " %; upgrade to increase to " + Mathf.Round((1-(Mathf.Pow (0.75f, (level+1)/7.0f) - 0.1f))*100) + "%)";
		return "Steam (slows creeps by " + Mathf.Round((1-percent)*100) + " %; maximum level reached)";
	}
}

// splash damage
public class DynamiteModule : TowerModule  {
	
	private float percent = 0f;
	private float radius = 0.0f;
	private float damage = 0.0f;
	
	private int level = 0;
	private int maxlevel;
	public DynamiteModule(float level, float maxlevel)
	{
		this.level = Mathf.RoundToInt(level);
		this.maxlevel = Mathf.RoundToInt(maxlevel);
	}
	
	override public void upgrade()
	{
		level++;
		percent = 1.2f - Mathf.Pow (0.7f, level/4.0f);
		radius = 0.3f + 0.2f * level;
	}
	
	override public bool canUpgrade()
	{
		return (level < maxlevel);
	}
	
	override public int getUpgradeCost()
	{
		return Mathf.RoundToInt((Mathf.Pow(1.9f, level) - 0.45f*level)*50);
	}

	override public int getPriority()
	{
		return 10;
	}
	
	override public float getDamage(float damage)
	{
		this.damage = damage;
		return damage;
	}
	
	override public void getImpactEffect(IList<ImpactEffect> effects)
	{
		effects.Add(new SplashDamageEffect(radius, damage*percent));
	}

	override public string getName()
	{
		if (canUpgrade())
			return "Dynamite (" + Mathf.Round(percent*100) + " % splash damage in a " + Mathf.Round(radius*10)/10 + " radius; upgrade for " + Mathf.Round((1.2f - Mathf.Pow (0.7f, (level+1)/4.0f))*100) + "% in a " + Mathf.Round((0.3f + 0.2f * (level+1))*10)/10 + " radius)";
		return "Dynamite (" + Mathf.Round(percent*100) + " % splash damage in a " + Mathf.Round(radius*10)/10 + " radius; maximum level reached)";
	}
}
