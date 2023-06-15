using System.ComponentModel.DataAnnotations;

namespace DDHelpers.Api
{
    public class BaseEntity
    {
        [Key]
        public int Id { get; set; }
    }
}
