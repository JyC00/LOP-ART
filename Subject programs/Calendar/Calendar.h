class CCalendarUnit  
{
public:
	CCalendarUnit();
	virtual ~CCalendarUnit();
	void CCalendarUnitInitialize(int CurrentPos);//added
	void setCurrentPos(int pCurrentPos);
	//virtual bool increment();
		
	int currentPos;
};

class CYear : public CCalendarUnit  
{
public:
	CYear();
	void CYearInitialize(int currentPos);//added
	virtual ~CYear();
    CYear(int year);
	int getYear();
    bool increment();
	bool isleap();	
};


class CMonth : public CCalendarUnit   
{
public:
	CMonth();
	void CMonthInitialize(int sizeIndex[12],int currentPos,CYear y);//added
	virtual ~CMonth();
    CMonth(int month, CYear year);
	void setMonth(int cur,CYear year);
	int getMonth();
	int getMonthSize();
	bool increment();

private:

	int sizeIndex[12];//{31,28,31,30,31,30,31,31,30,31,30,31};
	CYear y;
};

class CDay : public CCalendarUnit 
{
public:
	CDay();
	void CDayInitialize(int currentPos,CMonth m);//added
	virtual ~CDay();
    CDay(int day, CMonth month);
	void setDay(int day,CMonth month);
	int getDay();
    bool increment();
private:
	CMonth m;
};

class CDate 
{
public:
	CDate();
	void CDateInitialize(CDay d,CMonth m,CYear y);//added
	virtual ~CDate();
    CDate(int month,int day,int year);
	void dateincrement();
	void printDate();

private:
	CDay d;
	CMonth m;
	CYear y;
};
