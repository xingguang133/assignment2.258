using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YLCShopping.Models;

namespace YLCShopping.Controllers
{
    public class CartController : BaseController
    {
        // GET: Cart
        // 加入产品项目到购物车，如果没有传入 Amount 参数则预设购买数量为 1
        // 因为知道要透过 AJAX 呼叫这个 Action，所以可以先标示 [HttpPost] 属性
        [HttpPost]
        public ActionResult AddToCart(int ProductId, int Amount = 1)
        {
            var product = db.Products.Find(ProductId);
            if (product == null)
                return HttpNotFound();
            int id = (int)Session["memberid"];
            var member = db.Members.Find(id);
            var cc = db.Carts.Where(r => r.Member.Id == id && r.Product.Id == ProductId).FirstOrDefault();
            if (cc != null)
            {
                cc.Product = product;
                cc.Member = member;
                cc.Amount += 1;
                product.Amount -= 1;
                db.SaveChanges();
            }
            else
            {
                //使用构造器
                Cart cart = new Cart()
                {
                    Product = product,
                    Member = member,
                    Amount = Amount
                };
                product.Amount -= 1;
                db.Carts.Add(cart);
                db.SaveChanges();
            }
            return new HttpStatusCodeResult(System.Net.HttpStatusCode.Created);
        }

        // 显示目前的购物车项目
        public ActionResult Index()
        {
            int id = (int)Session["memberid"];
           
                var data = db.Carts.Where(x => x.Member.Id == id).ToList();
                return View(data);

        }

        // 移除购物车项目
        [HttpPost] // 因为知道要透过 AJAX 呼叫这个 Action，所以可以先标示 [HttpPost] 属性
        public ActionResult Remove(int ProductId)
        {
            var c = db.Carts.Where(p => p.Product.Id == ProductId).FirstOrDefault();
            if (c != null)
            {
                var a = db.Products.Where(x => x.Id == ProductId).FirstOrDefault();
                a.Amount += c.Amount;
                db.Carts.Remove(c);
                db.SaveChanges();
            }

            return new HttpStatusCodeResult(System.Net.HttpStatusCode.OK);
        }

        // 更新购物车中特定项目的购买数量
        // 因为知道要透过 AJAX 呼叫这个 Action，所以可以先标示 [HttpPost] 属性
        [HttpPost]
        public ActionResult UpdateAmount(List<Cart> Carts)
        {
            // int id = (int)Session["memberid"];
            // Cart cart = db.Carts.Where(x => x.Member.Id == id).FirstOrDefault();
            //int a= cart.Amount;
            if (Carts != null)
            {
                int a = 0;
                foreach (var item in Carts)
                {
                    var c = db.Carts.Where(p => p.Product.Id == item.Product.Id).FirstOrDefault();
                    if (c != null)
                    {
                        a = item.Amount - c.Amount;
                        c.Amount = item.Amount;
                    }
                    if (a > 0)
                    {
                        c.Product.Amount -= Math.Abs(a);
                    }
                    else
                    {
                        c.Product.Amount += Math.Abs(a);
                    }
                }

                db.SaveChanges();
            }

            return RedirectToAction("Index", "Cart");

        }
    }
}