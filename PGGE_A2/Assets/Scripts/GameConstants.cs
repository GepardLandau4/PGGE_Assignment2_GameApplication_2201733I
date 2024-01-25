using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PGGE
{
    public static class CameraConstants
    {
        //changed variable name to better fit naming convention
        public static Vector3 cameraAngleOffset { get; set; }
        public static Vector3 cameraPositionOffset { get; set; }
        public static float damping { get; set; }
        public static float rotationSpeed { get; set; }
        public static float minPitch { get; set; }
        public static float maxPitch { get; set; }
    }
}
