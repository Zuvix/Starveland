using System;
using UnityEngine;

[Serializable]
public class TalentSerializable
{
    public string Name;
    public string Description;
    public Sprite icon;
    public int[] Effect;
    public bool Ultimate;

    public (string, string, int[], Sprite, bool) Unpack()
    {
        return (Name, Description, Effect, icon, Ultimate);
    }

} 

