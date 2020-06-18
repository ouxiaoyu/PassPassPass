using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Text;

public class MovableCube : MonoBehaviour
{
    public AudioClip successClip;
    public AudioClip failClip;
    public AudioSource source;
    public Animator animator1;
    public Animator animator2;
    public Animator animator3;
    public Animator animator4;
    public GameObject bg;
    public Image great;
    public Image perfect;
    GameObject[] shapeArray = new GameObject[25];
    GameObject[] fixedCubeArray = new GameObject[25];

    Boolean turnRight = false;
    Boolean turnLeft = false;
    Vector3 rotateCenter;
    Vector3[] offset = new Vector3[4] { 
        new Vector3(-5, 5, 0), 
        new Vector3(5, 5, 0),
        new Vector3(-5, -5, 0), 
        new Vector3(5, -5, 0)};
    Vector3[] cubeOffset = new Vector3[8] { 
        new Vector3(-10, 10, 0), 
        new Vector3(0, 10, 0),
        new Vector3(10, 10, 0),
        new Vector3(-10, 0, 0),
        new Vector3(10, 0, 0),
        new Vector3(-10, -10, 0),
        new Vector3(0, -10, 0),
        new Vector3(10, -10, 0)};
    Boolean[] checkExist;
    int rotateAngle;
    int checkAngle = 0;
    public Button leftButton;
    public Button rightButton;
    public static int score = 0;
    public static Boolean isUpdate = true;

    int[,] updateArray = new int[2,25];
    static int[,] previousArray = new int[2,25];
    static Boolean isFailed = false;
    public static int wallColor;
    int num_color = 2;

    public static Color[] colorArray = new Color[] {
                            new Color(239 / 255.0F, 27 / 255.0F, 57 / 255.0F, 255 / 255.0F),
                            new Color(255 / 255.0F, 162 / 255.0F, 32 / 255.0F, 255 / 255.0F),
                            new Color(74 / 255.0F, 211 / 255.0F, 0 / 255.0F, 255 / 255.0F),
                            new Color(0  / 255.0F, 233 / 255.0F, 190 / 255.0F, 255 / 255.0F),
                            new Color(28 / 255.0F, 145 / 255.0F, 239 / 255.0F, 255 / 255.0F),
                            new Color(169 / 255.0F, 78 / 255.0F, 255 / 255.0F, 255 / 255.0F),
                            new Color(255 / 255.0F, 32 / 255.0F, 215 / 255.0F, 255 / 255.0F),
    };
    public static String[] colorString = new String[] {
                            "Red",
                            "Orange",
                            "Green",
                            "LightBlue",
                            "Blue",
                            "Purple",
                            "Pink",
    };

    void Start()
    {
    }

    String LoadFile(string path, string name)
    {
        StreamReader sr = null;
        try
        {
            sr = File.OpenText(path + "//" + name);
        }
        catch (Exception e)
        {
            return "0";
        }
        string info;
        info = sr.ReadToEnd();
        sr.Close();
        sr.Dispose();
        return info;
    }

    void WriteFile(string path, string name,String info)
    {
        StreamWriter sw = new StreamWriter(path + "//" + name, false);
        sw.Write(info);
        sw.Close();
        sw.Dispose();
    }


    void FixedUpdate()
    {
        UpdateShape();
        

        float interval = Time.fixedDeltaTime;
        //UnityEngine.Debug.Log(Time.fixedDeltaTime);
        if (turnRight)
        {
            gameObject.transform.RotateAround(rotateCenter, Vector3.forward, -15 * 50 * interval );
            checkAngle+=15;
        }
        else if (turnLeft)
        {

            gameObject.transform.RotateAround(rotateCenter, Vector3.forward, 15 * 50 * interval );
            checkAngle+=15;
        }
        if (checkAngle == rotateAngle)
        {
            turnRight = false;
            turnLeft = false;
            checkAngle = 0;
            leftButton.interactable = true;
            rightButton.interactable = true;
        }
    }

    void OnTriggerEnter(Collider collider)
    {
       /* var name = collider.name;
        UnityEngine.Debug.Log("Name is " + name);*/
        
        if (collider.tag == "Finish" || (collider.tag == "Clear" && GameObject.Find("Shape").tag != gameObject.tag))
        {
            isFailed = true;
            Time.timeScale = 0;
            animator1.SetBool("isAppear", true);
            animator2.SetBool("isAppear", true);
            animator3.SetBool("isAppear", true);
            animator4.SetBool("isAppear", true);
            bg.SetActive(true);
            FailSoundPlay();

#if UNITY_ANDROID || UNITY_IOS
                if (score > int.Parse(LoadFile(Application.persistentDataPath, "Best.txt")))
                    WriteFile(Application.persistentDataPath, "Best.txt", score.ToString());
                    CanvasController.setBest = true;
#endif
            if (score > int.Parse(File.ReadAllText("Assets//Resources//Best.txt")))
                    File.WriteAllText("Assets//Resources//Best.txt", score.ToString(), Encoding.Default);
            CanvasController.setBest = true;
            score = 0;

        }
        else if (collider.tag == "Clear" && GameObject.Find("Shape").tag == gameObject.tag)
        {
            SuccessSoundPlay();
            Wall.isReset = true;
            isUpdate = true;
            AddScore();
        }
        else if (collider.tag == "Red")
        {

            gameObject.GetComponent<Renderer>().material.color = colorArray[0];
            gameObject.tag = "Red";
        }
        else if (collider.tag == "Orange")
        {
            gameObject.GetComponent<Renderer>().material.color = colorArray[1];
            gameObject.tag = "Orange";

        }
        else if (collider.tag == "Green")
        {
            gameObject.GetComponent<Renderer>().material.color = colorArray[2];
            gameObject.tag = "Green";

        }
        else if (collider.tag == "LightBlue")
        {
            gameObject.GetComponent<Renderer>().material.color = colorArray[3];
            gameObject.tag = "LightBlue";
        }
        else if (collider.tag == "Blue")
        {
            gameObject.GetComponent<Renderer>().material.color = colorArray[4];
            gameObject.tag = "Blue";
        }
        else if (collider.tag == "Purple")
        {
            gameObject.GetComponent<Renderer>().material.color = colorArray[5];
            gameObject.tag = "Purple";
        }
        else if (collider.tag == "Pink")
        {
            gameObject.GetComponent<Renderer>().material.color = colorArray[6];
            gameObject.tag = "Pink";
        }

    }

    Boolean[] GetNear()
    { 
        Boolean[] c = new Boolean[cubeOffset.Length];
        for (int i = 0; i < cubeOffset.Length; i++)
        {
            RaycastHit hit;
            if (Physics.Raycast(gameObject.transform.position + cubeOffset[i] + new Vector3(0, 0, -10), gameObject.transform.position + cubeOffset[i], out hit, 10))
            {
                c[i] = true;
            }
            else
            {
                c[i] = false;
            }
        }

        if (gameObject.transform.position.y <= 14)
        {
            c[5] = true;
            c[6] = true;
            c[7] = true;
        }

        return c;
    }

    int GetCenter(int[] a)
    {
        if (a[0] != 0)
        {
            return 0;
        }
        if (a[1] != 0)
        {
            return 1;
        }
        if (a[2] != 0)
        {
            return 2;
        }
        if (a[3] != 0)
        {
            return 3;
        }
        return -1;
    }

    int[] GetAngle(Boolean isRight)
    {
        int[] a = new int[4];

        if (isRight)
        {
            if (checkExist[1] && !checkExist[0] && !checkExist[3])
            {
                a[0] = 180;
            }
            else if (checkExist[1] && checkExist[0] && !checkExist[3])
            {
                a[0] = 90;
            }
            else
            {
                a[0] = 0;
            }

            if (checkExist[4] && !checkExist[2] && !checkExist[1])
            {
                a[1] = 180;
            }
            else if (checkExist[4] && checkExist[2] && !checkExist[1])
            {
                a[1] = 90;
            }
            else
            {
                a[1] = 0;
            }

            if (checkExist[3] && !checkExist[5] && !checkExist[6])
            {
                a[2] = 180;
            }
            else if (checkExist[3] && checkExist[5] && !checkExist[6])
            {
                a[2] = 90;
            }
            else
            {
                a[2] = 0;
            }

            if (checkExist[6] && !checkExist[7] && !checkExist[4])
            {
                a[3] = 180;
            }
            else if (checkExist[6] && checkExist[7] && !checkExist[4])
            {
                a[3] = 90;
            }
            else
            {
                a[3] = 0;
            }

        }
        else if (!isRight)
        {
            if (checkExist[3] && !checkExist[0] && !checkExist[1])
            {
                a[0] = 180;
            }
            else if (checkExist[3] && checkExist[0] && !checkExist[1])
            {
                a[0] = 90;
            }
            else
            {
                a[0] = 0;
            }

            if (checkExist[1] && !checkExist[2] && !checkExist[4])
            {
                a[1] = 180;
            }
            else if (checkExist[1] && checkExist[2] && !checkExist[4])
            {
                a[1] = 90;
            }
            else
            {
                a[1] = 0;
            }

            if (checkExist[6] && !checkExist[5] && !checkExist[3])
            {
                a[2] = 180;
            }
            else if (checkExist[6] && checkExist[5] && !checkExist[3])
            {
                a[2] = 90;
            }
            else
            {
                a[2] = 0;
            }

            if (checkExist[4] && !checkExist[7] && !checkExist[6])
            {
                a[3] = 180;
            }
            else if (checkExist[4] && checkExist[7] && !checkExist[6])
            {
                a[3] = 90;
            }
            else
            {
                a[3] = 0;
            }

        }
        else
        {
            a[0] = 0;
            a[1] = 0;
            a[2] = 0;
            a[3] = 0;
        }
        return a;
    }

    public void RotateRight() 
    {        
        if (turnLeft)
            return;
        leftButton.interactable = false;
        rightButton.interactable = false;
        checkExist = GetNear();
        rotateCenter = gameObject.transform.position + offset[GetCenter(GetAngle(true))];
        rotateAngle = GetAngle(true)[GetCenter(GetAngle(true))];

        if (GetCenter(GetAngle(true))==-1 || Math.Abs(GetNextXPoition(GetCenter(GetAngle(true)), rotateAngle, true))> 25 || GetNextYPoition(GetCenter(GetAngle(true)), rotateAngle, true) < 10 || GetNextYPoition(GetCenter(GetAngle(true)), rotateAngle, true) > 60 )
        {
            leftButton.interactable = true;
            rightButton.interactable = true;
            return;
        } 
        turnRight = true;
        
    }

    public void RotateLeft()
    {
        if (turnRight)
            return;
        leftButton.interactable = false;
        rightButton.interactable = false;
        checkExist = GetNear();
        
        rotateCenter = gameObject.transform.position + offset[GetCenter(GetAngle(false))];
        rotateAngle = GetAngle(false)[GetCenter(GetAngle(false))];

        if (GetCenter(GetAngle(false)) == -1 || Math.Abs(GetNextXPoition(GetCenter(GetAngle(false)), rotateAngle, false)) > 25 || GetNextYPoition(GetCenter(GetAngle(false)), rotateAngle, false) < 10 || GetNextYPoition(GetCenter(GetAngle(false)), rotateAngle, false) > 60 )
        {
            leftButton.interactable = true;
            rightButton.interactable = true;
            return;
        }
        turnLeft = true;
    }

    float GetNextXPoition(int center,int angle,Boolean isRight)
    {
        if (isRight) 
        {
            switch (center)
            {
                case 0: if (angle == 0) return gameObject.transform.position.x; else return gameObject.transform.position.x - 10; break;
                case 1: if (angle == 180) return gameObject.transform.position.x + 10; else return gameObject.transform.position.x; break;
                case 2: if (angle == 180) return gameObject.transform.position.x - 10; else return gameObject.transform.position.x; break;
                case 3: if (angle == 0) return gameObject.transform.position.x; else return gameObject.transform.position.x + 10; break;
                default: return 0; break;
            }
        }
        else
        {
            switch (center)
            {
                case 0: if (angle == 180) return gameObject.transform.position.x-10; else return gameObject.transform.position.x; break;
                case 1: if (angle == 0) return gameObject.transform.position.x; else return gameObject.transform.position.x + 10; break;
                case 2: if (angle == 0) return gameObject.transform.position.x; else return gameObject.transform.position.x - 10; break;
                case 3: if (angle == 180) return gameObject.transform.position.x + 10; else return gameObject.transform.position.x; break;
                default: return 0; break;
            }
        }
    }

    float GetNextYPoition(int center, int angle, Boolean isRight)
    {
        if (isRight)
        {
            switch (center)
            {
                case 0: if (angle == 180) return gameObject.transform.position.y + 10; else return gameObject.transform.position.y ; break;
                case 1: if (angle == 0) return gameObject.transform.position.y; else return gameObject.transform.position.y + 10; break;
                case 2: if (angle == 0) return gameObject.transform.position.y; else return gameObject.transform.position.y - 10; break;
                case 3: if (angle == 180) return gameObject.transform.position.y - 10; else return gameObject.transform.position.y; break;
                default: return 0; break;
            }
        }
        else
        {
            switch (center)
            {
                case 0: if (angle == 0) return gameObject.transform.position.y; else return gameObject.transform.position.y + 10; break;
                case 1: if (angle == 180) return gameObject.transform.position.y + 10; else return gameObject.transform.position.y; break;
                case 2: if (angle == 180) return gameObject.transform.position.y - 10; else return gameObject.transform.position.y; break;
                case 3: if (angle == 0) return gameObject.transform.position.y; else return gameObject.transform.position.y - 10; break;
                default: return 0; break;
            }
        }
    }
    
    void AddScore()
    {
        
        score++;
        if (!CanvasController.setScore)
        {
            CanvasController.setScore = true;
        }
    }

    void UpdateShape()
    {
        if (isUpdate)
        {
            if (isFailed) 
            {
                updateArray = (int[,])previousArray.Clone();
                isFailed = false;
                //UnityEngine.Debug.Log("isFail");
            }
            else
            {
                wallColor = UnityEngine.Random.Range(4, 11);
                updateArray = (int[,])CreateShape(wallColor).Clone(); 
/*                for (int i=0;i<2;i++)
                {
                    for (int j=0;j<25;j++)
                    {
                    UnityEngine.Debug.Log(wallColor+":"+i+" "+j+":"+updateArray[i, j]);
                    }
                }*/
                
            }
            //fixedcube
            FixedCube.array = updateArray;
            FixedCube.isUpdate = true;
            //shape
            Shape.array = updateArray;
            Shape.wall_color = wallColor;
            Shape.isUpdate = true;            
            //color
            ColorChanger.array = updateArray;
            ColorChanger.isUpdate = true;
            //movable cube
            SetMovableCube(updateArray);
            gameObject.GetComponent<Renderer>().material.color = new Color(0 / 255.0F, 0 / 255.0F, 0 / 255.0F, 255 / 255.0F);

            isUpdate = false;
            previousArray= (int[,])updateArray.Clone();
        }
        if (ColorChanger.isUpdate)
        {
            Destroy(gameObject.GetComponent<Rigidbody>());
            gameObject.tag = "MovableCube";
        }
        else
        {
            if (gameObject.GetComponent<Rigidbody>() == null)
            {
                gameObject.AddComponent<Rigidbody>();
                gameObject.GetComponent<Rigidbody>().useGravity = false;
                gameObject.GetComponent<Rigidbody>().angularDrag = 0;
            }

        }
    }

    public void SetMovableCube(int[,] array) {
        for (int i = 0; i < 25; i++)
        {
            if(array[0,i]==2)
            {
                gameObject.transform.localPosition = new Vector3(-20 + 10 * (i % 5), 55 - 10 * (i / 5), 115);
            }
        }
    }

    public void SuccessSoundPlay()
    {
        source.PlayOneShot(successClip);
    }
    public void FailSoundPlay()
    {
        source.PlayOneShot(failClip);
    }

        public int[,] CreateShape(int wall_color)
    {
        int[,] shape = new int[2,25];
        int[] grow_dir = { -1, 1, -5, 5 };
        int[] move_dir = { -1, 1, -5, 5, -4, 4, -6, 6 };
        int num_one = UnityEngine.Random.Range(0, 5) + 5;
        int index_two = 0;
        int[] index = new int[num_one];
        int center = UnityEngine.Random.Range(0, 25);
        index[0] = center;
        shape[0,center] = 1;
        int it = 1;
        while (it < index.Length + 1)
        {
            //            System.out.println("it"+it);
            int prev;
            prev = index[UnityEngine.Random.Range(0, it)];

            int direction = grow_dir[UnityEngine.Random.Range(0, 4)];

            int next = prev + direction;
            //            System.out.println("it "+it);
            //            System.out.println("prev "+prev);
            //            System.out.println("next "+next);
            Boolean isEdge = (direction == 1 || direction == -1) && ((prev + next) % 10 == 9);
            //            System.out.println("isEdge "+isEdge);
            if (next >= 0 && next < 25 && !isEdge && shape[0,next] == 0)
            {
                if (it == num_one)
                {
                    index_two = next;
                    shape[0,next] = 2;
                }
                else
                {
                    index[it] = next;
                    shape[0,next] = 1;
                }
                it++;
            }

        }
        //获取此情况下2能到达的index

        int[] visit = new int[25];
        for (int i = 0; i < 25; i++)
        {
            visit[i] = -1;
        }
        //        Boolean isThreeFound = false;
        //        Boolean isColorFound = false;

        Queue<int> nodeQueue = new Queue<int>();
        List<int> accessible = new List<int>();
        int node = index_two;
        nodeQueue.Enqueue(node);
        while (nodeQueue != null && nodeQueue.Count > 0)
        {

            node = nodeQueue.Dequeue();
            if (node != index_two)
            {
                accessible.Add(node);
            }

            List<int> children = new List<int>();
            for (int i = 0; i < 8; i++)
            {
                int direction = move_dir[i];
                int next = node + direction;
                Boolean isNearOne = false;
                Boolean isEdge_1 = false;
                Boolean isEdge_4 = false;
                Boolean isEdge_6 = false;
                if (next >= 0 && next < 25)
                {
                    if (visit[next] == -1 && shape[0,next] != 1 && shape[0,next] != 2)
                    {
                        switch (direction)
                        {
                            case -1:
                                isEdge_1 = (node % 5 == 0);
                                if (isEdge_1)
                                {
                                    break;
                                }
                                if ((node - 6) >= 0 && (node + 5) < 25)
                                {
                                    isNearOne = (shape[0,node - 6] == 1 && shape[0,node - 5] == 1) || (shape[0,node + 4] == 1 && shape[0,node + 5] == 1);
                                }
                                else if ((node + 5) >= 25)
                                {
                                    isNearOne = shape[0,node - 6] == 1 && shape[0,node - 5] == 1;
                                }
                                else
                                {
                                    isNearOne = shape[0,node + 4] == 1 && shape[0,node + 5] == 1;
                                }
                                break;
                            case 1:
                                isEdge_1 = (node % 5 == 4);
                                if (isEdge_1)
                                {
                                    break;
                                }
                                if ((node - 5) >= 0 && (node + 6) < 25)
                                {
                                    isNearOne = (shape[0,node - 4] == 1 && shape[0,node - 5] == 1) || (shape[0,node + 6] == 1 && shape[0,node + 5] == 1);
                                }
                                else if ((node + 6) > 25)
                                {
                                    isNearOne = shape[0,node - 4] == 1 && shape[0,node - 5] == 1;
                                }
                                else
                                {
                                    isNearOne = shape[0,node + 6] == 1 && shape[0,node + 5] == 1;
                                }
                                break;
                            case -5:
                                if ((node % 5) == 0)
                                {
                                    isNearOne = shape[0,node - 4] == 1 && shape[0,node + 1] == 1;
                                }
                                else if ((node % 5) == 4)
                                {
                                    isNearOne = shape[0,node - 6] == 1 && shape[0,node - 1] == 1;
                                }
                                else
                                {
                                    isNearOne = (shape[0,node - 4] == 1 && shape[0,node + 1] == 1) || (shape[0,node - 6] == 1 && shape[0,node - 1] == 1);
                                }
                                break;
                            case 5:
                                if ((node % 5) == 0)
                                {
                                    isNearOne = shape[0,node + 6] == 1 && shape[0,node + 1] == 1;
                                }
                                else if ((node % 5) == 4)
                                {
                                    isNearOne = shape[0,node + 4] == 1 && shape[0,node - 1] == 1;
                                }
                                else
                                {
                                    isNearOne = (shape[0,node + 6] == 1 && shape[0,node + 1] == 1) || (shape[0,node + 4] == 1 && shape[0,node - 1] == 1);
                                }
                                break;
                            case -4:
                                isEdge_4 = (node + next) % 10 == 4;
                                if (isEdge_4)
                                {
                                    break;
                                }
                                isNearOne = (shape[0,node - 5] == 1) ^ (shape[0,node + 1] == 1);
                                break;
                            case 4:
                                isEdge_4 = (node + next) % 10 == 4;
                                if (isEdge_4)
                                {
                                    break;
                                }
                                isNearOne = (shape[0,node - 1] == 1) ^ (shape[0,node + 5] == 1);
                                break;
                            case -6:
                                isEdge_6 = (node + next) % 10 == 4;
                                if (isEdge_6)
                                {
                                    break;
                                }
                                isNearOne = (shape[0,node - 5] == 1) ^ (shape[0,node - 1] == 1);
                                break;
                            case 6:
                                isEdge_6 = (node + next) % 10 == 4;
                                if (isEdge_6)
                                {
                                    break;
                                }
                                isNearOne = (shape[0,node + 1] == 1) ^ (shape[0,node + 5] == 1);
                                break;
                            default:
                                break;

                        }
                        if (!isEdge_1 && !isEdge_4 && !isEdge_6 && isNearOne)
                        {
                            children.Add(next);
                            visit[next] = node;
                        }

                    }
                }
            }
            if (node != index_two && children.Count >= 2) {
                return CreateShape(wall_color);
            }
            if (children != null && children.Count > 0)
            {
                foreach (int child in children)
                {
                    nodeQueue.Enqueue(child);
                }
            }
        }

        int num_color = 0;
        List<int> start = new List<int>();
        for (int i = 0; i < 25; i++)
        {
            if (visit[i] == index_two)
            {
                start.Add(i);
                num_color++;
            }
        }
        if (accessible.Count < 2 + num_color)
        {
            return CreateShape(wall_color);
        }
        List<int> longPath = new List<int>();
        List<int> shortPath = new List<int>();
        int index_three_a = 0;
        int index_three_b = 0;
        int index_color;
        int num_color_type = 7;
        int another_color = UnityEngine.Random.Range(0, num_color_type) + 4;
        while (another_color == wall_color)
        {
            another_color = UnityEngine.Random.Range(0, num_color_type) + 4;
        }
        //wall_color
        //UnityEngine.Debug.Log("Num_Color: " + num_color);

        if (num_color == 1)
        {
            foreach (int i in accessible)
            {
                longPath.Add(i);
            }
            index_color = accessible[UnityEngine.Random.Range(0, accessible.Count)];
            shape[1,index_color] = wall_color;
            int removedNode = UnityEngine.Random.Range(0, accessible.Count);
            index_three_a = accessible[removedNode];
            accessible.RemoveAt(removedNode);
            index_three_b = accessible[UnityEngine.Random.Range(0, accessible.Count)];
            shape[0,index_three_a] = 3;
            shape[0,index_three_b] = 3;
        }
        else
        {
            int temp = accessible[accessible.Count - 1];
            while (temp != index_two)
            {
                longPath.Add(temp);
                temp = visit[temp];
            }
            foreach (int i in accessible)
            {
                if (!longPath.Contains(i))
                {
                    shortPath.Add(i);
                }
            }

            int p = UnityEngine.Random.Range(0, 4);
            int index_wall_color = 0;
            int index_another_color = 0;
            switch (p)
            {
                case 0:
                    index_wall_color = longPath[UnityEngine.Random.Range(0, longPath.Count)];
                    index_three_a = longPath[UnityEngine.Random.Range(0, longPath.Count)];
                    index_another_color = shortPath[UnityEngine.Random.Range(0, shortPath.Count)];
                    index_three_b = shortPath[UnityEngine.Random.Range(0, shortPath.Count)];
                    break;
                case 1:
                    index_another_color = longPath[UnityEngine.Random.Range(0, longPath.Count)];
                    index_three_a = longPath[UnityEngine.Random.Range(0, longPath.Count)];
                    index_wall_color = shortPath[UnityEngine.Random.Range(0, shortPath.Count)];
                    index_three_b = shortPath[UnityEngine.Random.Range(0, shortPath.Count)];
                    break;
                case 2:
                    index_three_a = longPath[UnityEngine.Random.Range(0, longPath.Count)];
                    index_three_b = longPath[UnityEngine.Random.Range(0, longPath.Count)];
                    index_wall_color = longPath[UnityEngine.Random.Range(0, longPath.Count)];
                    index_another_color = shortPath[UnityEngine.Random.Range(0, shortPath.Count)];
                    break;
                case 3:
                    index_wall_color = longPath[UnityEngine.Random.Range(0, longPath.Count / 2)];
                    index_another_color = longPath[UnityEngine.Random.Range(longPath.Count / 2, longPath.Count)];
                    index_three_a = longPath[UnityEngine.Random.Range(0, (longPath.Count))];
                    index_three_b = longPath[UnityEngine.Random.Range(0, (longPath.Count))];
                    break;
                default:
                    break;
            }
            shape[0,index_three_a] = 3;
            shape[0,index_three_b] = 3;
            shape[1,index_wall_color] = wall_color;
            shape[1,index_another_color] = another_color;
        }
        return shape;
    }

}
