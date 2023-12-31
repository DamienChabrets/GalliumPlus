namespace GalliumPlusApi.Exceptions
{
    public class InvalidItemException : GalliumPlusHttpException
    {
        public InvalidItemException() : base("Ressource invalide") { }
     
        public InvalidItemException(string message) : base(message) { }
    }
}