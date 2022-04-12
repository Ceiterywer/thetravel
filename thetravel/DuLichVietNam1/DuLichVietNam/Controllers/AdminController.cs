using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DuLichVietNam.Models;
using PagedList;
using PagedList.Mvc;

namespace DuLichVietNam.Controllers
{
    public class AdminController : Controller
    {
        DbDuLichVietDataContext db = new DbDuLichVietDataContext();
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Dangnhap()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Dangnhap(FormCollection collection)
        {
            var tendn = collection["UserAdmin"];
            var matkhau = collection["PassAdmin"];
            if (String.IsNullOrEmpty(tendn))
            {
                ViewData["Loi1"] = "Phai nhap ten dang nhap";
            }
            else if (String.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi2"] = "Phai nhap mat khau";
            }
            else
            {
                ADMIN ad = db.ADMINs.SingleOrDefault(n => n.UserAdmin == tendn && n.PassAdmin == matkhau);
                if (ad != null)
                {
                    ViewBag.Thongbao = "Chuc mung dang nhap thanh cong";
                    Session["UserAdmin"] = ad;
                    return Redirect("~/Admin/Index");
                }
                else
                    ViewBag.Thongbao = "Ten dang nhap hoac mat khau khong dung";
            }
            return RedirectToAction("Index");


        }
        public ActionResult Tour(int? page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 5;
            //return View(db.SACHES. ToList());
            return View(db.TOURs.ToList().OrderBy(n => n.MaTour).ToPagedList(pageNumber, pageSize));
        }
        [HttpGet]
        public ActionResult ThemmoiTour()
        {
            ViewBag.MaDD = new SelectList(db.DIADIEMs.ToList().OrderBy(n => n.TenDD), "MaDD", "TenDD");
            ViewBag.MaPT = new SelectList(db.PHUONGTIENs.ToList().OrderBy(n => n.LoaiPT), "MaPT", "LoaiPT");
            ViewBag.MaKS = new SelectList(db.KHACHSANs.ToList().OrderBy(n => n.TenKS), "MaKS", "TenKS");
            
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Themmoitour(TOUR tour, HttpPostedFileBase fileupload)
        {
            ViewBag.MaDD = new SelectList(db.DIADIEMs.ToList().OrderBy(n => n.TenDD), "MaDD", "TenDD");
            ViewBag.MaPT = new SelectList(db.PHUONGTIENs.ToList().OrderBy(n => n.LoaiPT), "MaPT", "LoaiPT");
            ViewBag.MaKS = new SelectList(db.KHACHSANs.ToList().OrderBy(n => n.TenKS), "MaKS", "TenKS");
            
            if (fileupload == null)
            {
                ViewBag.Thongbao = "Vui lòng chọn ảnh bìa";
                return View();
            }

            //Luu ten fie, luu y bo sung thu vien using System.I0;
            var fileName = Path.GetFileName(fileupload.FileName);
            // Luu duong dan cua file
            var path = Path.Combine(Server.MapPath("~/Content/images"), fileName);
            //Kiem tra hình anh ton tai chua?
            if (System.IO.File.Exists(path))
            {
                ViewBag.Thongbao = "Hình ảnh đã tồn tại";
            }
            else
            {
                fileupload.SaveAs(path);
            }
            tour.Anhbia = fileName;
            db.TOURs.InsertOnSubmit(tour);
            db.SubmitChanges();
            return RedirectToAction("Tour");
        }

        public ActionResult ChitietTour(int id)
        {
            //Lay ra doi tuong sach theo ma
            TOUR tour = db.TOURs.SingleOrDefault(n => n.MaTour== id);
            ViewBag.Masach = tour.MaTour;
            if (tour == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(tour);

        }
        public ActionResult Xoatour(int id)
        {
            //Lay ra doi tuong sach can xoa theo ma
            TOUR tour = db.TOURs.SingleOrDefault(n => n.MaTour == id);
            ViewBag.Masach = tour.MaTour;
            if (tour == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(tour);
        }
        [HttpPost, ActionName("Xoatour")]
        public ActionResult Xacnhanxoa(int id)
        {
            //Lay ra doi tuong sach can xoa theo ma
            TOUR tour = db.TOURs.SingleOrDefault(n => n.MaTour == id);
            ViewBag.Masach = tour.MaTour;
            if (tour == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            db.TOURs.DeleteOnSubmit(tour);
            db.SubmitChanges();
            return RedirectToAction("Tour");
        }
        [HttpGet]
        public ActionResult Suatour(int id)
        {

            //Lay ra doi tuong sach theo ma
            TOUR tour = db.TOURs.SingleOrDefault(n => n.MaTour == id);
            if (tour == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            ViewBag.MaPT = new SelectList(db.PHUONGTIENs.ToList().OrderBy(n => n.LoaiPT), "MaPT", "LoaiPT");
            ViewBag.MaKS = new SelectList(db.KHACHSANs.ToList().OrderBy(n => n.TenKS), "MaKS", "TenKS");
            
            ViewBag.MaDD = new SelectList(db.DIADIEMs.ToList().OrderBy(n => n.TenDD), "MaDD", "TenDD");
            return View(tour);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Suatour(TOUR tour, HttpPostedFileBase fileUpload)
        {
            //Dua du lieu vao dropdownload
            ViewBag.MaPT = new SelectList(db.PHUONGTIENs.ToList().OrderBy(n => n.LoaiPT), "MaPT", "LoaiPT");
            ViewBag.MaKS = new SelectList(db.KHACHSANs.ToList().OrderBy(n => n.TenKS), "MaKS", "TenKS");
            
            ViewBag.MaDD = new SelectList(db.DIADIEMs.ToList().OrderBy(n => n.TenDD), "MaDD", "TenDD");
            //Kiem tra duong dan file
            if (fileUpload == null)
            {
                ViewBag.Thongbao = "Vui lòng chọn ảnh bìa";
                return View();
            }
            //Them vao CSDL
            else
            {
                if (ModelState.IsValid)
                {
                    //Luu ten fie, luu y bo sung thu vien using System. I0;
                    var fileName = Path.GetFileName(fileUpload.FileName);
                    //Luu duong dan cua file
                    var path = Path.Combine(Server.MapPath("~/Content/images"), fileName);
                    //Kiem tra hình anh ton tai chua?
                    if (System.IO.File.Exists(path))
                        ViewBag.Thongbao = "Hình ảnh đã tồn tại";
                    else
                    {
                        //Luu hinh anh vao duong dan
                        fileUpload.SaveAs(path);
                    }
                    tour.Anhbia = fileName;
                    //Luu vao CSDL
                    UpdateModel(tour);
                    db.SubmitChanges();
                }
                return RedirectToAction("Tour");
            }
        }
        public ActionResult TinTuc(int? page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 5;
            //return View(db.SACHES. ToList());
            return View(db.TINTUCs.ToList().OrderBy(n => n.MaTT).ToPagedList(pageNumber, pageSize));
        }
        [HttpGet]
        public ActionResult ThemmoiTinTuc()
        {
            ViewBag.MaPT = new SelectList(db.TINTUCs.ToList().OrderBy(n => n.MaTT), "MaTT", "TenTT");
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ThemmoiTinTuc(TINTUC tt, HttpPostedFileBase fileupload)
        {
            ViewBag.MaPT = new SelectList(db.TINTUCs.ToList().OrderBy(n => n.MaTT), "MaTT", "TenTT");
            if (fileupload == null)
            {
                ViewBag.Thongbao = "Vui lòng chọn ảnh bìa";
                return View();
            }

            //Luu ten fie, luu y bo sung thu vien using System.I0;
            var fileName = Path.GetFileName(fileupload.FileName);
            // Luu duong dan cua file
            var path = Path.Combine(Server.MapPath("~/Content/images"), fileName);
            //Kiem tra hình anh ton tai chua?
            if (System.IO.File.Exists(path))
            {
                ViewBag.Thongbao = "Hình ảnh đã tồn tại";
            }
            else
            {
                fileupload.SaveAs(path);
            }
            tt.Anhbia = fileName;
            db.TINTUCs.InsertOnSubmit(tt);
            db.SubmitChanges();
            return RedirectToAction("TinTuc");
        }
        public ActionResult ChitietTinTuc(int id)
        {
            //Lay ra doi tuong sach theo ma
            TINTUC tt = db.TINTUCs.SingleOrDefault(n => n.MaTT == id);
            ViewBag.Masach = tt.MaTT;
            if (tt == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(tt);

        }
        public ActionResult Xoatintuc(int id)
        {
            //Lay ra doi tuong sach can xoa theo ma
            TINTUC tt = db.TINTUCs.SingleOrDefault(n => n.MaTT == id);
            ViewBag.Masach = tt.MaTT;
            if (tt == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(tt);
        }
        [HttpPost, ActionName("Xoatintuc")]
        public ActionResult Xacnhanxoatintuc(int id)
        {
            //Lay ra doi tuong sach can xoa theo ma
            TINTUC tt = db.TINTUCs.SingleOrDefault(n => n.MaTT == id);
            ViewBag.Masach = tt.MaTT;
            if (tt == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            db.TINTUCs.DeleteOnSubmit(tt);
            db.SubmitChanges();
            return RedirectToAction("TinTuc");
        }
        [HttpGet]
        public ActionResult Suatintuc(int id)
        {
            TINTUC tt = db.TINTUCs.SingleOrDefault(n => n.MaTT == id);
            if (tt == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            ViewBag.MaPT = new SelectList(db.TINTUCs.ToList().OrderBy(n => n.MaTT), "MaTT", "TenTT");
            return View(tt);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Suatintuc(TINTUC tt, HttpPostedFileBase fileUpload)
        {
            ViewBag.MaPT = new SelectList(db.TINTUCs.ToList().OrderBy(n => n.MaTT), "MaTT", "TenTT");
            if (fileUpload == null)
            {
                ViewBag.Thongbao = "Vui lòng chọn ảnh bìa";
                return View();
            }
            else
            {
                if (ModelState.IsValid)
                {
                    var fileName = Path.GetFileName(fileUpload.FileName);
                    var path = Path.Combine(Server.MapPath("~/Content/images"), fileName);
                    if (System.IO.File.Exists(path))
                        ViewBag.Thongbao = "Hình ảnh đã tồn tại";
                    else
                    {
                        fileUpload.SaveAs(path);
                    }
                    tt.Anhbia = fileName;
                    
                    UpdateModel(tt);
                    db.SubmitChanges();
                }
                return RedirectToAction("TinTuc");
            }
        }
        public ActionResult PhuongTien(int? page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 5;
            return View(db.PHUONGTIENs.ToList().OrderBy(n => n.MaPT).ToPagedList(pageNumber, pageSize));
        }
        [HttpGet]
        public ActionResult ThemmoiPT()
        {
            ViewBag.MaPT = new SelectList(db.PHUONGTIENs.ToList().OrderBy(n => n.MaPT), "MaPT", "TenPT");
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ThemmoiPT(PHUONGTIEN pt)
        {
            ViewBag.MaPT = new SelectList(db.PHUONGTIENs.ToList().OrderBy(n => n.MaPT), "MaPT", "TenPT");
            db.PHUONGTIENs.InsertOnSubmit(pt);
            db.SubmitChanges();
            return RedirectToAction("PhuongTien");
        }
        [HttpGet]
        public ActionResult Suaphuongtien(int id)
        {

            PHUONGTIEN pt = db.PHUONGTIENs.SingleOrDefault(n => n.MaPT == id);
            if (pt == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            ViewBag.MaPT = new SelectList(db.PHUONGTIENs.ToList().OrderBy(n => n.MaPT), "MaPT", "TenPT");
            return View(pt);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Suaphuongtien(PHUONGTIEN pt)
        {
            ViewBag.MaPT = new SelectList(db.PHUONGTIENs.ToList().OrderBy(n => n.MaPT), "MaPT", "TenPT");
            {
                if (ModelState.IsValid)
                {
                    UpdateModel(pt);
                    db.SubmitChanges();
                }
                return RedirectToAction("PhuongTien");
            }
        }
        public ActionResult Xoaphuongtien(int id)
        {
            //Lay ra doi tuong sach can xoa theo ma
            PHUONGTIEN pt = db.PHUONGTIENs.SingleOrDefault(n => n.MaPT == id);
            ViewBag.Masach = pt.MaPT;
            if (pt == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(pt);
        }
        [HttpPost, ActionName("Xoaphuongtien")]
        public ActionResult Xacnhanxoapt(int id)
        {
            //Lay ra doi tuong sach can xoa theo ma
            PHUONGTIEN pt = db.PHUONGTIENs.SingleOrDefault(n => n.MaPT == id);
            ViewBag.Masach = pt.MaPT;
            if (pt == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            db.PHUONGTIENs.DeleteOnSubmit(pt);
            db.SubmitChanges();
            return RedirectToAction("PhuongTien");
        }
        public ActionResult Khachsan(int? page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 5;

            return View(db.KHACHSANs.ToList().OrderBy(n => n.MaKS).ToPagedList(pageNumber, pageSize));
        }
        [HttpGet]
        public ActionResult ThemmoiKS()
        {
            ViewBag.MaKS = new SelectList(db.KHACHSANs.ToList().OrderBy(n => n.MaKS), "MaKS", "TenKS");
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ThemmoiKS(KHACHSAN ks, HttpPostedFileBase fileupload)
        {
            ViewBag.MaKS = new SelectList(db.KHACHSANs.ToList().OrderBy(n => n.MaKS), "MaKS", "TenKS");
            if (fileupload == null)
            {
                ViewBag.Thongbao = "Vui lòng chọn ảnh bìa";
                return View();
            }

            //Luu ten fie, luu y bo sung thu vien using System.I0;
            var fileName = Path.GetFileName(fileupload.FileName);
            // Luu duong dan cua file
            var path = Path.Combine(Server.MapPath("~/Content/images"), fileName);
            //Kiem tra hình anh ton tai chua?
            if (System.IO.File.Exists(path))
            {
                ViewBag.Thongbao = "Hình ảnh đã tồn tại";
            }
            else
            {
                fileupload.SaveAs(path);
            }
            ks.AnhKS = fileName;
            db.KHACHSANs.InsertOnSubmit(ks);
            db.SubmitChanges();
            return RedirectToAction("Khachsan");
        }
        [HttpGet]
        public ActionResult SuaKS(int id)
        {

            //Lay ra doi tuong sach theo ma
            KHACHSAN ks = db.KHACHSANs.SingleOrDefault(n => n.MaKS == id);
            if (ks == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            ViewBag.MaKS = new SelectList(db.KHACHSANs.ToList().OrderBy(n => n.MaKS), "MaKS", "TenKS");
            return View(ks);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SuaKS(KHACHSAN ks, HttpPostedFileBase fileupload)
        {
            ViewBag.MaKS = new SelectList(db.KHACHSANs.ToList().OrderBy(n => n.MaKS), "MaKS", "TenKS");
            if (fileupload == null)
            {

                return View();
            }
            else
            {
                if (ModelState.IsValid)
                {
                    var fileName = Path.GetFileName(fileupload.FileName);
                    var path = Path.Combine(Server.MapPath("~/Content/images"), fileName);
                    if (System.IO.File.Exists(path))
                    {
                        ViewBag.Thongbao = "Hình ảnh đã tồn tại";
                    }
                    else
                    {
                        fileupload.SaveAs(path);
                    }
                    ks.AnhKS = fileName;
                    UpdateModel(ks);
                    db.SubmitChanges();
                }
                return RedirectToAction("Khachsan");
            }
        }
        public ActionResult XoaKS(int id)
        {
            //Lay ra doi tuong sach can xoa theo ma
            KHACHSAN ks = db.KHACHSANs.SingleOrDefault(n => n.MaKS == id);
            ViewBag.MaKS = ks.MaKS;
            if (ks == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(ks);
        }
        [HttpPost, ActionName("XoaKS")]
        public ActionResult XacKS(int id)
        {
            KHACHSAN ks = db.KHACHSANs.SingleOrDefault(n => n.MaKS == id);
            ViewBag.MaKS = ks.MaKS;
            if (ks == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            db.KHACHSANs.DeleteOnSubmit(ks);
            db.SubmitChanges();
            return RedirectToAction("Khachsan");
        }
    }
}