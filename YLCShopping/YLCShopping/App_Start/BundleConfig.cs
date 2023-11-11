﻿using System.Web;
using System.Web.Optimization;

namespace YLCShopping
{
    public class BundleConfig
    {
        // 有关捆绑的详细信息，请访问 https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // 使用要用于开发和学习的 Modernizr 的开发版本。然后，当你做好
            // 生产准备就绪，请使用 https://modernizr.com 上的生成工具仅选择所需的测试。
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js", "~/Scripts/bootstrap.bundle.js", "~/Scripts/jquery.js",
                      "~/Scripts/bootstrap.bundle.min.js", "~/Scripts/bootstrap.min.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css", "~/Content/bootstrap.min.css",
                      "~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Content/YLC_CSS_JS/css").Include(
                      "~/Content/YLC_CSS_JS/YLC-CSS.css", "~/Content/YLC_CSS_JS/main_css.css"));

            bundles.Add(new StyleBundle("~/Content/YLC_CSS_JS/js").Include(
                      "~/Content/YLC_CSS_JS/YLC-JS.js"));

        }
    }
}
