using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Pool : MonoBehaviour
{

    public static Pool MainPool;
    public Queue<GameObject> bullets;
    public Transform poi;

    public GameObject bullet;
    // Start is called before the first frame update
    public GameObject[] collectableProtos;
    public float[] collectableCounts;
    
    public Queue<GameObject> superKicks;
    public GameObject superKick;
    
    
    void Start()
    {
        MainPool = this;
        bullets = new Queue<GameObject>();
        superKicks = new Queue<GameObject>();
        for (var i = 0; i < 100; i++)
        {
            bullets.Enqueue(Instantiate(bullet, transform));
        }

        for (var i = 0; i < 10; i++)
        {
            superKicks.Enqueue(Instantiate(superKick, transform));
        }
        
        for (var index = 0; index < collectableCounts.Length; index++)
        {
            var count = collectableCounts[index];
            var collectableObject = collectableProtos[index];
            // 直接放置
            var spsize = poi.GetComponent<SpriteMask>().bounds.size.x / 2;
            Debug.Log(spsize);
            for (var i = 0; i < count; i++)
            {
                var dis = spsize * Random.value;
                var randomAngle = 360 * Random.value;
                var newCo = Instantiate(collectableObject);
                var bias = new Vector3((float) (dis * Math.Cos(randomAngle)), (float) (dis * Math.Sin(randomAngle)), 0);
                newCo.transform.position =
                    poi.position + bias;
                newCo.SetActive(true);
            }
        }
    }

    public GameObject GetBullet()
    {
        return bullets.Dequeue();
    }

    public void ReceiveBullet(Bullet b)
    {
        bullets.Enqueue(b.gameObject);
    }
    
    public GameObject GetSuperKick()
    {
        return superKicks.Dequeue();
    }

    public void ReceiveSuperKick(GameObject b)
    {
        superKicks.Enqueue(b);
    }
}
