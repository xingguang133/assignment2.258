using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YLCShopping.Models;

namespace YLCShopping.Controllers
{
    public class BaseController : Controller
    {
        protected YLCShoppingContext db = new YLCShoppingContext();
        //protected override void OnActionExecuting(ActionExecutingContext filterContext)
        //{
        //    //如果没有设置Session值那么就跳转到登入页面
        //    //实现登录检查
        //    if (Session["memberid"] == null || Session["name"] == null)
        //    {
        //        //RedirectResult tourl = new RedirectResult("/Login/UserLogin"); 
        //        //filterContext.Result = tourl;
        //        ContentResult Cr = new ContentResult
        //        {
        //            Content = string.Format("<script languge='javascript'>alert('用户信息已过期，请重新登录');window.parent.location.href='/Member/Logout';</script>")
        //        };
        //        filterContext.Result = Cr;
        //    }
        //}
    }
}