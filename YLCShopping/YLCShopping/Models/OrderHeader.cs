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
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace YLCShopping.Models
{
    [DisplayName("订单主档")]
    [DisplayColumn("DisplayName")]
    public class OrderHeader
    {
        [Key]
        public int Id { get; set; }

        [DisplayName("订购地址")]
        [Required]
        public virtual MemberAddress MemberAddress { get; set; }

        [DisplayName("订单金额")]
        [Required]
        [DataType(DataType.Currency)]
        [Description("由于订单金额可能会受商品递送方式或优惠折扣等方式异动价格，因此必须保留购买当下算出来的订单金额")]
        public decimal TotalPrice { get; set; }

        [DisplayName("订单备注")]
        [DataType(DataType.MultilineText)]
        public string Memo { get; set; }

        [DisplayName("订购时间")]
        public DateTime BuyOn { get; set; }

        public virtual ICollection<OrderDetail> OrderDetailItems { get; set; }
    }
}