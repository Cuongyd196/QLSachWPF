namespace QLSachWPF.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("NguoiDung")]
    public partial class NguoiDung
    {
        [Key]
        public int IDNguoiDung { get; set; }

        [Required]
        [StringLength(100)]
        public string TenNguoiDung { get; set; }

        [StringLength(100)]
        public string Email { get; set; }

        [Required]
        [StringLength(100)]
        public string MatKhau { get; set; }

        public DateTime? NgayDangKy { get; set; }

        [StringLength(50)]
        public string VaiTro { get; set; }
    }
}
