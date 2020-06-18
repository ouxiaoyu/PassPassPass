using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Wall : MonoBehaviour
{
    public static Boolean isReset = false;
    void Start()
    {
        Reset();
    }

    void Update()
    {
        if (isReset)
        {
            Reset();
            isReset = false;
        }
        
        float speed = 100;
        gameObject.transform.localPosition = Vector3.MoveTowards(gameObject.transform.localPosition, new Vector3(0, 0, 0), speed * Time.deltaTime);
        
    }
     void Reset()
    {
        gameObject.transform.localPosition = new Vector3(0, 0, 675);
    }
}
