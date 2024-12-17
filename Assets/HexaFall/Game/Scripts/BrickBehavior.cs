using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickBehavior : MonoBehaviour
{
    [SerializeField] Collider trigger;
    [SerializeField] GameObject multyVisuals;

    public bool IsMulty { get; private set; }

    public void Init(bool isMulty)
    {
        trigger.enabled = true;
        IsMulty = isMulty;

        multyVisuals.SetActive(isMulty);
    }

    public void GetPickedUp()
    {
        trigger.enabled = false;
        multyVisuals.SetActive(false);
    }

    public void OnDisable()
    {
        multyVisuals.SetActive(false);
    }
}
