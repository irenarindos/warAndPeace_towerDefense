﻿using UnityEngine;
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
		this.health = 50 + wave*50;
		this.maxHealth = this.health;
		this.value = wave;
		this.speed = 0.9f;
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

public class LargeCreep : CreepModule 
{
	private float health;
	private float maxHealth;
	private int value;
	private float speed;
	
	public LargeCreep(int wave)
	{
		this.health = 200 + wave*80;
		this.maxHealth = this.health;
		this.value = 5*wave;
		this.speed = 0.7f;
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

public class BossCreep : CreepModule 
{
	private float health;
	private float maxHealth;
	private int value;
	private float speed;
	
	public BossCreep(int wave)
	{
		this.health = 2500 + wave*250;
		this.maxHealth = this.health;
		this.value = 100*wave + 100;
		this.speed = 0.5f;
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

public class FastCreep : CreepModule 
{

	override public float getSpeed(float speed)
	{
		return speed*1.7f;
	}

	override public int getPriority()
	{
		return 2;
	}

}

public class CreepShield : CreepModule 
{
	private float lastShield = 0;
	override public float damage(float dmg)
	{
		if (lastShield + 5 <= Time.time)
		{
			lastShield = Time.time;
			dmg = 0;
		}
		return dmg;
	}

	override public int getPriority()
	{
		return -1;
	}
	
}
