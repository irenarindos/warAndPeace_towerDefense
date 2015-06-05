using UnityEngine;
using System.Collections;

public class ImpactEffect  {
	public virtual void apply(Creep target, TowerBehavior source)
	{
	}
}

public class SlowEffect : ImpactEffect {
	private float duration;
	private float percent;

	public SlowEffect(float duration, float percent)
	{
		this.duration = duration;
		this.percent = percent;
	}

	public override void apply(Creep target, TowerBehavior source)
	{
		SlowModule mod = target.getModule<SlowModule>();
		mod.activate(duration, percent);
	}
}

public class MoneyEffect : ImpactEffect {
	private float amount;
	
	public MoneyEffect(float amount)
	{
		this.amount = amount;
	}
	
	public override void apply(Creep target, TowerBehavior source)
	{
		if (!target.dead)
		{
		    source.map.resources += Mathf.RoundToInt(amount);
		}
	}
}

public class SplashDamageEffect : ImpactEffect {
	private float radius;
	private float damage;
	
	public SplashDamageEffect(float radius, float damage)
	{
		this.radius = radius;
		this.damage = damage;
	}
	
	public override void apply(Creep target, TowerBehavior source)
	{
		foreach (Creep c in source.map.creeps)
		{
			if (c != target && (c.transform.position - target.transform.position).magnitude <= radius)
			{
				c.damage(damage);
				c.realizeDamage(damage);
			}
		}
		
	}
}

