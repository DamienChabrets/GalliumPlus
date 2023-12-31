using System.Diagnostics;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace GalliumPlusApi.Dto
{
    public class Error
    {
        public string Code { get; set; } = "UNKNOWN";
        
        public string Message { get; set; } = "Erreur inconnue.";

        public object? DebugInfo { get; set; } = null;

        public string DetailedMessage
        {
            get
            {
                StringBuilder sb = new();
                sb.Append(Message);
#if DEBUG
                if (DebugInfo != null)
                {
                    sb.Append("\n\nInformations de débogage:\n");
                    sb.Append(JsonSerializer.Serialize(DebugInfo, new JsonSerializerOptions { WriteIndented = true }));
                }
                else
                {
                    sb.Append("\n\nAucune informations de débogage.");
                }
#endif
                return sb.ToString();
            }
        }
    }
}
