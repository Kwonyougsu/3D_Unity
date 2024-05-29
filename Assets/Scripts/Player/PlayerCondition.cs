using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build;
using UnityEngine;

public interface IDamagalbe
{
    void TakePhysicalDamage(int damage);
}

public class PlayerCondition : MonoBehaviour, IDamagalbe
{
    public UIConditions uIConditions;

    public Condition health { get { return uIConditions.health; } }
    public Condition hunger { get { return uIConditions.hunger; } }

    public Condition stamina{ get { return uIConditions.stamina; } }

    public float noHungerHealthDecay;

    public event Action onTakeDamage;

    void Update()
    {
        Debug.Log(hunger.gameObject.name);
        hunger.Add(hunger.passiveValue * Time.deltaTime);
        stamina.Add(stamina.passiveValue * Time.deltaTime);

        if (hunger.curValue < 0f)
        {
            health.Subtract(noHungerHealthDecay * Time.deltaTime);
        }
        if (health.curValue == 0f)
        {
            Die();
        }
    }

    public void Heal(float amout)
    {
        health.Add(amout);
    }
    public void Eat(float amout)
    {
        hunger.Add(amout);
    }

    private void Die()
    {
        Debug.Log("die");
    }

    public void TakePhysicalDamage(int damage)
    {
        health.Subtract(damage);
        onTakeDamage?.Invoke();
    }

    public bool UseStamina(float amount)
    {
        if (stamina.curValue - amount < 0)
        {
            return false;
        }
        stamina.Subtract(amount);
        return true;
    }
}
