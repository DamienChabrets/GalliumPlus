using GalliumPlusApi.Dto;
using GalliumPlusApi.Exceptions;
using Modeles;
using System.Text.Json;

namespace GalliumPlusApi.Dao
{
    public class SessionDao
    {
        private UserDetails.Mapper detailsMapper = new();
        private RoleDetails.Mapper roleMapper = new();

        public (Account, Role)? LogIn(string username, string password)
        {
            using var client = new GalliumPlusHttpClient();
            client.JsonOptions.PropertyNamingPolicy = null; // pas de casse chameau en sortie

            try
            {
                var loggedIn = client.Post<LoggedIn>("v1/login", new { Username = username, Password = password });

                SessionStorage.Current.Put("token", loggedIn.Token);

                if (loggedIn.User == null) return null;

                return (detailsMapper.ToModel(loggedIn.User), roleMapper.ToModel(loggedIn.User.Role));
            }
            catch (UnauthenticatedException)
            {
                return null;
            }
        }
    }
}
