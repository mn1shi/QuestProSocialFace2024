using System.Collections;
using System.Collections.Generic;
using Ubiq.NetworkedBehaviour;
using UnityEngine;

[NetworkTransform]
public class behaviour : MonoBehaviour
{
    private void Start()
    {
        NetworkedBehaviours.Register(this);
    }

    private void Update()
    {
        //GetComponent<Renderer>().material.color = color;
    }
}
