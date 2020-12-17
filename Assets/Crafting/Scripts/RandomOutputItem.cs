using System;
using System.Collections.Generic;
[Serializable]
public abstract class RandomOutputItem
{
    public int Probability;
    public abstract object GetStoredItem();
}
