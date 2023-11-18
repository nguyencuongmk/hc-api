using System.ComponentModel;

namespace HC.Foundation.Core.Constants
{
    public static class Constants
    {
        public enum Status
        {
            [Description("Deleted")]
            Deleted,
            [Description("Created")]
            Created,
            [Description("Modified")]
            Modified
        }
    }
}