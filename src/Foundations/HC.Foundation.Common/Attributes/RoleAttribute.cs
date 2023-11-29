using static HC.Foundation.Common.Constants.Constants;

namespace HC.Foundation.Common.Attributes
{
    public class RoleInfoAttribute : Attribute
    {
        public string Name { get; set; }

        public string Code { get; set; }

        public static List<Role> GetValues()
        {
            List<Role> es = Enum.GetValues(typeof(Role)).Cast<Role>().ToList();
            return es;
        }

        public static Role? FromName(string name)
        {
            IEnumerable<Role> es = Enum.GetValues(typeof(Role)).Cast<Role>();
            es = es.Where(e => string.Equals(e.Attribute<RoleInfoAttribute>().Name, name, StringComparison.OrdinalIgnoreCase));
            Role? f = First(es);
            return f;
        }

        public static string ToName(Role? e)
        {
            return e?.Attribute<RoleInfoAttribute>().Name;
        }

        public static Role? FromCode(string code)
        {
            IEnumerable<Role> es = Enum.GetValues(typeof(Role)).Cast<Role>();
            es = es.Where(e => string.Equals(e.Attribute<RoleInfoAttribute>().Code, code, StringComparison.OrdinalIgnoreCase));
            Role? f = First(es);
            return f;
        }

        public static string ToCode(Role? e)
        {
            return e?.Attribute<RoleInfoAttribute>().Code;
        }

        private static Role? First(IEnumerable<Role> es)
        {
            Role? e = null;
            e = es == null || es.Any() ? es.First() : e;
            return e;
        }
    }

    public static class RoleAttributeExtension
    {
        public static A Attribute<A>(this Enum value)
            where A : RoleInfoAttribute
        {
            var enumType = value.GetType();
            var name = Enum.GetName(enumType, value);
            return enumType.GetField(name).GetCustomAttributes(false).OfType<A>().SingleOrDefault();
        }
    }
}