using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AutoObject : MonoBehaviour
{
    // Start is called before the first frame update
    
    public float mass = 100f;
    private Rigidbody2D _rigidbody;
    public float speed = 3f;
    public float moveForce = 10f;
    public float autoChangeDirectionTime = 5f;
    public float hitForce = 500f;  // 不太疼
    public Transform poi;

    private Vector2 _currentMovement = new Vector2();
    private float _autoChangeDirectionTime = 5f;
    private bool _outOfPoi = false;
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _rigidbody.mass = mass;
        _autoChangeDirectionTime = autoChangeDirectionTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (_currentMovement == new Vector2())
        {
            // 没有动作
           _currentMovement  = new Vector2(Random.value > 0.5 ? 1 : -1, Random.value > 0.5 ? 1 : -1);
        }
        Move();
        _autoChangeDirectionTime -= Time.deltaTime;
        if (_autoChangeDirectionTime < 0)
        {
            _autoChangeDirectionTime = autoChangeDirectionTime;
            if (_outOfPoi)
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
    
    private void Move()
    {
        // 运动系统
        var xM = _currentMovement.x;
        var yM = _currentMovement.y;
        Vector2 movement = new Vector2(xM, yM);
        _rigidbody.AddForce(movement * moveForce);
        var velocity = _rigidbody.velocity;
        if (_rigidbody.velocity.magnitude >= speed)
        {
            velocity = velocity / velocity.magnitude * speed;
            _rigidbody.velocity = velocity;
        }

        if (Math.Abs(velocity.x) > 0.005f || Math.Abs(velocity.y) > 0.005f)
        {
            transform.right = velocity;
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Poi"))
        {
            _outOfPoi = true;
            var dis = other.transform.position - transform.position;
            _currentMovement = (new Vector2(dis.x, dis.y).normalized + new Vector2(Random.Range(-0.15f, 0.15f), 0)).normalized;
            _autoChangeDirectionTime = autoChangeDirectionTime;
        }
    }
    
    public void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Poi"))
        {
            _outOfPoi = false;
        }
        if (other.gameObject.CompareTag("Human"))  // 一般函数
        {
            var contactPoint = other.contacts[0];  // 找到撞击点
            // 从撞击点算，如果撞击点处于正面才猛
            // 朝着反方向
            var otherPos = other.transform.position;
            var movement = (new Vector2(otherPos.x, otherPos.y) - contactPoint.point).normalized;
            other.gameObject.GetComponent<Person>().SetHitState(movement * hitForce);
        }
    }
}
