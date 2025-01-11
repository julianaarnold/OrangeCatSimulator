using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatAnimator : MonoBehaviour
{
    public Transform heightOffset;

    public IKFeetTracker footFrontLeft;
    public IKFeetTracker footFrontRight;
    public IKFeetTracker footBackLeft;
    public IKFeetTracker footBackRight;

    public float height;
    public float prepareJumpHeight;

    private float heightOffsetTarget;
    private float heightOffsetVelocity;

    private void Update()
    {
        float newHeight = Mathf.SmoothDamp(heightOffset.localPosition.y, heightOffsetTarget, ref heightOffsetVelocity, 0.1f);

        heightOffset.localPosition = new Vector3(heightOffset.localPosition.x, newHeight, heightOffset.localPosition.z);
    }

    public void PrepareJump()
    {
        heightOffsetTarget = prepareJumpHeight;
        heightOffsetVelocity = 2f;
    }

    public void Jump()
    {
        heightOffsetTarget = height;
        heightOffsetVelocity = 0.5f;
    }
}
