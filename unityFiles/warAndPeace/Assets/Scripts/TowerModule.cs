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
		if (level < 0) level = 0;
		this.level = Mathf.RoundToInt(level);
		this.maxlevel = Mathf.RoundToInt(maxlevel);
		percent = calcPercent(level);
	}

	float calcPercent(float level)
	{
		if (level > 0) return Mathf.Pow(1.15f, level) - level*0.1f;
		return 1.0f;
	}

	override public float getRange(float range)
	{
		return range*percent;
	}

	override public void upgrade()
	{
		level++;
		percent = calcPercent(level);
	}
	
	override public bool canUpgrade()
	{
		return (this.level < this.maxlevel) || maxlevel < 0;
	}
	
	override public int getUpgradeCost()
	{
		return Mathf.RoundToInt((Mathf.Pow(1.65f, level) - 0.5f*level)*20);
	}

	override public string getName()
	{
		if (canUpgrade ())
		    return "Scope (range; upgrade to " + Mathf.Round(10*tower.RANGE*calcPercent(level+1))/10  +  " units)";
		return "Scope (range; maximum level reached)";
	}
}

// increase damage
public class PowerIncreaseModule : TowerModule  {
	
	private float percent = 1.0f;
	private int level = 0;
	private int maxlevel;
	public  PowerIncreaseModule(float level, float maxlevel)
	{
		if (level < 0) level = 0;
		this.level = Mathf.RoundToInt(level);
		this.maxlevel = Mathf.RoundToInt(maxlevel);
		percent = calcPercent(level);
	}

	float calcPercent(float level)
	{
		float result = 1.0f;
		if (level > 0)
		    result = 0.35f + Mathf.Pow (1.45f, level) - level*0.15f;
		return result;

	}

	override public float getDamage(float damage)
	{
		return damage*percent;
	}
	

	override public void upgrade()
	{
		level++;
		percent = calcPercent(level);
	}
	
	override public bool canUpgrade()
	{
		return (this.level < this.maxlevel) || maxlevel < 0;
	}
	
	override public int getUpgradeCost()
	{
		return Mathf.RoundToInt((Mathf.Pow(1.8f, level) - 0.75f*level)*30);
	}

	override public string getName()
	{
		if (canUpgrade())
			return " Power Increase (damage; upgrade to " + Mathf.Round(tower.DAMAGE*calcPercent (level+1)) + " dmg)";
		return " Power Increase (damage; maximum level reached)";
	}
}

// decrease fire delay
public class GearModule : TowerModule  {
	
	private float percent = 1f;
	private int level = 0;
	private int maxlevel;
	public GearModule(float level, float maxlevel)
	{
		if (level < 0) level = 0;
		this.level = Mathf.RoundToInt(level);
		this.maxlevel = Mathf.RoundToInt(maxlevel);
		calcPercent();
	}

	void calcPercent()
	{
		percent = Mathf.Pow (0.7f, level/7.0f);
	}
	
	override public float getShootDelay(float delay)
	{
		return delay*percent;
	}

	override public void upgrade()
	{
		level++;
		calcPercent();
	}
	
	override public bool canUpgrade()
	{
		return (this.level < this.maxlevel) || maxlevel < 0;
	}
	
	override public int getUpgradeCost()
	{
		return Mathf.RoundToInt((Mathf.Pow(1.9f, level) - 0.65f*level)*25);
	}

	override public string getName()
	{
		if (canUpgrade())
		    return "Gear (attack delay; upgrade to: " + Mathf.Round(100*tower.SHOOTDELAY * Mathf.Pow (0.7f, (level + 1)/7.0f))/100 + "s delay)";
		return "Gear (attack delay; maximum level reached)";
	}
}

// decrease creep speed
public class SteamModule : TowerModule  {
	
	private float percent = 1f;
	
	private int level = 0;
	private int maxlevel;
	public SteamModule(float level, float maxlevel)
	{
		if (level < 0) level = 0;
		this.level = Mathf.RoundToInt(level);
		this.maxlevel = Mathf.RoundToInt(maxlevel);
		percent = calcPercent(level);
	}

	float calcPercent(float level)
	{
		if (level > 0)
		  return Mathf.Pow (0.75f, level/7.0f) - 0.1f;
		return 1.0f;
	}
	
	override public void upgrade()
	{
		level++;
		percent = calcPercent(level);
	}
	
	override public bool canUpgrade()
	{
		return (this.level < this.maxlevel) || maxlevel < 0;
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
		    return "Steam (slows creeps; currently by " + Mathf.Round((1-percent)*100) + " %; upgrade to increase to " + Mathf.Round((1-calcPercent(level+1))*100) + "%)";
		return "Steam (slows creeps by " + Mathf.Round((1-percent)*100) + " %; maximum level reached)";
	}
}

// gain energy from hitting creeps
public class EnergyDrainModule : TowerModule  
{
	private int level = 0;
	private int maxlevel;
	public EnergyDrainModule(float level, float maxlevel)
	{
		if (level < 0) level = 0;
		this.level = Mathf.RoundToInt(level);
		this.maxlevel = Mathf.RoundToInt(maxlevel);
	}
	
	float calcAmount(float level)
	{
		return level;
	}
	
	override public void upgrade()
	{
		level++;
	}

	override public int getPriority()
	{
		return 12;
	}
	
	override public bool canUpgrade()
	{
		return (this.level < this.maxlevel) || maxlevel < 0;
	}
	
	override public int getUpgradeCost()
	{
		return Mathf.RoundToInt((Mathf.Pow(1.9f, level) - 0.45f*level)*70);
	}
	
	override public void getImpactEffect(IList<ImpactEffect> effects)
	{
		if (level > 0)
			effects.Add(new MoneyEffect(calcAmount(level)));
	}
	
	override public string getName()
	{
		if (canUpgrade())
			return "Energy Drain (get resources for hitting creeps; currently gains " + calcAmount(level) + "; upgrade to get " + calcAmount(level+1) + " per hit)";
		return "Energy Drain (get " + calcAmount(level) + " resources for each creep hit; maximum level reached)";

	}
}

// splash damage
public class ChainLightningModule : TowerModule  {
	
	private float percent = 0f;
	private float radius = 0.0f;
	private float damage = 0.0f;
	
	private int level = 0;
	private int maxlevel;
	public ChainLightningModule(float level, float maxlevel)
	{
		if (level < 0) level = 0;
		this.level = Mathf.RoundToInt(level);
		this.maxlevel = Mathf.RoundToInt(maxlevel);
		calcPercent();
	}

	void calcPercent()
	{
		percent = 1.2f - Mathf.Pow (0.7f, level/4.0f);
		radius = 0.3f + 0.2f * level;
	}
	
	override public void upgrade()
	{
		level++;
		calcPercent();
	}
	
	override public bool canUpgrade()
	{
		return (this.level < this.maxlevel) || maxlevel < 0;
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
			return "Chain Lightning (" + Mathf.Round(percent*100) + " % splash damage in a " + Mathf.Round(radius*10)/10 + " radius; upgrade for " + Mathf.Round((1.2f - Mathf.Pow (0.7f, (level+1)/4.0f))*100) + "% in a " + Mathf.Round((0.3f + 0.2f * (level+1))*10)/10 + " radius)";
		return "Chain Lightning (" + Mathf.Round(percent*100) + " % splash damage in a " + Mathf.Round(radius*10)/10 + " radius; maximum level reached)";
	}
}
