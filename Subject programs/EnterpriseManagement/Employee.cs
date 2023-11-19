using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EnterpriseManagement
{
    //员工类
    class Employee:Person
    {
        private string EmployeeID; //员工号
        private string address; //住址
        private int EmployeeIDnum;//身份证号码
        private Department dept;//员工部门
        private Wage wage = new Wage();//员工工资
        private Pact pact = new Pact();//员工合同
        private Check attendance = new Check();//员工考勤
        public void EmployeeInitialize(string EmployeeID, string address, int EmployeeIDnum, string name, string sex, int age, double weight, double height, int bookInTime, int employeeBirth, string employeeCETLevel,string employeeCollege ,string employeeDutyType ,string employeeFolk ,int employeeJoinWorkTime ,string employeeMarriage ,string employeeMobileTel ,string employeeNativePlace ,int employeePartyTime ,string employeePolitics_Visage ,string employeeResume ,string employeeSpeciality ,string employeeTel ,int employeeUnitWorkAge ,int employeeWorkAge ,string employeeWorkType, Department dept, Wage wage, Pact pact, Check attendance)
        {
            this.EmployeeID=EmployeeID;
            this.address=address;
            this.EmployeeIDnum=EmployeeIDnum;
            this.name = name;
            this.sex = sex;
            this.age = age;
            this.weight = weight;
            this.height = height;
            this.bookInTime=bookInTime;
            this.employeeBirth=employeeBirth;
            this.employeeCETLevel=employeeCETLevel;
            this.employeeCollege=employeeCollege;
            this.employeeDutyType=employeeDutyType;
            this.employeeFolk=employeeFolk;
            this.employeeJoinWorkTime=employeeJoinWorkTime;
            this.employeeMarriage=employeeMarriage;
            this.employeeMobileTel=employeeMobileTel;
            this.employeeNativePlace=employeeNativePlace;
            this.employeePartyTime=employeePartyTime;
            this.employeePolitics_Visage=employeePolitics_Visage;
            this.employeeResume=employeeResume;
            this.employeeSpeciality=employeeSpeciality;
            this.employeeTel=employeeTel;
            this.employeeUnitWorkAge=employeeUnitWorkAge;
            this.employeeWorkAge=employeeWorkAge;
            this.employeeWorkType = employeeWorkType;
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
        public Employee(string name,string sex,int age,double weight,double height,string EmployeeID,string address,int EmployeeIDnum)
            :base(name,sex,age,weight,height)
        {
            this.EmployeeID = EmployeeID;
            this.address = address;
            this.EmployeeIDnum = EmployeeIDnum;
           
        }
        public Employee()
        {
        }
        /// <summary>
        /// 获取员工号
        /// </summary>
        /// <returns></returns>
        public string getEmployeeID()
        {
             return EmployeeID; 
          
        }

        /// <summary>
        /// 获取员工地址
        /// </summary>
        /// <returns></returns>
        public string getAddress()
        {
            return address;
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
            double result = this.wage.GetWage();
            this.wage = new Wage();
            return result;
        }

        public string employeeCollege;
        /// <summary>
        /// 获取员工毕业的大学
        /// </summary>
        /// <returns></returns>
        public string EmployeeCollege
        {
            get 
            {
                return employeeCollege;
            }
            set 
            { 
                employeeCollege = value; 
            }
        }

        /// <summary>
        /// 获取员工身份证号码
        /// </summary>
        /// <returns></returns>
        public int getEmployeeIDnum()
        {
            return EmployeeIDnum; 
            
        }


        public string employeeCETLevel;
        /// <summary>
        /// 获取员工英语等级水平
        /// </summary>
        /// <returns></returns>
        public string getEmployeeCETLevel
        {
            get 
            { 
                return employeeCETLevel; 
            }

            set 
            { 
                employeeCETLevel = value;
            }
        }
        

        public string employeeMobileTel;
        /// <summary>
        /// 获得员工手机号码
        /// </summary>
        /// <returns></returns>
        public string getEmployeeMobileTel
        {
            get 
            { 
                return employeeMobileTel;
            }

            set
            { 
                employeeMobileTel = value; 
            }
        }

       public string employeeTel;
        /// <summary>
        /// 获得员工座机号码
        /// </summary>
        /// <returns></returns>
        public string getEmployeeTel
        {
            get 
            { 
                return employeeTel;
            }

            set 
            { employeeTel = value;
            }
        }

        public string employeeNativePlace;
        /// <summary>
        /// 获得员工出生地
        /// </summary>
        /// <returns></returns>
        public string getEmployeeNativePlace
        {
            get
            { 
                return employeeNativePlace;
            }
            set 
            { 
                employeeNativePlace = value;
            }
        }

       
        public string employeeResume;
        /// <summary>
        /// 获得员工简历
        /// </summary>
        /// <returns></returns>
        public string getEmployeeResume
        {
            get 
            { return employeeResume;
            }

            set
            { 
                employeeResume = value;
            }
        }

        public string employeeSpeciality;
        /// <summary>
        /// 获得员工特长
        /// </summary>
        /// <returns></returns>
        public string getEmployeeSpeciality
        {
            get
            { 
                return employeeSpeciality;
            }

            set
            {
                employeeSpeciality = value;
            }
        }
        
        public int employeeUnitWorkAge;
        /// <summary>
        /// 获得员工开始参加工作年龄
        /// </summary>
        /// <returns></returns>
        public int getEmployeeUnitWorkAge
        {
            get 
            { 
                return employeeUnitWorkAge;
            }

            set 
            { 
                employeeUnitWorkAge = value; 
            }
        }

        public int employeeWorkAge;
        /// <summary>
        /// 获得员工工龄
        /// </summary>
        /// <returns></returns>
        public int getEmployeeWorkAge
        {
            get
            { 
                return employeeWorkAge;
            }

            set
            { 
                employeeWorkAge = value;
            }
        }
        
        public string employeeDutyType;
        /// <summary>
        /// 获得员工职务类型
        /// </summary>
        /// <returns></returns>
        public string getEmployeeDutyType
        {
            get 
            { 
                return employeeDutyType; 
            }

            set
            { 
                employeeDutyType = value;
            }
        }

       public string employeeFolk;
        /// <summary>
        /// 获得员工所属民族
        /// </summary>
        /// <returns></returns>
        public string getEmployeeFolk()
        {
                return employeeFolk;
        
        }

       public string employeeMarriage;
        /// <summary>
        /// 获得员工婚姻情况
        /// </summary>
        /// <returns></returns>
        public string EmployeeMarriage
        {
            get 
            { 
                return employeeMarriage;
            }

            set
            { 
                employeeMarriage = value;
            }
        }

        public string employeePolitics_Visage;
        /// <summary>
        /// 获得员工政治面貌
        /// </summary>
        /// <returns></returns>
        public string getEmployeePoliticsVisage
        {
            get 
            { 
                return employeePolitics_Visage;
            }

            set 
            { 
                employeePolitics_Visage = value;
            }
        }

    
       public string employeeWorkType;
        /// <summary>
        /// 获得员工工作类型
        /// </summary>
        /// <returns></returns>
        public string getEmployeeWorkType
        {
            get 
            { 
                return employeeWorkType; 
            }

            set
            { 
                employeeWorkType = value;
            }
        }

       public int bookInTime;
        /// <summary>
        /// 获得员工入职时间
        /// </summary>
        /// <returns></returns>
        public int getBookInTime
        {
            get
            { 
                return bookInTime;
            }

            set 
            { 
                bookInTime = value;
            }
        }

       public int employeeBirth;
        /// <summary>
        /// 获得员工生日
        /// </summary>
        /// <returns></returns>
        public int getEmployeeBirth
        {
            get 
            { 
                return employeeBirth;
            }

            set
            { 
                employeeBirth = value;
            }
        }

       public int employeePartyTime;
        /// <summary>
        /// 获得员工入党时间
        /// </summary>
        /// <returns></returns>
        public int getEmployeePartyTime
        {
            get
            { 
                return employeePartyTime;
            }

            set 
            { 
                employeePartyTime = value;
            }
        }

        
       public int employeeJoinWorkTime;
        /// <summary>
        /// 获得员工参加工作时间
        /// </summary>
        /// <returns></returns>
        public int getEmployeeJoinWorkTime
        {
            get 
            { 
                return employeeJoinWorkTime; 
            }

            set 
            { 
                employeeJoinWorkTime = value;
            }
        }
    }
}
