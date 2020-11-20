using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Events;

public class UnitHungry
{
    private readonly int MaxSatiety;
    private int CurrentSatiety;
    public Unit Unit { get;  private set; }
    public UnityEvent<float> OnEatUpgrade; 

    public UnitHungry(Unit Unit, int MaxSatiety = 5)
    {
        this.MaxSatiety = MaxSatiety;
        this.CurrentSatiety = 0;
        this.Unit = Unit;
        this.OnEatUpgrade = new UnityEvent<float>();
    }
    public void Eat(int Amount)
    {
        this.CurrentSatiety += Amount;
        OnEatUpgrade.Invoke((float)this.CurrentSatiety/this.MaxSatiety);
    }
    public bool IsFed()
    {
        return this.CurrentSatiety >= MaxSatiety;
    }
}
