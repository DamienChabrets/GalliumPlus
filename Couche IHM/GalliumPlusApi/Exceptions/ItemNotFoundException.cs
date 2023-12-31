namespace GalliumPlusApi.Exceptions
{
    internal class ItemNotFoundException : GalliumPlusHttpException
    {
        public ItemNotFoundException() : base("Le ressource demandée n'existe pas") { }

        public ItemNotFoundException(string message) : base(message) { }
    }
}