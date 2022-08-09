using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowerMovement : MonoBehaviour
{
    public CharacterStats stats;
    private GameObject master;

    // Start is called before the first frame update
    void Start()
    {
        master = GameObject.Find("Marcel");
    }

    // Update is called once per frame
    void Update()
    {

    }

}
