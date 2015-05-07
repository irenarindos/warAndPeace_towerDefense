using UnityEngine;
using System.Collections;

public class CreepModule  {
	protected Creep creep;
	
	virtual public void init(Creep creep)
	{
		this.creep = creep;
	}

	virtual public int getPriority()
	{
		return 1;
	}

	virtual public float getHealth(float health)
	{
		return health;
	}

	virtual public float damage(float dmg)
	{
		return dmg;
	}

	virtual public float getSpeed(float speed)
	{
		return speed;
	}

	virtual public int getValue(int value)
	{
		return value;
	}

	virtual public float getMaxHealth(float maxHealth)
	{
		return maxHealth;
	}

}

public class NormalCreep : CreepModule 
{
	private float health;
	private float maxHealth;
	private int value;
	private float speed;

	public NormalCreep(int wave)
	{
		this.health = 100 + wave*25;
		this.maxHealth = this.health;
		this.value = wave + 1;
		this.speed = 1.0f;
	}

	override public float getHealth(float health)
	{
		return this.health;
	}
	
	override public float damage(float dmg)
	{
		this.health -= dmg;
		return 0f;
	}
	
	override public float getSpeed(float speed)
	{
		return this.speed;
	}
	
	override public int getValue(int value)
	{
		return this.value;
	}
	
	override public float getMaxHealth(float maxHealth)
	{
		return this.maxHealth;
	}
}
