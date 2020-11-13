using System;
using System.Collections;
using UnityEngine;
class ActivityStateNull : ActivityState
{
    private static ActivityStateNull Inst = null;
    public static ActivityStateNull Instance
    {
        get
        {
            if (Inst == null)
            {
                Inst = new ActivityStateNull();
            }
            return Inst;
        }
    }
    public override IEnumerator PerformAction(Unit Unit)
    {
        yield return Unit.StartCoroutine(Unit.WaitEmpty());
    }
}