using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EnterpriseManagement
{
    class Person
    {
        protected string name; //姓名
        protected string sex; //性别
        protected int age;    //年龄
        protected double weight;//体重
        protected double height;//身高
        public Person()
        {
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name"></param>
        /// <param name="sex"></param>
        /// <param name="age"></param>
        /// <param name="weight"></param>
        /// <param name="height"></param>
        public Person(string name, string sex, int age,double weight,double height)
        {
            this.name = name;
            this.sex = sex;
            this.age = age;
            this.weight = weight;
            this.height = height;
        }
        public void PersonInitialize(string name, string sex, int age, double weight, double height)
        {
            this.name = name;
            this.sex = sex;
            this.age = age;
            this.weight = weight;
            this.height = height;
        }
        /// <summary>
        /// 获取姓名
        /// </summary>
        /// <returns></returns>
        public string getName()
        {
            return name;
        }

        /// <summary>
        /// 获取性别
        /// </summary>
        /// <returns></returns>
        public string getSex()
        {
            return sex;
        }

        /// <summary>
        /// 获取年龄
        /// </summary>
        /// <returns></returns>
        public int getAge()
        {
            return age;
        }

        /// <summary>
        /// 获取身高
        /// </summary>
        /// <returns></returns>
        public double getHeight()
        {
            return height;
        }

        /// <summary>
        /// 获取体重
        /// </summary>
        /// <returns></returns>
        public double getWeight()
        {
            return weight;
        }
    }
}
