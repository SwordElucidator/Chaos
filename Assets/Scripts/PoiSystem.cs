using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoiSystem : MonoBehaviour
{

    public float[] shrinkWaitingTimes;  // 缩小时间
    public float[] shrinkDurations;  // 缩小时长
    public float[] shrinkTos;  // 缩小
    public float initialMaximumBias = 20;
    public float initialWidth = 12;

    public int nextShrinkIndex = 0;

    
    private float _nextRadius = 0;
    private Vector3 _nextCenter;
    private float _leftTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        // 初始化时应随机位置
        transform.position = new Vector3(Random.Range(- initialMaximumBias, initialMaximumBias), Random.Range(- initialMaximumBias, initialMaximumBias), 0);
        transform.localScale = new Vector3(initialWidth, initialWidth, 1);
        
        // 多久以后开始收缩？
        StartCoroutine(Shrink());
    }

    private IEnumerator Shrink()
    {
        if (nextShrinkIndex >= shrinkWaitingTimes.Length) yield break;
        
        yield return new WaitForSeconds(shrinkWaitingTimes[nextShrinkIndex]);
        // 开始缩小
        
        var toRadius = shrinkTos[nextShrinkIndex];
        var duration = shrinkDurations[nextShrinkIndex];
        
        // 计算位置
        var currentRadius = transform.localScale.x;
        var xRelPos = Random.Range(toRadius - currentRadius, currentRadius - toRadius);
        var yRelPos = Random.Range(toRadius - currentRadius, currentRadius - toRadius);
        _nextCenter = transform.position + new Vector3(xRelPos, yRelPos, 0);  // 下一个中心
        _nextRadius = toRadius;
        _leftTime = duration;

        yield return new WaitForSeconds(duration);  // 收缩时间
        
        nextShrinkIndex += 1;
        StartCoroutine(Shrink());
    }

    // Update is called once per frame
    void Update()
    {
        if (_leftTime > 0)
        {
            // 开始收缩
            var position = transform.position;
            var speed = (_nextCenter - position) / _leftTime;
            var localScale = transform.localScale;
            var rSpeed = (new Vector3(_nextRadius, _nextRadius, 1) - localScale) / _leftTime;
            _leftTime -= Time.deltaTime;
            position = position + speed * Time.deltaTime;
            transform.position = position;
            localScale = localScale + rSpeed * Time.deltaTime;
            transform.localScale = localScale;
        }
    }
}
