namespace Server.Core.Helpers
{
    public static class InterfaceHelpers
    {
        public static List<Type> GetDirectParentInterfaces(Type type)
        {
            var interfaces = type.GetInterfaces();

            var directInterfaces = interfaces
                .Where(i => !interfaces.Any(other => other != i && other.IsAssignableFrom(i)))
                .ToList();

            return directInterfaces;
        }
    }
}
