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

	virtual public float getProspectiveHealth(float health)
	{
		return health;
	}

	virtual public float realizeDamage(float dmg)
	{
		return damage (dmg);
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

	virtual public string getName()
	{
		return "";
	}

}

public class CreepType : CreepModule
{
	protected float health;
	protected float prospectiveHealth;
	protected float maxHealth;
	protected int value;
	protected float speed;

	override public float getHealth(float health)
	{
		return this.health;
	}

	override public float getProspectiveHealth(float health)
	{
		return this.prospectiveHealth;
	}
	
	override public float damage(float dmg)
	{
		this.prospectiveHealth -= dmg;
		return 0f;
	}

	override public float realizeDamage(float dmg)
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

	override public string getName()
	{
		return "CreepType";
	}
}

public class NormalCreep : CreepType 
{


	public NormalCreep(int wave)
	{
		this.health = 90 + wave*wave*10;
		this.prospectiveHealth = this.health;
		this.maxHealth = this.health;
		this.value = wave;
		this.speed = 0.9f;
	}

	override public string getName()
	{
		return "Normal Creep";
	}
}

public class LargeCreep : CreepType 
{
	
	public LargeCreep(int wave)
	{
		this.health = 100 + wave*wave*25;
		this.prospectiveHealth = this.health;
		this.maxHealth = this.health;
		this.value = 5*wave;
		this.speed = 0.7f;
	}

	override public string getName()
	{
		return "Large Creep";
	}
}

public class BossCreep : CreepType 
{

	public BossCreep(int wave)
	{
		this.health = 5000 + wave*wave*50;
		this.prospectiveHealth = this.health;
		this.maxHealth = this.health;
		this.value = 100*wave + 100;
		this.speed = 0.5f;
	}

	override public string getName()
	{
		return "Boss Creep";
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

	override public string getName()
	{
		return "Fast";
	}
}


public class SlowModule : CreepModule 
{
	private float expiration = 0.0f;
	private float percent = 1.0f;
	override public float getSpeed(float speed)
	{
		if (Time.time >= expiration) return speed;
		return speed*percent;
	}

	public void activate(float time, float percent)
	{
		expiration = Time.time + time;
		this.percent = Mathf.Min(percent, this.percent);
	}
	
	override public int getPriority()
	{
		return 6;
	}

	override public string getName()
	{
		return "Slowed";
	}
}

public class CreepShield : CreepModule 
{
	private float lastShield = 0;
	private bool shielded = false;
	override public float damage(float dmg)
	{
		if (lastShield + 5 <= Time.time)
		{
			lastShield = Time.time;
			shielded = true;
			dmg = 0;
		}
		return dmg;
	}

	override public float realizeDamage(float dmg)
	{
		if (shielded)
		{
			dmg = 0;
			shielded = false;
		}
		return dmg;
	}

	override public int getPriority()
	{
		return -10;
	}

	override public string getName()
	{
		return "Shielded";
	}
}

public class CreepArmor : CreepModule 
{
	override public float damage(float dmg)
	{

		return Mathf.Max (dmg - 50.0f, 0);
	}

	override public int getPriority()
	{
		return -2;
	}

	override public string getName()
	{
		return "Armored";
	}
}


public class CreepPlating : CreepModule 
{
	public float maximum = 50.0f;
	override public float damage(float dmg)
	{
		return Mathf.Min (dmg, maximum);
	}
	
	override public int getPriority()
	{
		return -1;
	}

	override public string getName()
	{
		if (maximum > 10)
		    return "Plated";
		if (maximum > 1)
			return "Heavily Plated";
		return "Very heavily plated";
	}
	
}

public class EnrageCreep : CreepModule 
{
	// increase movement speed by 1% for each % of missing health
	override public float getSpeed(float speed)
	{
		return speed*(2- (creep.health/creep.maxHealth));
	}
	
	override public int getPriority()
	{
		return 5;
	}

	override public string getName()
	{
		return "Enraged";
	}
}
