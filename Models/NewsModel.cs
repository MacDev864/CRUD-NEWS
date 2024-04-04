using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRUD_NEWS.Models
{
    public class NewsModel
    {
        public string ? id { get; set; }

        [DisplayName("ชื่อข่าวสาร")]
        public string ? name { get; set; }

        [DisplayName("รายละเอียดข่าวสาร")]
        public string ? description { get; set; } = null;

        [DisplayName("รูปข่าวสาร")]
        public string? img { get; set; } = null;

        [Column(TypeName = "datetime2")]
        public DateTime created_at { get; set; }
        [Column(TypeName = "datetime2")]

        public DateTime updated_at { get; set; }

        public bool is_deleted { get; set; } = false;
    }
}
