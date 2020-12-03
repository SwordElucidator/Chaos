using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedUpItem : CollectableObject
{
    public override void OnCollection(Person person)
    {
        base.OnCollection(person);
        // _originalSpeed = person.speed;
        person.StartCoroutine(person.ModifySpeed(5, 100, 10));
    }
}
