using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EnterpriseManagement
{
    //经理类
    class Manager:Person
    {
        private Department dept;//部门
        private Wage wage = new Wage();//薪水
        private Pact pact = new Pact(); //合同
        private Check attendance = new Check();//考勤
        private string status;//工作状态
        private string ManagerID; //工作号
        public Manager()
        {
        }
        public void ManagerInitialize(string status, string ManagerID, string name, string sex, int age, double weight, double height,int managerBirth,string managerFolk,int managerJoinWorkTime,string managerMarriage,string managerMobileTel,string managerNativePlace,int managerPartyTime,string managerPolitics_Visage,string managerSpeciality,string managerTel,int managerWorkAge, Department dept, Wage wage, Pact pact, Check attendance)
        {
            this.status = status;
            this.ManagerID = ManagerID;
            this.name = name;
            this.sex = sex;
            this.age = age;
            this.weight = weight;
            this.height = height;
            this.managerBirth=managerBirth;
            this.managerFolk=managerFolk;
            this.managerJoinWorkTime=managerJoinWorkTime;
            this.managerMarriage=managerMarriage;
            this.managerMobileTel=managerMobileTel;
            this.managerNativePlace=managerNativePlace;
            this.managerPartyTime=managerPartyTime;
            this.managerPolitics_Visage=managerPolitics_Visage;
            this.managerSpeciality=managerSpeciality;
            this.managerTel=managerTel;
            this.managerWorkAge = managerWorkAge;
            this.dept=dept;
            this.wage=wage;
            this.pact=pact;
            this.attendance = attendance;
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name"></param>
        /// <param name="sex"></param>
        /// <param name="age"></param>
        public Manager(string name,string sex,int age,double weight,double height,string ManagerID)
            :base(name,sex,age,weight,height)
        {
            this.ManagerID = ManagerID;
        }

        /// <summary>
        /// 任命
        /// </summary>
        /// <param name="newDept"></param>
        public void Appoint(Department newDept)
        {
            this.dept = newDept;
        }

        /// <summary>
        /// 设置合同
        /// </summary>
        /// <param name="newP"></param>
        public void setPact(Pact newP)
        {
            this.pact = newP;
        }
        /// <summary>
        /// 延长合同
        /// </summary>
        /// <param name="year"></param>
        public void LengthenPact(int year)
        {
            this.pact.LengthenPactYear(year);
        }

        /// <summary>
        /// 更改基本工资
        /// </summary>
        /// <param name="wage"></param>
        public void ModifyBasicWage(double wage)
        {
            this.pact.ModifyPactBasicWage(wage);
        }

        /// <summary>
        /// 改变部门
        /// </summary>
        /// <param name="newp"></param>
        public void ChangePact(Pact newp)
        {
            this.pact = newp;
        }

        /// <summary>
        /// 计算工资
        /// </summary>
        /// <param name="month"></param>
        public void CalculateWage(int month)
        {
            for (int i = 0; i < month; i++)
            {
                this.wage.AddBasicWage(this.pact);
            }
            wage.CalAttendanceWage(this.attendance, this.dept.ManagerPolicy);
            this.attendance = new Check();
        }

        /// <summary>
        /// 发工资
        /// </summary>
        /// <returns></returns>
        public double AccountWage()
        {
            double result =this.wage.GetWage();
            this.wage = new Wage();
            return result;
        }

        /// <summary>
        /// 设置工作状态
        /// </summary>
        /// <param name="newStatus"></param>
        public void setStatus(string newStatus)
        {
            this.status = newStatus;
        }

        /// <summary>
        /// 考勤
        /// </summary>
        public void Sign()
        {
            if (this.attendance == null)
            {
                this.attendance = new Check();
            }

            if (this.status == "AskForLeave")
            {
                this.attendance.CheckAFL();
                return;
            }

            if (this.status == "Attend")
            {
                this.attendance.CheckAttend();
                return;
            }

            if (this.status == "Absent")
            {
                this.attendance.CheckAbsent();
                return;
            }

            if (this.status == "Overtime")
            {
                this.attendance.CheckOvertime();
                return;
            }
        }

        public string managerMobileTel;
        /// <summary>
        /// 获得经理手机号码
        /// </summary>
        /// <returns></returns>
        public string getManagerMobileTel
        {
            get
            {
                return managerMobileTel;
            }

            set
            {
                managerMobileTel = value;
            }
        }

        public string managerTel;
        /// <summary>
        /// 获得经理座机号码
        /// </summary>
        /// <returns></returns>
        public string getManagerTel
        {
            get
            {
                return managerTel;
            }

            set
            {
                managerTel = value;
            }
        }

        public string managerNativePlace;
        /// <summary>
        /// 获得经理出生地
        /// </summary>
        /// <returns></returns>
        public string getManagerNativePlace()
        {
                return managerNativePlace; 
        }

        public string managerSpeciality;
        /// <summary>
        /// 获得经理特长
        /// </summary>
        /// <returns></returns>
        public string getManagerSpeciality
        {
            get
            {
                return managerSpeciality;
            }

            set
            {
                managerSpeciality = value;
            }
        }

        public int managerWorkAge;
        /// <summary>
        /// 获得经理工龄
        /// </summary>
        /// <returns></returns>
        public int getManagerWorkAge
        {
            get
            {
                return managerWorkAge;
            }

            set
            {
                managerWorkAge = value;
            }
        }

        public string managerFolk;
        /// <summary>
        /// 获得经理所属民族
        /// </summary>
        /// <returns></returns>
        public string getManagerFolk()
        {
            return managerFolk;
         }

  
        public string managerMarriage;
        /// <summary>
        /// 获得经理婚姻情况
        /// </summary>
        /// <returns></returns>
        public string getManagerMarriage
        {
            get
            {
                return managerMarriage;
            }

            set
            {
                managerMarriage = value;
            }
        }

        public string managerPolitics_Visage;
        /// <summary>
        /// 获得经理政治面貌
        /// </summary>
        /// <returns></returns>
        public string getManagerPoliticsVisage
        {
            get
            {
                return managerPolitics_Visage;
            }

            set
            {
                managerPolitics_Visage = value;
            }
        }

        public int managerBirth;
        /// <summary>
        /// 获得经理生日
        /// </summary>
        /// <returns></returns>
        public int getManagerBirth()
        {
                return managerBirth;
        }

        public int managerPartyTime;
        /// <summary>
        /// 获得经理入党时间
        /// </summary>
        /// <returns></returns>
        public int getManagerPartyTime()
        {
                return managerPartyTime;

        }

        public int managerJoinWorkTime;
        /// <summary>
        /// 获得经理参加工作时间
        /// </summary>
        /// <returns></returns>
        public int getManagerJoinWorkTime
        {
            get
            {
                return managerJoinWorkTime;
            }

            set
            {
                managerJoinWorkTime = value;
            }
        }
    }
}
