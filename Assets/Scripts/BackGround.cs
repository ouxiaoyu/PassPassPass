using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGround : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float speed = 100;
        if (gameObject.transform.localPosition == new Vector3(0, 0, -600)) {
            gameObject.transform.localPosition = new Vector3(0, 0, 0);
        }
        gameObject.transform.localPosition = Vector3.MoveTowards(gameObject.transform.localPosition, new Vector3(0, 0, -600), speed * Time.deltaTime);
        
    }
}

