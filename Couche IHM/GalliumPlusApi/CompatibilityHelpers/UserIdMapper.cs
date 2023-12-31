using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalliumPlusApi.CompatibilityHelpers
{
    internal class UserIdMapper
    {
        private static UserIdMapper? current;

        public static UserIdMapper Current
        {
            get
            {
                if (current == null)
                {
                    current = new UserIdMapper();
                }
                return current;
            }
        }

        private Dictionary<string, int> ids;

        private UserIdMapper()
        {
            ids = new Dictionary<string, int>();
        }

        public int GetIdFor(string username)
        {
            lock (ids)
            {
                if (ids.ContainsKey(username))
                {
                    return ids[username];
                }
                else
                {
                    int id = HashUsername(username);
                    ids[username] = id;
                    return id;
                }
            }
        }

        public string FindUsernameOf(int numericId)
        {
            lock (ids)
            {
                return ids.First(kvp => kvp.Value == numericId).Key;
            }
        }

        private static readonly string BASE64_DU_BLED = "0123456789azertyuiopqsdfghjklmwxcvbnAZERTYUIOPQSDFGHJKLMWXCVBN";

        private static int HashUsername(string username)
        {
            int result = 0;

            foreach(char c in username.Take(4))
            {
                result += BASE64_DU_BLED.IndexOf(c);
                result <<= 6;
            }

            return result;
        }
    }
}
