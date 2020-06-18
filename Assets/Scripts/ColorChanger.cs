using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChanger : MonoBehaviour
{
    public static Boolean isUpdate = false;
    GameObject[,] colorArray = new GameObject[7,25];
    public static int[,] array;

    void Start()
    {
        for (int j = 0; j < 7; j++)
        {
            String name = null;
            switch (j)
            {
                case 0: name = "Red"; break;
                case 1: name = "Orange"; break;
                case 2: name = "Green"; break;
                case 3: name = "LightBlue"; break;
                case 4: name = "Blue"; break;
                case 5: name = "Purple"; break;
                case 6: name = "Pink"; break;
            }
            for (int i = 0; i < 25; i++)
            {
                colorArray[j,i] = GameObject.Find(name+" (" + (i + 1) + ")");
            }
        }
        
    }

    void Update()
    {
        if (isUpdate)
        {
            for (int j = 0; j < 7; j++)
            {
                for (int i = 0; i < 25; i++)
                {
                    colorArray[j, i].SetActive(false);
                }
            }

            for (int i = 0; i < 25; i++)
            {
                switch (array[1,i])
                {
                    case 4:
                        colorArray[0,i].SetActive(true);
                        break;
                    case 5:
                        colorArray[1,i].SetActive(true);
                        break;
                    case 6:
                        colorArray[2,i].SetActive(true);
                        break;
                    case 7:
                        colorArray[3,i].SetActive(true);
                        break;
                    case 8:
                        colorArray[4,i].SetActive(true);
                        break;
                    case 9:
                        colorArray[5,i].SetActive(true);
                        break;
                    case 10:
                        colorArray[6,i].SetActive(true);
                        break;
                    defalt: break;
                }
            }
            isUpdate = false;
        }
    }


}
