using YLCShopping.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace YLCShopping.Controllers
{
    [Authorize] // 必须登入会员才能使用订单结帐功能
    public class OrderController : BaseController
    {

        // 显示完成订单的表单页面
        public ActionResult Complete(int id = 0)
        {
            int ID = (int)Session["memberid"];
            var data = db.MemberAddresses.Where(x => x.Member.Id == ID).ToList();
            ViewData["Alladdress"] = data;
            if (id != 0)
            {
                var data2 = db.MemberAddresses.Find(id);
                return View(data2);
            }
            return View();
        }

        /// <summary>
        /// 将订单资料与购物车资料写入资料库
        /// </summary>
        /// <param name="form"></param>
        /// <param name="memo"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Complete(MemberAddress form,string memo =null)
        {
            //查找是否有该用户  用户是否登录
            var member = db.Members.Where(p => p.Email == User.Identity.Name).FirstOrDefault();
            if (member == null) return RedirectToAction("Index", "Home");
            //通过用户id查找用户购物车内商品
            int id = (int)Session["memberid"];
            var c = db.Carts.Where(p => p.Member.Id == id).ToList();
            if (c == null) return RedirectToAction("Index", "Cart");

            //
            MemberAddress address = new MemberAddress();

            //根据用户查询是否存在输入地址
            var data = db.MemberAddresses.Where(x => x.Member.Id == id).ToList();
            if(data.Count == 0)
            {
                //用户没有加入地址
                
                address = form;
                address.Member = member;
                db.MemberAddresses.Add(address);
            }
            else
            {
                //用户加入了地址
                foreach(var item in data)
                {
                    if (item.ContactName == form.ContactName && item.ContactAddress == form.ContactAddress && item.ContactPhoneNo == form.ContactPhoneNo)
                    {
                       
                        address = item;
                        

                    }
                    else
                    {
                        address = form;
                        address.Member = member;
                        db.MemberAddresses.Add(address);
                    }
                }
            }
            // 将订单资料与购物车资料写入资料库
            OrderHeader oh = new OrderHeader()
            {
                MemberAddress = address,
                BuyOn = DateTime.Now,
                Memo = memo,
                OrderDetailItems = new List<OrderDetail>()
            };

            //用于计算总价格
            decimal total_price = 0;
            //遍历用户购物车
            foreach (var item in c)
            {
                //查找购物车内是否有商品
                var product = db.Products.Find(item.Product.Id);
                if (product == null) return RedirectToAction("Index", "Cart");

                total_price += item.Product.Price * item.Amount;
                oh.OrderDetailItems.Add(
                    new OrderDetail()
                    {
                        Product = product,
                        Price = product.Price * item.Amount,
                        Amount = item.Amount
                    }
                    );
            }

            oh.TotalPrice = total_price;
            // 订单完成后必须清空现有购物车资料

            foreach (Cart cc in c)
            {
                db.Carts.Remove(cc);
            }
            
            db.Orders.Add(oh);
            db.SaveChanges();
            // 订单完成后回到网站首页
            return RedirectToAction("Index", "Home");
        }
    }
}