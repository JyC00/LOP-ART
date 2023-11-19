using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EnterpriseManagement
{
    //合同
    class Pact
    {
        int year;//年份
        double basicwage;//基本工资
        string type;//合同类型
        public Pact()
        {
        }
        public void PactInitialize(int year, double basicwage, string type)
        {
            this.year=year;
            this.basicwage=basicwage;
            this.type = type;
        }
        /// <summary>
        /// 获得合同信息
        /// </summary>
        /// <returns></returns>
        public string GetPact()
        {
            string result;
            result = type + "，合同长度：" + year + "年，基本工资" + basicwage + "元。";
            return result;
        }

        /// <summary>
        /// 获取合同类型
        /// </summary>
        /// <returns></returns>
        public string GetPactType()
        {
            return type;
        }

        /// <summary>
        /// 获得合同年限
        /// </summary>
        /// <returns></returns>
        public int GetPactYear()
        {
            return year;
        }

        /// <summary>
        /// 获得合同上写明的基本工资
        /// </summary>
        /// <returns></returns>
        public double GetPactBasicWage()
        {
            return basicwage;
        }

        /// <summary>
        /// 延长合同年限
        /// </summary>
        /// <param name="l"></param>
        public void LengthenPactYear(int l)
        {
            this.year += l;
        }

        /// <summary>
        /// 改变部门基本工资
        /// </summary>
        /// <param name="newWage"></param>
        public void ModifyPactBasicWage(double newWage)
        {
            this.basicwage = newWage;
        }

        /// <summary>
        /// 改变合同类型
        /// </summary>
        /// <param name="newType"></param>
        public void ChangePactType(string newType)
        {
            this.type = newType;
        }
    }
}
