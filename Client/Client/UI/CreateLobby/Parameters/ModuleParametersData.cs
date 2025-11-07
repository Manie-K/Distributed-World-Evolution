namespace Client.UI.CreateLobby.Parameters
{
    public class ModuleParametersData
    {
        public string Name;
        public string Value;
        public int Type;
        public string AdditionalDescription;

        public ModuleParametersData(string name, string value, int type, string additionalDescription = "")
        {
            Name = name;
            Value = value;
            Type = type;
            AdditionalDescription = additionalDescription;
        }
    }
}
