using System.Collections;
using UnityEngine;
using UnityEngine.TextCore.Text;

public abstract class StatusEffect
{
    public TriggerKeyword statusEffectKeyword;
    public float Duration { get; protected set; }
    public abstract void Apply(BaseController character);
    public abstract void Remove(BaseController character);
}

public class Bleeding : StatusEffect
{
    private float lastDamageTime = 0f;
    private int damagePerSecond;
    private BaseController target;
    public Bleeding(int damagePerSecond, float duration)
    {
        this.damagePerSecond = damagePerSecond;
        Duration = duration;
    }

    public override void Apply(BaseController character)
    {
        statusEffectKeyword = TriggerKeyword.Bleeding;
        target = character;
        target.UpdateStatus += UpdateStatus;
    }

    public override void Remove(BaseController character)
    {
        character.UpdateStatus -= UpdateStatus;
    }

    private void UpdateStatus()
    {
        if (Time.time >= lastDamageTime + 0.5f)
        {
            lastDamageTime = Time.time;
            target.GetDamageByDotDamage(damagePerSecond / 2);

            Duration -= 1f; // Every second we reduce the duration.
            if (Duration <= 0)
            {
                // If duration is done, remove effect.
                Remove(target);
                target.RemoveEffect(this);
            }
        }
    }
}
public class Stun : StatusEffect
{
    private BaseController target;
    private State originState;
    private float applyTime;

    public Stun(int damagePerSecond, float duration)
    {
        Duration = duration;
    }

    public override void Apply(BaseController character)
    {
        statusEffectKeyword = TriggerKeyword.Stun;
        target = character;
        originState = target.currentState;
        target.ChangeState(new StunState());
        applyTime = Time.time; // Save the time of application
        target.UpdateStatus += UpdateStatus;
    }

    public override void Remove(BaseController character)
    {
        character.UpdateStatus -= UpdateStatus;
        target.ChangeState(originState);
    }

    private void UpdateStatus()
    {
        // Check if the duration has passed
        if (Time.time >= applyTime + Duration)
        {
            // If duration is done, remove effect.
            Remove(target);
            target.RemoveEffect(this);
        }
    }
}
//public class Buff : StatusEffect
//{
//    public override void Apply(Character character)
//    {
//        // Apply buff logic here
//    }

//    public override void Remove(Character character)
//    {
//        // Remove buff logic here
//    }
//}