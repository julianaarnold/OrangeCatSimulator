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

    public Bonker bonker;

    public float height;
    public float prepareJumpHeight;

    private float heightOffsetTarget;
    private float heightOffsetVelocity;

    private void Update()
    {
        float newHeight = Mathf.SmoothDamp(heightOffset.localPosition.y, heightOffsetTarget, ref heightOffsetVelocity, Time.deltaTime);

        heightOffset.localPosition = new Vector3(heightOffset.localPosition.x, newHeight, heightOffset.localPosition.z);
    }

    public void PrepareJump()
    {
        heightOffsetTarget = prepareJumpHeight;
        heightOffsetVelocity = .05f;
    }

    public void Jump()
    {
        heightOffsetTarget = height;
        heightOffsetVelocity = 0.01f;
    }

    public void BonkLeft()
    {
        footFrontLeft.StartBonk(bonker);
    }

    public void BonkRight()
    {
        footFrontRight.StartBonk(bonker);
    }

    public void SetJumping(bool isJumping)
    {
        footFrontLeft.SetJumping(isJumping);
        footFrontRight.SetJumping(isJumping);
        footBackLeft.SetJumping(isJumping);
        footBackRight.SetJumping(isJumping);
    }
}
