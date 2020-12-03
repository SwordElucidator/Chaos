using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Enemy : Person
{
    
    public float maxAutoChangeDirectionTime = 5f;
    
    private Vector2 _currentMovement = new Vector2();
    private float _autoChangeDirectionTime = 0.5f;

    protected override void PostStart()
    {
        _autoChangeDirectionTime = Random.Range(0, maxAutoChangeDirectionTime);
    }
    
    void Update()
    {
        if (_currentMovement == new Vector2())
        {
            // 没有动作
            _currentMovement = new Vector2(Random.value > 0.5 ? 1 : -1, Random.value > 0.5 ? 1 : -1);
        }
        Move(_currentMovement.x, _currentMovement.y);
        _autoChangeDirectionTime -= Time.deltaTime;
        if (_autoChangeDirectionTime < 0)
        {
            _autoChangeDirectionTime = Random.Range(0, maxAutoChangeDirectionTime);
            var spsize = poi.GetComponent<SpriteMask>().bounds.size.x / 2;
            if ((poi.position - transform.position).magnitude >= spsize * 0.8)  // 危险
            {
                var dis = poi.position - transform.position;
                _currentMovement = (new Vector2(dis.x, dis.y).normalized + new Vector2(Random.Range(-0.15f, 0.15f), 0)).normalized;
            }
            else
            {
                _currentMovement  = new Vector2(Random.value > 0.5 ? 1 : -1, Random.value > 0.5 ? 1 : -1);
            }
            
        }
    }
}
