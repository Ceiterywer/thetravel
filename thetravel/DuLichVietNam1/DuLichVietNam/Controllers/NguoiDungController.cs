using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using DuLichVietNam.Models;

namespace DuLichVietNam.Controllers
{
    public class NguoiDungController : Controller
    {
        DbDuLichVietDataContext db = new DbDuLichVietDataContext();
        // GET: NguoiDung
        
        [HttpGet]
        
        public ActionResult DangKy()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DangKy(FormCollection collection, TTKHACHHANG kh)
        {
            var hoten = collection["HotenKH"];
            var tendn = collection["TenDN"];
            var matkhau = collection["Matkhau"];
            var matkhaunhaplai = collection["Matkhaunhaplai"];
            var diachi = collection["Diachi"];
            var email = collection["Email"];
            var encrup_password = collection["matkhau"];
            var dienthoai = collection["Dienthoai"];
            var ngaysinh = string.Format("{0:MM/dd/yyyy}", collection["Ngaysinh"]);
            if (String.IsNullOrEmpty(hoten))
            {
                ViewData["Loi1"] = "Họ tên khách hàng không được để trống!";
            }
            if (String.IsNullOrEmpty(tendn))
            {
                ViewData["Loi2"] = "Phải nhập tên đăng nhập!";
            }
            if (String.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi3"] = "Phải nhập mật khẩu!";
            }
            if (String.IsNullOrEmpty(matkhaunhaplai))
            {
                ViewData["Loi4"] = "Phải nhập lại mật khẩu!";
            }
            if (String.IsNullOrEmpty(email))
            {
                ViewData["Loi5"] = "Email không được bỏ trống!";
            }
            if (String.IsNullOrEmpty(dienthoai))
            {
                ViewData["Loi6"] = "SĐT không được bỏ trống!";
            }
            if (String.IsNullOrEmpty(diachi))
            {
                ViewData["Loi7"] = "Địa chỉ không được bỏ trống!";
            }
            
            else
            {
                kh.HotenKH = hoten;
                kh.Taikhoan = tendn;
                kh.Matkhau = encrup_password;
                kh.Email = email;
                kh.DiaChiKH = diachi;
                kh.SDT = dienthoai;
                
                db.TTKHACHHANGs.InsertOnSubmit(kh);
                db.SubmitChanges();
                return RedirectToAction("Dangnhap");
            }      
            return View();
        }
        [HttpGet]
        
        public ActionResult DangNhap()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Dangnhap(FormCollection collection)
        {
            var tendn = collection["TenDN"];
            var matkhau = collection["Matkhau"];

            if (String.IsNullOrEmpty(tendn))
            {
                ViewData["Loi1"] = "Phải nhập tên đăng nhập!";
            }
            else if (String.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi2"] = "Phải nhập mật khẩu!";
            }
            else
            {
                TTKHACHHANG kh = db.TTKHACHHANGs.SingleOrDefault(n => n.Taikhoan == tendn && n.Matkhau == matkhau);
                if (kh != null)
                {
                    ViewBag.Thongbao = "Đăng nhập thành công!";
                    Session["Taikhoan"] = kh;
                    return RedirectToAction("Index", "WebDuLich");
                }
                else

                    ViewBag.Thongbao = "Tên tài khoản hoặc mật khẩu không đúng!";
            }
            return View();


        }
    }
}