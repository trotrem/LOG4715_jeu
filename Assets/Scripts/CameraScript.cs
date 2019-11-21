using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour {

public static CameraScript instance;

private Vector3 _originalPos;
private float _timeAtCurrentFrame;
private float _timeAtLastFrame;
private float _fakeDelta;

public Transform bossCameraPos;
private bool isBossTime = false;
public float bossPanTime = 2f;
Vector3 _ = Vector3.zero;

void Awake()
{
    instance = this;
}

void Update() {
    // Calculate a fake delta time, so we can Shake while game is paused.
    _timeAtCurrentFrame = Time.realtimeSinceStartup;
    _fakeDelta = _timeAtCurrentFrame - _timeAtLastFrame;
    _timeAtLastFrame = _timeAtCurrentFrame; 
    if(isBossTime){
        transform.position = Vector3.SmoothDamp(transform.position, bossCameraPos.position, ref _, bossPanTime);
    }
}

public static void Shake (float duration, float amount) {
    instance._originalPos = instance.gameObject.transform.localPosition;
    instance.StopAllCoroutines();
    instance.StartCoroutine(instance.cShake(duration, amount));
}

public static void SetupForBoss(){
    instance.transform.SetParent(instance.bossCameraPos, true);
    instance.isBossTime = true;
}

public static void move(Vector3 newPos)
{
    if(!instance.isBossTime){
        instance._originalPos = newPos;
        instance.transform.position = newPos;
    }
}

public IEnumerator cShake (float duration, float amount) {
    float endTime = Time.time + duration;

    while (duration > 0) {
        transform.localPosition = _originalPos + Random.insideUnitSphere * amount;

        duration -= _fakeDelta;

        yield return null;
    }

    transform.localPosition = _originalPos;
}
}