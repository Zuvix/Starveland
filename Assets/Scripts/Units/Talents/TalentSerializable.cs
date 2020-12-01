using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TalentSerializable
{
    public string Name;
    public string Description;
    public Sprite icon;
    [Tooltip("Each element is talent tier. Size must be same as the number of recieve talent levels except last which is reserved for ultimate talent!")]
    public List<EffectList> EffectList;

    public (string, string, List<EffectList>, Sprite) Unpack()
    {
        return (Name, Description, EffectList, icon);
    }

} 

[Serializable]
public class Effect
{
    public string effectDescription;
    public int effectValue;
}

[Serializable]
public class EffectList
{
    public List<Effect> Effects;
}

