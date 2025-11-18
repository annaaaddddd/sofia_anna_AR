using UnityEngine;
using UnityEngine.XR.Hands;

public class HandDataReader : MonoBehaviour
{
    XRHandSubsystem handSubsystem;

    void Start()
    {
        handSubsystem = XRGeneralSettings.Instance
            .Manager
            .activeLoader
            .GetLoadedSubsystem<XRHandSubsystem>();
    }

    void Update()
    {
        if (handSubsystem == null) return;

        var left = handSubsystem.leftHand;
        var right = handSubsystem.rightHand;

        if (left.isTracked)
        {
            var leftPalm = left.GetJoint(XRHandJointID.Palm);
            Debug.Log("Left palm: " + leftPalm.TryGetPose(out Pose p));
        }
        if (right.isTracked)
        {
            var rightPalm = right.GetJoint(XRHandJointID.Palm);
            Debug.Log("Left palm: " + rightPalm.TryGetPose(out Pose p));
        }

        bool IsFlatHand(XRHand hand)
        {
            XRHandJointID[] tips = {
                XRHandJointID.IndexTip,
                XRHandJointID.MiddleTip,
                XRHandJointID.RingTip,
                XRHandJointID.LittleTip
            };

            foreach (var tip in tips)
            {
                var joint = hand.GetJoint(tip);
                if (!joint.TryGetPose(out Pose tipPose))
                    return false;

                if (Vector3.Dot(tipPose.forward, Vector3.forward) < 0.7f)
                    return false;
            }

            return true;
        }
        if (left.isTracked && IsFlatHand(left))
            Debug.Log("Left hand: Flat hand detected");
        if (right.isTracked && IsFlatHand(right))
            Debug.Log("Left hand: Flat hand detected");
    }
}
