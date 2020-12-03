using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpItem : CollectableObject
{
    public override void OnCollection(Person person)
    {
        base.OnCollection(person);
        person.GainHp(1);
    }
}
