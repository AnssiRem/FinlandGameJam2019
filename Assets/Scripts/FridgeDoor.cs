using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FridgeDoor : MonoBehaviour
{
    public Animator DoorAnimator;
    public float ActionDelay;
    public float Dampening;

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
        }
        else if (DoorAnimator.GetFloat("Openness") >= 1)//Door is fully open
        {
            openness = 1;

            if (DoorAnimator.GetBool("HandOn"))
            {
                if (Input.GetMouseButtonUp(0))
                {
                    openness = 0;

                    DoorAnimator.SetTrigger("Slam");
                    DoorAnimator.SetBool("HandOn", false);
                    nextAction = ActionDelay;
                }
            }
        }
        else
        {
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
            openness += mousePosDelta.x / Dampening;

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
