using YLCShopping.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;

namespace YLCShopping.Controllers
{
    public class HomeController : BaseController
    {
        // GET: Home
        // 首页
        public ActionResult Index(string search = null, int p = 1)
        {
            var data = db.Products.Where(x => x.Amount > 0).ToList();
            int id = (int)Session["memberid"];
            var member = db.Members.Where(x => x.Id==id).FirstOrDefault();
            ViewData["User"] = member.Id;
            //  var data = db.Products.ToList();
            //只显示商品数量大于零部分商品
            if (search == null)
            {
                
                return View(data);
            }
            else
            {
                //var data = db.Products.Where(x => x.Amount > 0 && x.Name == seach).ToList();
                var data1 = data.Where(x => x.Name.Contains(search)|| x.Made.Contains(search)).ToList();
                var pagedData = data1.ToPagedList(pageNumber: p, pageSize: 10);
                return View(pagedData);
            }
        }
        // 分类商品列表
        public ActionResult ProductList(int id = 0, string search = null, int p = 1)
        {
            var data1 = db.ProductCategories.ToList();
            ViewData["toedit"] = data1;
            if (id != 0)
            {
                if (search != null)
                {
                    var productCategory1 = db.ProductCategories.Find(id);

                    if (productCategory1 != null)
                    {
                        var data = productCategory1.Products.Where(x => x.Amount > 0).ToList();
                        var data11 = data.Where(x => x.Name.Contains(search) || x.Made.Contains(search)).ToList();
                        var pagedData11 = data11.ToPagedList(pageNumber: p, pageSize: 15);

                        return View(pagedData11);
                    }
                    else
                    {
                        return HttpNotFound();
                    }
                }
                else
                {
                    var productCategory = db.ProductCategories.Find(id);

                    if (productCategory != null)
                    {
                        var data = productCategory.Products.Where(x => x.Amount > 0).ToList();

                        var pagedData = data.ToPagedList(pageNumber: p, pageSize: 15);

                        return View(pagedData);
                    }
                    else
                    {
                        return HttpNotFound();
                    }
                }
            }
            else
            {
                if (search != null)
                {
                    var data22 = db.Products.Where(x => x.Amount > 0).ToList();
                    var data222 = data22.Where(x => x.Name.Contains(search) || x.Made.Contains(search)).ToList();
                    var pagedData22 = data222.ToPagedList(pageNumber: p, pageSize: 15);
                    return View(pagedData22);
                }
                else
                {
                    var data2 = db.Products.Where(x => x.Amount > 0).ToList();
                    var pagedData2 = data2.ToPagedList(pageNumber: p, pageSize: 15);
                    return View(pagedData2);
                }
            }
        }
        // 商品明细
        public ActionResult ProductDetail(int id)
        {
            var data = db.Products.Find(id);
            if (data == null)
            {
                return View();
            }
            return View(data);

        }

        //商品图片
        public FileContentResult GetImage(int Id)
        {
            Product prod = db.Products
                .FirstOrDefault(p => p.Id == Id);
            if (prod != null)
            {
                return File(prod.ImageData, prod.ImageMimeType);
            }
            else
            {
                return null;
            }
        }
    }
}