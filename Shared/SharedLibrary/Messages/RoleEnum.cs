using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SharedLibrary.Messages
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum RoleEnum
    {
        User,
        UI
    }
}
