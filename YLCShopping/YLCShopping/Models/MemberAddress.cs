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
    public class MemberAddress
    {

        [Key]
        public int Id { get; set; }

        [DisplayName("订购会员")]
        [Required]
        public virtual Member Member { get; set; }

        [DisplayName("收件人姓名")]
        [Required(ErrorMessage = "请输入收件人姓名")]
        [MaxLength(40, ErrorMessage = "收件人姓名长度不可超过 40 个字元")]
        [Description("订购的会员不一定就是收到商品的人")]
        public string ContactName { get; set; }

        [DisplayName("联络电话")]
        [Required(ErrorMessage = "请输入您的联络电话，例如: +886-2-23222480#6342")]
        [MaxLength(25, ErrorMessage = "电话号码长度不可超过 25 个字元")]
        [DataType(DataType.PhoneNumber)]
        public string ContactPhoneNo { get; set; }

        [DisplayName("递送地址")]
        [Required(ErrorMessage = "请输入商品递送地址")]
        public string ContactAddress { get; set; }

    }
}