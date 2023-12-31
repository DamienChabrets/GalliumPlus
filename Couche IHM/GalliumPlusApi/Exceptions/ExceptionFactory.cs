using GalliumPlusApi.Dto;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace GalliumPlusApi.Exceptions
{
    internal static class ExceptionFactory
    {
        public static void ThrowForStatus(HttpResponseMessage response, JsonSerializerOptions? jsonOptions = null)
        {
            if (!response.IsSuccessStatusCode)
            {
                throw response.StatusCode switch
                {
                    HttpStatusCode.Unauthorized => new UnauthenticatedException(),
                    HttpStatusCode.BadRequest => ResponseToException(response, jsonOptions) ?? new InvalidItemException(),
                    HttpStatusCode.Forbidden => ResponseToException(response, jsonOptions) ?? new PermissionDeniedException(),
                    HttpStatusCode.NotFound => ResponseToException(response, jsonOptions) ?? new ItemNotFoundException(),
                    _ => new GalliumPlusHttpException($"Erreur HTTP non gérée : {(int)response.StatusCode} {response.ReasonPhrase}"),
                };
            }
        }

        private static GalliumPlusHttpException? ResponseToException(HttpResponseMessage response, JsonSerializerOptions? jsonOptions)
        {
            Task<Error?> task = response.Content.ReadFromJsonAsync<Error>(jsonOptions);
            task.Wait();
            if (task.Result is Error error)
            {
                return error.Code switch
                {
                    "InvalidItem" => new InvalidItemException(error.DetailedMessage),
                    "PermissionDenied" => new PermissionDeniedException(error.DetailedMessage),
                    "ItemNotFound" => new ItemNotFoundException(error.DetailedMessage),
                    "CantSell" => new CantSellException(error.DetailedMessage),
                    _ => throw new NotImplementedException($"Code d'erreur non géré : « {error.Code} »")
                };
            }
            else
            {
                return null;
            }
        }
    }
}
