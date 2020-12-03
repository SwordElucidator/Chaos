using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    
    public float speed = 10f;
    public float lifetime = 3f;
    private Vector2 _direction;
    private bool _active = false;

    // Start is called before the first frame update

    public void Active(Transform parent, Vector3 pos, Vector2 direction)
    {
        transform.parent = parent;
        transform.position = pos;
        transform.right = direction;
        gameObject.SetActive(true);
        _direction = direction;
        _active = true;
        StartCoroutine(AutoDeath());
    }

    private IEnumerator AutoDeath()
    {
        yield return new WaitForSeconds(lifetime);
        Respawn();
    }
    

    // Update is called once per frame
    void Update()
    {
        if (!_active) return;
        transform.position += new Vector3(_direction.x, _direction.y, 0) * (speed * Time.deltaTime);
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            // 飞出宇宙
            Respawn();
        }
    }

    private void Respawn()
    {
        if (!_active) return;
        _active = false;
        gameObject.SetActive(false);
        transform.parent = Pool.MainPool.transform;
        Pool.MainPool.ReceiveBullet(this);
    }
    
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Human"))
        {
            // 中弹
            other.gameObject.GetComponent<Person>().TriggerDeath();
        }
        if (other.gameObject.CompareTag("Human") || other.gameObject.CompareTag("Chengguan")) Respawn();
    }
}
