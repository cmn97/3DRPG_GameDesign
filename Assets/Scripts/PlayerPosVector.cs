using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PlayerPosVector : ScriptableObject
{
    public Vector3 initialValue;
    public int sceneTransitions;
    public MapStates currentState;
    //public GameObject enemy;
    //public GameObject[] enemies;

    public enum MapStates
    {
        NORMAL,
        BATTLE,
        RAN
    }

    void Start()
    {
        currentState = MapStates.NORMAL;
    }

    void Awake()
    {
        sceneTransitions = 0;
    }
}
