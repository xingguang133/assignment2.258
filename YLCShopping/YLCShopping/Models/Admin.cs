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
    public class Admin
    {

        [Key]
        public int Id { get; set; }
        [DisplayName("管理员账号")]
        [Required]
        public string AdminName { get; set; }

        [DisplayName("密码")]
        [Required]
        public string Pwd { get; set; }

        [DisplayName("姓名")]
        [Required]
        public string ChinaName { get; set; }
    }
}