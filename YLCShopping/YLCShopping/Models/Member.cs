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
using System.Web.Mvc;

namespace YLCShopping.Models
{
    [DisplayName("会员资料")]
    [DisplayColumn("Name")]
    public class Member
    {
        [Key]
        public int Id { get; set; }

        [DisplayName("帐号")]
        [Required(ErrorMessage = "请输入 Email 地址")]
        [Description("我们直接以 Email 当成会员的登入帐号")]
        [MaxLength(250, ErrorMessage = "Email地址长度无法超过250个字元")]
        [DataType(DataType.EmailAddress)]
        [Remote("CheckDup", "Member", HttpMethod = "POST", ErrorMessage = "您输入的 Email 已经有人注册过了!")]
        public string Email { get; set; }

        [DisplayName("密码")]
        [Required(ErrorMessage = "请输入密码")]
        [MaxLength(40, ErrorMessage = "请输入密码")]
        [Description("密码将以SHA1进行杂凑运算，透过SHA1杂凑运算后的结果转为HEX表示法的字串长度皆为40个字元")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DisplayName("姓名")]
        [Required(ErrorMessage = "请输入中文姓名")]
        [MaxLength(5, ErrorMessage = "中文姓名不可超过5个字")]
        [Description("暂不考虑有外国人用英文注册会员的情况")]
        public string Name { get; set; }

        [DisplayName("用户名")]
        [Required(ErrorMessage = "请输入用户名")]
        [MaxLength(10, ErrorMessage = "用户名请勿输入超过10个字")]
        public string Nickname { get; set; }

        [DisplayName("会员注册时间")]
        public DateTime RegisterOn { get; set; }
        // 用户头像
        [DisplayName("商品图片")]
        public byte[] ImageData { get; set; }
        [DisplayName("商品图片")]
        public string ImageMimeType { get; set; }
        [DisplayName("会员启用认证码")]
        [MaxLength(36)]
        [Description("当 AuthCode 等于 null 则代表此会员已经通过Email有效性验证")]
        public string AuthCode { get; set; }
        [DisplayName("用户地址")]

        public virtual ICollection<MemberAddress> MemberAddresses { get; set; }
    }
}