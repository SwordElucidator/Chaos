using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableObject : MonoBehaviour
{
    public virtual void OnCollection(Person person)
    {
        this.gameObject.SetActive(false);
        // TODO 回收
    }
}
