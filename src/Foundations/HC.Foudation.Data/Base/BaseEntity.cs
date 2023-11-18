using System.ComponentModel.DataAnnotations.Schema;
using HC.Foundation.Data.Base.IBase;
using static HC.Foundation.Core.Constants.Constants;

namespace HC.Foundation.Data.Base
{
    public class BaseEntity : IBaseEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime UpdatedOn { get; set; }

        public string? CreatedBy { get; set; }

        public string? UpdatedBy { get; set; }

        public Status Status { get; set; }
    }
}