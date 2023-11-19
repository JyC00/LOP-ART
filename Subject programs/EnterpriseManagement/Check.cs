using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EnterpriseManagement
{
    class Check//考勤类
    {
        int askforleave;//请假次数
        int attend;//上班次数
        int absent;//缺席次数
        int overtime;//加班次数

        /// <summary>
        /// 构造函数
        /// </summary>
        public Check()
        {
            askforleave = 0;
            attend = 0;
            absent = 0;
            overtime = 0;
        }

        public void CheckInitialize(int askforleave, int attend, int absent, int overtime)
        {
            this.askforleave=askforleave;
            this.attend=attend;
            this.absent=absent;
            this.overtime = overtime;
        }
        /// <summary>
        /// 修改请假次数
        /// </summary>
        /// <param name="newAFL"></param>
        public void ModifyAFL(int newAFL)
        {
            this.askforleave = newAFL;
        }

        /// <summary>
        /// 增加请假次数
        /// </summary>
        public void CheckAFL()
        {
            askforleave++;
        }

        /// <summary>
        /// 修改上班次数
        /// </summary>
        /// <param name="newAttend"></param>
        public void ModifyAttend(int newAttend)
        {
            this.attend = newAttend;
        }

        /// <summary>
        /// 增加上班次数
        /// </summary>
        public void CheckAttend()
        {
            attend++;
        }

        /// <summary>
        /// 修改缺席次数
        /// </summary>
        /// <param name="newAbsent"></param>
        public void ModifyAbsent(int newAbsent)
        {
            this.absent = newAbsent;
        }

        /// <summary>
        /// 增加缺席次数
        /// </summary>
        public void CheckAbsent()
        {
            absent++;
        }

        /// <summary>
        /// 修改加班次数
        /// </summary>
        /// <param name="newOvertime"></param>
        public void ModifyOvertime(int newOvertime)
        {
            this.overtime = newOvertime;
        }

        /// <summary>
        /// 增加加班次数
        /// </summary>
        public void CheckOvertime()
        {
            overtime++;
        }

        /// <summary>
        /// 获得考勤信息
        /// </summary>
        /// <returns></returns>
        public string GetCheck()
        {
            string result;
            int tmp = askforleave + attend + absent;
            result="应到："+tmp+"次，实到："+attend+"次，请假："+askforleave+"次，缺席"+absent+"次，加班"+overtime+"次。";
            return result;
        }

        /// <summary>
        /// 获得请假次数
        /// </summary>
        /// <returns></returns>
        public int GetAFL()
        {
            return askforleave;
        }

        /// <summary>
        /// 获得上班次数
        /// </summary>
        /// <returns></returns>
        public int GetAttend()
        {
            return attend;
        }

        /// <summary>
        /// 获得缺席次数
        /// </summary>
        /// <returns></returns>
        public int GetAbsent()
        {
            return absent;
        }

        /// <summary>
        /// 获得加班次数
        /// </summary>
        /// <returns></returns>
        public int GetOvertime()
        {
            return overtime;
        }
    }
}
