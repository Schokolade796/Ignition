using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;
using Valve.VR;
using Unity.VisualScripting;

public class MainGunGrab : MonoBehaviour
{
    private Interactable interactable; // �θ� Interactable (����)
    public Interactable subInter; // �ڽ� Interactable

    private void Start()
    {
        interactable = GetComponent<Interactable>();
    }

    // �������� �������� �˻�
    private void LateUpdate()
    {
        // ���� �����̸� ��� �ִ� ���¿��� ���� �����̸� ���� ��
        if (!interactable.attachedToHand && subInter.attachedToHand)
        {
            subInter.attachedToHand.HoverUnlock(subInter);
            subInter.attachedToHand.DetachObject(subInter.gameObject);
            subInter.DetachFromHand();
        }

        // check if grabbed
        if (interactable.attachedToHand != null)
        {
            // Get the hand source
            SteamVR_Input_Sources source = interactable.attachedToHand.handType;
        }
    }

    // �θ� ��ü�� ��� �ִ� ����
    private void HandHoverUpdate(Hand hand)
    {
        if (!interactable.attachedToHand)
        {
            hand.DetachObject(subInter.gameObject);
            hand.HoverUnlock(subInter);
        }
    }
}
