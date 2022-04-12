using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList.Mvc;
using PagedList;
using DuLichVietNam.Models;

namespace DuLichVietNam.Controllers
{
    public class WebDuLichController : Controller
    {
        DbDuLichVietDataContext data = new DbDuLichVietDataContext();
        // GET: WebDuLich
        public ActionResult Index()
        {
            var tourmoi = Tourmoi(5);
            return View(tourmoi);
        }
        
        // TOUR
        private List<TOUR> Tourmoi(int count)
        {
            return data.TOURs.Take(count).ToList();
        }
        
        public ActionResult Tour(int? page)
        {
            var tourmoi = Tourmoi(5);
            int pageNumber = (page ?? 1);
            int pageSize = 9;
            //return View(db.SACHES. ToList());
            return View(data.TOURs.ToList().OrderBy(n => n.MaTour).ToPagedList(pageNumber, pageSize));
        }
        // TIN TUC
        private List<TINTUC> Tintucmoi(int count)
        {
            return data.TINTUCs.Take(count).ToList();
        }
        public ActionResult TinTuc(int? page)
        {
            var tintucmoi = Tintucmoi(5);
            int pageNumber = (page ?? 1);
            int pageSize = 9;
            return View(data.TINTUCs.ToList().OrderBy(n => n.MaTT).ToPagedList(pageNumber, pageSize));
        }
        // CHI TIET
        public ActionResult ChiTietTour(int id)
        {
            var tour = from t in data.TOURs
                       
                       where t.MaTour == id   
                       select t;
            
            return View(tour.Single());
        }
        public ActionResult ChiTietTT(int id)
        {
            var tintuc = from tt in data.TINTUCs
                       where tt.MaTT == id
                       select tt;
            return View(tintuc.Single());
        }

        public ActionResult DiaDiem()
        {
            var diadiem  = from dd in data.DIADIEMs select dd;
            return PartialView(diadiem);
        }
        public ActionResult SPtheoDiaDiem(int id, int? page)
        {
            int pageSize = 9;
            int PageNum = (page ?? 1);
            var sptdd = from s in data.TOURs where s.MaDD == id select s;
            return View(sptdd.ToPagedList(PageNum, pageSize));
        }   
        public ActionResult ChiTiettheoDiaDiem(int id)
        {
            var cttdd = from s in data.TOURs where s.MaDD == id select s;
            return View(cttdd.Single());
        }
        private List<KHACHSAN> KSmoi(int count)
        {
            return data.KHACHSANs.Take(count).ToList();
        }
        public ActionResult KhachSan(int? page)
        {
            var khachsan = KSmoi(5);
            int pageNumber = (page ?? 1);
            int pageSize = 9;
            return View(data.KHACHSANs.ToList().OrderBy(n => n.MaKS).ToPagedList(pageNumber, pageSize));
        }
        public ActionResult ChiTietKS(int id)
        {
            var khachsan = from ks in data.KHACHSANs
                         where ks.MaKS == id
                         select ks;
            return View(khachsan.Single());
        }
        public ActionResult DD_KhachSan()
        {
            var diadiem = from dd in data.KS_DIADIEMs select dd;
            return PartialView(diadiem);
        }
        public ActionResult KStheoDiaDiem(int id, int? page)
        {
            int pageSize = 9;
            int PageNum = (page ?? 1);
            var kstdd = from s in data.KHACHSANs where s.MaPLKS == id select s;
            return View(kstdd.ToPagedList(PageNum, pageSize));
        }
        public ActionResult ChiTietKStheoDiaDiem(int id)
        {
            var cttdd = from s in data.KHACHSANs where s.MaPLKS == id select s;
            return View(cttdd.Single());
        }
    }
}