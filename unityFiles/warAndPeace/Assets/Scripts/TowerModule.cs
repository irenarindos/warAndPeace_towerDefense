using UnityEngine;
using System.Collections;

public class TowerModule  {
	protected TowerBehavior tower;

	virtual public void init(TowerBehavior tower)
	{
		this.tower = tower;
	}

	virtual public void upgrade()
	{

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
		percent = Mathf.Pow (0.8f, level) + level*0.1f;
	}
	
	override public bool canUpgrade()
	{
		return true;
	}
	
	override public int getUpgradeCost()
	{
		return Mathf.RoundToInt((Mathf.Pow(1.9f, level) - 0.65f*level)*25);
	}
}
