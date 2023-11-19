#include <iostream>
#include<fstream>
#include "Calendar.h"
using namespace std;

CCalendarUnit::CCalendarUnit()
{
}

CCalendarUnit::~CCalendarUnit()
{
}

void CCalendarUnit::setCurrentPos(int pCurrentPos)
{
	currentPos = pCurrentPos;
}

CYear::CYear()
{
}

CYear::~CYear()
{
}

CYear::CYear(int year)
{
	setCurrentPos(year);
}

int CYear::getYear()
{
	return currentPos;
}

bool CYear:: increment()
{
	currentPos = currentPos + 1;
	return true;
}

bool CYear::isleap()
{
	if (currentPos/4 == 0 && currentPos/400 == 0 || currentPos/400 == 0)
	{
		return true;
	}
	else
	{
		return false;
	}
}

CMonth::CMonth()
{
   sizeIndex[0]=31;
   sizeIndex[1]=28;
   sizeIndex[2]=31;
   sizeIndex[3]=30;
   sizeIndex[4]=31;
   sizeIndex[5]=30;
   sizeIndex[6]=31;
   sizeIndex[7]=31;
   sizeIndex[8]=30;
   sizeIndex[9]=31;
   sizeIndex[10]=30;
   sizeIndex[11]=31;
}

CMonth::~CMonth()
{
}

CMonth::CMonth( int cur, CYear year)
{
   sizeIndex[0]=31;
   sizeIndex[1]=28;
   sizeIndex[2]=31;
   sizeIndex[3]=30;
   sizeIndex[4]=31;
   sizeIndex[5]=30;
   sizeIndex[6]=31;
   sizeIndex[7]=31;
   sizeIndex[8]=30;
   sizeIndex[9]=31;
   sizeIndex[10]=30;
   sizeIndex[11]=31;
   setMonth( cur, year);
} 

void CMonth::setMonth( int month, CYear year)
{
	setCurrentPos(month);
	y = year;
}

int CMonth::getMonth()
{
	return currentPos;
}

int CMonth::getMonthSize()
{
	if(y.isleap())
	{
		sizeIndex[1]=29;
	}
	else
	{
		sizeIndex[1]=28;
	}
	return sizeIndex[currentPos-1];
}

bool CMonth::increment()
{
	currentPos = currentPos +1;
	if (currentPos > 12)
	{
		return false;
	}
	else
	{
		return true;
	}
}

CDay::CDay()
{
}

CDay::~CDay()
{
}

CDay::CDay(int day,CMonth month)
{
	setDay(day,month);
}
void CDay::setDay(int day,CMonth month)
{
	setCurrentPos(day);
	m = month;
}

int CDay::getDay()
{
	return currentPos;
}

bool CDay::increment()
{
	currentPos = currentPos + 1;
	if (currentPos <= m.getMonthSize())//?
	{
		return true;
	}
	else
	{
		return false;
	}
}

CDate::CDate()
{
}

CDate::~CDate()
{
}

CDate::CDate(int month, int day, int year)
{
	y = CYear(year);
	m = CMonth(month,y);
	d = CDay(day,m);
}

void CDate::dateincrement()
{
	if (!d.increment())
	{
		if (!(m.increment()))
		{
			y.increment();
			m.setMonth(1,y);
		}
		d.setDay(1,m);
	}
} 

void CDate::printDate()
{
//	CString dateresult;
//	CString tempYear,tempMonth,tempDay;
//	tempYear.Format("%i",y.getYear());
//	tempMonth.Format("%i",m.getMonth());
//	tempDay.Format("%i",d.getDay());
//	dateresult = tempMonth+_T("/")+tempDay+_T("/")+tempYear;
	cout<<"The result is:";
	cout<<m.getMonth()<<"/"<<d.getDay()<<"/"<<y.getYear()<<endl;
}

void main()
{
	int testMonth,testDay,testYear;
	cout<<"Please input the month:";
	cin>>testMonth;
	cout<<"Please input the day:";
    cin>>testDay;
    cout<<"Please input the year:";
    cin>>testYear;
	
	CDate testdate(testMonth,testDay,testYear);
	testdate.dateincrement();
	testdate.printDate();
	//int a=0;
}
void CCalendarUnit::CCalendarUnitInitialize(int CurrentPos)//added
{
	this->currentPos=currentPos;
}
void CYear::CYearInitialize(int currentPos)//added
{
	this->currentPos=currentPos;
}
void CMonth::CMonthInitialize(int sizeIndex[12],int currentPos,CYear y)//added
{
	this->y=y;
	for(int i=0;i<12;i++)
	{
		this->sizeIndex[i]=sizeIndex[i];
	}
	this->currentPos=currentPos;
}
void CDay::CDayInitialize(int currentPos,CMonth m)//added
{
	this->m=m;
	this->currentPos=currentPos;
}
void CDate::CDateInitialize(CDay d,CMonth m,CYear y)//added
{
	this->d=d;
	this->m=m;
	this->y=y;
}