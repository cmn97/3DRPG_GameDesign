using System;
using System.Collections.Generic;

public class StatBase
{
    public int baseValue;
    private readonly List<StatModifier> statModifiers;

    // Get value
    private bool valueUpdated = true;
    private int _value;

    public int Value {
        get {
            if(valueUpdated) { 
                _value = CalculateFinalValue();
                valueUpdated = false;
            }
            return _value;
        }
    }

    // Constructor
    public StatBase(int BaseValue)
    {
        baseValue = BaseValue;
        statModifiers = new List<StatModifier>();
    }

    // Calculations
    public void AddModifier(StatModifier mod)
    {
        valueUpdated = true;
        statModifiers.Add(mod);
    }
    public bool RemoveModifier(StatModifier mod)
    {
        valueUpdated = true;
        return statModifiers.Remove(mod);
    }
    private int CalculateFinalValue()
    {
        int finalValue = baseValue;
        for (int i = 0; i < statModifiers.Count; i++)
        {
            finalValue += statModifiers[i].value;
        }
        
        return finalValue;
    }
}
