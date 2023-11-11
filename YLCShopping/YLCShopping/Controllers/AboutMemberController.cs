using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YLCShopping.Models;

namespace YLCShopping.Controllers
{
    public class AboutMemberController : BaseController
    {
        
        // GET: AboutMember
        public ActionResult Index()
        {
            int id;

            if (Session["memberid"] != null)
            {
                id = (int)Session["memberid"];
            }
            else
            {
                // 处理 Session["memberid"] 为 null 的情况，例如给一个默认值
                id = 0; // 或者其他适当的默认值
            }

            // 继续使用 id 变量

            //所有订单
            var data1 = db.OrderDetailItems.Where(x => x.OrderHeader.MemberAddress.Member.Id == id).ToList();
            ViewData["Allorders"] = data1;
            //已发货订单
            var data2 = db.OrderDetailItems.Where(x => x.OrderHeader.MemberAddress.Member.Id == id && x.Shipmentnumber != null).ToList();
            ViewData["Shippedorders"] = data2;
            //未发货订单
            var data3 = db.OrderDetailItems.Where(x => x.OrderHeader.MemberAddress.Member.Id == id && x.Shipmentnumber == null).ToList();
            ViewData["Unshippedorders"] = data3;

            var data4 = db.MemberAddresses.Where(x => x.Member.Id == id).ToList();
            ViewData["Dizhi"] = data4;
            var data = db.Members.Find(id);
            return View(data);
        }


        //删除地址
        public ActionResult AddressDelect(int id)
        {
            var data = db.MemberAddresses.Find(id);
            if (data != null)
            {
                db.MemberAddresses.Remove(data);
                db.SaveChanges();
            }
            return RedirectToAction("Index", "AdoutMember");
        }
        //用户信息修改
    [HttpPost]
        public ActionResult MemberEdit([Bind(Exclude = "RegisterOn,Password,AuthCode,Email")] Member member , HttpPostedFileBase image = null)
        {
            int id = (int)Session["memberid"];
            var data = db.Members.Where(x => x.Id == id).FirstOrDefault();
            if (image != null && data != null)
            {
                member.ImageMimeType = image.ContentType;
                member.ImageData = new byte[image.ContentLength];
                image.InputStream.Read(member.ImageData, 0, image.ContentLength);
                data.Name = member.Name;
                data.Nickname = member.Nickname;
                data.ImageData = member.ImageData;
                data.ImageMimeType =member.ImageMimeType;
            }
            else
            {
  
                data.Name = member.Name;
                data.Nickname = member.Nickname;
                
            }
            db.SaveChanges();
            return RedirectToAction("Index", "AboutMember");
        }

    }
}