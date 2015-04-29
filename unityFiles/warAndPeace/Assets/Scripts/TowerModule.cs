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

	private float percent = 1.1f;

	override public float getRange(float range)
	{
		return range*percent;
	}

	public static int cost = 25;
}

// increase damage
public class BombModule : TowerModule  {
	
	private float percent = 1.1f;
	
	override public float getDamage(float damage)
	{
		return damage*percent;
	}

	public static int cost = 20;
}

// decrease fire delay
public class GearModule : TowerModule  {
	
	private float percent = 0.9f;
	
	override public float getShootDelay(float delay)
	{
		return delay*percent;
	}

	public static int cost = 25;
}
