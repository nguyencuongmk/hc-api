using static HC.Foundation.Common.Constants.Constants;

namespace HC.Foundation.Common.Attributes
{
    public class RoleInfoAttribute : Attribute
    {
        public string Name { get; set; }

        public string Code { get; set; }

        public static List<RoleType> GetValues()
        {
            List<RoleType> es = Enum.GetValues(typeof(RoleType)).Cast<RoleType>().ToList();
            return es;
        }

        public static RoleType? FromName(string name)
        {
            IEnumerable<RoleType> es = Enum.GetValues(typeof(RoleType)).Cast<RoleType>();
            es = es.Where(e => string.Equals(e.Attribute<RoleInfoAttribute>().Name, name, StringComparison.OrdinalIgnoreCase));
            RoleType? f = First(es);
            return f;
        }

        public static string ToName(RoleType? e)
        {
            return e?.Attribute<RoleInfoAttribute>().Name;
        }

        public static RoleType? FromCode(string code)
        {
            IEnumerable<RoleType> es = Enum.GetValues(typeof(RoleType)).Cast<RoleType>();
            es = es.Where(e => string.Equals(e.Attribute<RoleInfoAttribute>().Code, code, StringComparison.OrdinalIgnoreCase));
            RoleType? f = First(es);
            return f;
        }

        public static string ToCode(RoleType? e)
        {
            return e?.Attribute<RoleInfoAttribute>().Code;
        }

        private static RoleType? First(IEnumerable<RoleType> es)
        {
            RoleType? e = null;
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