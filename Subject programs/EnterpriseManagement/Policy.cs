using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EnterpriseManagement
{
    //工资计算政策
    class Policy
    {
        double attendwagefactor;//上班因子
        double askforleavewagefactor;//请假因子
        double absentwagefactor;//缺席因子
        double overtimewagefactor;//加班因子
        public Policy()
        {
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="initAttend"></param>
        /// <param name="initAFL"></param>
        /// <param name="initAbsent"></param>
        /// <param name="initOvertime"></param>
        public Policy(double initAttend,double initAFL,double initAbsent,double initOvertime)
        {
            AttendWageFactor = initAttend;
            AFLWageFactor = initAFL;
            AbsentWageFactor = initAbsent;
            OvertimeWageFactor = initOvertime;
        }
        public void PolicyInitialize(double attendwagefactor, double askforleavewagefactor, double absentwagefactor, double overtimewagefactor)
        {
            this.attendwagefactor=attendwagefactor;
            this.askforleavewagefactor=askforleavewagefactor;
            this.absentwagefactor=absentwagefactor;
            this.overtimewagefactor = overtimewagefactor;
        }
        /// <summary>
        /// 上班因子
        /// </summary>
        public double AttendWageFactor
        {
            get
            {
                return attendwagefactor;
            }

            set
            {
                attendwagefactor = value;
            }
        }

        /// <summary>
        /// 请假因子
        /// </summary>
        public double AFLWageFactor
        {
            get
            {
                return askforleavewagefactor;
            }

            set
            {
                askforleavewagefactor = value;
            }
        }

        /// <summary>
        /// 缺席因子
        /// </summary>
        public double AbsentWageFactor
        {
            get
            {
                return absentwagefactor;
            }

            set
            {
                absentwagefactor = value;
            }
        }

        /// <summary>
        /// 加班工资计算因子
        /// </summary>
        public double OvertimeWageFactor
        {
            get
            {
                return overtimewagefactor;
            }

            set
            {
                overtimewagefactor = value;
            }
        }

        /// <summary>
        /// 计算考勤工资
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public double CalculateAttendanceWage(Check c)
        {
            return c.GetAFL() * AFLWageFactor + c.GetAttend() * AttendWageFactor + c.GetAbsent() * AbsentWageFactor + c.GetOvertime() * OvertimeWageFactor;
        }
    }
}
