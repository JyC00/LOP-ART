/* SATM.cpp 
according to P179-P183 P219 P229
author: wangjing 
date: 2005/11/23
*/

#include <string>
#include <iostream>
using namespace std;

class SATM
{
public:
	SATM(){};
	void SATMInitialize(string m_PIN,string m_password,int m_balance)
	{
		this->m_PIN = m_PIN;
		this->m_password = m_password;
		this->m_balance = m_balance;
	}

	bool Initial(string m_PIN, string m_password);              //输入PIN、密码，并初始化余额
	
	int Withdraw(int account);            //取款

	int Deposit(int account);             //存款

	int const GetBalance();    //查询余额

	void ModifyPassword(string password,string password1,string password2);       //修改密码

	static bool ValidPIN(string const pin)           //验证PIN码是否有效
	{
		if(pin.compare("111111") == 0)
			return true;	
		else 
			return false;
	}

	static bool ValidPassword(string const password) //验证密码是否正确
	{
		if(password.compare("888888") == 0)	
			return true;	
		else 
			return false;
	}

private:
	string m_PIN;                //帐号
	string m_password;           //密码
	int m_balance;             //余额
};

/*读入PIN和密码，并初始化余额为0*/
bool SATM::Initial(string m_PIN, string m_password)
{
	int times;

	//读入PIN
	for(times=0; times < 3; times++)
	{
		cout << "\t请输入PIN：";
		//cin >> m_PIN;
		if(SATM::ValidPIN(m_PIN)==true)		
			break;	
		else if (times == 2)
		{
			cout << "\t对不起，PIN 输入错误次数大于3次！程序将退出！\n";
			return false;
		}
		else
			cout << "\tPIN 输入错误！\n";		
	}

	//读入密码
	for(times=0; times < 3; times++)
	{
		cout << "\t请输入密码：";
		//cin >> m_password;
		if(SATM::ValidPassword(m_password))		
			break;	
		else if (times == 2)
		{
			cout << "\t对不起，密码输入错误次数大于3次！程序将退出！\n";
			return false;
		}
		else
			cout << "\t密码输入错误！\n";		
	}
	
	//初始化余额为0
	m_balance = 0;
	return true;
}

/* 存款 */
int SATM::Deposit(int account)
{
	//int account;
	cout << "\t请输入存款金额：";
	//scanf("%f",&account);
	if(account <= 0)		
		cout << "\t金额输入错误，必须输入非负数\n";		
	else
	{
		m_balance += account;
		cout << "\t您的余额为："<< m_balance << endl;			
	}
	return m_balance;
}

/* 取款 */
int SATM::Withdraw(int account)
{
	//int account;
	cout << "\t请输入取款金额：";
	//scanf("%f",&account);
	if(account <= 0)
		cout << "\t金额输入错误，必须输入非负数\n";	
	else if(account > m_balance)
	{
		cout << "\t余额不足！\n";	
		return -1;
	}
	else 
	{
		m_balance -= account;
		cout << "\t您的余额为："<< m_balance << endl;			
	}
	return m_balance;
}

/* 查询语言 */
int const SATM::GetBalance()
{
	return m_balance;
}

/* 修改密码 */
void SATM::ModifyPassword(string password,string password1,string password2)
{
	//string password1;
	//string password2;
	cout << "\t请输入旧密码：";
	//cin >> password1;
	if(password.compare(m_password) != 0)
		cout << "\t密码不正确！\n";
	else{
		cout << "\t请输入新密码：";
		//cin >> password1;
		if(password1.length() != 6) 
			cout << "\t密码输入错误！密码应该为六位长度的字符串！\n";
		else
		{
			cout << "\t请再次输入新密码：";
			//cin >> password2;
			if(password1.compare(password2) != 0)
			{
				cout << "\t密码不匹配，修改失败！\n";
				return;
			}
			else
			{
				m_password =password1;
				cout << "\t密码修改成功！\n";
			}
		}
	}
}

void main(void)
{
	SATM satm;
	string m_PIN = "111111";
	string m_password = "888888";
	string password="888888";
	string password1="000000";
	string password2="000000";
	if (satm.Initial(m_PIN,m_password))
	{
		int option;
		do{		
			cout << "\n\t******    SATM    *******" << endl;
			cout << "\t|\t            \t|" << endl;
			cout << "\t|\t 1 查询余额 \t|" << endl;
			cout << "\t|\t 2 存    款 \t|" << endl;
			cout << "\t|\t 3 取    款 \t|" << endl;
			cout << "\t|\t 4 修改密码 \t|" << endl;
			cout << "\t|\t 0 退    出 \t|" << endl;
			cout << "\t|\t            \t|" << endl;
			cout << "\t*************************" << endl;	
			cout << "\t请选择(0-4): ";
			cin >> option;
			switch (option)
			{
			case 0: return;
			case 1: 
				cout << "\t您的余额为："<< satm.GetBalance() << endl;
				break;
			case 2: satm.Deposit(1000);	break;
			case 3: satm.Withdraw(1000); break;
			case 4: satm.ModifyPassword( password, password1,  password2); break;
			default: cout << "\t选择错误！\n"; break;
			}
		}while(true);
	}
}
 
