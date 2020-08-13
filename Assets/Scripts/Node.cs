using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{  
    bool[] neigbour = new bool[4] {false, false, false, false }; //[0] - up, [1] - right, [2] - down, [3] - left

    bool preferRight;
    bool preferUp;

    int z;
    int x;

    public bool[] checkNeigbours(int[,] allNodes)
    {
        if (allNodes[z + 1, x] != 0 && z < allNodes.Length/x)
            neigbour[0] = true; //up
        if (allNodes[z, x + 1] != 0 && x + 1 < allNodes.GetLength(z))
            neigbour[1] = true; //right
        if (allNodes[z - 1, x] != 0 && z-1 >= 0)
            neigbour[2] = true; //down
        if (allNodes[z, x - 1] != 0 && x-1 >= 0)
            neigbour[3] = true; // left

        return neigbour;
    }

}
