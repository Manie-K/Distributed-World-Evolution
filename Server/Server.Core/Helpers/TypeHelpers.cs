namespace Server.Core.Helpers
{
    public static class TypeHelpers
    {
        public static List<Type> GetDirectParentInterfaces(Type type)
        {
            var interfaces = type.GetInterfaces();

            var directInterfaces = interfaces
                .Where(i => !interfaces.Any(other => other != i && other.IsAssignableFrom(i)))
                .ToList();

            return directInterfaces;
        }

        public static Type? GetFirstAbstractParentType(Type type)
        {
            Type? current = type.BaseType;

            while (current != null)
            {
                if (current.IsAbstract && current.IsClass)
                {
                    return current;
                }

                current = current.BaseType;
            }

            return null;
        }
    }
}
