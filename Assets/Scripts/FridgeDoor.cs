using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FridgeDoor : MonoBehaviour
{
    public float ActionDelay;
    public float Dampening;

    public enum State { Closed, Slighly, Open }
    public State DoorState = State.Closed;

    public Animator DoorAnimator;

    private bool gameOver = false;
    private bool slamming;
    private float nextAction;
    private float openness;

    private GameObject hand;
    private GameObject handle;

    private Vector3 mousePosDelta;
    private Vector3 mousePrevPos;


    private void Start()
    {
        nextAction = ActionDelay;

        hand = GameObject.Find("Hand");
        handle = GameObject.Find("kahva");
    }

    private void Update()
    {
        DoorAction();

        SetDoorOpennes();
        
        MouseDelta();

        //Update door animation
        DoorAnimator.SetFloat("Openness", openness);
    }

    private void FixedUpdate()
    {
        if (nextAction > 0)
        {
            nextAction -= Time.deltaTime;
        }
    }

    private void DoorAction()
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
    }

    private void SetDoorOpennes()
    {
        if (nextAction < 0 && !gameOver)
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
    }

    private void MouseDelta()
    {
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
    }

    public void GameOver()
    {
        gameOver = true;

        handle.transform.SetParent(hand.transform);
    }

}
