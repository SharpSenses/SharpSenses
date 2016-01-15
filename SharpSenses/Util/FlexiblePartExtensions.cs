namespace SharpSenses.Util {
    public static class FlexiblePartExtensions {
        public static void SetOpenness(this FlexiblePart part, int scaleZeroToHundred) {
            if (scaleZeroToHundred > 75) {
                part.IsOpen = true;
            }
            else if (scaleZeroToHundred < 35) {
                part.IsOpen = false;
            }
        }
    }
}