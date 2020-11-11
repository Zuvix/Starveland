using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class UnitAnimal : Unit
{
    protected override void Awake()
    {
        this.MovementSpeed = 10.0f;
        this.MaxHealth = 100;
        this.Health = this.MaxHealth;
        base.Awake();
    }
    protected override void Start()
    {
        base.Start();
        objectName = "Unicorn";
    }
}

