/******************************************************************
* Copyright（c）SyearsSregisteredorganization$ All Rights Reserved.
* CLR版本：Sclrversions
* 机器名称：$machinenames
* 公司名称：$registeredorganizations
* 命名空间：Srootnamespaces
* 文件名：Ssafeitemnames
* 版本号：V1.0.0.0
* 唯一标识：$guid10s
* 当前的用户域：Suserdomains
* 创建人：Susernames
* 电子邮箱：zouqiongjunekjy.com
* 创建时间：Stimes
* 描述：
* ======================================================================
* 修改标记
* 修改时间：Stimes
 * 修改人：$usernames
  * 版本号：V1.0.0.0
* 描述：
* **********************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Linq;
using System.Web;

namespace YLCShopping.Models
{
    [DisplayName("商品资讯")]
    [DisplayColumn("Name")]
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [DisplayName("商品类别")]
        [Required]
        public virtual ProductCategory ProductCategory { get; set; }

        [DisplayName("商品名称")]
        [Required(ErrorMessage = "请输入商品名称")]
        [MaxLength(60, ErrorMessage = "商品名称不可超过60个字")]
        public string Name { get; set; }
        // 商品图片
        [DisplayName("商品图片")]
        public byte[] ImageData { get; set; }
        [DisplayName("商品图片")]
        public string ImageMimeType { get; set; }

        //库存数量
        [Required(ErrorMessage = "请输入商品库存数量")]
        [Display(Name = "库存数量")]
        public int Amount { get; set; }


        [DisplayName("商品简介")]
        [Required(ErrorMessage = "请输入商品简介")]
        [MaxLength(250, ErrorMessage = "商品简介请勿输入超过250个字")]
        public string Description { get; set; }

        [DisplayName("制造商")]
        [Required(ErrorMessage = "请选择制造商")]
        public string Made { get; set; }

        [DisplayName("商品售价")]
        [Required(ErrorMessage = "请输入商品售价")]
        [Range(1, 100000, ErrorMessage = "商品售价必须介于 1 ~ 10,0000 之间")]
        public decimal Price { get; set; }

        [DisplayName("上架时间")]
        [Description("如果不设定上架时间，代表此商品永不上架")]
        public DateTime? PublishOn { get; set; }
    }
}