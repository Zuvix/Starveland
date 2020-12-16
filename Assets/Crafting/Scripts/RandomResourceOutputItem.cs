using System;
[Serializable]
public class RandomResourceOutputItem : RandomOutputItem
{
    public Resource OfferedResource;

    public override object GetStoredItem()
    {
        return OfferedResource;
    }
}
