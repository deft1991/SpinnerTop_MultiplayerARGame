using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoosterScript : MonoBehaviour
{
    public enum BoosterType
    {
        SPEED, // todo rename to HEAL
        PUSH,
        DAMAGE,
        FREEZE
    }

    public BoosterType boosterType;
    public float increaseSpeed;
    public float pushSpeed;
    public float damageAmount;
    public float freezeTime;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }
}