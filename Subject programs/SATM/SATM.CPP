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

	bool Initial(string m_PIN, string m_password);              //����PIN�����룬����ʼ�����
	
	int Withdraw(int account);            //ȡ��

	int Deposit(int account);             //���

	int const GetBalance();    //��ѯ���

	void ModifyPassword(string password,string password1,string password2);       //�޸�����

	static bool ValidPIN(string const pin)           //��֤PIN���Ƿ���Ч
	{
		if(pin.compare("111111") == 0)
			return true;	
		else 
			return false;
	}

	static bool ValidPassword(string const password) //��֤�����Ƿ���ȷ
	{
		if(password.compare("888888") == 0)	
			return true;	
		else 
			return false;
	}

private:
	string m_PIN;                //�ʺ�
	string m_password;           //����
	int m_balance;             //���
};

/*����PIN�����룬����ʼ�����Ϊ0*/
bool SATM::Initial(string m_PIN, string m_password)
{
	int times;

	//����PIN
	for(times=0; times < 3; times++)
	{
		cout << "\t������PIN��";
		//cin >> m_PIN;
		if(SATM::ValidPIN(m_PIN)==true)		
			break;	
		else if (times == 2)
		{
			cout << "\t�Բ���PIN ��������������3�Σ������˳���\n";
			return false;
		}
		else
			cout << "\tPIN �������\n";		
	}

	//��������
	for(times=0; times < 3; times++)
	{
		cout << "\t���������룺";
		//cin >> m_password;
		if(SATM::ValidPassword(m_password))		
			break;	
		else if (times == 2)
		{
			cout << "\t�Բ���������������������3�Σ������˳���\n";
			return false;
		}
		else
			cout << "\t�����������\n";		
	}
	
	//��ʼ�����Ϊ0
	m_balance = 0;
	return true;
}

/* ��� */
int SATM::Deposit(int account)
{
	//int account;
	cout << "\t���������";
	//scanf("%f",&account);
	if(account <= 0)		
		cout << "\t���������󣬱�������Ǹ���\n";		
	else
	{
		m_balance += account;
		cout << "\t�������Ϊ��"<< m_balance << endl;			
	}
	return m_balance;
}

/* ȡ�� */
int SATM::Withdraw(int account)
{
	//int account;
	cout << "\t������ȡ���";
	//scanf("%f",&account);
	if(account <= 0)
		cout << "\t���������󣬱�������Ǹ���\n";	
	else if(account > m_balance)
	{
		cout << "\t���㣡\n";	
		return -1;
	}
	else 
	{
		m_balance -= account;
		cout << "\t�������Ϊ��"<< m_balance << endl;			
	}
	return m_balance;
}

/* ��ѯ���� */
int const SATM::GetBalance()
{
	return m_balance;
}

/* �޸����� */
void SATM::ModifyPassword(string password,string password1,string password2)
{
	//string password1;
	//string password2;
	cout << "\t����������룺";
	//cin >> password1;
	if(password.compare(m_password) != 0)
		cout << "\t���벻��ȷ��\n";
	else{
		cout << "\t�����������룺";
		//cin >> password1;
		if(password1.length() != 6) 
			cout << "\t���������������Ӧ��Ϊ��λ���ȵ��ַ�����\n";
		else
		{
			cout << "\t���ٴ����������룺";
			//cin >> password2;
			if(password1.compare(password2) != 0)
			{
				cout << "\t���벻ƥ�䣬�޸�ʧ�ܣ�\n";
				return;
			}
			else
			{
				m_password =password1;
				cout << "\t�����޸ĳɹ���\n";
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
			cout << "\t|\t 1 ��ѯ��� \t|" << endl;
			cout << "\t|\t 2 ��    �� \t|" << endl;
			cout << "\t|\t 3 ȡ    �� \t|" << endl;
			cout << "\t|\t 4 �޸����� \t|" << endl;
			cout << "\t|\t 0 ��    �� \t|" << endl;
			cout << "\t|\t            \t|" << endl;
			cout << "\t*************************" << endl;	
			cout << "\t��ѡ��(0-4): ";
			cin >> option;
			switch (option)
			{
			case 0: return;
			case 1: 
				cout << "\t�������Ϊ��"<< satm.GetBalance() << endl;
				break;
			case 2: satm.Deposit(1000);	break;
			case 3: satm.Withdraw(1000); break;
			case 4: satm.ModifyPassword( password, password1,  password2); break;
			default: cout << "\tѡ�����\n"; break;
			}
		}while(true);
	}
}
 