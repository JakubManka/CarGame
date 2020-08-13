using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GenerateTrack : MonoBehaviour
{
    public GameObject straight;
    public GameObject cornerL;
    public GameObject cornerR;   
    public GameObject ground;

    List<int> trackZ;
    List<int> trackX;

    Dictionary<Vector3, GameObject> tiles;   

    Vector3 currentPosition;      
    Vector3 previousPosition;      

    Quaternion rotate0 = Quaternion.Euler(0, 0, 0);
    Quaternion rotate90 = Quaternion.Euler(0, 90, 0);
    Quaternion rotate180 = Quaternion.Euler(0, 180, 0);
    Quaternion rotate270 = Quaternion.Euler(0, 270, 0);

    Vector3 moveRight = new Vector3(40, 0, 0);
    Vector3 moveLeft = new Vector3(-40, 0, 0);
    Vector3 moveForward = new Vector3(0, 0, 40);
    Vector3 moveBackward = new Vector3(0, 0, -40);

    float direction;

    GameObject temp = null;
    GameObject[] tile;
    bool trackIsDone;
    //caly czas kończy się zakrętem
    //nie znajduje drogi do mety, bo nie jest skonczony algorytm

    void Start()
    {
        trackIsDone = false;
        
        while (!trackIsDone)
        {
            Generate();            
            currentPosition = new Vector3(0, 0, 0);                    
            try
            {
                Create();
            }
            catch
            {
                print("s");
                tile = GameObject.FindGameObjectsWithTag("tile");
                tiles.Clear();
                foreach (var t in tile)
                {
                    Destroy(t);
                }               
            }
        }

        FindWayToMeta();
       
    }
        
    void Create()
    {
        direction = 1; // 1 - Axis Z positive, -1 - Axis Z negative, 2 - Axis X positive, -2 - Axis X negative

        for (int i = 0; i < 2; i++)
        {
            temp = Instantiate(straight, currentPosition, rotate0) as GameObject;
            tiles.Add(currentPosition, temp);
            currentPosition += moveForward;             
        }

        for (int i = 0; i < trackZ.Count; i++)
        {
            if (trackX[i] > 0)
            {
                if (direction == 1)
                {
                    Turn(cornerR, moveRight, rotate90, trackX[i], 2);
                }
                else if (direction == -1)
                {
                    Turn(cornerL, moveRight, rotate90, trackX[i], 2);
                }
            }
            else if (trackX[i] < 0)
            {
                if (direction == 1)
                {
                    Turn(cornerL, moveLeft, rotate270, trackX[i], -2);
                }
                else if (direction == -1)
                {
                    Turn(cornerR, moveLeft, rotate270, trackX[i], -2);
                }
            }
            if (trackZ[i] > 0)
            {
                if (direction == 2)
                {                    
                    Turn(cornerL, moveForward, rotate0, trackZ[i], 1);
                }
                else if (direction == -2)
                {
                    Turn(cornerR, moveForward, rotate0, trackZ[i], 1);
                }
            }
            else if (trackZ[i] < 0)
            {
                if (direction == 2)
                {
                    Turn(cornerR, moveBackward, rotate180, trackZ[i], -1);
                }
                else if (direction == -2)
                {                    
                    Turn(cornerL, moveBackward, rotate180, trackZ[i], -1);
                }
            }
        }
        if (tiles[currentPosition].GetComponent<GameObject>() == cornerL.GetComponent<GameObject>() ||
                    tiles[currentPosition].GetComponent<GameObject>() == cornerR.GetComponent<GameObject>())
        {            
            print("sadsadsS");
        }
        trackIsDone = true;

      
    }

    void Turn(GameObject gameObject, Vector3 move, Quaternion rotate, int times, float direction)
    {
        if (tiles.ContainsKey(currentPosition))
        {          
            previousPosition = temp.transform.position;
            tiles.Remove(previousPosition);
            Destroy(tiles[previousPosition]);
            temp = Instantiate(gameObject, previousPosition, rotate);
            tiles.Add(previousPosition, temp);
            currentPosition = previousPosition + move;           
            
        }else
        {
            temp = Instantiate(gameObject, currentPosition, rotate);
            tiles.Add(currentPosition, temp);
            currentPosition += move;            
        }
        
        for (int j = 0; j < times - 1; j++)
        {
            if (tiles.ContainsKey(currentPosition))
            {     
                if(tiles[currentPosition].GetComponent<GameObject>() == cornerL.GetComponent<GameObject>() ||
                    tiles[currentPosition].GetComponent<GameObject>() == cornerR.GetComponent<GameObject>())
                {
                    previousPosition = temp.transform.position;
                    tiles.Remove(previousPosition);
                    Destroy(tiles[previousPosition]);
                    temp = Instantiate(gameObject , previousPosition, rotate);
                    tiles.Add(previousPosition, temp);
                    currentPosition = previousPosition + move;
                    break;
                }               
                else
                {
                    previousPosition = temp.transform.position;
                    tiles.Remove(previousPosition);
                    Destroy(tiles[previousPosition]);                    
                    break;
                }
                
            }
            else {
                temp = Instantiate(straight, currentPosition, rotate);
                tiles.Add(currentPosition, temp);
                currentPosition += move;                
            }
           
        }
        this.direction = direction;       
    }   

    void Generate()
    {
        tiles = new Dictionary<Vector3, GameObject>();
        currentPosition = transform.position;       
        trackZ = new List<int>();
        trackX = new List<int>();
        int axisZ;
        int axisX;
        for (int i = 0; i < 7; i++)
        {
            int tempZ = UnityEngine.Random.Range(0, 16);
            int tempX = UnityEngine.Random.Range(0, 16);
            tempX -= 8;
            tempZ -= 8;

            if (tempX == 0)
                tempX = 3;
            if (tempZ == 0)
                tempZ = 3;

            trackZ.Add(tempZ);
            trackX.Add(tempX);                    
        }
    
    }

    void FindWayToMeta()
    {
        trackZ.Clear();
        trackX.Clear();

        float z = currentPosition.z;
        float x = currentPosition.x;
        int[,] fields;

        if (z > x)
        {
            int lengthZ = (int)z / 10;
            fields = new int[lengthZ+30, lengthZ+30];
        }
        else
        {
            int lengthX = (int)x / 10;
            fields = new int[lengthX+30, lengthX+30];
        }

        Vector3 position = currentPosition;

        for (int i = 0; i < fields.GetLength(0); i++)
        {
            for (int j = 0; j < fields.GetLength(1); j++)
            {
                fields[i, j] = 0;                
            }
        }

        foreach(var entry in tiles)
        {
            int axisZ = (int)entry.Key.z/10 - 1;
            int axisX = (int)entry.Key.x/10 - 1;

            fields[axisZ + 15, axisX + 15] = 1;
        }        
        for (int i = 0; i < fields.GetLength(0); i++)
        {
            for (int j = 0; j < fields.GetLength(1); j++)
            {
                /*if (fields[i, j] == 1)*/
                    /*print(fields[i, j] + "z = " + i + " x = " + j);*/
            }
            
        }
       
    }
}
