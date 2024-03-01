// Author: Scottywonderful
// Created: 16th Feb 2024
// Version: 0.4.3.5

using Rage;
using System.Collections.Generic;

namespace SWLCallouts.Stuff
{
    public static class LocationChooser
    {

        public static Vector3 chooseNearestLocation(List<Vector3> list)
        {
            Vector3 closestLocation = list[0];
            float closestDistance = Vector3.Distance(Game.LocalPlayer.Character.Position, list[0]);
            for (int i = 1; i < list.Count; i++)
            {
                if (Vector3.Distance(Game.LocalPlayer.Character.Position, list[i]) <= closestDistance)
                {
                    closestDistance = Vector3.Distance(Game.LocalPlayer.Character.Position, list[i]);
                    closestLocation = list[i];
                }
            }
            return closestLocation;
        }
        public static int nearestLocationIndex(List<Vector3> list)
        {
            int closestLocationIndex = 0;
            float closestDistance = Vector3.Distance(Game.LocalPlayer.Character.Position, list[0]);
            for (int i = 1; i < list.Count; i++)
            {
                if (Vector3.Distance(Game.LocalPlayer.Character.Position, list[i]) <= closestDistance)
                {
                    closestDistance = Vector3.Distance(Game.LocalPlayer.Character.Position, list[i]);
                    closestLocationIndex = i;
                }
            }
            return closestLocationIndex;
        }
    }
}
