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
using System.Linq;
using System.Web;

namespace YLCShopping.Models
{
    [DisplayName("订单明细")]
    [DisplayColumn("Name")]
    public class OrderDetail
    {
        [Key]
        public int Id { get; set; }

        [DisplayName("订单主档")]
        [Required]
        public virtual OrderHeader OrderHeader { get; set; }

        [DisplayName("订购商品")]
        [Required]
        public virtual Product Product { get; set; }

        [DisplayName("商品售价")]
        [Required(ErrorMessage = "请输入商品售价")]
        [Range(1, 10000000, ErrorMessage = "商品售价必须介于 99 ~ 10,000 之间")]
        [Description("由于商品售价可能会经常异动，因此必须保留购买当下的商品售价")]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }
        
        [DisplayName("购买数量")]
        [Required]
        public int Amount { get; set; }
        [DisplayName("发货单号")]
        public string Shipmentnumber { get; set; }
    }
}