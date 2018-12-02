using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeTimer : MonoBehaviour {

    public float lifeTime = 1f;
    void Start(){
        StartCoroutine(ExpireAfterSeconds());
    }

    IEnumerator ExpireAfterSeconds(){
        yield return new WaitForSeconds(this.lifeTime);
        Destroy(this.gameObject);
    }
}