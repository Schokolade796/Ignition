using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class SubGunGrab : MonoBehaviour
{
    public Interactable mainInter; // �θ� Interactable
    private Interactable interactable; // �ڽ� Interactable (����)
    private Quaternion secondRotationOffset; // �θ� �������� ȸ������ �����ϱ� ���� ��

    private void Start()
    {
        interactable = GetComponent<Interactable>();
    }

    // �θ��� ������ ���� ������ ������(�ڽ��� ��� �ִ� ��) ������ ���� �Լ�
    //public void ForceDetach()
    //{
    //    if (interactable.attachedToHand)
    //    {
    //        interactable.attachedToHand.HoverUnlock(interactable);
    //        interactable.attachedToHand.DetachObject(gameObject);
    //    }
    //}

    // �θ��� ���� ������ �ڽ��� ���� ������ ����Ͽ� Quaternion���� ��ȯ
    private Quaternion GetTargetRotation()
    {
        Vector3 mainHandUp = mainInter.attachedToHand.objectAttachmentPoint.up;
        Vector3 secondHandUp = interactable.attachedToHand.objectAttachmentPoint.up;

        return Quaternion.LookRotation(interactable.attachedToHand.transform.position - mainInter.attachedToHand.transform.position, mainHandUp);
    }

    // �θ�� �ڽ� ����Ʈ�� ����ִ� ���� (�� ��)
    private void HandAttachedUpdate(Hand hand)
    {
        if (mainInter.attachedToHand)
        {
            if (mainInter.skeletonPoser)
            {
                Quaternion customHandPoserRotation = mainInter.skeletonPoser.GetBlendedPose(mainInter.attachedToHand.skeleton).rotation;
                mainInter.transform.rotation = GetTargetRotation() * secondRotationOffset * customHandPoserRotation;
            }
            else
            {
                mainInter.attachedToHand.objectAttachmentPoint.rotation = GetTargetRotation() * secondRotationOffset;
            }
        }
    }

    // �ڽ� ����Ʈ�� ��� �ִ� ����
    private void HandHoverUpdate(Hand hand)
    {
        GrabTypes grabType = hand.GetGrabStarting();
        bool isGrabEnding = hand.IsGrabEnding(gameObject);

        // Grab
        if (interactable.attachedToHand == null && grabType != GrabTypes.None)
        {
            hand.AttachObject(gameObject, grabType, 0);
            hand.HoverLock(interactable);
            hand.HideGrabHint();
            secondRotationOffset = Quaternion.Inverse(GetTargetRotation()) 
                * mainInter.attachedToHand.currentAttachedObjectInfo.Value.handAttachmentPointTransform.rotation;
        }

        // Release
        else if (isGrabEnding)
        {
            hand.DetachObject(gameObject);
            hand.HoverUnlock(interactable);
        }
    }
}
