namespace Editarrr.Misc
{
    public static partial class Extension
    {
        public static float ToDegree(this Rotation rotation)
        {
            switch (rotation)
            {
                case Rotation.North:
                    return 0;
                case Rotation.East:
                    return 90;
                case Rotation.South:
                    return 180;
                case Rotation.West:
                    return 270;
            }

            return 0;
        }
    }
}
