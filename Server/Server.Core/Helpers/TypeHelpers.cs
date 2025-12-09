namespace Server.Core.Helpers
{
    /// <summary>
    /// Class containing helper methods for type reflection.
    /// </summary>
    public static class TypeHelpers
    {
        /// <summary>
        /// Retrieves the direct parent interfaces of a given type.
        /// </summary>
        /// <param name="type"> The type to analyze. </param>
        /// <returns> List of direct parent interfaces. </returns>
        public static List<Type> GetDirectParentInterfaces(Type type)
        {
            var interfaces = type.GetInterfaces();

            var directInterfaces = interfaces
                .Where(i => !interfaces.Any(other => other != i && other.IsAssignableFrom(i)))
                .ToList();

            return directInterfaces;
        }

        /// <summary>
        /// Retrieves the first abstract parent class of a given type.
        /// </summary>
        /// <param name="type"> The type to analyze. </param>
        /// <returns> The first abstract parent class, or null if none exists. </returns>
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