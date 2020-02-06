using UnityEngine;
using System;


public class RotateY:MonoBehaviour{
    
    public void Start() {
    
    }
    
    public void Update() {
    	var tmp_cs1=transform.rotation;
        var tmp_cs2=tmp_cs1.eulerAngles;
        tmp_cs2.y+=1 * Time.deltaTime;
        tmp_cs1.eulerAngles=tmp_cs2;
        transform.rotation=tmp_cs1;
    }
}