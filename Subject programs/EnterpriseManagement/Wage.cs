using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EnterpriseManagement
{
    //工资
    class Wage
    {
        double basicwage;//基本工资
        double extrawage;//额外工资

        public void WageInitialize(double basicwage, double extrawage)
        {
            this.basicwage=basicwage;
            this.extrawage = extrawage;
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        public Wage()
        {
            basicwage = 0;
            extrawage = 0;
        }

        /// <summary>
        /// 增加基本工资
        /// </summary>
        /// <param name="p"></param>
        public void AddBasicWage(Pact p)
        {
            basicwage += p.GetPactBasicWage();
        }

        /// <summary>
        /// 增加额外工资
        /// </summary>
        /// <param name="extra"></param>
        public void AddExtraWage(double extra)
        {
            basicwage += extra;
        }

        /// <summary>
        /// 计算工资
        /// </summary>
        /// <param name="c"></param>
        /// <param name="p"></param>
        public void CalAttendanceWage(Check c, Policy p)
        {
            extrawage += p.CalculateAttendanceWage(c);
        }

        /// <summary>
        /// 获得工资
        /// </summary>
        /// <returns></returns>
        public double GetWage()
        {
            return basicwage + extrawage;
        }

        /// <summary>
        /// 获得基本工资
        /// </summary>
        /// <returns></returns>
        public double GetBasicWage()
        {
            return basicwage;
        }

        /// <summary>
        /// 获得额外工资
        /// </summary>
        /// <returns></returns>
        public double GetExtraWage()
        {
            return extrawage;
        }
    }
}
