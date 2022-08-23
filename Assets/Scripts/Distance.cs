using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Distance
{
    // Distance class: must be dragged to a game object. select cubes
    // within the inspector.

    public GameObject playerCube;
    public GameObject otherCube;
    public float distance_;
    public bool withinDistance;


    public Distance(GameObject cube1, GameObject cube2)
    {
        playerCube = cube1;
        otherCube = cube2;
    }

    public bool CheckDistance()
    {
        distance_ = Vector3.Distance(playerCube.transform.position, otherCube.transform.position);
        if (distance_ < 5) // if distance between cubes is less than 3 units
        {
            withinDistance = true;
        }
        else withinDistance = false;
        return withinDistance;
    }
}
