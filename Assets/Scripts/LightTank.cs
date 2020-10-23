using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightTank : AutoObject
{
    // 轻坦克简单，就是撞得狠
    public Transform hitEdge1;
    public Transform hitEdge2;
    public float superFacingHitForce = 5000f;
    
    public new void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Human"))
        {
            var contactPoint = other.contacts[0];  // 找到撞击点
            // 从撞击点算，如果撞击点处于正面才猛

            var hitAt = contactPoint.point;
            var edge1 = new Vector2(hitEdge1.position.x, hitEdge1.position.y);
            var edge2 = new Vector2(hitEdge2.position.x, hitEdge2.position.y);

            var facing = (edge1 - hitAt).magnitude + (edge2 - hitAt).magnitude - (edge1 - edge2).magnitude <= 0.2f;
            
            // 在前边撞的
            // 朝着反方向
            var otherPos = other.transform.position;
            var movement = (new Vector2(otherPos.x, otherPos.y) - contactPoint.point).normalized;
            other.gameObject.GetComponent<Person>().SetHitState(movement * (facing ? superFacingHitForce : hitForce));
        }
    }
}
