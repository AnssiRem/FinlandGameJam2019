using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buddha : MonoBehaviour
{
    public AudioSource AudioS;

    private bool fired = false;
    private FridgeDoor door;

    private void Start()
    {
        door = GameObject.Find("Ovi").GetComponent<FridgeDoor>();
    }

    private void Update()
    {
        if (!fired && door.DoorState == FridgeDoor.State.Open)
        {
            AudioS.enabled = true;
            AudioS.PlayOneShot(AudioS.clip);
            fired = true;
        }
    }
}
