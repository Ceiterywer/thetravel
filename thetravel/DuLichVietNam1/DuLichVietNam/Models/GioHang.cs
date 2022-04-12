using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DuLichVietNam.Models;

namespace DuLichVietNam.Models
{
    public class GioHang
    {
        DbDuLichVietDataContext data = new DbDuLichVietDataContext();
        public int iMaTour { set; get; }
        public string sTenTour { set; get; }
        public string sAnhbia { set; get; }
        public Double dGiaTour { set; get; }
        public string sThoiGian { set; get; }
        public string sXuatPhat { set; get; }
        public int iSL { set; get; }
        public Double ThanhTien
        {
            get { return iSL * dGiaTour; }
        }

        public GioHang(int MaTour)
        {
            iMaTour = MaTour;
            TOUR sp = data.TOURs.Single(n => n.MaTour == iMaTour);
            sTenTour = sp.TenTour;
            sAnhbia = sp.Anhbia;
            sThoiGian = sp.ThoiGian;
            sXuatPhat = sp.XuatPhat;
            dGiaTour = float.Parse(sp.GiaTour.ToString());
            iSL = 1;
        }
        
    }
}