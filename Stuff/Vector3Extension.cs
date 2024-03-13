// Author: Scottywonderful
// Created: 16th Feb 2024
// Version: 0.4.8.0

#region

#endregion

namespace SWLCallouts.Stuff;

    public static class Vector3Extension
    {
        public static Vector3 ExtensionAround(this Vector3 start, float radius)
        {
            Vector3 direction = ExtensionRandomXY();
            Vector3 around = start + (direction * radius);
            return around;
        }

        public static float ExtensionDistanceTo(this Vector3 start, Vector3 end)
        {
            return (end - start).Length();
        }

        public static Vector3 ExtensionRandomXY()
        {
            Vector3 vector3 = new Vector3
            {
                X = (float)(Rndm.NextDouble() - 0.5),
                Y = (float)(Rndm.NextDouble() - 0.5),
                Z = 0.0f
            };
            vector3.Normalize();
            return vector3;
        }
    }