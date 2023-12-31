namespace GalliumPlusApi.Exceptions
{
    public class GalliumPlusHttpException : Exception
    {
        public GalliumPlusHttpException() : base() { }

        public GalliumPlusHttpException(string message) : base(message) { }
    }
}
