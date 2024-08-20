using System.Collections.Generic;
using UnityEngine;

public class PickUpController : MonoBehaviour
{
    public bool equipped;
    public static bool slotfull;
    public GameObject gun;

    private void Start()
    {
        if (!equipped) slotfull = false;

        else slotfull = false;
    }

    public void PickUp()
    {
        Debug.Log("c");
        gun.SetActive(true);
    }

}
