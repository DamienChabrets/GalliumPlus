using System.Text;

namespace GalliumPlusApi.CompatibilityHelpers
{
    internal static class RandomHelper
    {
        public static string RandomUsername()
        {
            Random rng = new();
            StringBuilder sb = new("user");
            for (int i = 0; i < 6; i++)
            {
                sb.Append(rng.Next(0, 10));
            }
            return sb.ToString();
        }
    }
}
