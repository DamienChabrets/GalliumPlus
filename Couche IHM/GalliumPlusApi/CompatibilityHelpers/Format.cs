namespace GalliumPlusApi.CompatibilityHelpers
{
    internal static class Format
    {
        public static decimal? FloatToMonetary(float value)
        {
            if (value < 0) return null;
            string roundedRepr = value.ToString("0.00");
            return decimal.Parse(roundedRepr);
        }
    }
}
