using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : PickupBase
{
    public override void Collect()
    {
        Debug.Log("You collected it!");
    }
}
