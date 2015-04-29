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

	override public float getRange(float range)
	{
		return range*percent;
	}

	private int cost = 20;
	
	override public void upgrade()
	{
		percent *= 1.1f;
		cost *= 2;
	}
	
	override public bool canUpgrade()
	{
		return true;
	}
	
	override public int getUpgradeCost()
	{
		return cost;
	}
}

// increase damage
public class BombModule : TowerModule  {
	
	private float percent = 1.0f;
	
	override public float getDamage(float damage)
	{
		return damage*percent;
	}

	private int cost = 30;
	
	override public void upgrade()
	{
		percent *= 1.25f;
		cost *= 2;
	}
	
	override public bool canUpgrade()
	{
		return true;
	}
	
	override public int getUpgradeCost()
	{
		return cost;
	}
}

// decrease fire delay
public class GearModule : TowerModule  {
	
	private float percent = 0.9f;
	
	override public float getShootDelay(float delay)
	{
		return delay*percent;
	}

	private int cost = 25;

	override public void upgrade()
	{
		percent *= 0.9f;
		cost *= 2;
	}
	
	override public bool canUpgrade()
	{
		return true;
	}
	
	override public int getUpgradeCost()
	{
		return cost;
	}
}
