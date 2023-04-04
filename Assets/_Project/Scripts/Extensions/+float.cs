namespace System
{
    /// <summary>
    /// Contains extension methods for the float data type.
    /// </summary>
    public static partial class Extension
    {
        /// <summary>
        /// Returns the absolute value of the float.
        /// </summary>
        public static float Abs(this float value) => Math.Abs(value);

        /// <summary>
        /// Returns the sign of the float. Returns 1 if the value is positive or zero, and -1 if the value is negative.
        /// </summary>
        public static float Sign(this float value) => Math.Sign(value);

        /// <summary>
        /// Returns the sign of the float as an integer. Returns 1 if the value is positive or zero, and -1 if the value is negative.
        /// </summary>
        public static int SignInt(this float value) => Math.Sign(value);

        #region Clamp
        /// <summary>
        /// Clamps the float value between the minimum and maximum values.
        /// </summary>
        public static float Clamp(this float value, float min, float max)
        {
            if (value < min)
                value = min;

            if (value > max)
                value = max;

            return value;
        }

        /// <summary>
        /// Clamps the float value between the minimum value and positive infinity.
        /// </summary>
        public static float ClampMin(this float value, float min)
        {
            return value.Max(value, min);
        }

        /// <summary>
        /// Clamps the float value between negative infinity and the maximum value.
        /// </summary>
        public static float ClampMax(this float value, float max)
        {
            return value.Min(value, max);
        }

        #endregion

        /// <summary>
        /// Loops the float value within the range of 0 and the given length.
        /// </summary>
        public static float Loop(this float value, float length)
        {
            return Clamp(value - (float)Math.Floor(value / length) * length, 0.0f, length);
        }

        /// <summary>
        /// Returns the value of a ping-pong function with the given length.
        /// </summary>
        public static float PingPong(this float value, float length)
        {
            value = value.Loop(length * 2F);
            float toReturn = length - Math.Abs(value - length);

            return toReturn;
        }

        #region Lerp

        /// <summary>
        /// Linearly interpolates between two float values.
        /// </summary>
        public static float Lerp(this float from, float to, float time)
        {
            return from + (to - from) * time.Clamp(0, 1);
        }

        /// <summary>
        /// Linearly interpolates between two float values without clamping the time parameter.
        /// </summary>
        public static float LerpU(this float from, float to, float time)
        {
            return from + (to - from) * time;
        }

        #endregion

        /// <summary>
        /// Returns the float value raised to the specified power.
        /// </summary>
        public static float Pow(this float value, float power) => (float)Math.Pow(value, power);

        /// <summary>
        /// Maps the float value from one range to another.
        /// </summary>
        public static float Map(this float value, float from1, float to1, float from2, float to2)
        {
            float vFrom = value - from1,
                toFrom1 = to1 - from1,
                toFrom2 = to2 - from2;

            return vFrom / toFrom1 * toFrom2 + from2;
        }

        /// <summary>
        /// Returns the minimum value among a set of floats.
        /// </summary>
        public static float Min(this float value, params float[] other)
        {
            for (int i = 0; i < other.Length; i++)
            {
                value = Math.Min(value, other[i]);
            }

            return value;
        }

        /// <summary>
        /// Returns the maximum value among a set of floats.
        /// </summary>
        public static float Max(this float value, params float[] other)
        {
            for (int i = 0; i < other.Length; i++)
            {
                value = Math.Max(value, other[i]);
            }

            return value;
        }

        /// <summary>
        /// Moves a value towards a target, without exceeding the target by more than a maximum amount.
        /// </summary>
        public static float MoveTowards(this float value, float target, float maxDelta)
        {
            if ((target - value).Abs() <= maxDelta)
                return target;

            return value + (target - value).Sign() * maxDelta;
        }

        /// <summary>
        /// Moves a value towards a target by a percentage, with an origin value as a reference point.
        /// </summary>
        public static float MoveTowards(this float value, float origin, float target, float t)
        {
            float delta = (target - origin).Abs();

            float lerp = 0f.Lerp(delta, t);
            float toReturn = value.MoveTowards(target, lerp);

            return toReturn;
        }
    }
}
