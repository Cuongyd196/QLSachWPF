namespace QLSachWPF.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Sach")]
    public partial class Sach
    {
        [Key]
        public int IDSach { get; set; }

        [StringLength(10)]
        public string MaSach { get; set; }

        [Required]
        [StringLength(250)]
        public string TenSach { get; set; }

        public decimal? DonGia { get; set; }

        public int? SoLuong { get; set; }

        public int IDLoaiSach { get; set; }

        [StringLength(250)]
        public string AnhBia { get; set; }

        public virtual LoaiSach LoaiSach { get; set; }
    }
}
