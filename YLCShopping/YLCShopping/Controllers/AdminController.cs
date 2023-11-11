using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using YLCShopping.Models;
using PagedList;
namespace YLCShopping.Controllers
{
    public class AdminController : BaseController
    {
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Login()
        {
            return View();
        }
        public ActionResult AboutMe()
        {
            return View();
        }
        [HttpGet]
        public ActionResult ProductCount()
        {
            var product = db.Products.Select(x => new { x.Id, x.Name, x.Amount, x.Price }).ToList();
            return Json(product, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult ProductCate()
        {
            var product = db.ProductCategories.Select(x => new { x.Id, x.Name, x.Products.Count, }).ToList();
            return Json(product, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult TodayDetail()
        {
            var time = DateTime.Now;
            var data = db.Members.Where(x => x.RegisterOn > time).ToList();
            foreach (var item in data)
            {
                var uu = item.RegisterOn;
            }
            var data1 = data.Select(x => new { data.Count, x.RegisterOn }).ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 订单管理页面
        /// </summary>
        public ActionResult Ordermanagement(int p = 1, int p1 = 1, int p2 = 1, int p3 = 1)
        {
            //已发货订单
            var data2 = db.OrderDetailItems.Where(x => x.Shipmentnumber != null).ToList();
            var pagedData2 = data2.ToPagedList(pageNumber: p1, pageSize: 8);
            ViewData["Shippedorders"] = pagedData2;
            //未发货订单
            var data3 = db.OrderDetailItems.Where(x => x.Shipmentnumber == null).ToList();
            var pagedData3 = data3.ToPagedList(pageNumber: p2, pageSize: 8);
            ViewData["Unshippedorders"] = pagedData3;
            var data4 = db.Carts.ToList();
            var pagedData4 = data4.ToPagedList(pageNumber: p3, pageSize: 8);
            ViewData["AddShoppingCart"] = pagedData4;
            var data = db.Orders.ToList();
            var pagedData = data.ToPagedList(pageNumber: p, pageSize: 8);
            return View(pagedData);
        }
        //删除订单
        [HttpPost]
        public ActionResult Deleteorder(int id)
        {
            OrderDetail data = db.OrderDetailItems.Where(x => x.Id == id).FirstOrDefault();
            string deproduct = data.Product.Name;
            //将订单数量再次加入商品数量
            var data2 = db.Products.Where(x => x.Id == data.Product.Id).FirstOrDefault();
            data2.Amount += data.Amount;
            //删除该订单后需要在主订单价格字段里减去该商品的价格
            var order = db.Orders.Where(x => x.Id == data.OrderHeader.Id).FirstOrDefault();
            order.TotalPrice -= data.Price;
            if (order.TotalPrice <= 0)
            {
                db.Orders.Remove(order);
            }

            db.OrderDetailItems.Remove(data);
            db.SaveChanges();
            return Json(deproduct, JsonRequestBehavior.AllowGet);
        }

        //订单详细信息
        public ActionResult Orderdetails(int? id, int p = 1)
        {
            if (id == null)
            {
                id = 0;
                return View("Error");  // 返回错误页面示例
            }
            else
            {
                var data = db.OrderDetailItems.Where(x => x.OrderHeader.Id == id.Value).ToList();
                var pagedData = data.ToPagedList(pageNumber: p, pageSize: 8);
                return View(pagedData);
            }
        }


        [HttpPost]
        public ActionResult Login(string AdminName, string Pwd)
        {
            Admin admin = db.Admins.Where(p => p.AdminName == AdminName && p.Pwd == Pwd).FirstOrDefault();
            if (admin != null)
            {
                var ad = admin.ChinaName;
                Session["adminname1"] = ad;
                return RedirectToAction("Index", "Admin");
            }
            else
            {
                ModelState.AddModelError("", "您输入的帐号或密码错误");
                return View();
            }

        }
        //显示用户管理
        public ActionResult Users(int p = 1, string user = null)
        {
            if (user != null)
            {
                var data1= db.Members.Where(x => x.Email.Contains(user) || x.Name.Contains(user) || x.Nickname.Contains(user)).ToList();
                // = db.Members.Where(x => x.Email == user || x.Name == user || x.Nickname == user).ToList();
                var pagedData1 = data1.ToPagedList(pageNumber: p, pageSize: 10);
                return View(pagedData1);
            }
            var data = db.Members.ToList();
            var pagedData = data.ToPagedList(pageNumber: p, pageSize: 10);
            return View(pagedData);
        }

        
        //添加用户
        public ActionResult AdminUserCreate()
        {
            return View();
        }

       
        //用户通过验证
        [HttpPost]
        public ActionResult AdminUserAuthCode(int id)
        {
            Member member = db.Members.Where(p => p.Id == id).FirstOrDefault();
            if (member.AuthCode != null)
                member.AuthCode = null;
            db.SaveChanges();
            return Json(member, JsonRequestBehavior.AllowGet);
        }
        //跳转修改用户信息页面

        public ActionResult AdminUserEdit(int id)
        {
            var data = db.Members.Find(id);
            return View(data);
        }



        [HttpPost]
        public ActionResult AdminUserEdit([Bind(Exclude = "Password")] Member member, HttpPostedFileBase image = null)
        {
            Member mm = db.Members.Where(p => p.Id == member.Id).FirstOrDefault();
            if (image != null)
            {
                member.ImageMimeType = image.ContentType;
                member.ImageData = new byte[image.ContentLength];
                image.InputStream.Read(member.ImageData, 0, image.ContentLength);
                if (mm != null)
                {
                    mm.Name = member.Name;
                    mm.Nickname = member.Nickname;
                    mm.RegisterOn = member.RegisterOn;
                    mm.ImageData = member.ImageData;
                    mm.ImageMimeType = member.ImageMimeType;
                }
            }
            else
            {
                if (mm != null)
                {
                    mm.Name = member.Name;
                    mm.Nickname = member.Nickname;
                    mm.RegisterOn = member.RegisterOn;
                }
            }
            db.SaveChanges();
            //SendAuthCodeToMember(member);

            return RedirectToAction("Users", "Admin");
        }

        //删除用户
        [HttpPost]
        public ActionResult AdminUserDelete(int id)

        {
            var data = db.Members.Where(p => p.Id == id).FirstOrDefault();
            db.Members.Remove(data);
            db.SaveChanges();
            return new HttpStatusCodeResult(System.Net.HttpStatusCode.OK);
        }





        //显示商品管理
        public ActionResult Products(int p = 1, string product = null)
        {
            //搜索框
            if (product != null)
            {
                var data1=db.Products.Where(x => x.Name.Contains(product) || x.Made.Contains(product) || x.ProductCategory.Name.Contains(product)).ToList();
                var pagedData1 = data1.ToPagedList(pageNumber: p, pageSize: 10);
                ViewData["Productpp2"] = db.ProductCategories.ToList();
                return View(pagedData1);
            }
            var data = db.Products.ToList();
            var pagedData = data.ToPagedList(pageNumber: p, pageSize: 10);
            ViewData["Productpp2"] = db.ProductCategories.ToList();
            return View(pagedData);
        }
        //新建商品类别
        public ActionResult AdminProductPPCreate(ProductCategory productCategory, string PPCreate= null)
        {
            string ss = "";
            if(PPCreate != null)
            {
                var data = db.ProductCategories.Where(x => x.Name == PPCreate).FirstOrDefault();
                if(data == null)
                {
                    productCategory.Name = PPCreate;
                    db.ProductCategories.Add(productCategory);
                    db.SaveChanges();
                    ss = "Ok";
                }
            }
            
            return Json(ss , JsonRequestBehavior.AllowGet);
        }
       
        //商品类别修改
        [HttpPost]
        public ActionResult AdminProductPPEtdit(int id,string ppid = null)
        {
            string ok = "";
            var data2 = db.ProductCategories.Where(x => x.Name == ppid).FirstOrDefault();
            if(data2 == null)
            {
                var data = db.ProductCategories.Find(id);
                if (data.Name != ppid)
                {
                    data.Name = ppid;
                    db.SaveChanges();
                    ok = "ok";
                }
            }
            return Json(ok, JsonRequestBehavior.AllowGet);
        }
        //删除商品类别
        public ActionResult AdminProductPPDelete(int id)
        {
            string yy = "";
            var data = db.Products.Where(x => x.ProductCategory.Id == id).ToList();
            if(data == null)
            {
                var data2 = db.ProductCategories.Find(id);
                db.ProductCategories.Remove(data2);
                db.SaveChanges();
                yy = "ok";
            }
            return Json(yy, JsonRequestBehavior.AllowGet);
        }
        // 商品新建
        public ActionResult AdminProductCreate()
        {
            var data1 = db.ProductCategories.ToList();
            ViewData["Productpp"] = data1;
            return View();
        }
        [HttpPost]
        public ActionResult AdminProductCreate(string pp,DateTime bday,Product product, HttpPostedFileBase image = null)
        {
            var data = db.Products.Find(product.Id);
            ProductCategory Pc = db.ProductCategories.Where(p => p.Name == pp).FirstOrDefault();
            if (Pc != null)
            {
                // 数据库中已经存在该分类  直接把这个数据 传给 该商品就行
                product.ProductCategory = Pc;
            }
            if (image != null)
            {
                product.ImageMimeType = image.ContentType;
                product.ImageData = new byte[image.ContentLength];
                image.InputStream.Read(product.ImageData, 0, image.ContentLength);
            }
            //这里设计 img 可以为空
            if (data == null)
            {
                product.PublishOn = bday;

                data = product;
            }
            db.Products.Add(data);
            db.SaveChanges();
            //SendAuthCodeToMember(member);
            return RedirectToAction("Products", "Admin");

        }

        //删除商品
        [HttpPost]
        public ActionResult AdminProduvtDelete(int id)
        {
            var data = db.Products.Where(p => p.Id == id).FirstOrDefault();
            db.Products.Remove(data);
            db.SaveChanges();
            return new HttpStatusCodeResult(System.Net.HttpStatusCode.OK);
        }
        //修改商品

        public ActionResult AdminProductEdit(int id)
        {
            if (id != 0)
            {
                
                ViewData["Productpp"] = db.ProductCategories.ToList();
                var data = db.Products.Find(id);
                return View(data);
            }
            else
            {
                return View();
            }

        }

        [HttpPost]
        public ActionResult AdminProductEdit(string pp, Product product, HttpPostedFileBase image = null)
        {
            Product pp1 = db.Products.Where(P => P.Id == product.Id).FirstOrDefault();
            ProductCategory Pc = db.ProductCategories.Where(p => p.Name == pp).FirstOrDefault();
            if (Pc != null)
            {
                // 数据库中已经存在该分类  直接把这个数据 传给 该商品就行
                product.ProductCategory = Pc;
            }
            if (image != null)
            {
                product.ImageMimeType = image.ContentType;
                product.ImageData = new byte[image.ContentLength];
                image.InputStream.Read(product.ImageData, 0, image.ContentLength);
                pp1.ImageMimeType = product.ImageMimeType;
                pp1.ImageData = product.ImageData;
            }
            if (pp1 != null)
            {
                pp1.Name = product.Name;
                pp1.Made = product.Made;
                pp1.Price = product.Price;
                pp1.PublishOn = product.PublishOn;
                pp1.ProductCategory = product.ProductCategory;
                pp1.Amount = product.Amount;
                pp1.Description = product.Description;
            }
            db.SaveChanges();
            //SendAuthCodeToMember(member);

            return RedirectToAction("Products", "Admin");

        }
    }
}