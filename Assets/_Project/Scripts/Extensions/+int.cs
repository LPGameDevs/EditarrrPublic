namespace System
{
    /// <summary>
    /// Contains extension methods for the int type.
    /// </summary>
    public static partial class Extension
    {
        /// <summary>
        /// Returns the specified value raised to the specified power.
        /// </summary>
        public static int Pow(this int value, int power)
        {
            // Should be sufficent
            return (int)Math.Pow(value, power);
        }

        /// <summary>
        /// Restricts the specified value to be within the specified range.
        /// </summary>
        public static int Clamp(this int value, int min, int max)
        {
            if (value < min)
                value = min;

            if (value > max)
                value = max;

            return value;
        }

        /// <summary>
        /// Restricts the specified value to be greater than or equal to the specified minimum value.
        /// </summary>
        public static int ClampMin(this int value, int min)
        {
            return value.Max(value, min);
        }

        /// <summary>
        /// Restricts the specified value to be less than or equal to the specified maximum value.
        /// </summary>
        public static int ClampMax(this int value, int max)
        {
            return value.Min(value, max);
        }

        /// <summary>
        /// Returns the smallest value among the specified values.
        /// </summary>
        public static int Min(this int value, params int[] other)
        {
            for (int i = 0; i < other.Length; i++)
            {
                value = Math.Min(value, other[i]);
            }

            return value;
        }

        /// <summary>
        /// Returns the largest value among the specified values.
        /// </summary>
        public static int Max(this int value, params int[] other)
        {
            for (int i = 0; i < other.Length; i++)
            {
                value = Math.Max(value, other[i]);
            }

            return value;
        }

        /// <summary>
        /// Returns the absolute value of the specified integer.
        /// </summary>
        public static int Abs(this int value)
        {
            return Math.Abs(value);
        }

        /// <summary>
        /// Returns a value indicating the sign of the specified integer.
        /// </summary>
        public static int Sign(this int value)
        {
            return Math.Sign(value);
        }

        /// <summary>
        /// Wraps the specified integer to fit within the range [0, exclusiveMax).
        /// </summary>
        public static int Loop(this int value, int exclusiveMax)
        {
            return (value % exclusiveMax + exclusiveMax) % exclusiveMax;
        }
    }
}
