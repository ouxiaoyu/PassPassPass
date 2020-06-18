using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedCube : MonoBehaviour
{
    public static Boolean isUpdate = false;
    public GameObject[] fixedCubeArray = new GameObject[25];
    public static int[,] array;

    void Start()
    {
        for (int i = 0; i < 25; i++)
        {
            fixedCubeArray[i] = GameObject.Find("FixedCube (" + (i + 1) + ")");
        }
    }

    void Update()
    {
        if (isUpdate)
        {
            for (int i = 0; i < 25; i++)
            {
                if (array[0,i]==1)
                {
                    fixedCubeArray[i].SetActive(true);
                }
                else
                {
                    fixedCubeArray[i].SetActive(false);
                }
            }
            isUpdate = false;
        }
    }
}
