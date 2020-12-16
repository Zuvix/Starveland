using System;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class RandomUnitOutputItem : RandomOutputItem
{
    public GameObject OfferedUnit;
    public override object GetStoredItem()
    {
        return OfferedUnit;
    }
}
