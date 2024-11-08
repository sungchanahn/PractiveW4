using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamagable
{
    void TakePhysicalDamage(int _damage);
}

public class PlayerCondition : MonoBehaviour, IDamagable
{
    public UICondition UiCondition;

    public Condition health { get { return UiCondition.Health; } }
    public Condition hunger { get { return UiCondition.Hunger; } }
    public Condition stamina { get { return UiCondition.Stamina; } }
    public Condition mana { get { return UiCondition.Mana; } }

    public float NoHungerHealthDecay;

    public event Action OnTakeDamage;

    private void Update()
    {
        hunger.Substract(hunger.PassiveValue * Time.deltaTime);
        stamina.Add(stamina.PassiveValue * Time.deltaTime);
        mana.Add(mana.PassiveValue * Time.deltaTime);

        if (hunger.CurValue == 0f)
        {
            health.Substract(NoHungerHealthDecay * Time.deltaTime);
        }

        if (health.CurValue == 0f)
        {
            Die();
        }
    }

    public void Heal(float _amount)
    {
        health.Add(_amount);
    }

    public void Eat(float _amount)
    {
        hunger.Add(_amount);
    }

    public void Die()
    {
        Debug.Log("Die");
    }

    public void TakePhysicalDamage(int _damage)
    {
        health.Substract(_damage);
        OnTakeDamage?.Invoke();
    }

    public bool CanUseCondition(Condition condition, float amount)
    {
        if (condition.CurValue - amount < 0f)
        {
            return false;
        }
        return true;
    }

    public void UseCondition(Condition condition, float amount)
    {
        condition.Substract(amount);
    }
}
