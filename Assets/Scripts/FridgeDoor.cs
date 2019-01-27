using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FridgeDoor : MonoBehaviour
{
    public Animator DoorAnimator;
    public float ActionDelay;
    public float Dampening;

    public enum State { Closed, Slighly, Open}
    public State DoorState = State.Closed;

    private bool slamming;
    private float nextAction;
    private float openness;
    private Vector3 mousePosDelta;
    private Vector3 mousePrevPos;


    private void Start()
    {
        nextAction = ActionDelay;
    }

    private void Update()
    {
        if (DoorAnimator.GetFloat("Openness") < 0)//Door is closed
        {
            openness = 0;
            DoorState = State.Closed;
        }
        else if (DoorAnimator.GetFloat("Openness") >= 1)//Door is fully open
        {
            openness = 1;
            DoorState = State.Open;

            if (DoorAnimator.GetBool("HandOn"))
            {
                if (Input.GetMouseButtonUp(0))
                {
                    openness = 0;

                    slamming = true;
                    DoorAnimator.SetTrigger("Slam");
                    DoorAnimator.SetBool("HandOn", false);
                    nextAction = ActionDelay;
                }
            }
        }
        else
        {
            DoorState = State.Slighly;

            if (nextAction < 0)
            {
                if (!DoorAnimator.GetBool("HandOn"))
                {
                    if (Input.GetMouseButton(0))
                    {
                        DoorAnimator.SetBool("HandOn", true);
                        nextAction = ActionDelay;
                    }
                }
                else
                {
                    if (!Input.GetMouseButton(0))
                    {
                        DoorAnimator.SetBool("HandOn", false);
                        nextAction = ActionDelay;
                    }
                }
            }
        }

            if (Input.GetMouseButton(0))
            {
                mousePosDelta = Input.mousePosition - mousePrevPos;
                mousePrevPos = Input.mousePosition;
            }
            else
            {
                mousePosDelta = Vector3.zero;
                mousePrevPos = Input.mousePosition;
            }

        if (nextAction < 0)
        {
            if (!slamming)
            {
                openness += mousePosDelta.x / Dampening;
            }
            else
            {
                slamming = false;
                DoorState = State.Closed;
            }
        }
            DoorAnimator.SetFloat("Openness", openness);
    }

    private void FixedUpdate()
    {
        if (nextAction > 0)
        {
            nextAction -= Time.deltaTime;
        }
    }
}
