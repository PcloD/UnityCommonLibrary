namespace UnityCommonLibrary {
    public static class Math {

        /// <summary>
        /// Clamps a float to the -1...1 inclusive range
        /// </summary>
        /// <param name="f"></param>
        /// <returns>A normalized float.</returns>
        public static float Normalize(float f) {
            if(f > 1f) {
                return 1f;
            }
            else if(f < -1f) {
                return -1f;
            }
            else {
                return f;
            }
        }

    }
}
