using UnityEngine;

public enum BeatState { Down, Left, Right, Up }

public class beatRecognizer : MonoBehaviour
{
    void Start()
    {
        
    }

    public XRHandSubsystem hands;
    public BeatState state = BeatState.Down;

    Vector3 lastPos;

    void Update()
    {
        var right = hands.rightHand;
        if (!right.isTracked) return;

        var wrist = right.GetJoint(XRHandJointID.Wrist);
        if (!wrist.TryGetPose(out Pose wristPose)) return;

        Vector3 velocity = (wristPose.position - lastPos) / Time.deltaTime;
        lastPos = wristPose.position;

        switch (state)
        {
            case BeatState.Down:
                if (velocity.y < -0.2f)
                {
                    state = BeatState.Left;
                    Debug.Log("Beat 1 (Down)");
                }
                break;

            case BeatState.Left:
                if (velocity.x < -0.2f)
                {
                    state = BeatState.Right;
                    Debug.Log("Beat 2 (Left)");
                }
                break;

            case BeatState.Right:
                if (velocity.x > 0.2f)
                {
                    state = BeatState.Up;
                    Debug.Log("Beat 3 (Right)");
                }
                break;

            case BeatState.Up:
                if (velocity.y > 0.2f)
                {
                    state = BeatState.Down;
                    Debug.Log("Beat 4 (Up) — Pattern Complete");
                }
                break;
        }
    }
}
