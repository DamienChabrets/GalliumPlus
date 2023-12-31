namespace GalliumPlusApi.Exceptions
{
    public class PermissionDeniedException : GalliumPlusHttpException
    {
        public PermissionDeniedException() : base("Permission refusée.") { }

        public PermissionDeniedException(string message) : base(message) { }
    }
}