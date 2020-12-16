using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceSourceFishing : ResourceSourceGeneric
{
    public List<RandomResourceOutputItem> Output;
    protected override Resource RetrieveResource(int _, out bool isDepleted)
    {
        isDepleted = false;
        Resource Result = RandomItemChoice.SelectRandomOutputItem<Resource>(Output);
        return Result;
    }
}
