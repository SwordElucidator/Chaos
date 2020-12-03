using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Person : MonoBehaviour
{


    public int hp = 5;
    public float speed = 4f;
    public float moveForce = 40f;
    public float mass = 2f;
    public float friction = 20f;
    public Transform poi;
    public float hitForce = 1000f;
    public float hitCD = 3f;
    public float hitOutOfControlTime = 1f;
    public GameObject canHitObject;
    public bool isPlayer = false;
    public CentralController central;
    public float initialImmunityDuration = 2f;
    

    private bool _isDead = false;
    private bool _isRespawning = false;
    private bool _canHit = false;
    private int _hitRecoverIndex = 0;
    private bool _isHittedOutOfControl = false; // 撞击失控
    private int _hittedOutOfControlIndex = 0;
    private bool _isImmune = false;
    private GameObject _superKick = null;
    
    protected Rigidbody2D _rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _rigidbody.mass = mass;
        GetComponent<FrictionJoint2D>().maxForce = friction;
        StartCoroutine(RecoverHit(_hitRecoverIndex));
        PostStart();
        if (isPlayer) central.leftHpText.text = "HEART × " + hp;
        StartCoroutine(SetImmune());
    }

    protected virtual void PostStart()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }
    
    private IEnumerator SetImmune()
    {
        _isImmune = true;
        GetComponent<CircleCollider2D>().enabled = false;
        var color = GetComponent<SpriteRenderer>().color;
        GetComponent<SpriteRenderer>().color = new Color(color.r, color.g, color.b, 0.235f);
        yield return new WaitForSeconds(initialImmunityDuration);
        _isImmune = false;
        GetComponent<SpriteRenderer>().color = new Color(color.r, color.g, color.b, 1f);
        GetComponent<CircleCollider2D>().enabled = true;
        _isRespawning = false;
    }


    protected void Move(float moveHorizontal, float moveVertical)
    {
        if (_isDead || _isHittedOutOfControl)
        {
            return;
        }
        // 运动系统
        var xM = moveHorizontal;
        var yM = moveVertical;
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
        if (other.gameObject.CompareTag("Poi") && !_isRespawning)
        {
            TriggerDeath();
        }
    }

    public void TriggerDeath()
    {
        if (_isDead) return;
        _isDead = true;
        GetComponent<SpriteRenderer>().color = Color.red;
        hp -= 1;
        if (isPlayer)
            if (isPlayer) central.leftHpText.text = "HEART × " + hp;
        if (hp > 0)
        {
            StartCoroutine(GoRespawn());
        }
        else
        {
            // GAME OVER
            if (isPlayer)
            {
                central.gameOver.SetActive(true);
            }
            else
            {
                if (central.Win())
                {
                    // 玩家赢了！
                    central.gameOver.GetComponent<Text>().text = "YOU WIN!";
                    central.gameOver.SetActive(true);
                }
            }
        }
    }

    private IEnumerator GoRespawn()
    {
        _isRespawning = true;
        yield return new WaitForSeconds(3f);
        Respawn();
    }

    private void Respawn()
    {
        var col = poi.GetComponent<CircleCollider2D>();
        var bounds = col.bounds;
        var radius = bounds.extents.x * 0.8f;  // 别太靠边
        var center = bounds.center;
        var dis = Random.Range(0, radius);
        var cor = Random.Range(0, (float)(2 * Math.PI));
        var newDes = center + new Vector3((float)(dis * Math.Cos(cor)), (float)(dis * Math.Sin(cor)), transform.position.z);
        // 在这出生
        transform.position = newDes;
        GetComponent<SpriteRenderer>().color = Color.white;
        _isDead = false;
        StartCoroutine(SetImmune());
    }

    public IEnumerator RecoverHit(int index)
    {
        yield return new WaitForSeconds(hitCD);
        if (index == _hitRecoverIndex)
        {
            _canHit = true;
            canHitObject.SetActive(true);
            // 展示可以
        }
    }

    public void SetHitState(Vector2 force)
    {
        if (_isImmune)
        {
            return;
        }
        gameObject.GetComponent<Rigidbody2D>().AddForce(force);
        _isHittedOutOfControl = true;
        _hittedOutOfControlIndex += 1;

        StartCoroutine(HitLast(_hittedOutOfControlIndex));

    }

    private IEnumerator HitLast(int index)
    {
        yield return new WaitForSeconds(hitOutOfControlTime);
        if (index == _hittedOutOfControlIndex)
        {
            _isHittedOutOfControl = false;
        }
    }
    
    public void OnCollisionEnter2D(Collision2D other)
    {
        if (_isImmune)
        {
            return;
        }
        if (other.gameObject.CompareTag("Human") && _canHit)
        {
            var contactPoint = other.contacts[0];  // 找到撞击点
            // 从撞击点算
            // 朝着反方向
            var otherPos = other.transform.position;
            var movement = (new Vector2(otherPos.x, otherPos.y) - contactPoint.point).normalized;
            // var newDir = Vector3.Reflect(curDir, contactPoint.normal);
            // Quaternion rotation = Quaternion.FromToRotation(Vector3.forward, newDir);
            // transform.rotation = rotation;
            if (_superKick)
            {
                other.gameObject.GetComponent<Person>().SetHitState(movement * (hitForce * 2));
                _superKick.transform.parent = Pool.MainPool.transform;
                _superKick.SetActive(false);
                Pool.MainPool.ReceiveSuperKick(_superKick);
                _superKick = null;
            }
            else
            {
                other.gameObject.GetComponent<Person>().SetHitState(movement * hitForce);
            }
            
        }
        _canHit = false;
        canHitObject.SetActive(false);
        _hitRecoverIndex += 1;
        StartCoroutine(RecoverHit(_hitRecoverIndex));  // 重新开始
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        // 捡东西
        if (other.gameObject.CompareTag("Collectable"))
        {
            other.gameObject.GetComponent<CollectableObject>().OnCollection(this);
        }
    }

    public void GainHp(int amount)
    {
        hp += amount;
        if (isPlayer) central.leftHpText.text = "HEART × " + hp;
    }

    public IEnumerator ModifySpeed(int speedChange, int moveForceChange, int time)
    {
        speed += speedChange;
        moveForce += moveForceChange;
        yield return new WaitForSeconds(time);
        speed -= speedChange;
        moveForce -= moveForceChange;
    }

    public void GainSuperKick()
    {
        if (_superKick) return;
        var superKick = Pool.MainPool.GetSuperKick();
        _superKick = superKick;
        _superKick.transform.parent = transform;
        _superKick.transform.localPosition = new Vector3(0, 0, _superKick.transform.position.z);
        superKick.SetActive(true);
    }
}
