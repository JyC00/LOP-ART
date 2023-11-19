using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EnterpriseManagement
{
    //部门
    class Department
    {
        string deptname;//部门名称
        string deptaddr;//部门地址
        string deptphoneNo;//部门联系号码
        public Policy employeePolicy;//
        public Policy ManagerPolicy;//
        string deptManager;//
        public Department()
        {
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="initName"></param>
        /// <param name="initAddr"></param>
        /// <param name="initPhoneNo"></param>
        /// <param name="initManager"></param>
        public Department(string initName, string initAddr, string initPhoneNo, string initManager)
        {
            deptname = initName;
            deptaddr = initAddr;
            deptphoneNo = initPhoneNo;
            deptManager = initManager;
        }
        public void DepartmentInitialize(string deptname, string deptaddr, string deptphoneNo, string deptManager, Policy employeePolicy, Policy ManagerPolicy)
        {
            this.deptname=deptname;
            this.deptaddr=deptaddr;
            this.deptphoneNo=deptphoneNo;
            this.deptManager=deptManager;
            this.employeePolicy=employeePolicy;
            this.ManagerPolicy = ManagerPolicy;
        }
        /// <summary>
        /// 获得部门名
        /// </summary>
        /// <returns></returns>
        public string GetDepartmentName()
        {
            return deptname;
        }

        /// <summary>
        /// 获得部门地址
        /// </summary>
        /// <returns></returns>
        public string GetDepartmentAddress()
        {
            return deptaddr;
        }

        /// <summary>
        /// 获得部门联系号码
        /// </summary>
        /// <returns></returns>
        public string GetDepartmentPhoneNumber()
        {
            return deptphoneNo;
        }

        /// <summary>
        /// 获得部门经理
        /// </summary>
        /// <returns></returns>
        public string GetDepartmentManager()
        {
            return deptManager;
        }

        /// <summary>
        /// 更换部门经理
        /// </summary>
        /// <param name="m"></param>
        public void ChangeDepartmentManager(string m)
        {
            this.deptManager = m;
        }

        /// <summary>
        /// 更换部门地址
        /// </summary>
        /// <param name="newAddr"></param>
        public void ChangeDepartmentAddress(string newAddr)
        {
            this.deptaddr = newAddr;
        }

        /// <summary>
        /// 更换部门联系号码
        /// </summary>
        /// <param name="newPhoneNo"></param>
        public void ChangeDepartmentPhoneNumber(string newPhoneNo)
        {
            this.deptphoneNo = newPhoneNo;
        }

        /// <summary>
        /// 显示部门信息
        /// </summary>
        /// <returns></returns>
        public string DepartmentDetail()
        {
            string result;
            result=deptname+"，部门地址："+deptaddr+"，联系方式："+deptphoneNo+"，部门经理："+deptManager+"。";
            return result;
        }
    }
}
