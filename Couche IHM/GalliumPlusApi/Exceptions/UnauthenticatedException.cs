namespace GalliumPlusApi.Exceptions
{
    public class UnauthenticatedException : GalliumPlusHttpException
    {
        public UnauthenticatedException() : base("Requête non authentifiée.") { }
    }
}
