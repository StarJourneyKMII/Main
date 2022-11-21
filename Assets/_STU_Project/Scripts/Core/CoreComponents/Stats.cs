using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : CoreComponent
{
    public event Action OnHealthZero;
    
    [SerializeField] private float maxHealth = 1;
    [SerializeField] private int startStarCount = 3;
    [SerializeField] private int maxStarCount = 10;

    private float currentHealth;
    public int StartStarCount { get { return startStarCount; } }
    public int MaxStarCount { get { return maxStarCount; } }

    protected override void Awake()
    {
        base.Awake();

        currentHealth = maxHealth;
    }

    public void DecreaseHealth(float amount)
    {
        currentHealth -= amount;

        if(currentHealth <= 0)
        {
            currentHealth = 0;
            
            OnHealthZero?.Invoke();
        }
    }

    public void IncreaseHealth(float amount)
    {
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
    }
}
