using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLBV
{
    public class PhieuKhamInfo2
    {
        public string MaPhieuKham { get; set; }
        public string HoTenBN { get; set; } // Patient Name
        public string MaBN { get; set; } // Patient Code
        public string HoTenNhanVien { get; set; } // Employee Name
        public string MaPhongKham { get; set; }
        public string TrieuChung { get; set; } // Symptoms
        public string KetLuan { get; set; } // Conclusion
        public DateTime NgayKham { get; set; } // Date of Examination
        public string GhiChu { get; set; } // Notes

    }



    internal class PhieuKhamInfo
    {
    }
   
}
