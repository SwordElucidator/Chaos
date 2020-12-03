using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Panzer : AutoObject
{
    public int bulletGroupSize = 5;
    public float innerInterval = 0.2f;
    public float outerInterval = 2f;
    public Transform muzzle;

    protected override void ExtraStart()
    {
        StartCoroutine(StartFire());
    }

    private IEnumerator StartFire()
    {
        yield return new WaitForSeconds(outerInterval);
        for (var i = 0; i < bulletGroupSize; i++)
        {
            var bullet = Pool.MainPool.GetBullet();
            var position = muzzle.transform.position;
            var dir = position - transform.position;
            bullet.GetComponent<Bullet>().Active(transform.parent, position, new Vector2(dir.x, dir.y).normalized);
            yield return new WaitForSeconds(innerInterval);
        }

        StartCoroutine(StartFire());
    }

}
