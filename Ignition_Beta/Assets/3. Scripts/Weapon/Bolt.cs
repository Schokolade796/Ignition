using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;
using Valve.VR;

public class Bolt : MonoBehaviour
{
    private Interactable interactable;
    private SpringJoint joint;

    public GameObject Round;
    private Vector3 originPosition;
    private Quaternion originRotation;
    public float endPositionValue;
    public int jointValue;
    public bool redyToShot;
    public bool boltRetraction;


    private void Awake()
    {
        interactable = GetComponent<Interactable>();
        joint = GetComponent<SpringJoint>();
        originPosition = transform.localPosition;
        originRotation = transform.localRotation;
    }

    // Update is called once per frame
    void Update()
    {
        transform.localRotation = originRotation;
        if (interactable.attachedToHand != null)
        {
            //SteamVR_Input_Sources source = interactable.attachedToHand.handType;
            joint.spring = jointValue;
            transform.localPosition = new Vector3(originPosition.x, originPosition.y, transform.localPosition.z);
            if (transform.localPosition.z <= originPosition.z - endPositionValue)
            {
                transform.localPosition = new Vector3
                    (originPosition.x, originPosition.y, originPosition.z - endPositionValue);
            }
        }
        else
        {
            joint.spring = 0;
            transform.localPosition = originPosition;
        }
    }
}
