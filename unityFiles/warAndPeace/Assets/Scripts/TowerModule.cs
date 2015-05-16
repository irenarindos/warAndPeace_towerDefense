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
		return tower.DAMAGE;
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
		return true;
	}
	
	override public int getUpgradeCost()
	{
		return Mathf.RoundToInt((Mathf.Pow(1.65f, level) - 0.5f*level)*20);
	}

	override public string getName()
	{
		return "Scope (range; upgrade to " + tower.RANGE*(Mathf.Pow(1.15f, level+1) - (level+1)*0.1f)  +  " units)";
	}
}

// increase damage
public class BombModule : TowerModule  {
	
	private float percent = 1.0f;
	private int level = 0;
	
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
		return true;
	}
	
	override public int getUpgradeCost()
	{
		return Mathf.RoundToInt((Mathf.Pow(1.8f, level) - 0.75f*level)*30);
	}

	override public string getName()
	{
		return "Bomb (damage; upgrade to " + tower.DAMAGE*(0.5f + Mathf.Pow (1.4f, level+1) - (level+1)*0.15f ) + " dmg)";
	}
}

// decrease fire delay
public class GearModule : TowerModule  {
	
	private float percent = 1f;
	
	override public float getShootDelay(float delay)
	{
		return delay*percent;
	}

	private int level = 0;

	override public void upgrade()
	{
		level++;
		percent = Mathf.Pow (0.7f, level/7.0f);
	}
	
	override public bool canUpgrade()
	{
		return true;
	}
	
	override public int getUpgradeCost()
	{
		return Mathf.RoundToInt((Mathf.Pow(1.9f, level) - 0.65f*level)*25);
	}

	override public string getName()
	{
		return "Gear (attack delay; upgrade to: " + tower.SHOOTDELAY * Mathf.Pow (0.7f, (level + 1)/7.0f) + "s delay)";
	}
}

// decrease creep speed
public class SteamModule : TowerModule  {
	
	private float percent = 1f;
	
	private int level = 0;
	
	override public void upgrade()
	{
		level++;
		percent = Mathf.Pow (0.75f, level/7.0f) - 0.1f;
	}
	
	override public bool canUpgrade()
	{
		return true;
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
		return "Steam (slows creeps; currently by " + (1-percent)*100 + " %; upgrade to increase to " + (1-(Mathf.Pow (0.75f, (level+1)/7.0f) - 0.1f))*100 + "%)";
	}
}

// splash damage
public class DynamiteModule : TowerModule  {
	
	private float percent = 0f;
	private float radius = 0.0f;
	private float damage = 0.0f;
	
	private int level = 0;
	
	override public void upgrade()
	{
		level++;
		percent = 1 - Mathf.Pow (0.7f, level/4.0f);
		radius = 1f + 0.2f * level;
	}
	
	override public bool canUpgrade()
	{
		return true;
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
		return "Dynamite (" + percent*100 + " % splash damage in a " + radius + " radius; upgrade for " + (1 - Mathf.Pow (0.7f, (level+1)/4.0f))*100 + "% in a " + (1f + 0.2f * (level+1)) + " radius)";
	}
}
