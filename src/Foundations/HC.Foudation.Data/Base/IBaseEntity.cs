using System.ComponentModel.DataAnnotations.Schema;
using static HC.Foundation.Core.Constants.Constants;

namespace HC.Foundation.Data.Base
{
    public interface IBaseEntity : IBaseInfo
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public Status Status { get; set; }
    }

    public interface IBaseInfo
    {
        public DateTime CreatedOn { get; set; }

        public DateTime UpdatedOn { get; set; }

        public string? CreatedBy { get; set; }

        public string? UpdatedBy { get; set; }
    }
}