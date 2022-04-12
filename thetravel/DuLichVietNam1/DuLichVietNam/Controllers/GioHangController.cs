using DuLichVietNam.ThanhToan;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using DuLichVietNam.Models;

namespace MvcBookStore.Controllers
{
    public class GioHangController : Controller
    {
        // GET: Giohang
        //Tao doi tuong data chua dữ liệu từ model dbBansach đã tạo.
        DbDuLichVietDataContext data = new DbDuLichVietDataContext();
        public List<GioHang> Laygiohang()
        {
            List<GioHang> lstGiohang = Session["Giohang"] as List<GioHang>;
            if (lstGiohang == null)
            {

                lstGiohang = new List<GioHang>();
                Session["Giohang"] = lstGiohang;
            }
            return lstGiohang;
        }
        public ActionResult ThemGiohang(int iMaTour, string strURL)
        {
            
            List<GioHang> lstGiohang = Laygiohang();

            GioHang sanpham = lstGiohang.Find(n => n.iMaTour == iMaTour);
            if (sanpham == null)
            {
                sanpham = new GioHang(iMaTour);
                lstGiohang.Add(sanpham);
                return RedirectToAction("Index", "WebDuLich");
            }
            else
            {
                sanpham.iSL++;
                return RedirectToAction("Index", "WebDuLich");
            }

        }
        private int TongSoluong()
        {
            int TongSoLuong = 0;
            List<GioHang> lstGiohang = Session["GioHang"] as List<GioHang>;
            if (lstGiohang != null)
            {
                TongSoLuong = lstGiohang.Sum(n => n.iSL);
            }
            return TongSoLuong;
        }
        private double TongTien()
        {
            double TongTien = 0;
            List<GioHang> lsGioHang = Session["GioHang"] as List<GioHang>;
            if (lsGioHang != null)
            {
                TongTien = lsGioHang.Sum(n => n.ThanhTien);
            }
            return TongTien;
        }
        public ActionResult CapNhatGioHang(int MaTour, FormCollection f)
        {
            List<GioHang> lsGioHang = Laygiohang();
            GioHang tour = lsGioHang.SingleOrDefault(n => n.iMaTour == MaTour);
            if (tour != null)
            {
                tour.iSL = int.Parse(f["txtSoLuong"].ToString());
            }
            return RedirectToAction("GioHang");
        }

        public ActionResult GioHang()
        {
            List<GioHang> lstGiohang = Laygiohang();
            if (lstGiohang.Count == 0)
            {
                return RedirectToAction("Index", "WebDuLich");
            }
            ViewBag.Tongsoluong = TongSoluong();
            ViewBag.TongTien = TongTien();
            return View(lstGiohang);
        }
        public ActionResult GiohangPartial()
        {
            ViewBag.Tongsoluong = TongSoluong();
            return PartialView();
        }
        public ActionResult XoaGiohang(int IMASP)
        {
            //Lay gio hang tu Session
            List<GioHang> lstGiohang = Laygiohang();
            //Kiem tra sach da co trong Session["Giohang"]
            GioHang sanpham = lstGiohang.SingleOrDefault(n => n.iMaTour == IMASP);
            //Neu ton tai thi cho sua Soluong
            if (sanpham != null)
            {
                lstGiohang.RemoveAll(n => n.iMaTour == IMASP);
                return RedirectToAction("GioHang");
            }
            if (lstGiohang.Count == 0)
            {
                return RedirectToAction("Index", "BookStore");
            }
            return RedirectToAction("GioHang");
        }
        public ActionResult CapnhatGiohang(int iMaSP, FormCollection f)
        {
            //Lay gio hang tu Session
            List<GioHang> lstGiohang = Laygiohang();
            //Kiem tra sach da co trong Session["Giohang"]
            GioHang sanpham = lstGiohang.SingleOrDefault(n => n.iMaTour == iMaSP);
            //Neu ton tai thi cho sua Soluong
            if (sanpham != null)
            {
                sanpham.iSL = int.Parse(f["txtSoluong"].ToString());
            }
            return RedirectToAction("Giohang");
        }
        public ActionResult XoaTatcaGiohang()
        {
            //Lay gio hang tu Session
            List<GioHang> lstGiohang = Laygiohang();
            lstGiohang.Clear();
            return RedirectToAction("Index", "BookStore");
        }
        [HttpGet]
        public ActionResult DatTour()
        {
            ViewBag.MaThanhToan = new SelectList(data.PTTHANHTOANs.ToList().OrderBy(n => n.TenPT), "MaThanhToan", "TenPT");
            if (Session["Taikhoan"] == null || Session["Taikhoan"].ToString() == "")
            {
                return RedirectToAction("DangNhap", "NguoiDung");
            }
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("Index", "WebDuLich");
            }
                List<GioHang> lstGiohang = Laygiohang();
                ViewBag.Tongsoluong = TongSoluong();
                ViewBag.TongTien = TongTien();
            return View(lstGiohang);
        }

        [HttpPost]
        public ActionResult DatTour(CHITIETDAT ctt, FormCollection collection)
        {
            if(ctt.MaThanhToan == 2)
            {
                ViewBag.MaThanhToan = new SelectList(data.PTTHANHTOANs.ToList().OrderBy(n => n.TenPT), "MaThanhToan", "TenPT");
                CHITIETDAT ddh = new CHITIETDAT();
                TTKHACHHANG kh = (TTKHACHHANG)Session["Taikhoan"];
                List<GioHang> gh = Laygiohang();
                ddh.MaKH = kh.MaKH;
                var Ngaydat = DateTime.Now;
                var Ngaydi = String.Format("{0:MM/dd/yyyy}", collection["NgayDi"]);
                ddh.NgayDi = DateTime.Parse(Ngaydi);
                var sl = collection["SoLuong"];
                if (String.IsNullOrEmpty(sl))
                {
                    ViewData["Loi1"] = "Nhập số lượng người đi";
                }
                else
                {
                    ddh.SoLuong = sl;
                    data.CHITIETDATs.InsertOnSubmit(ddh);
                    data.SubmitChanges();
                }
                Session["GioHang"] = null;
                return RedirectToAction("ConfirmPaymentClient", "ThanhToanMoMo");
            }
            else
            {
                ViewBag.MaThanhToan = new SelectList(data.PTTHANHTOANs.ToList().OrderBy(n => n.TenPT), "MaThanhToan", "TenPT");
                CHITIETDAT ddh = new CHITIETDAT();
                TTKHACHHANG kh = (TTKHACHHANG)Session["Taikhoan"];
                List<GioHang> gh = Laygiohang();
                ddh.MaKH = kh.MaKH;
                var Ngaydat = DateTime.Now;
                var Ngaydi = String.Format("{0:MM/dd/yyyy}", collection["NgayDi"]);
                ddh.NgayDi = DateTime.Parse(Ngaydi);
                var sl = collection["SoLuong"];
                if (String.IsNullOrEmpty(sl))
                {
                    ViewData["Loi1"] = "Nhập số lượng người đi";
                }
                else
                {
                    ddh.SoLuong = sl;
                    data.CHITIETDATs.InsertOnSubmit(ddh);
                    data.SubmitChanges();
                }
                Session["GioHang"] = null;
                return RedirectToAction("Xacnhandon", "GioHang");
            }
        }
        public ActionResult Payment()
        {
            List<GioHang> gioHangs = Laygiohang();
            string endpoint = "https://test-payment.momo.vn/gw_payment/transactionProcessor";
            string partnerCode = "MOMOCOOH20220330";
            string accessKey = "5nDntPAFGSQCYsp7";
            string serectkey = "VA6jzpzQV8EXEFyMWxyFCRV0nw7KRCKc";
            string orderInfo = "test";
            string returnUrl = "https://thevntravel.tk/GioHang/Xacnhandon";
            string notifyurl = "http://ba1adf48beba.ngrok.io/Home/SavePayment";

            string amount = gioHangs.Sum(n => n.dGiaTour).ToString();
            string orderid = DateTime.Now.Ticks.ToString();
            string requestId = DateTime.Now.Ticks.ToString();
            string extraData = "";

            //Before sign HMAC SHA256 signature
            string rawHash = "partnerCode=" +
                partnerCode + "&accessKey=" +
                accessKey + "&requestId=" +
                requestId + "&amount=" +
                amount + "&orderId=" +
                orderid + "&orderInfo=" +
                orderInfo + "&returnUrl=" +
                returnUrl + "&notifyUrl=" +
                notifyurl + "&extraData=" +
                extraData;


            MoMoSecurity crypto = new MoMoSecurity();
            //sign signature SHA256
            string signature = crypto.signSHA256(rawHash, serectkey);
            //build body json request
            JObject message = new JObject
            {
                { "partnerCode", partnerCode },
                { "accessKey", accessKey },
                { "requestId", requestId },
                { "amount", amount },
                { "orderId", orderid },
                { "orderInfo", orderInfo },
                { "returnUrl", returnUrl },
                { "notifyUrl", notifyurl },
                { "extraData", extraData },
                { "requestType", "captureMoMoWallet" },
                { "signature", signature }
            };


            string responseFromMomo = RequestPayment.sendRequestPayment(endpoint, message.ToString());


            JObject jmessage = JObject.Parse(responseFromMomo);

            return Redirect(jmessage.GetValue("payUrl").ToString());

        }
        public ActionResult ComfirmPaymentClient()
        {
            //hien thi thong bao cho nguoi dung
            return View();
        }
        public void SavePayment()
        {
            //cap nhat du lieu vao database
        }

        public ActionResult Xacnhandon()
        {
            Session["GioHang"] = null;
            return View();  
        }
    }
}