using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperKickItem : CollectableObject
{
    public override void OnCollection(Person person)
    {
        base.OnCollection(person);
        person.GainSuperKick();
    }
}
