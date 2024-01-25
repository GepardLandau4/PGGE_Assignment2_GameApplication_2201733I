using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PGGE
{
    public class TPCFollowIndependentRotation : TPCBase
    {
        FixedTouchField mTouchField;
        private float angleX = 0.0f;
        public TPCFollowIndependentRotation(Transform cameraTransform, Transform playerTransform)
            : base(cameraTransform, playerTransform)
        {
        }

#if UNITY_ANDROID
        public TPCFollowIndependentRotation(Transform cameraTransform, Transform playerTransform, FixedTouchField fixedTouch)
            : base(cameraTransform, playerTransform)
        {
            mTouchField = fixedTouch;
        }
#endif

        public override void Update()
        {
            //implement the Update for this camera controls    public override void Update()
#if UNITY_STANDALONE
        //changed variable name to better fit naming convention
        float mX, mY;
        mX = Input.GetAxis("Mouse X");
        mY = Input.GetAxis("Mouse Y");
#endif
#if UNITY_ANDROID
            float mX, mY;
            mX = mTouchField.TouchDist.x * Time.deltaTime;
            mY = mTouchField.TouchDist.y * Time.deltaTime;
#endif

            // We apply the initial rotation to the camera.
            Quaternion initialRotation = Quaternion.Euler(CameraConstants.cameraAngleOffset);

            //renamed variable to make it clearer what it is
            Vector3 eulerAngleRotation = mCameraTransform.rotation.eulerAngles;

            angleX -= mY * CameraConstants.rotationSpeed;

            // We clamp the angle along the Xaxis to be between the min and max pitch.
            angleX = Mathf.Clamp(angleX, CameraConstants.minPitch, CameraConstants.maxPitch);

            eulerAngleRotation.y += mX * CameraConstants.rotationSpeed;
            Quaternion newRot = Quaternion.Euler(angleX, eulerAngleRotation.y, 0.0f) * initialRotation;

            mCameraTransform.rotation = newRot;

            Vector3 forward = mCameraTransform.rotation * Vector3.forward;
            Vector3 right = mCameraTransform.rotation * Vector3.right;
            Vector3 up = mCameraTransform.rotation * Vector3.up;

            Vector3 targetPos = mPlayerTransform.position;
            Vector3 desiredPosition = targetPos
                + forward * CameraConstants.cameraPositionOffset.z
                + right * CameraConstants.cameraPositionOffset.x
                + up * CameraConstants.cameraPositionOffset.y;

            Vector3 position = Vector3.Lerp(mCameraTransform.position,
                desiredPosition,
                Time.deltaTime * CameraConstants.damping);

            mCameraTransform.position = position;
        }
    }
}
