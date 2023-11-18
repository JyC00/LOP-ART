using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Diagnostics;
using System.Text;
using System.IO;
using System.Xml;
namespace ARTOO
{
    public struct Result
    {
        public double T1;
        public double T2;
        public double T3_Fm;
        public double T3_Em;
        public int Fm;
        public double Fm_time1;
        public double Fm_time2;
        public int Em;
        public int EmTCs;
        public double EmTCs_time1;
        public double EmTCs_time2;
    }
    public enum Language { Csharp, Cplusplus }
    public enum Algorithm{LOPART,UnRand,ARTooX_M,ARTOO,ARTGen,UnRand_M}
  
    public class FSCS_ART
    {
        string xmlFilePath;
        
        int currentAlgorithmIndex;
        #region 测试程序参数
        Language language;
        Algorithm algorithm;
        int forgetParameter;
        int ARTGenDiverseMaxNum;
        bool isTestcaseNumFixed;
        bool isRunFSCS_ART_FiexdTime;
        int[] FixedTestcaseNums;
        #region 函数参数的自定义typerange
        bool FunctionParamterCustomTyperangeUsed;
        int FunctionParamterCustomTyperangeMin;
        int FunctionParamterCustomTyperangeMax;
        #endregion        
        int loopCreateNumMax;//解决循环引用导致的死循环，暂定为3次
        string testProgramBefore;
        string testProgramAfter;
        string classDiagramFile;
        string testProgramBeforeNameSpace;
        string testProgramAfterNameSpace;
        #endregion
        private string version;
     
        private Random rnd;
      
        private List<Object> objects;
       
        public static Dictionary<string, Object> objectsDictionary;
       
        private int nextCreateObjectNum;        
        
        private StreamWriter sw;
        private StreamWriter consoleSw;
        private Result consoleResult;
        
        public List<TestCase> testCasePool;
       
        public List<List<List<TestCase>>> subTestcasePools;

        public List<TestCase> executedTestCaseSet;
  
        public FSCS_ART()
        {
            nextCreateObjectNum = 1;
            rnd = new Random(5);
            objects = new List<Object>();
            testCasePool = new List<TestCase>();
            subTestcasePools = new List<List<List<TestCase>>>();

            executedTestCaseSet = new List<TestCase>();
            //allFunctions = new List<string>();
            //allMembers = new List<string>();
            //allRefMembers = new List<string>();

        }
   
     
        public FSCS_ART(string xmlFilePath,int currentAlgorithmIndex)
        {
            nextCreateObjectNum = 1;
            rnd = new Random(5);
            objects = new List<Object>();
            testCasePool = new List<TestCase>();
            subTestcasePools = new List<List<List<TestCase>>>();
            this.xmlFilePath = xmlFilePath;
            this.currentAlgorithmIndex = currentAlgorithmIndex;
            executedTestCaseSet = new List<TestCase>();

        }
        public FSCS_ART(int randomseed)
        {
            nextCreateObjectNum = 1;
            rnd = new Random(randomseed);
            objects = new List<Object>();
            testCasePool = new List<TestCase>();
            subTestcasePools = new List<List<List<TestCase>>>();
           
            executedTestCaseSet = new List<TestCase>();

        }
        
        public void ExecuteFSCS_ART(int testCasePoolNum, int objectNumMax, int methLengthMax, int candidatesNum, int seed,int testProject)
        {            
            ExecuteFSCS_ARTInitialize(testCasePoolNum, objectNumMax, methLengthMax, candidatesNum, seed);
            GenerateTestCasePoolAndCalculateTime(testCasePoolNum, objectNumMax, methLengthMax, classDiagramFile);

            /*consoleSw = new StreamWriter("TestCases4.txt", false);
            for (int i = 0; i < 50;i++ )
            {

                foreach (Object o in testCasePool[i].Objects)
                {
                    consoleSw.WriteLine("name: {0} type: {1}", o.name, o.type);
                    consoleSw.WriteLine("o.behaviorSection.definitionMethods.Count: {0}", o.behaviorSection.definitionMethods.Count);
                    foreach (BehaviorMember b in o.behaviorSection.definitionMethods)
                        consoleSw.WriteLine(b.name);
                    consoleSw.WriteLine("o.staticSection.definitionSection.Count: {0}", o.staticSection.definitionSection.Count);
                    foreach (ObjectMember om in o.staticSection.definitionSection)
                        consoleSw.WriteLine(om.type);
                    consoleSw.WriteLine();
                }                             
            }*/
                
            GenerateCodeBeforeMutationAndCalculateTime(version, language);
            //if (language == Language.Csharp)
            //{
            //    //string s = "TestProject" + testProject++ + ".exe";

            //    ExcuteCodeCsharp("TestProject1.exe", testProgramBefore, version + "CodeBeforeMutation.cs");
            //}
            //else
            //{
            //    ExcuteCodeCplusplus(version + "CodeBeforeMutation.cpp");
            //}
            ReadBeforeMutationResultAndCalculateTime("UnRand"+seed.ToString() + "CodeBeforeMutation_result.txt");
            GenerateCodeAfterMutationAndCalculateTime(version, language);
            //if (language == Language.Csharp)
            //{
            //    //string s = "TestProject" + (testProject++) + ".exe";
            //    ExcuteCodeCsharp("TestProject1.exe", testProgramAfter, version + "CodeAfterMutation.cs");
            //}
            //else
            //{
            //    ExcuteCodeCplusplus(version + "CodeAfterMutation.cpp");
            //}
            ReadAfterMutationResultAndCalculateTime("UnRand" + seed.ToString() + "CodeAfterMutation_result.txt");
          
            FSCS_ART_AlgorithmAndCalculateTime(testCasePool, candidatesNum, 2); 
         
            CalculateTimeResult(testCasePoolNum);
            OutputResult();
            sw.Close();            
            consoleSw.Close();
        }
        private void CalculateTimeResult(int testCasePoolNum)
        {
            consoleResult.Fm_time1 = (consoleResult.T1 + consoleResult.T2) / testCasePoolNum * consoleResult.Fm + consoleResult.T3_Fm;
            consoleResult.Fm_time2 = consoleResult.T1 + consoleResult.T2 + consoleResult.T3_Fm;
            consoleResult.EmTCs_time1 = (consoleResult.T1 + consoleResult.T2) / testCasePoolNum * consoleResult.EmTCs + consoleResult.T3_Em;
            consoleResult.EmTCs_time2 = consoleResult.T1 + consoleResult.T2 + consoleResult.T3_Em;
        }
        private List<TestCase> ShallowCopyTestCasePool(List<TestCase> testCasePoolToCopy)
        {
            List<TestCase> testCasePoolCopyed = new List<TestCase>();
            foreach (TestCase t in testCasePoolToCopy)
            {
                testCasePoolCopyed.Add(t);
            }
            return testCasePoolCopyed;
        }
        private List<List<List<TestCase>>> ShallowCopySubTestCasePools(List<List<List<TestCase>>> testCasePoolToCopy)
        {
            List<List<List<TestCase>>> testCasePoolCopyed = new List<List<List<TestCase>>>();
            foreach (List<List<TestCase>> classTestcasePool in testCasePoolToCopy)
            {
                List<List<TestCase>> NewClassTestcasePool = new List<List<TestCase>>();
                foreach (List<TestCase> methodTestcasePool in classTestcasePool)
                {
                    List<TestCase> NewMethodTestcasePool = new List<TestCase>();
                    foreach (TestCase t in methodTestcasePool)
                    {
                        NewMethodTestcasePool.Add(t);
                    }
                    NewClassTestcasePool.Add(NewMethodTestcasePool);
                }
                testCasePoolCopyed.Add(NewClassTestcasePool);
            }
            return testCasePoolCopyed;
        }
        private void ExecuteFSCS_ARTInitialize(int testCasePoolNum, int objectNumMax, int methLengthMax, int CandidateNum, int seed)
        {
            ReadParametersFromXml(xmlFilePath);
            version = algorithm.ToString() + seed.ToString();
           
            string result =  version+   "TestCaseGenerateResult.txt";
            sw = new StreamWriter(result, false);
            rnd = new Random(seed);
           
            consoleSw = new StreamWriter( version + "ConsoleResultResult.txt", false);
           
            consoleSw.WriteLine("please input the number of test cases:{0}", testCasePoolNum.ToString());
            consoleSw.WriteLine("please input the max object number in one test case:{0}", objectNumMax.ToString());
            consoleSw.WriteLine("please input the max method length in one test case:{0}", methLengthMax.ToString());
            consoleSw.WriteLine("please input the Candidate number in one test case:{0}", CandidateNum.ToString());
            consoleSw.WriteLine("please input random seed:{0}", seed.ToString());
            Console.WriteLine("current tested method:{0}", algorithm.ToString());
            consoleSw.WriteLine("current tested method:{0}", algorithm.ToString());            
        }
        private void OutputResult()
        {
            if (consoleSw == null)
            {
                throw new ArgumentException("consoleSw 未初始化");
            }
            Console.WriteLine("T1(s):{0}", consoleResult.T1);
            Console.WriteLine("T2(s):{0}", consoleResult.T2);
            Console.WriteLine("T3(Fm)(s):{0}", consoleResult.T3_Fm);
            if (isTestcaseNumFixed == true || isRunFSCS_ART_FiexdTime == true)
            {
                Console.WriteLine("T3(Em)(s):{0}", consoleResult.T3_Em);
            }
            Console.WriteLine("Fm:{0}", consoleResult.Fm);
            Console.WriteLine("Fm-time1(s):{0}", consoleResult.Fm_time1);
            Console.WriteLine("Fm-time2(s):{0}", consoleResult.Fm_time2);
            if (isTestcaseNumFixed == true || isRunFSCS_ART_FiexdTime == true)
            {
                Console.WriteLine("Em:{0}", consoleResult.Em);
                Console.WriteLine("EmTCs:{0}", consoleResult.EmTCs);
                Console.WriteLine("EmTCs-time1(s):{0}", consoleResult.EmTCs_time1);
                Console.WriteLine("EmTCs-time2(s):{0}", consoleResult.EmTCs_time2);
            }
            consoleSw.WriteLine("T1(s):{0}", consoleResult.T1);
            consoleSw.WriteLine("T2(s):{0}", consoleResult.T2);
            consoleSw.WriteLine("T3(Fm)(s):{0}", consoleResult.T3_Fm);
            if (isTestcaseNumFixed == true || isRunFSCS_ART_FiexdTime == true)
            {
                consoleSw.WriteLine("T3(Em)(s):{0}", consoleResult.T3_Em);
            }
            consoleSw.WriteLine("Fm:{0}", consoleResult.Fm);
            consoleSw.WriteLine("Fm-time1(s):{0}", consoleResult.Fm_time1);
            consoleSw.WriteLine("Fm-time2(s):{0}", consoleResult.Fm_time2);
            if (isTestcaseNumFixed == true || isRunFSCS_ART_FiexdTime == true)
            {
                consoleSw.WriteLine("Em:{0}", consoleResult.Em);
                consoleSw.WriteLine("EmTCs:{0}", consoleResult.EmTCs);
                consoleSw.WriteLine("EmTCs-time1(s):{0}", consoleResult.EmTCs_time1);
                consoleSw.WriteLine("EmTCs-time2(s):{0}", consoleResult.EmTCs_time2);
            }
            Console.WriteLine("all completed successfully!");
            consoleSw.WriteLine("all completed successfully!");
            consoleSw.Flush();
        }
  
    
        private void GetDifferCandidates(int poolLength, int num, int[] indexArray)
        {
            int i = 0, j = 0;
            while (i < num)
            {
                int randomIndex = rnd.Next(poolLength);
                for (j = 0; j < i; j++)
                {
                    if (randomIndex == indexArray[j])
                    {
                        break;
                    }
                }
                if (j == i)
                {
                    indexArray[i] = randomIndex;
                    i++;
                }
            }
        }
     
       
        private void FSCS_ART_Algorithm(List<TestCase> testCasePool, int candidatesNum)
        {
            bool stoppingConditionSatisfied = false;
            List<string> bugsFind = new List<string>();
            int testcaseFailNum = 0;
            int testcaseUsed = 0;
            Console.WriteLine("...Test case executing until find fault");
            consoleSw.WriteLine("...Test case executing until find fault");            
            sw.WriteLine("Test case execution order:");
           
            TestCase firstExecutedTestCase = ExecuteFirstTestcaseAndJudge(testCasePool,/* executedTestCaseSet, */bugsFind, ref testcaseFailNum, ref testcaseUsed);            
            if (firstExecutedTestCase.isTestcasePass == false) stoppingConditionSatisfied = true;                        
            while (stoppingConditionSatisfied == false)
            {                
                if (testCasePool.Count < candidatesNum)
                {
                    Console.WriteLine("测试用例池已全部执行完");
                    consoleSw.WriteLine("测试用例池已全部执行完");
                    break;
                }
                TestCase nextExecutedTestcase = ExecuteNextTestcaseAndJudge(testCasePool, candidatesNum,/* executedTestCaseSet,*/ bugsFind, ref testcaseFailNum, ref testcaseUsed);                
                if (nextExecutedTestcase.isTestcasePass == false) stoppingConditionSatisfied = true;                
            }
            Console.WriteLine("testCase used:{0}", testcaseUsed);
            consoleSw.WriteLine("testCase used:{0}", testcaseUsed);
            consoleResult.Fm = testcaseUsed;
            MethodSum(/*executedTestCaseSet*/);
        }
      
        /// <param name="executedTestCaseSet">已执行的测试用例集</param>
        private void MethodSum(/*List<TestCase> executedTestCaseSet*/)
        {
            int sum = 0;
            foreach (TestCase t in executedTestCaseSet)
            {
                sum += t.methodSequences.Count;
            }
            Console.WriteLine("method sum:{0}", sum);
            consoleSw.WriteLine("method sum:{0}", sum);
        }
       
        /// <param name="executedTestCaseSetArray">已执行的测试用例集</param>
        private void MethodSumOneMethod(List<List<List<TestCase>>> executedTestCaseSetArray, List<List<TestCase>> deletedExecutedTestCaseSetArray)
        {
            int sum = 0;
            foreach (List<List<TestCase>> t1 in executedTestCaseSetArray)
            {
                foreach(List<TestCase> t2 in t1)
                {
                    foreach(TestCase t in t2)
                    {
                        sum += t.methodSequences.Count;
                    }
                }                
            }
            foreach(List<TestCase> t1 in deletedExecutedTestCaseSetArray)
            {
                foreach(TestCase t in t1)
                {
                    sum += t.methodSequences.Count;
                }
            }
            Console.WriteLine("method sum:{0}", sum);
            consoleSw.WriteLine("method sum:{0}", sum);
        }
   
     
        private void FSCS_ART_Algorithm(List<TestCase> testCasePool, int candidatesNum, int timeMinute)
        {
            if (isTestcaseNumFixed == true)
            {
               
                CheckFixedNumsValid(FixedTestcaseNums);
            }
            int currentFixedNumIndex = 0;
            int currentFixedNum = FixedTestcaseNums[currentFixedNumIndex];
            int testCasePoolNum = testCasePool.Count;
            Stopwatch time = new Stopwatch();
            TimeSpan timelimit = new TimeSpan(0, timeMinute, 0);
            List<string> bugsFind = new List<string>();
            bool stoppingConditionSatisfied = false;
      
            executedTestCaseSet.Clear();
            Executed.clear();
       
            int testcaseFailNum = 0;
            int testcaseUsed = 0;
            Console.WriteLine("...Test case executing for {0} minute(s)", timeMinute);
            consoleSw.WriteLine("...Test case executing for {0} minute(s)", timeMinute);
            sw.WriteLine("Test case execution order2:");
            time.Start();
            TestCase firstExecutedTestCase = ExecuteFirstTestcaseAndJudge(testCasePool/*, executedTestCaseSet*/, bugsFind, ref testcaseFailNum, ref testcaseUsed);
            while (stoppingConditionSatisfied == false)
            {
                if (testCasePool.Count < candidatesNum)
                {
                    Console.WriteLine("测试用例池已全部执行完");
                    consoleSw.WriteLine("测试用例池已全部执行完");
                    break;
                }
                ExecuteNextTestcaseAndJudge(testCasePool, candidatesNum,/* executedTestCaseSet,*/ bugsFind, ref testcaseFailNum, ref testcaseUsed);
                if (CheckTestcaseNumFixedStoppingConditionSatisfied(testcaseUsed, testcaseFailNum, testCasePoolNum, time, bugsFind, ref currentFixedNumIndex, ref currentFixedNum) == true)
                {
                    break;
                }

                if (isTestcaseNumFixed == false)
                {
                    if (time.Elapsed > timelimit)
                    {
                        stoppingConditionSatisfied = true;
                    }
                }
            }
            Console.WriteLine("testCase fail num:{0}", testcaseFailNum);
            consoleSw.WriteLine("testCase fail num:{0}", testcaseFailNum);
            Console.WriteLine("testCase used:{0}", testcaseUsed);
            consoleSw.WriteLine("testCase used:{0}", testcaseUsed);
            consoleResult.Em = testcaseFailNum;
            consoleResult.EmTCs = testcaseUsed;
            showBugsList(bugsFind);
            MethodSum(/*executedTestCaseSet*/);
        }
       
        private void CheckFixedNumsValid(int[] fixedTestcaseNums)
        {
            if (fixedTestcaseNums.Length == 0)
            {
                throw new ArgumentException("fixedNums 输入错误，fixedNums为空!");
            }
            if (fixedTestcaseNums[0] <= 2)
            {
                throw new ArgumentException("fixedNums 输入错误，fixedNums中元素必须大于2!");
            }
            for (int i = 0; i < fixedTestcaseNums.Length - 1; i++)
            {
                if (fixedTestcaseNums[i] >= fixedTestcaseNums[i + 1])
                {
                    throw new ArgumentException("fixedNums 输入错误，不是递增序列!");
                }
            }
        }
        private bool CheckTestcaseNumFixedStoppingConditionSatisfied(int testcaseUsed, int testcaseFailNum,int testCasePoolNum,Stopwatch time, List<string> bugsFind, ref int currentFixedNumIndex, ref int currentFixedNum)
        {
            if (isTestcaseNumFixed == true)
            {
                if (testcaseUsed == currentFixedNum)
                {
                    Console.WriteLine("固定测试用例{0}结果：", currentFixedNum);
                    consoleSw.WriteLine("固定测试用例{0}结果：", currentFixedNum);
                    Console.WriteLine("Em:{0}", testcaseFailNum);
                    consoleSw.WriteLine("Em:{0}", testcaseFailNum);
                    double EmTCs_time1 = (consoleResult.T1 + consoleResult.T2) / testCasePoolNum * currentFixedNum + time.Elapsed.TotalSeconds;
                    double EmTCs_time2 = consoleResult.T1 + consoleResult.T2 + time.Elapsed.TotalSeconds;
                    Console.WriteLine("EmTCs_time1:{0}", EmTCs_time1);
                    consoleSw.WriteLine("EmTCs_time1:{0}", EmTCs_time1);
                    Console.WriteLine("EmTCs_time2:{0}", EmTCs_time2);
                    consoleSw.WriteLine("EmTCs_time2:{0}", EmTCs_time2);
                    showBugsList(bugsFind);
                    
                    if (currentFixedNum == FixedTestcaseNums[FixedTestcaseNums.GetUpperBound(0)])
                    {
                        return true;
                    }
                    else
                    {
                        currentFixedNumIndex++;
                        currentFixedNum = FixedTestcaseNums[currentFixedNumIndex];
                    }
                }
                return false;
            }
            else
            {
                return false;
            }
        }
  
        private void FSCS_ART_AlgorithmAndCalculateTime(List<TestCase> testCasePool, int candidatesNum, int timeMinute)
        {
            List<TestCase> testCasePoolCopy=new List<TestCase>();
            List<List<List<TestCase>>> subTestcasePoolsCopy=new List<List<List<TestCase>>>();
            if (isTestOneMethod() == true) subTestcasePoolsCopy = ShallowCopySubTestCasePools(subTestcasePools);
            else testCasePoolCopy = ShallowCopyTestCasePool(testCasePool);

            Stopwatch time = new Stopwatch();
            time.Start();
            if (isTestOneMethod() == true)//
            {
                FSCS_ART_AlgorithmOneMethod(subTestcasePoolsCopy, candidatesNum);
            }
            else
            {
       
                FSCS_ART_Algorithm(testCasePoolCopy, candidatesNum);
            }
            time.Stop();
            Console.WriteLine("FSCS_ART_Algorithm time used：{0}", time.Elapsed);
            consoleSw.WriteLine("FSCS_ART_Algorithm time used：{0}", time.Elapsed);
            consoleResult.T3_Fm = time.Elapsed.TotalSeconds;
            if (isTestcaseNumFixed == true || isRunFSCS_ART_FiexdTime == true)//这点干嘛的？
            {
                time = new Stopwatch();
                time.Start();
                if (isTestOneMethod() == true)
                {
                    FSCS_ART_AlgorithmOneMethod(subTestcasePools, candidatesNum, timeMinute);
                }
                else
                {
                    
                    FSCS_ART_Algorithm(testCasePool, candidatesNum, timeMinute);
                }
                time.Stop();
                Console.WriteLine("FSCS_ART_Algorithm time used：{0}", time.Elapsed);
                consoleSw.WriteLine("FSCS_ART_Algorithm time used：{0}", time.Elapsed);
                consoleResult.T3_Em = time.Elapsed.TotalSeconds;
            }
        }

       /* private void FSCS_ART_AlgorithmOneMethod2(List<List<List<TestCase>>> subTestcasePools, int candidatesNum)
        {
            bool stoppingConditionSatisfied = false;
            ObjectTuple2 executedTestCaseSet = new ObjectTuple2(4);
            List<TestCase> currentTestcasePool = new List<TestCase>();
            List<List<ObjectTuple2>> executedTestCaseSetArray = new List<List<ObjectTuple2>>();
            List<List<TestCase>> deletedExecutedTestCaseSetArray = new List<List<TestCase>>();
            int testcaseFailNum = 0;
            int testcaseUsed = 0;
            List<string> bugsFind = new List<string>();
            Console.WriteLine("...Test case executing until find fault");
            consoleSw.WriteLine("...Test case executing until find fault");
            sw.WriteLine("Test case execution order:");
            DeleteEmptySubTestCasePools(subTestcasePools);
            InitialExecutedTestCaseSetArray2(executedTestCaseSetArray, subTestcasePools);
            do
            {
                int index1 = rnd.Next(subTestcasePools.Count);
                int index2 = rnd.Next(subTestcasePools[index1].Count);
                currentTestcasePool = subTestcasePools[index1][index2];
                executedTestCaseSet = executedTestCaseSetArray[index1][index2];
                if ((executedTestCaseSet.notNullCount +executedTestCaseSet.nullCount)==0)
                {
                    TestCase firstExecutedTestCase = ExecuteFirstTestcaseAndJudge(currentTestcasePool, executedTestCaseSet, bugsFind, ref testcaseFailNum, ref testcaseUsed);
                    if (firstExecutedTestCase.isTestcasePass == false) stoppingConditionSatisfied = true;
                }
                else
                {
                    TestCase nextExecutedTestcase = ExecuteNextTestcaseAndJudge(currentTestcasePool, candidatesNum, executedTestCaseSet,  bugsFind, ref testcaseFailNum, ref testcaseUsed);
                    if (nextExecutedTestcase.isTestcasePass == false) stoppingConditionSatisfied = true;
                }
                if (currentTestcasePool.Count < candidatesNum)
                {
                    subTestcasePools[index1].RemoveAt(index2);
                    deletedExecutedTestCaseSetArray.Add(executedTestCaseSetArray[index1][index2]);
                    executedTestCaseSetArray[index1].RemoveAt(index2);
                    if (subTestcasePools[index1].Count == 0)
                    {
                        subTestcasePools.RemoveAt(index1);
                        executedTestCaseSetArray.RemoveAt(index1);
                    }
                    if (subTestcasePools.Count == 0) break;
                }
            } while (stoppingConditionSatisfied == false);
            Console.WriteLine("testCase used:{0}", testcaseUsed);
            consoleSw.WriteLine("testCase used:{0}", testcaseUsed);
            consoleResult.Fm = testcaseUsed;
            MethodSumOneMethod(executedTestCaseSetArray, deletedExecutedTestCaseSetArray);
        }*/
        private void InitialExecutedObjectTupleArray(List<List<ObjectTuple2>> ExecutedTestcaseInObjectTuple2, List<List<List<TestCase>>> subTestcasePools)
        {
            for (int i = 0; i < subTestcasePools.Count; i++)
            {
                ExecutedTestcaseInObjectTuple2.Add(new List<ObjectTuple2>());
                for (int j = 0; j < subTestcasePools[i].Count; j++)
                {
                    ExecutedTestcaseInObjectTuple2[i].Add(new ObjectTuple2(4));
                }
            }
        }
        private void FSCS_ART_AlgorithmOneMethod(List<List<List<TestCase>>> subTestcasePools, int candidatesNum)
        {
            bool stoppingConditionSatisfied = false;
           
            List<TestCase> executedTestCaseSet = new List<TestCase>();
            List<TestCase> currentTestcasePool=new List<TestCase>();
            List<List<List<TestCase>>> executedTestCaseSetArray = new List<List<List<TestCase>>>();
            List<List<TestCase>> deletedExecutedTestCaseSetArray = new List<List<TestCase>>();
            List<List<ObjectTuple2>> ExecutedTestcaseInObjectTuple2 = new List<List<ObjectTuple2>>();
            int testcaseFailNum = 0;
            int testcaseUsed = 0;
            List<string> bugsFind = new List<string>();
            Console.WriteLine("...Test case executing until find fault");
            consoleSw.WriteLine("...Test case executing until find fault");            
            sw.WriteLine("Test case execution order:");
            DeleteEmptySubTestCasePools(subTestcasePools);
            InitialExecutedTestCaseSetArray(executedTestCaseSetArray, subTestcasePools);
            InitialExecutedObjectTupleArray(ExecutedTestcaseInObjectTuple2, subTestcasePools);
            do
            {
                int index1 = rnd.Next(subTestcasePools.Count);
                int index2 = rnd.Next(subTestcasePools[index1].Count);
                currentTestcasePool = subTestcasePools[index1][index2];
                executedTestCaseSet = executedTestCaseSetArray[index1][index2];
              
                if (executedTestCaseSet.Count == 0)
                {
                    TestCase firstExecutedTestCase = ExecuteFirstTestcaseAndJudgeOneMethod(executedTestCaseSetArray[index1][index2],currentTestcasePool, ExecutedTestcaseInObjectTuple2[index1][index2], bugsFind, ref testcaseFailNum, ref testcaseUsed);
                    if (firstExecutedTestCase.isTestcasePass == false) stoppingConditionSatisfied = true;
                }
                else
                {
                    TestCase nextExecutedTestcase = ExecuteNextTestcaseAndJudgeOneMethod(currentTestcasePool, candidatesNum, ExecutedTestcaseInObjectTuple2[index1][index2], bugsFind, ref testcaseFailNum, ref testcaseUsed);
                    if (nextExecutedTestcase.isTestcasePass == false) stoppingConditionSatisfied = true;
                }
                if (currentTestcasePool.Count < candidatesNum)
                {
                    subTestcasePools[index1].RemoveAt(index2);
                    deletedExecutedTestCaseSetArray.Add(executedTestCaseSetArray[index1][index2]);
                    executedTestCaseSetArray[index1].RemoveAt(index2);
                    if(subTestcasePools[index1].Count==0)
                    {
                        subTestcasePools.RemoveAt(index1);
                        executedTestCaseSetArray.RemoveAt(index1);
                    }
                    if (subTestcasePools.Count == 0) break;                
                }
            } while (stoppingConditionSatisfied == false);
            Console.WriteLine("testCase used:{0}", testcaseUsed);
            consoleSw.WriteLine("testCase used:{0}", testcaseUsed);
            consoleResult.Fm = testcaseUsed;
            MethodSumOneMethod(executedTestCaseSetArray, deletedExecutedTestCaseSetArray);
        }
        private void FSCS_ART_AlgorithmOneMethod(List<List<List<TestCase>>> subTestcasePools, int candidatesNum, int timeMinute)
        {
            if (isTestcaseNumFixed == true)
            {
                CheckFixedNumsValid(FixedTestcaseNums);
            }
            int currentFixedNumIndex = 0;
            int currentFixedNum = FixedTestcaseNums[currentFixedNumIndex];
            int testCasePoolNum = testCasePool.Count;
            Stopwatch time = new Stopwatch();
            TimeSpan timelimit = new TimeSpan(0, timeMinute, 0);
            bool stoppingConditionSatisfied = false;
            List<TestCase> executedTestCaseSet = new List<TestCase>();
            List<TestCase> currentTestcasePool = new List<TestCase>();
            List<List<List<TestCase>>> executedTestCaseSetArray = new List<List<List<TestCase>>>();
            List<List<TestCase>> deletedExecutedTestCaseSetArray = new List<List<TestCase>>();
            List<List<ObjectTuple2>> ExecutedTestcaseInObjectTuple2 = new List<List<ObjectTuple2>>();
            InitialExecutedObjectTupleArray(ExecutedTestcaseInObjectTuple2, subTestcasePools);
            
          
            List<string> bugsFind = new List<string>();
            int testcaseFailNum = 0;
            int testcaseUsed = 0;
            if (isTestcaseNumFixed == false)
            {
                Console.WriteLine("...Test case executing for {0} minute(s)", timeMinute);
                consoleSw.WriteLine("...Test case executing for {0} minute(s)", timeMinute);
            }
            sw.WriteLine("Test case execution order2:");
            time.Start();         
            DeleteEmptySubTestCasePools(subTestcasePools);
            InitialExecutedTestCaseSetArray(executedTestCaseSetArray, subTestcasePools);
           
            do
            {
                int index1 = rnd.Next(subTestcasePools.Count);
                int index2 = rnd.Next(subTestcasePools[index1].Count);
                currentTestcasePool = subTestcasePools[index1][index2];
                executedTestCaseSet = executedTestCaseSetArray[index1][index2];
                if (executedTestCaseSet.Count == 0)
                {
                    //Console.WriteLine("zhulili:{0},{1}", index1, index2);
                    ExecuteFirstTestcaseAndJudgeOneMethod(executedTestCaseSetArray[index1][index2], currentTestcasePool, ExecutedTestcaseInObjectTuple2[index1][index2], bugsFind, ref testcaseFailNum, ref testcaseUsed);
                   // ExecuteFirstTestcaseAndJudge(currentTestcasePool,/* executedTestCaseSet,*/ bugsFind, ref testcaseFailNum, ref testcaseUsed);
                }
                else
                {
                    //Console.WriteLine("zhulili:{0},{1}", index1, index2);
                    //for (int i = 0; i < subTestcasePools.Count; i++)
                    //    Console.WriteLine("testcasepool:{0}", subTestcasePools[i].Count);
                    ExecuteNextTestcaseAndJudgeOneMethod(currentTestcasePool, candidatesNum, ExecutedTestcaseInObjectTuple2[index1][index2], bugsFind, ref testcaseFailNum, ref testcaseUsed);
                   // ExecuteNextTestcaseAndJudge(currentTestcasePool, candidatesNum, /* executedTestCaseSet,*/  bugsFind, ref testcaseFailNum, ref testcaseUsed);
                }
                if (CheckTestcaseNumFixedStoppingConditionSatisfied(testcaseUsed, testcaseFailNum, testCasePoolNum, time, bugsFind, ref currentFixedNumIndex, ref currentFixedNum) == true)
                {
                    break;
                }
                if (isTestcaseNumFixed == false)
                {
                    if (time.Elapsed > timelimit)
                    {
                        stoppingConditionSatisfied = true;
                    }
                }
                if (currentTestcasePool.Count < candidatesNum)
                {
                    subTestcasePools[index1].RemoveAt(index2);
                    ExecutedTestcaseInObjectTuple2[index1].RemoveAt(index2);
                    deletedExecutedTestCaseSetArray.Add(executedTestCaseSetArray[index1][index2]);
                    executedTestCaseSetArray[index1].RemoveAt(index2);
                    if (subTestcasePools[index1].Count == 0)
                    {
                        subTestcasePools.RemoveAt(index1);
                        ExecutedTestcaseInObjectTuple2.RemoveAt(index1);
                        executedTestCaseSetArray.RemoveAt(index1);
                    }
                    if (subTestcasePools.Count == 0) break;
                }
            } while (stoppingConditionSatisfied == false);            
            Console.WriteLine("testCase fail num:{0}", testcaseFailNum);
            consoleSw.WriteLine("testCase fail num:{0}", testcaseFailNum);
            Console.WriteLine("testCase used:{0}", testcaseUsed);
            consoleSw.WriteLine("testCase used:{0}", testcaseUsed);
            consoleResult.Em = testcaseFailNum;
            consoleResult.EmTCs = testcaseUsed;
            showBugsList(bugsFind);
            MethodSumOneMethod(executedTestCaseSetArray, deletedExecutedTestCaseSetArray);
        }
        private void InitialExecutedTestCaseSetArray(List<List<List<TestCase>>> executedTestCaseSetArray, List<List<List<TestCase>>> subTestcasePools)
        {
            for (int i = 0; i < subTestcasePools.Count; i++)
            {
                executedTestCaseSetArray.Add(new List<List<TestCase>>());
                for (int j = 0; j < subTestcasePools[i].Count; j++)
                {
                    executedTestCaseSetArray[i].Add(new List<TestCase>());
                }
            }
        }

        private void InitialExecutedTestCaseSetArray2(List<List<ObjectTuple2>> executedTestCaseSetArray, List<List<List<TestCase>>> subTestcasePools)
        {
            for (int i = 0; i < subTestcasePools.Count; i++)
            {
                executedTestCaseSetArray.Add(new List<ObjectTuple2>());
                for (int j = 0; j < subTestcasePools[i].Count; j++)
                {
                    executedTestCaseSetArray[i].Add(new ObjectTuple2(4));
                }
            }
        }
   
        private TestCase ARTOO_XSelectNextExecutedTestcase(List<TestCase> candidates)
        {
            TestCase nextExecutedTestcase = new TestCase();
            Distance dist = new Distance(-1, 0);
            //float minDist=-1;
            List<TestCase> SameDistTestcases = new List<TestCase>();
            foreach (TestCase c in candidates)
            {
              
                Distance di = CalutateTestcaseDistance(c/*, executedTestCaseSet*/);
                //Console.WriteLine("{0}: {1}", c.name, di);
                if (di > dist)
                {
                    SameDistTestcases.Clear();
                    dist = di;
                    SameDistTestcases.Add(c);
                }
                else if (di == dist)
                {
                    SameDistTestcases.Add(c);
                }
            }

            //if (SameDistTestcases.Count == 1)
            return SameDistTestcases[0];
         
            /*else
            {
                int executedCandidatesNum = Math.Min(executedTestCaseSet.Count, forgetParameter);
                List<TestCase> executedCandidates = new List<TestCase>();
                if (forgetParameter < executedTestCaseSet.Count)
                {
                    executedCandidates = GetCandidates(executedTestCaseSet, executedCandidatesNum);
                }
                else
                    executedCandidates = executedTestCaseSet;

                float virDist = -1;

                foreach (TestCase c in SameDistTestcases)
                {
                    float di = CalutateVirualTestcaseDistance(c, executedCandidates);//要改这个方法
                    if (di > virDist)
                    {
                        virDist = di;
                        nextExecutedTestcase = c;
                    }
                }
                return nextExecutedTestcase;
            }*/
        }
      
        private TestCase ARTOOSelectNextExecutedTestcase(List<TestCase> executedTestCaseSet, List<TestCase> candidates)
        {
            TestCase nextExecutedTestcase = new TestCase();
            Distance dist = new Distance(-1, 0);
            foreach (TestCase c in candidates)
            {
                Distance di = new Distance(CalutateTestcaseDistanceSum(c, executedTestCaseSet),0);
                if (di > dist)
                {
                    dist = di;
                    nextExecutedTestcase = c;
                }
            }
            return nextExecutedTestcase;
        }
      
   
        private TestCase RandomSelectNextExecutedTestcase(List<TestCase> candidates)
        {
            int index = rnd.Next(candidates.Count);
            TestCase nextExecutedTestcase = candidates[index];
            return nextExecutedTestcase;
        }
     
      
     
        private TestCase ExecuteNextTestcaseAndJudge(List<TestCase> testCasePool, int candidatesNum/* ,List<TestCase> executedTestCaseSet*/ ,List<string> bugsFind, ref int testcaseFailNum, ref int testcaseUsed)
        {
            List<TestCase> candidates = GetCandidates(testCasePool, candidatesNum);
          
            TestCase nextExecutedTestcase = SelectNextExecutedTestcase(/* executedTestCaseSet,*/candidates);
            Executed.update(nextExecutedTestcase);
            executedTestCaseSet.Add(nextExecutedTestcase);
            Executed.executedTestcase++;
            testCasePool.Remove(nextExecutedTestcase);
            testcaseUsed++;
            JudgeExecutedTestCaseAndWriteInformation(nextExecutedTestcase, bugsFind, ref testcaseFailNum);
            sw.Flush();
            return nextExecutedTestcase;
        }
        private TestCase ExecuteNextTestcaseAndJudgeOneMethod(List<TestCase> testCasePool, int candidatesNum ,ObjectTuple2 objecttuple , List<string> bugsFind, ref int testcaseFailNum, ref int testcaseUsed)
        {
            List<TestCase> candidates = GetCandidates(testCasePool, candidatesNum);

            TestCase nextExecutedTestcase = SelectNextExecutedTestcaseOneMethod(objecttuple, candidates);
            objecttuple.update(nextExecutedTestcase.Objects[0]);  
            testCasePool.Remove(nextExecutedTestcase);
            testcaseUsed++;
            JudgeExecutedTestCaseAndWriteInformation(nextExecutedTestcase, bugsFind, ref testcaseFailNum);
            sw.Flush();
            return nextExecutedTestcase;
        }
        private TestCase SelectNextExecutedTestcaseOneMethod(ObjectTuple2  objecttuple, List<TestCase> candidates)
        {
            TestCase nextExecutedTestcase = new TestCase();
            if (algorithm == Algorithm.ARTOO || algorithm == Algorithm.ARTGen)
            {
                nextExecutedTestcase = ARTOOWithSumSelectNextExecutedTestcase(objecttuple, candidates);
            }
            else
            {
                throw new ArgumentException("Algorithm input error");
            }
            return nextExecutedTestcase;
        }
        private TestCase ARTOOWithSumSelectNextExecutedTestcase(ObjectTuple2 objecttuple, List<TestCase> candidates)
        {
            TestCase nextExecutedTestcase = new TestCase();
            float dist =-1;
            List<TestCase> SameDistTestcases = new List<TestCase>();
            foreach (TestCase c in candidates)
            {
                
                float di = ARTOOSumCalutateTestcaseDistance(objecttuple, c);
                //Console.WriteLine(di);
                if (di > dist)
                {
                    SameDistTestcases.Clear();
                    dist = di;
                    SameDistTestcases.Add(c);
                }
                else if (di == dist)
                {
                    SameDistTestcases.Add(c);
                }
            }
          
            return SameDistTestcases[0];  
        }
        private float ARTOOSumCalutateTestcaseDistance(ObjectTuple2 objecttuple, TestCase candidate)
        {
            NewObjectDistanceCalculator2 tc = new NewObjectDistanceCalculator2(candidate.Objects[0],objecttuple, objects);
            float Dist = tc.CalculateObjectDistance();
            return Dist;
        }
       
        /// <param name="executedTestCaseSet"></param>
        /// <param name="candidates"></param>
        /// <returns></returns>
        private TestCase SelectNextExecutedTestcase(/*List<TestCase> executedTestCaseSet,*/ List<TestCase> candidates)
        {
            TestCase nextExecutedTestcase = new TestCase();
            if (algorithm == Algorithm.LOPART || algorithm == Algorithm.ARTooX_M)
            {
                nextExecutedTestcase = ARTOO_XSelectNextExecutedTestcase(/* executedTestCaseSet,*/candidates);
            }
            else if (algorithm == Algorithm.UnRand || algorithm == Algorithm.UnRand_M)
            {
                nextExecutedTestcase = RandomSelectNextExecutedTestcase(candidates);
            }
            else if(algorithm == Algorithm.ARTOO ||algorithm == Algorithm.ARTGen)
            {
                nextExecutedTestcase=ARTOOSelectNextExecutedTestcase(executedTestCaseSet, candidates);//
            }
            else
            {
                throw new ArgumentException("Algorithm input error");
            }
            return nextExecutedTestcase;
        }
       
      
        private TestCase ExecuteFirstTestcaseAndJudge(List<TestCase> testCasePool, /*List<TestCase> executedTestCaseSet, */List<string> bugsFind, ref int testcaseFailNum, ref int testcaseUsed)
        {
            TestCase firstExecutedTestCase = ExecuteFirstTestcase(testCasePool/*, executedTestCaseSet*/);
            testcaseUsed++;
            JudgeExecutedTestCaseAndWriteInformation(firstExecutedTestCase, bugsFind, ref testcaseFailNum);
            sw.Flush();
            return firstExecutedTestCase;
        }
       
        /// <param name="testCasePool">c#的机制到底是什么样的啊，传进来的testCasePool若改变了，会影响最开始的赋值的那个值吗？</param>
        /// <param name="executedTestCaseSet"></param>
        /// <param name="bugsFind"></param>
        /// <param name="testcaseFailNum"></param>
        /// <param name="testcaseUsed"></param>
        /// <returns></returns>
        private TestCase ExecuteFirstTestcaseAndJudgeOneMethod(List<TestCase> executedTestCaseSet2,List<TestCase> testCasePool, ObjectTuple2 executedTestCaseSet, List<string> bugsFind, ref int testcaseFailNum, ref int testcaseUsed)
        {
            TestCase firstExecutedTestCase = ExecuteFirstTestcaseOneMethod(executedTestCaseSet2,testCasePool, executedTestCaseSet);
            testcaseUsed++;
            JudgeExecutedTestCaseAndWriteInformation(firstExecutedTestCase, bugsFind, ref testcaseFailNum);
            sw.Flush();
            return firstExecutedTestCase;
        }

        private TestCase ExecuteFirstTestcaseOneMethod(List<TestCase> executedTestCaseSet2, List<TestCase> testCasePool, ObjectTuple2 executedTestCaseSet)
        {          
            int firstExecutedTestCaseIndex = rnd.Next(testCasePool.Count);
            executedTestCaseSet.Initialize();
            TestCase firstTestcase = testCasePool[firstExecutedTestCaseIndex];
            executedTestCaseSet2.Add(firstTestcase);
            //Console.WriteLine(firstTestcase.Objects.Count());
            executedTestCaseSet.update(firstTestcase.Objects[0]);        
            testCasePool.RemoveAt(firstExecutedTestCaseIndex);
            return firstTestcase;
        }
        
      
        private TestCase ExecuteFirstTestcase(List<TestCase> testCasePool/*,List<TestCase> executedTestCaseSet*/)
        {
            
            int firstExecutedTestCaseIndex = rnd.Next(testCasePool.Count);
            Executed.Initialize();
            executedTestCaseSet.Add(testCasePool[firstExecutedTestCaseIndex]);
            Executed.executedTestcase++;
            Executed.update(testCasePool[firstExecutedTestCaseIndex]);
            //Console.WriteLine("update success!!");
            testCasePool.RemoveAt(firstExecutedTestCaseIndex);            
            return executedTestCaseSet[0];
        }
     
      
        private void JudgeExecutedTestCaseAndWriteInformation(TestCase executedTestCase, List<string> bugsFind, ref int testcaseFailNum)
        {
            if (executedTestCase.isTestcasePass == false)
            {
                if (bugsFind.Contains(executedTestCase.bugInWhichMethod))
                {
                    sw.WriteLine(executedTestCase.name + " fail:" + executedTestCase.bugInWhichMethod + "(bug has found before)");
                }
                else
                {
                    sw.WriteLine(executedTestCase.name + " fail:" + executedTestCase.bugInWhichMethod);
                    testcaseFailNum++;
                    bugsFind.Add(executedTestCase.bugInWhichMethod);
                }
            }
            else
            {
                sw.WriteLine(executedTestCase.name);
            }
        }
        private void DeleteEmptySubTestCasePools(List<List<List<TestCase>>> subTestcasePools)
        {
            for (int i = 0; i < subTestcasePools.Count; i++)
            {
                if (subTestcasePools[i].Count == 0)
                {
                    subTestcasePools.RemoveAt(i);
                    i--;
                }
            }
            if (subTestcasePools.Count == 0)
            {
                throw new ArgumentException("测试用例池为空！");
            }
        }        
        private void showBugsList(List<string> bugsFind)
        {
            Console.WriteLine("bugs list:");
            consoleSw.WriteLine("bugs list:");
            foreach (string s in bugsFind)
            {
                Console.WriteLine(s);
                consoleSw.WriteLine(s);
            }
        }
       
        private List<TestCase> GetCandidates(List<TestCase> fromSet, int candidatesNum)
        {
            List<TestCase> candidates = new List<TestCase>();
            int[] candidatesIndex = new int[candidatesNum];
            GetDifferCandidates(fromSet.Count, candidatesNum, candidatesIndex);
          
            for (int i = 0; i <= candidatesIndex.GetUpperBound(0); i++)
            {
                candidates.Add(fromSet[candidatesIndex[i]]);
            }
            return candidates;
        }
 
       /* private Distance CalutateTestcaseDistance(TestCase candidate, List<TestCase> executedTestcases)
        {
            Distance minDist = new Distance(float.MaxValue, float.MaxValue);
            foreach (TestCase t in executedTestCaseSet)
            {
                TestCaseDistanceCalculator tc = new TestCaseDistanceCalculator(t, candidate, objects);
                Distance newDist = tc.CalculateTestCaseDistance();//改这个
                if (newDist < minDist)
                {
                    minDist = newDist;
                }
            }
            return minDist;
        }*/
       /* private float CalutateVirualTestcaseDistance(TestCase candidate, List<TestCase> executedTestcases)
        {
            float minDist = float.MaxValue;
            foreach (TestCase t in executedTestCaseSet)
            {
                float sumDist = 0;
                //这里的每个测试用例的Objects已经改完，都是相等的
                int minLength = Math.Min(candidate.Objects.Count, t.Objects.Count);
                for (int i = 0; i < minLength; i++)
                {
                    if (candidate.Objects[i].type != "null" && t.Objects[i].type != "null")
                    {
                        VirutalSectionDC tc = new VirutalSectionDC(candidate.Objects[i].staticSection.definitionSection, t.Objects[i].staticSection.definitionSection);
                        sumDist += tc.calculateVirtualDistance();
                    }
                    else if (candidate.Objects[i].type == "null" && t.Objects[i].type != "null")
                        sumDist += t.Objects[i].staticSection.definitionSection.Count;
                    else if (candidate.Objects[i].type != "null" && t.Objects[i].type == "null")
                        sumDist += candidate.Objects[i].staticSection.definitionSection.Count;                      
                }

                if (sumDist < minDist)
                {
                    minDist = sumDist;
                }
            }
            return minDist;
        }*/
    
        private Distance CalutateTestcaseDistance(TestCase candidate/*, List<TestCase> executedTestcases*/)
        {
            
           NewTestCaseDistanceCalculator tc = new NewTestCaseDistanceCalculator(candidate, objects);
           Distance Dist = tc.CalculateTestCaseDistance();//改这个
           return Dist;
       }
      
        private float CalutateTestcaseDistanceSum(TestCase candidate, List<TestCase> executedTestcases)
        {
            float distSum = 0;
            foreach (TestCase t in executedTestcases)
            {
               
                ObjectDistanceCalculator2 odc = new ObjectDistanceCalculator2(t.Objects[0],candidate.Objects[0], objects);
              
                float newDist = odc.CalculateObjectDistance();
                distSum = distSum + newDist;
            }
            return distSum;
        }
      
        private void GenerateTestCasePool(int testCasePoolNum, int objectNumMax, int methLengthMax, string classDiagramFile)
        {
            nextCreateObjectNum = 1;
            objects.Clear();
            testCasePool.Clear();         
         
            List<Object> tempObjects;
            List<CustomType> customTypes;
            List<TestCase> tempTestCases;
            Help.Load_ADF_File(classDiagramFile, out tempObjects, out customTypes, out tempTestCases);//out什么用啊
      
            Help.GetFunctionsMembers(customTypes);
          
            for (int i = 0; i < testCasePoolNum; i++)
            {
                int objectNum = 0;
                objectNum = rnd.Next(1, objectNumMax + 1);
                testCasePool.Add(new TestCase());
                testCasePool[i].Objects = CreateObject(objectNum, customTypes);

                methLengthMax = Math.Max(objectNum, methLengthMax);
                testCasePool[i].methodSequences = MethSeqGeneration(testCasePool[i].Objects, objectNum, methLengthMax);

                MethSeqParameterCreate(customTypes, testCasePool[i].methodSequences);

                testCasePool[i].name = "TestCase" + (i + 1).ToString();
            }
        }
        private void GenerateTestCasePoolAndCalculateTime(int testCasePoolNum, int objectNumMax, int methLengthMax, string classDiagramFile)
        {
            Stopwatch time = new Stopwatch();
            time.Start();
            if (isTestOneMethod() == true)
            {
                GenerateTestCasePoolOneMethod(testCasePoolNum, objectNumMax, methLengthMax, classDiagramFile);
            }
            else
            {
                GenerateTestCasePool(testCasePoolNum, objectNumMax, methLengthMax, classDiagramFile);
            }
            time.Stop();
            Console.WriteLine("GenerateTestCasePool time used：{0}", time.Elapsed);
            consoleSw.WriteLine("GenerateTestCasePool time used：{0}", time.Elapsed);
            consoleResult.T1 = time.Elapsed.TotalSeconds;
            time.Reset();
            time.Start();
            objectsDictionary = new Dictionary<string, Object>();
            foreach (Object ob in objects)
            {
                objectsDictionary.Add(ob.name, ob);
            }
            time.Stop();
            Console.WriteLine("objectsDictionary generate time used：{0}", time.Elapsed);
        }
        public void GenerateTestCasePoolOneMethod(int testCasePoolNum, int objectNumMax, int methLengthMax, string classDiagramFile)
        {
            nextCreateObjectNum = 1;
            objects.Clear();
            testCasePool.Clear();
            subTestcasePools.Clear();
            List<Object> tempObjects;
            List<CustomType> customTypes;
            List<TestCase> tempTestCases;
            Help.Load_ADF_File(classDiagramFile, out tempObjects, out customTypes, out tempTestCases);

            int methodSum = 0;
            for (int i = 0; i < customTypes.Count; i++)
            {
                methodSum += customTypes[i].classFunctions.Count;
            }
            for (int ii = 0; ii < customTypes.Count; ii++)
            {
                List<List<TestCase>> classTestcasePool = new List<List<TestCase>>();
                for (int jj = 0; jj < customTypes[ii].classFunctions.Count; jj++)
                {
                    List<TestCase> methodTestcasePool = new List<TestCase>();
                    for (int i = 0; i < (testCasePoolNum / methodSum); i++)
                    {
                        int objectNum = 0;
                        objectNum = rnd.Next(1, objectNumMax + 1);
                        TestCase newTestcase = new TestCase();
                        newTestcase.Objects = new List<Object>();
                        newTestcase.Objects.Add(CreateOneObject(customTypes[ii], customTypes,0));
                        List<BehaviorMember> newOneMethodSequence = new List<BehaviorMember>();
                        BehaviorMember newBM = new BehaviorMember();
                        newBM.name = customTypes[ii].classFunctions[jj].name;
                        newBM.authority = customTypes[ii].classFunctions[jj].authority;
                        newBM.InWhichObject = newTestcase.Objects[0];
                        newBM.defineInWhichClass = customTypes[ii].classFunctions[jj].defineInWhichClass;
                        newBM.returnType = customTypes[ii].classFunctions[jj].returnType;
                        newBM.parameterList = customTypes[ii].classFunctions[jj].parameterList;
                        if (algorithm == Algorithm.ARTGen)
                        {
                            int ARTGenDiverseNum = rnd.Next(ARTGenDiverseMaxNum+1);
                            newOneMethodSequence = MethSeqGenerationNew(newTestcase.Objects[0], ARTGenDiverseNum);
                        }                        
                        newOneMethodSequence.Add(newBM);
                        newTestcase.methodSequences = newOneMethodSequence;
                        MethSeqParameterCreate(customTypes, newTestcase.methodSequences);
                        newTestcase.name = "TestCase" + (testCasePool.Count + 1).ToString();
                        methodTestcasePool.Add(newTestcase);
                        testCasePool.Add(newTestcase);
                    }
                    classTestcasePool.Add(methodTestcasePool);
                }
                subTestcasePools.Add(classTestcasePool);
            }
        }
        private List<BehaviorMember> MethSeqGenerationNew(Object ob, int length)
        {
            List<BehaviorMember> MethSeqs = new List<BehaviorMember>();
            int seqLength = length;
            for (int i = 0; i < seqLength; i++)
            {
                if (ob.behaviorSection.definitionMethods.Count > 0)
                {
                    int methIndex = rnd.Next(ob.behaviorSection.definitionMethods.Count);
                    MethSeqs.Add(new BehaviorMember(ob.behaviorSection.definitionMethods[methIndex]));
                }
            }
            return MethSeqs;
        }

        private List<Object> CreateObject(int num, List<CustomType> info)
        {
            List<Object> createObjects = new List<Object>();
            for (int i = 0; i < num; i++)
            {
                Object ob = new Object();
                int createObjectIndex = rnd.Next(info.Count);
                ob = CreateOneObject(info[createObjectIndex], info,0);
                createObjects.Add(ob);
            }
            return createObjects;
        }

        private Object CreateOneObject(CustomType customType, List<CustomType> customTypes, int loopCreateNum)
        {
            Object ob = new Object();
            ob.name = "object" + nextCreateObjectNum.ToString();
            nextCreateObjectNum++;
            ob.type = customType.name;

            for (int i = 0; i < customType.members.Count; i++)
            {
                ObjectMember om = new ObjectMember();
                om.name = ob.name + "_ObjectMember" + (i + 1).ToString();
                om.type = customType.members[i].type;
                om.typeRange = customType.members[i].typeRange;
                if (om.type.Contains("[") && om.type.Contains("]"))
                {
                    string[] arrayGeWeidu = om.type.Split('[')[1].Split(']')[0].Split(',');
                    int arraySize = 1;
                    foreach(string weiDu in arrayGeWeidu)
                    {
                        arraySize *= int.Parse(weiDu);
                    }
                    string arrayType = om.type.Split('[')[0];
                    switch (arrayType)
                    {                        
                        case "sbyte":
                            for (int arrayIndex = 0; arrayIndex < arraySize - 1; arrayIndex++)
                            {
                                om.value += (GenerateSbyte(om.typeRange).ToString() + ",");
                            }
                            om.value += GenerateSbyte(om.typeRange).ToString();
                            ob.staticSection.definitionSection.Add(om);
                            break;
                        case "byte":
                            for (int arrayIndex = 0; arrayIndex < arraySize - 1; arrayIndex++)
                            {
                                om.value += (GenerateByte(om.typeRange).ToString() + ",");
                            }
                            om.value += GenerateByte(om.typeRange).ToString();
                            ob.staticSection.definitionSection.Add(om);
                            break;
                        case "short":
                            for (int arrayIndex = 0; arrayIndex < arraySize - 1; arrayIndex++)
                            {
                                om.value += (GenerateShort(om.typeRange).ToString() + ",");
                            }
                            om.value += GenerateShort(om.typeRange).ToString();
                            ob.staticSection.definitionSection.Add(om);
                            break;
                        case "ushort":
                            for (int arrayIndex = 0; arrayIndex < arraySize - 1; arrayIndex++)
                            {
                                om.value += (GenerateUshort(om.typeRange).ToString() + ",");
                            }
                            om.value += GenerateUshort(om.typeRange).ToString();
                            ob.staticSection.definitionSection.Add(om);
                            break;
                        case "int":
                            for (int arrayIndex = 0; arrayIndex < arraySize - 1; arrayIndex++)
                            {
                                om.value += (GenerateInt(om.typeRange).ToString() + ",");
                            }
                            om.value += GenerateInt(om.typeRange).ToString();
                            ob.staticSection.definitionSection.Add(om);
                            break;
                        case "uint":
                            for (int arrayIndex = 0; arrayIndex < arraySize - 1; arrayIndex++)
                            {
                                om.value += (GenerateUint(om.typeRange).ToString() + ",");
                            }
                            om.value += GenerateUint(om.typeRange).ToString();
                            ob.staticSection.definitionSection.Add(om);
                            break;
                        case "long":
                            for (int arrayIndex = 0; arrayIndex < arraySize - 1; arrayIndex++)
                            {
                                om.value += (GenerateLong(om.typeRange).ToString() + ",");
                            }
                            om.value += GenerateLong(om.typeRange).ToString();
                            ob.staticSection.definitionSection.Add(om);
                            break;
                        case "ulong":
                            for (int arrayIndex = 0; arrayIndex < arraySize - 1; arrayIndex++)
                            {
                                om.value += (GenerateUlong(om.typeRange).ToString() + ",");
                            }
                            om.value += GenerateUlong(om.typeRange).ToString();
                            ob.staticSection.definitionSection.Add(om);
                            break;
                        case "float":
                            for (int arrayIndex = 0; arrayIndex < arraySize - 1; arrayIndex++)
                            {
                                om.value += (GenerateFloat(om.typeRange).ToString() + ",");
                            }
                            om.value += GenerateFloat(om.typeRange).ToString();
                            ob.staticSection.definitionSection.Add(om);
                            break;
                        case "double":
                            for (int arrayIndex = 0; arrayIndex < arraySize - 1; arrayIndex++)
                            {
                                om.value += (GenerateDouble(om.typeRange).ToString() + ",");
                            }
                            om.value += GenerateDouble(om.typeRange).ToString();
                            ob.staticSection.definitionSection.Add(om);
                            break;
                        case "decimal":
                            for (int arrayIndex = 0; arrayIndex < arraySize - 1; arrayIndex++)
                            {
                                om.value += (GenerateDecimal(om.typeRange).ToString() + ",");
                            }
                            om.value += GenerateDecimal(om.typeRange).ToString();
                            ob.staticSection.definitionSection.Add(om);
                            break;
                        case "char":
                            for (int arrayIndex = 0; arrayIndex < arraySize - 1; arrayIndex++)
                            {
                                om.value += ("'" + GenerateChar().ToString() + "'" + ",");
                            }
                            om.value += "'" + GenerateChar().ToString() + "'";
                            ob.staticSection.definitionSection.Add(om);
                            break;
                        case "bool":
                            for (int arrayIndex = 0; arrayIndex < arraySize - 1; arrayIndex++)
                            {
                                om.value += (GenerateBool().ToString() + ",");
                            }
                            om.value += GenerateBool().ToString();
                            ob.staticSection.definitionSection.Add(om);
                            break;
                        case "string":
                            for (int arrayIndex = 0; arrayIndex < arraySize - 1; arrayIndex++)
                            {
                                om.value += ("\"" + GenerateString(om.typeRange) + "\"" + ",");
                            }
                            om.value += "\"" + GenerateString(om.typeRange) + "\"";
                            ob.staticSection.definitionSection.Add(om);
                            break;
                        default:
                            MessageBox.Show("\"" + om.type.ToString() + "\"" + " type is not generate!", "Error");
                            break;
                    }
                }
                else
                {
                    switch (om.type)
                    {
                        case "bool":
                            om.value = GenerateBool().ToString().ToLower();
                            ob.staticSection.definitionSection.Add(om);
                            break;
                        case "int":
                            om.value = GenerateInt(om.typeRange).ToString();
                            ob.staticSection.definitionSection.Add(om);
                            break;
                        case "uint":
                            om.value = GenerateUint(om.typeRange).ToString();
                            ob.staticSection.definitionSection.Add(om);
                            break;
                        case "float":
                            om.value = GenerateFloat(om.typeRange).ToString();
                            ob.staticSection.definitionSection.Add(om);
                            break;
                        case "double":
                            om.value = GenerateDouble(om.typeRange).ToString();
                            ob.staticSection.definitionSection.Add(om);
                            break;
                        case "enum":
                            om.value = GenerateEnum(om.typeRange).ToString();
                            ob.staticSection.definitionSection.Add(om);
                            break;
                        case "char":
                            om.value = "'" + GenerateChar().ToString() + "'";
                            ob.staticSection.definitionSection.Add(om);
                            break;
                        case "string":
                            om.value = "\"" + GenerateString(om.typeRange) + "\"";
                            ob.staticSection.definitionSection.Add(om);
                            break;
                        default:
                            CustomType ct = new CustomType();
                            bool isFind = false;
                            for (int j = 0; j < customTypes.Count; j++)
                            {
                                if (customTypes[j].name == om.type)
                                {
                                    ct = customTypes[j];
                                    isFind = true;
                                    break;
                                }
                            }
                            if (isFind == true)
                            {
                                if (ob.type == om.type)
                                {
                                    loopCreateNum++;
                                    if (loopCreateNum > loopCreateNumMax)
                                    {
                                        om.value = "null";
                                    }
                                    else
                                    {
                                        Object subObject = CreateOneObject(ct, customTypes, loopCreateNum);
                                        om.value = subObject.name;
                                    }
                                }
                                else
                                {
                                    Object subObject = CreateOneObject(ct, customTypes,0);
                                    om.value = subObject.name;
                                }
                            }
                            else
                            {
                                throw new ArgumentException(om.type.ToString() + " type is not generate!");
                            }
                            ob.staticSection.embeddedSection.Add(om);
                            break;
                    }
                }
            }
            foreach (ClassFunction cf in customType.classFunctions)
            {
                BehaviorMember newBM = new BehaviorMember();
                newBM.name = cf.name;
                newBM.authority = cf.authority;
                newBM.InWhichObject = ob;
                newBM.defineInWhichClass = cf.defineInWhichClass;
                newBM.returnType = cf.returnType;
                newBM.parameterList = cf.parameterList;
                if (newBM.authority != "private")
                {
                    ob.behaviorSection.definitionMethods.Add(newBM);
                }
            }
            objects.Add(ob);
            return ob;
        }

        private List<BehaviorMember> MethSeqGeneration(List<Object> objects, int lengthMin, int lengthMax)
        {
            List<BehaviorMember> MethSeqs = new List<BehaviorMember>();
            int seqLength = rnd.Next(Math.Max(1, lengthMin), lengthMax);
            for (int i = 0; i < seqLength; i++)
            {
                int objectIndex = rnd.Next(objects.Count);
                if (objects[objectIndex].behaviorSection.definitionMethods.Count > 0)
                {
                    int methIndex = rnd.Next(objects[objectIndex].behaviorSection.definitionMethods.Count);

               
                    MethSeqs.Add(new BehaviorMember(objects[objectIndex].behaviorSection.definitionMethods[methIndex]));
                }
            }
            return MethSeqs;
        }

        public void MethSeqParameterCreate(List<CustomType> customTypes, List<BehaviorMember> MethSeqs)
        {
            int currentBehaviorMemberIndexInMethSeqs = -1;
            foreach (BehaviorMember bm in MethSeqs)
            {
                currentBehaviorMemberIndexInMethSeqs++;
                bm.parameters = new List<ParameterMember>();
                if (bm.parameterList != "")
                {
                    string[] parameters = bm.parameterList.Split(',');
                    for (int i = 0; i < parameters.Length; i++)
                    {
                        ParameterMember pm = new ParameterMember();
                        pm.name = bm.InWhichObject.name+"_"+bm.name + "_ParamaterMember" + (i + 1).ToString()+"_"+currentBehaviorMemberIndexInMethSeqs.ToString();
                        string currentParameter=parameters[i].Trim();
                        string[] typeTemp;
                        string arrayDefinePart="";
                        if (currentParameter.Contains("["))
                        {
                            arrayDefinePart = "[" + currentParameter.Trim().Split('[')[1];
                            typeTemp = currentParameter.Trim().Split('[')[0].Split(' ');
                        }
                        else
                        {
                            typeTemp = currentParameter.Trim().Split(' ');
                        }
                        for (int j = 0; j < typeTemp.Length - 1; j++)
                        {
                            pm.type += typeTemp[j] + " ";
                        }
                        pm.type = pm.type.Trim();
                        if (currentParameter.Contains("["))
                        {
                            pm.type += arrayDefinePart;
                        }                        
                        pm.typeRange = new TypeRange();
                        pm.typeRange.isInputMinMax = FunctionParamterCustomTyperangeUsed;
                        pm.typeRange.min = FunctionParamterCustomTyperangeMin;
                        pm.typeRange.max = FunctionParamterCustomTyperangeMax;
                        if (pm.type.Contains("[") && pm.type.Contains("]"))
                        {
                            pm.isBaseType = true;
                            int arraySize = int.Parse(pm.type.Split('[')[1].Split(']')[0]);
                            string arrayType = pm.type.Split('[')[0];
                            switch (arrayType)
                            {
                                case "int":
                                    for (int arrayIndex = 0; arrayIndex < arraySize - 1; arrayIndex++)
                                    {
                                        pm.value += (GenerateInt(pm.typeRange).ToString() + ",");
                                    }
                                    pm.value += GenerateInt(pm.typeRange).ToString();
                                    bm.parameters.Add(pm);
                                    break;
                                default:
                                    MessageBox.Show("\"" + pm.type.ToString() + "\"" + " type is not generate!", "Error");
                                    break;
                            }
                        }
                        else
                        {
                            switch (pm.type)
                            {
                                case "bool":
                                    pm.value = GenerateBool().ToString().ToLower(); ;
                                    pm.isBaseType = true;
                                    bm.parameters.Add(pm);
                                    break;
                                case "int":
                                    pm.value = GenerateInt(pm.typeRange).ToString();
                                    pm.isBaseType = true;
                                    bm.parameters.Add(pm);
                                    break;
                                case "uint":
                                    pm.value = GenerateUint(pm.typeRange).ToString();
                                    pm.isBaseType = true;
                                    bm.parameters.Add(pm);
                                    break;
                                case "unsigned long":
                                case "unsigned long*":
                                case "const unsigned long*":
                                    pm.value = GenerateUlong(pm.typeRange).ToString();
                                    pm.isBaseType = true;
                                    bm.parameters.Add(pm);
                                    break;
                                case "float":
                                    pm.value = GenerateFloat(pm.typeRange).ToString();
                                    pm.isBaseType = true;
                                    bm.parameters.Add(pm);
                                    break;
                                case "double":
                                    pm.value = GenerateDouble(pm.typeRange).ToString();
                                    pm.isBaseType = true;
                                    bm.parameters.Add(pm);
                                    break;
                                case "enum":
                                    pm.value = GenerateEnum(pm.typeRange).ToString();
                                    pm.isBaseType = true;
                                    bm.parameters.Add(pm);
                                    break;
                                case "char":
                                case "char*":
                                case "const char*":
                                    pm.value = "'" + GenerateChar().ToString() + "'";
                                    pm.isBaseType = true;
                                    bm.parameters.Add(pm);
                                    break;
                                case "string":
                                case "string&":
                                case "const string&":
                                    pm.value = "\"" + GenerateString(pm.typeRange) + "\"";
                                    pm.isBaseType = true;
                                    bm.parameters.Add(pm);
                                    break;
                                default:
                                    pm.isBaseType = false;
                                    CustomType ct = new CustomType();
                                    bool isFind = false;
                                    for (int j = 0; j < customTypes.Count; j++)
                                    {
                                        if (customTypes[j].name == pm.type)
                                        {
                                            ct = customTypes[j];
                                            isFind = true;
                                            break;
                                        }
                                    }
                                    if (isFind == true)
                                    {
                                        Object subObject = CreateOneObject(ct, customTypes,0);
                                        pm.value = subObject.name;
                                    }
                                    else
                                    {
                                        MessageBox.Show("\"" + pm.type.ToString() + "\"" + " type is not generate!", "Error");
                                    }
                                    bm.parameters.Add(pm);
                                    break;
                            }
                        }
                    }
                }
            }
        }
 
        private sbyte GenerateSbyte(TypeRange typeRange)
        {
            List<sbyte> particular = new List<sbyte>();
            if (typeRange.isInputMinMax == true)
            {
                particular.Add((sbyte)typeRange.min);
                particular.Add((sbyte)typeRange.max);
                particular.Add((sbyte)(typeRange.min + 1));
                particular.Add((sbyte)(typeRange.max - 1));
            }
            else
            {
                particular.Add(0);
                particular.Add(1);
                particular.Add(sbyte.MinValue);
                particular.Add(sbyte.MaxValue);
                particular.Add(sbyte.MinValue + 1);
                particular.Add(sbyte.MaxValue - 1);
            }
            int randomNum = rnd.Next(4);
            if (randomNum == 0)
            {
                int randomIndex = rnd.Next(particular.Count);
                return particular[randomIndex];
            }
            else
            {
                if (typeRange.isInputMinMax == true)
                {
                    return (sbyte)(typeRange.min + (rnd.NextDouble() * (typeRange.max - typeRange.min)));
                }
                else
                {
                    return (sbyte)(sbyte.MinValue + (rnd.NextDouble() * (sbyte.MaxValue - sbyte.MinValue)));
                }
            }
        }

        private byte GenerateByte(TypeRange typeRange)
        {
            List<byte> particular = new List<byte>();
            if (typeRange.isInputMinMax == true)
            {
                particular.Add((byte)typeRange.min);
                particular.Add((byte)typeRange.max);
                particular.Add((byte)(typeRange.min + 1));
                particular.Add((byte)(typeRange.max - 1));
            }
            else
            {
                particular.Add(0);
                particular.Add(1);
                particular.Add(byte.MinValue);
                particular.Add(byte.MaxValue);
                particular.Add(byte.MinValue + 1);
                particular.Add(byte.MaxValue - 1);
            }
            int randomNum = rnd.Next(4);
            if (randomNum == 0)
            {
                int randomIndex = rnd.Next(particular.Count);
                return particular[randomIndex];
            }
            else
            {
                if (typeRange.isInputMinMax == true)
                {
                    return (byte)(typeRange.min + (rnd.NextDouble() * (typeRange.max - typeRange.min)));
                }
                else
                {
                    return (byte)(byte.MinValue + (rnd.NextDouble() * (byte.MaxValue - byte.MinValue)));
                }
            }
        }
      
        private short GenerateShort(TypeRange typeRange)
        {
            List<short> particular = new List<short>();
            if (typeRange.isInputMinMax == true)
            {
                particular.Add((short)typeRange.min);
                particular.Add((short)typeRange.max);
                particular.Add((short)(typeRange.min + 1));
                particular.Add((short)(typeRange.max - 1));
            }
            else/
            {
                particular.Add(0);
                particular.Add(1);
                particular.Add(short.MinValue);
                particular.Add(short.MaxValue);
                particular.Add(short.MinValue + 1);
                particular.Add(short.MaxValue - 1);
            }
            int randomNum = rnd.Next(4);
            if (randomNum == 0)
            {
                int randomIndex = rnd.Next(particular.Count);
                return particular[randomIndex];
            }else
            {
                if (typeRange.isInputMinMax == true)
                {
                    return (short)rnd.Next((int)typeRange.min, (int)typeRange.max);
                }
                else
                {
                    return (short)rnd.Next(short.MinValue, short.MaxValue);
                }
            }
        }

        private ushort GenerateUshort(TypeRange typeRange)
        {
            List<ushort> particular = new List<ushort>();
            if (typeRange.isInputMinMax == true)
            {
                particular.Add((ushort)typeRange.min);
                particular.Add((ushort)typeRange.max);
                particular.Add((ushort)(typeRange.min + 1));
                particular.Add((ushort)(typeRange.max - 1));
            }
            else
            {
                particular.Add(0);
                particular.Add(1);
                particular.Add(ushort.MinValue);
                particular.Add(ushort.MaxValue);
                particular.Add(ushort.MinValue + 1);
                particular.Add(ushort.MaxValue - 1);
            }
            int randomNum = rnd.Next(4);
            if (randomNum == 0)
            {
                int randomIndex = rnd.Next(particular.Count);
                return particular[randomIndex];
            }
            else
            {
                if (typeRange.isInputMinMax == true)
                {
                    return (ushort)rnd.Next((int)typeRange.min, (int)typeRange.max);
                }
                else
                {
                    return (ushort)rnd.Next(ushort.MinValue, ushort.MaxValue);
                }
            }
        }

        private int GenerateInt(TypeRange typeRange)
        {
            List<int> particular = new List<int>();
            if (typeRange.isInputMinMax == true)
            {
                particular.Add((int)typeRange.min);
                particular.Add((int)typeRange.max);
                particular.Add((int)typeRange.min + 1);
                particular.Add((int)typeRange.max - 1);
            }
            else
            {
                particular.Add(0);
                particular.Add(-1);
                particular.Add(1);
                particular.Add(int.MinValue);
                particular.Add(int.MaxValue);
                particular.Add(int.MinValue + 1);
                particular.Add(int.MaxValue - 1);
            }
            int randomNum = rnd.Next(4);
            if (randomNum == 0)
            {
                int randomIndex = rnd.Next(particular.Count);
                return particular[randomIndex];
            }
            else
            {
                if (typeRange.isInputMinMax == true)
                {
                    return rnd.Next((int)typeRange.min, (int)typeRange.max);
                }
                else
                {
                    return rnd.Next(int.MinValue, int.MaxValue);
                }
            }
        }
      
        private uint GenerateUint(TypeRange typeRange)
        {
            List<uint> particular = new List<uint>();
            if (typeRange.isInputMinMax == true)
            {
                particular.Add((uint)typeRange.min);
                particular.Add((uint)typeRange.max);
                particular.Add((uint)typeRange.min + 1);
                particular.Add((uint)typeRange.max - 1);
            }
            else
            {
                particular.Add(0);
                particular.Add(1);
                particular.Add(uint.MinValue);
                particular.Add(uint.MaxValue);
                particular.Add(uint.MinValue + 1);
                particular.Add(uint.MaxValue - 1);
            }
            int randomNum = rnd.Next(4);
            if (randomNum == 0)
            {
                int randomIndex = rnd.Next(particular.Count);
                return particular[randomIndex];
            }
            else
            {
                if (typeRange.isInputMinMax == true)
                {
                    return (uint)(typeRange.min + rnd.NextDouble() * (typeRange.max - typeRange.min));
                }
                else
                {
                    return (uint)(uint.MinValue + rnd.NextDouble() * (uint.MaxValue - uint.MinValue));
                }
            }
        }
   
        private long GenerateLong(TypeRange typeRange)
        {
            List<long> particular = new List<long>();
            if (typeRange.isInputMinMax == true)
            {
                particular.Add((long)typeRange.min);
                particular.Add((long)typeRange.max);
                particular.Add((long)typeRange.min + 1);
                particular.Add((long)typeRange.max - 1);
            }
            else
            {
                particular.Add(0);
                particular.Add(1);
                particular.Add(long.MinValue);
                particular.Add(long.MaxValue);
                particular.Add(long.MinValue + 1);
                particular.Add(long.MaxValue - 1);
            }
            int randomNum = rnd.Next(4);
            if (randomNum == 0)
            {
                int randomIndex = rnd.Next(particular.Count);
                return particular[randomIndex];
            }
            else
            {
                if (typeRange.isInputMinMax == true)
                {
                    return (long)(typeRange.min + (rnd.NextDouble() * (typeRange.max - typeRange.min)));
                }
                else
                {
                    return (long)(long.MinValue + (rnd.NextDouble() * ulong.MaxValue));
                }
            }
        }
        
        private ulong GenerateUlong(TypeRange typeRange)
        {
            List<ulong> particular = new List<ulong>();
            if (typeRange.isInputMinMax == true)
            {
                particular.Add((ulong)typeRange.min);
                particular.Add((ulong)typeRange.max);
                particular.Add((ulong)typeRange.min + 1);
                particular.Add((ulong)typeRange.max - 1);
            }
            else
            {
                particular.Add(0);
                particular.Add(1);
                particular.Add(ulong.MinValue);
                particular.Add(ulong.MaxValue);
                particular.Add(ulong.MinValue + 1);
                particular.Add(ulong.MaxValue - 1);
            }
            int randomNum = rnd.Next(4);
            if (randomNum == 0)
            {
                int randomIndex = rnd.Next(particular.Count);
                return particular[randomIndex];
            }
            else
            {
                if (typeRange.isInputMinMax == true)
                {
                    return (ulong)(typeRange.min + (rnd.NextDouble() * (typeRange.max - typeRange.min)));
                }
                else
                {
                    return (ulong)(ulong.MinValue + (rnd.NextDouble() * (ulong.MaxValue - ulong.MinValue)));
                }
            }
        }
    
        private float GenerateFloat(TypeRange typeRange)
        {
            List<float> particular = new List<float>();
            if (typeRange.isInputMinMax == true)
            {
                particular.Add((float)typeRange.min);
                particular.Add((float)typeRange.max);
                particular.Add((float)typeRange.min + 1);
                particular.Add((float)typeRange.max - 1);
            }
            else
            {
                particular.Add(0);
                particular.Add(-1);
                particular.Add(1);
                particular.Add(float.MinValue);
                particular.Add(float.MaxValue);
                particular.Add(float.MinValue + 1);
                particular.Add(float.MaxValue - 1);
            }
            int randomNum = rnd.Next(4);
            if (randomNum == 0)
            {
                int randomIndex = rnd.Next(particular.Count);
                return particular[randomIndex];
            }
            else
            {
                if (typeRange.isInputMinMax == true)
                {
                    return (float)(typeRange.min + (rnd.NextDouble() * (typeRange.max - typeRange.min)));
                }
                else
                {
                    return (float)(float.MinValue + (rnd.NextDouble() * (float.MaxValue - float.MinValue)));
                }
            }
        }
       
        private double GenerateDouble(TypeRange typeRange)
        {
            List<double> particular = new List<double>();
            if (typeRange.isInputMinMax == true)
            {
                particular.Add((double)typeRange.min);
                particular.Add((double)typeRange.max);
                particular.Add((double)typeRange.min + 1);
                particular.Add((double)typeRange.max - 1);
            }
            else
            {
                particular.Add(0);
                particular.Add(-1);
                particular.Add(1);
                particular.Add(-1.79769313486E+308);
                particular.Add(1.79769313486E+308);
            }
            int randomNum = rnd.Next(4);
            if (randomNum == 0)
            {
                int randomIndex = rnd.Next(particular.Count);
                return particular[randomIndex];
            }
            else
            {
                if (typeRange.isInputMinMax == true)
                {
                    return typeRange.min + (rnd.NextDouble() * (typeRange.max - typeRange.min));
                }
                else
                {
                    double result = rnd.NextDouble() * double.MaxValue;
                    randomNum = rnd.Next(2);
                    if (randomNum == 0)
                    {
                        return result;
                    }
                    else
                    {
                        return (-1)*result;
                    }
                    
                }
            }
        }

        private decimal GenerateDecimal(TypeRange typeRange)
        {
            List<decimal> particular = new List<decimal>();
            if (typeRange.isInputMinMax == true)
            {
                particular.Add((decimal)typeRange.min);
                particular.Add((decimal)typeRange.max);
                particular.Add((decimal)typeRange.min + 1);
                particular.Add((decimal)typeRange.max - 1);
            }
            else
            {
                particular.Add(0);
                particular.Add(-1);
                particular.Add(1);
                particular.Add(decimal.MinValue);
                particular.Add(decimal.MaxValue);
                particular.Add(decimal.MinValue + 1);
                particular.Add(decimal.MaxValue - 1);
            }
            int randomNum = rnd.Next(4);
            if (randomNum == 0)
            {
                int randomIndex = rnd.Next(particular.Count);
                return particular[randomIndex];
            }
            else
            {
                if (typeRange.isInputMinMax == true)
                {
                    return (decimal)(typeRange.min + (rnd.NextDouble() * (typeRange.max - typeRange.min)));
                }
                else
                {
                    return (decimal)((double)decimal.MinValue +(rnd.NextDouble() * ((double)decimal.MaxValue - (double)decimal.MinValue)));
                }
            }
        }
    
        private char GenerateChar()
        {
            int c = rnd.Next(122);
            while (c < 48 || (c > 57 && c < 65) || (c > 90 && c < 97))
            {
                c = rnd.Next(122);
            }
            return (char)c;
        }
        
        private bool GenerateBool()
        {
            int randomNum = rnd.Next(2);
            if (randomNum == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }                
 
        private string GenerateString(TypeRange typeRange)
        {
            int length = 0;
            if (typeRange.isInputMinMax == true)
            {
                length = rnd.Next((int)typeRange.max + 1);
            }
            else
            {
                length = rnd.Next(51);
            }
            StringBuilder s = new StringBuilder(length);
            for (int i = 0; i < length; i++)
            {
                s.Append(GenerateChar());
            }
            return s.ToString();
        }
      
        private int GenerateEnum(TypeRange typeRange)
        {
            if (typeRange.isInputMinMax == true)
            {
                return rnd.Next((int)typeRange.min, (int)typeRange.max + 1);
            }
            else
            {
                return rnd.Next((int)DistanceCalculatorForm.enumMin, (int)DistanceCalculatorForm.enumMax);
            }
        }
  
        private void ReadBeforeMutationResult(string fileName)
        {
            StreamReader sr;
            sr = new StreamReader(fileName);
            string currentLine;
            int testcaseCurrentIndex = 0;
            while (!sr.EndOfStream)
            {
                currentLine = sr.ReadLine();
                currentLine = sr.ReadLine();
                if (currentLine == "pass")
                {
                    foreach (BehaviorMember bm in testCasePool[testcaseCurrentIndex].methodSequences)
                    {
                        if (bm.returnType != "void")
                        {
                            if (bm.returnType == "bool")
                            {
                                bm.returnOracal = sr.ReadLine().ToLower();
                            }
                            else if (bm.returnType == "string")
                            {
                                bm.returnOracal = "\"" + sr.ReadLine() + "\"";
                            }
                            else
                            {
                                bm.returnOracal = sr.ReadLine();
                            }
                        }
                    }
                    testcaseCurrentIndex++;
                }
                else
                {
                    testCasePool.RemoveAt(testcaseCurrentIndex);
                }
            }
            sr.Close();
        }
        private void ReadBeforeMutationResultOneMethod(string fileName)
        {
            
            StreamReader sr;
            sr = new StreamReader(fileName);
            string currentLine;
            int testcaseCurrentIndex = 0;
            for (int i = 0; i < subTestcasePools.Count; i++)
            {
                for (int j = 0; j < subTestcasePools[i].Count; j++)
                {
                    for (int k = 0; k < subTestcasePools[i][j].Count; k++)
                    {
                        currentLine = sr.ReadLine();
                        currentLine = sr.ReadLine();
                      

                        if (currentLine == "pass")
                        {
                            foreach (BehaviorMember bm in subTestcasePools[i][j][k].methodSequences)
                            {
                                if (bm.returnType != "void")
                                {
                                    if (bm.returnType == "bool")
                                    {
                                        bm.returnOracal = sr.ReadLine().ToLower();
                                    }
                                    else if (bm.returnType == "string")
                                    {
                                        bm.returnOracal = "\"" + sr.ReadLine() + "\"";
                                    }
                                    else
                                    {
                                        bm.returnOracal = sr.ReadLine();
                                    }
                                }
                            }
                            testcaseCurrentIndex++;
                        }
                        else
                        {
                          
                            subTestcasePools[i][j].RemoveAt(k);
                            testCasePool.RemoveAt(testcaseCurrentIndex);
                        }
                    }
                }
            }
            sr.Close();
       

        }
        private void ReadBeforeMutationResultAndCalculateTime(string fileName)
        {
            Stopwatch time = new Stopwatch();
            time.Start();
            if (isTestOneMethod()==true)
            {
                ReadBeforeMutationResultOneMethod(fileName);                
            }
            else
            {
                ReadBeforeMutationResult(fileName);
            }
            time.Stop();
            Console.WriteLine("ReadBeforeMutationResult time used：{0}", time.Elapsed);
            consoleSw.WriteLine("ReadBeforeMutationResult time used：{0}", time.Elapsed);
        }
    
        private bool isTestOneMethod()
        {
            if (algorithm == Algorithm.ARTooX_M || algorithm == Algorithm.ARTOO || algorithm == Algorithm.ARTGen || algorithm == Algorithm.UnRand_M)
            {
                return true;
            }
            else if(algorithm == Algorithm.LOPART || algorithm == Algorithm.UnRand)
            {
                return false;
            }
            else
            {
                throw new ArgumentException("Algorithm choose error!");
            }
        }
       
        private void ReadAfterMutationResult(string fileName)
        {
            
            StreamReader sr;
            sr = new StreamReader(fileName);
            string currentLine;
            int testcaseCurrentIndex = 0;
            while (!sr.EndOfStream)
            {
                currentLine = sr.ReadLine();
                currentLine = sr.ReadLine();
                if (currentLine == "pass")
                {
                    testCasePool[testcaseCurrentIndex].isTestcasePass = true;
                                     
                }
                else
                {
                    testCasePool[testcaseCurrentIndex].isTestcasePass = false;
                    testCasePool[testcaseCurrentIndex].bugInWhichMethod = sr.ReadLine();
                }
                testcaseCurrentIndex++;
            }
            sr.Close();
        }
        private void ReadAfterMutationResultAndCalculateTime(string fileName)
        {
            Stopwatch time = new Stopwatch();
            time.Start();
            ReadAfterMutationResult(fileName);
            time.Stop();
            Console.WriteLine("ReadAfterMutationResult time used：{0}", time.Elapsed);
            consoleSw.WriteLine("ReadAfterMutationResult time used：{0}", time.Elapsed);
        }
      
        private List<string> ObjectCreateCodeGenerate(Object ob, Language language)
        {
            List<string> codes = new List<string>();
            string newLine;
            if (language == Language.Csharp)
            {
                newLine = ob.type + " " + ob.name + "=new " + ob.type + "()" + ";";
            }
            else if (language == Language.Cplusplus)
            {
                newLine = ob.type + " " + ob.name + ";";
            }
            else
            {
                newLine = "";
                throw new ArgumentException("Language error!");
            }
            codes.Add(newLine);
            foreach (ObjectMember om in ob.staticSection.embeddedSection)
            {
          
                Object subOb = FindObjectByNameWithobjectsDictionary(om.value);
                if (subOb != null)
                {
                    codes.AddRange(ObjectCreateCodeGenerate(subOb, language));
                }
            }
            foreach (ObjectMember om in ob.staticSection.definitionSection)
            {
                if (om.type.Contains("[") && om.type.Contains("]"))
                {
                    string arrayType = om.type.Split('[')[0];
                    string value = ChangeValue(om);
                    if (language == Language.Cplusplus)
                    {
                        newLine = arrayType + " " + om.name + "[" + om.type.Split('[')[1].Split(']')[0].Replace(",","][") +"]" + "=" + value + ";";
                    }
                    else
                    {
                        if (language == Language.Csharp)
                        {
                            newLine = arrayType + "[";
                            for (int i = 0; i < om.type.Split('[')[1].Split(']')[0].Split(',').Length-1; i++)
                            {
                                newLine += ",";
                            }
                            newLine += "] " + om.name + "=" + value + ";";
                        }
                        else
                        {
                            throw new ArgumentException("Language error!");
                        }
                    }
                    codes.Add(newLine);
                }
                if (om.type.Contains("*"))
                {
                    string type = om.type;
                    type = type.Replace("*", " ");
                    type = type.Trim();
                    if (type.StartsWith("const"))
                    {
                        type = type.Replace("const", " ");
                        type = type.Trim();
                    }
                    newLine = type + " " + om.name + "=" + om.value + ";";
                    codes.Add(newLine);
                }
            }
            newLine = ob.name + "." + ob.type + "Initialize(";
            foreach (ObjectMember om in ob.staticSection.definitionSection)
            {
                if (om.type.Contains("[") && om.type.Contains("]"))
                {
                    newLine += om.name;
                    newLine += ",";
                }
                else if (om.type.Contains("*"))
                {
                    newLine += "&" + om.name;
                    newLine += ",";
                }
                else
                {
                    newLine += om.value;
                    newLine += ",";
                }
            }
            foreach (ObjectMember om in ob.staticSection.embeddedSection)
            {
                if (om.value != "null")
                {
                    newLine += om.value;
                    newLine += ",";
                }
            }
            if (newLine.EndsWith(","))
            {
                newLine = newLine.Remove(newLine.Length - 1);
            }
            newLine += ");";
            codes.Add(newLine);
            return codes;
        }
        
        string ChangeValue(ObjectMember om)
        {
            string[] weiDu = om.type.Split('[')[1].Split(']')[0].Split(',');
            Stack<int> bound = new Stack<int>();
            foreach(string s in weiDu)
            {
                bound.Push(int.Parse(s));
            }
            //string[] currentSet2 = om.value.Split(',');
            Queue<string> currentSet = new Queue<string>();
            foreach (string s in om.value.Split(','))
            {
                currentSet.Enqueue(s);
            }
            while(bound.Count>0)
            {
                int currentBound = bound.Pop();
                Queue<string> newSet = new Queue<string>();
                while(currentSet.Count>0)
                {
                    string newElement = "{";
                    for(int i=0;i<currentBound-1;i++)
                    {
                        newElement += currentSet.Dequeue();
                        newElement += ",";
                    }
                    newElement += currentSet.Dequeue();
                    newElement += "}";
                    newSet.Enqueue(newElement);
                }
                currentSet = newSet;
            }
            return currentSet.Dequeue();
        }

        private Object FindObjectByName(string name)
        {
            foreach (Object ob in objects)
            {
                if (ob.name == name)
                {
                    return ob;
                }
            }
            return null;
        }
  
        private Object FindObjectByNameWithobjectsDictionary(string name)
        {
            Object result;
            if (objectsDictionary.TryGetValue(name, out result))
            {
                return result;
            }
            else
            {
                return null;
            }
        }
       
        private List<string> FunctionCreateCodeGenerate(BehaviorMember bm, string returnParaName, Language language)
        {
            List<string> codes = new List<string>();
            string newLine;
            foreach (ParameterMember pm in bm.parameters)
            {
                if (pm.isBaseType == false)
                {
                    Object ob = FindObjectByNameWithobjectsDictionary(pm.value);
                    codes.AddRange(ObjectCreateCodeGenerate(ob, language));
                }
                if(pm.type.Contains("["))
                {
                    string arrayType=pm.type.Split('[')[0];
                    if(language==Language.Cplusplus)
                    {
                        newLine = arrayType +" "+pm.name +"["+pm.type.Split('[')[1]+"={"+pm.value+"};";
                    }
                    else
                    {
                        if(language==Language.Csharp)
                        {
                            newLine=arrayType +"[] "+pm.name+"={"+pm.value+"};";
                        }
                        else
                        {
                            throw new ArgumentException("Language error!");
                        }
                    }
                    codes.Add(newLine);
                }
            }
            if (bm.returnType == "void")
            {
                newLine = bm.ToString() + "(";
            }
            else
            {
                newLine = bm.returnType + " " + returnParaName + "=" + bm.ToString() + "(";
            }
            foreach (ParameterMember pm in bm.parameters)
            {
                if (pm.type.Contains("*"))
                {
                    string type = pm.type;
                    type = type.Replace("*", " ");
                    type = type.Trim();
                    if (type.StartsWith("const"))
                    {
                        type = type.Replace("const", " ");
                        type = type.Trim();
                    }
                    newLine += "new " + type + "(" + pm.value + ")";
                    newLine += ",";
                }
                else
                {
                    if (pm.type.Contains("["))
                    {
                        newLine += pm.name;
                        newLine += ",";
                    }
                    else
                    {
                        newLine += pm.value;
                        newLine += ",";
                    }
                }
            }
            if (newLine.EndsWith(","))
            {
                newLine = newLine.Remove(newLine.Length - 1);
            }
            newLine += ");";
            codes.Add(newLine);
            return codes;
        }
        
        private void GenerateCodeBeforeMutationCsharp2(string version)
        {
            string result = version + "CodeBeforeMutation.cs";
            StreamWriter sw2 = new StreamWriter(result, false);            
            sw2.WriteLine("using " + testProgramBeforeNameSpace + ";");            
            sw2.WriteLine("using System;");
            sw2.WriteLine("using System.IO;");
            sw2.WriteLine("using Microsoft.VisualStudio.TestTools.UnitTesting;");
            sw2.WriteLine("namespace ARTOO_Test1");
            sw2.WriteLine("{");
            sw2.WriteLine("	public class UnitTest1");
            sw2.WriteLine("	{");
            sw2.WriteLine("		private static StreamWriter sw=new StreamWriter(\"" + version + "CodeBeforeMutation_result.txt\",false);");
            sw2.WriteLine("		public static void Main()");
            sw2.WriteLine("		{");
            for (int i = 0; i < testCasePool.Count; i++)
            {
                sw2.WriteLine("			TestMethod" + (i + 1).ToString() + "();");
            }
            sw2.WriteLine("		}");
            for (int i = 0; i < testCasePool.Count; i++)
            {
                sw2.WriteLine("		public static void TestMethod" + (i + 1).ToString() + "()");
                sw2.WriteLine("		{");
                sw2.WriteLine("			sw.WriteLine(\"" + testCasePool[i].name + "_result\");");
                sw2.WriteLine("			try");
                sw2.WriteLine("			{");
                List<string> codes = new List<string>();
                foreach (Object ob in testCasePool[i].Objects)
                {
                    codes = ObjectCreateCodeGenerate(ob, Language.Csharp);
                    foreach (string s in codes)
                    {
                        sw2.WriteLine("				" + s);
                    }
                }
                int returnParaNum = 0;
                List<bool> isStringFlag = new List<bool>(); ;
                isStringFlag.Add(false);
                foreach (BehaviorMember bm in testCasePool[i].methodSequences)
                {
                    if (bm.returnType != "void")
                    {
                        returnParaNum++;
                        if (bm.returnType == "string")
                        {
                            isStringFlag.Add(true);
                        }
                        else
                        {
                            isStringFlag.Add(false);
                        }
                    }
                    codes = FunctionCreateCodeGenerate(bm, "result" + returnParaNum.ToString(),Language.Csharp);
                    foreach (string s in codes)
                    {
                        sw2.WriteLine("				" + s);
                    }
                }
                sw2.WriteLine("				sw.WriteLine(\"pass\");");
                for (int j = 1; j <= returnParaNum; j++)
                {
                    if (isStringFlag[j] == true)
                    {
                        sw2.WriteLine("				sw.WriteLine(result" + j + ");");
                    }
                    else
                    {
                        sw2.WriteLine("				sw.WriteLine(result" + j + ".ToString());");
                    }
                }
                sw2.WriteLine("			}");
                sw2.WriteLine("			catch");
                sw2.WriteLine("			{");
                sw2.WriteLine("				sw.WriteLine(\"fail\");");
                sw2.WriteLine("			}");
                sw2.WriteLine("			sw.Flush();");
                sw2.WriteLine("		}");
            }
            sw2.WriteLine("	}");
            sw2.WriteLine("}");
            sw2.Close();
        }
        private void GenerateCodeBeforeMutationCsharp(string version)
        {
            string result = version + "CodeBeforeMutation.cs";
            StreamWriter sw2 = new StreamWriter(result, false);
            sw2.WriteLine("using " + testProgramBeforeNameSpace + ";");
            sw2.WriteLine("using System;");
            sw2.WriteLine("using System.IO;");
            sw2.WriteLine("using System.Diagnostics;");
            sw2.WriteLine("using Microsoft.VisualStudio.TestTools.UnitTesting;");
            sw2.WriteLine("namespace ARTOO_Test1");
            sw2.WriteLine("{");
            sw2.WriteLine("	public class UnitTest1");
            sw2.WriteLine("	{");
            sw2.WriteLine("		private static StreamWriter sw=new StreamWriter(\"" + version + "CodeBeforeMutation_result.txt\",false);");
            sw2.WriteLine("		public static void Main()");
            sw2.WriteLine("		{");
            sw2.WriteLine("			Stopwatch time = new Stopwatch();");
            sw2.WriteLine("			time.Start();");

            for (int i = 0; i < testCasePool.Count; i++)
            {
                sw2.WriteLine("			TestMethod" + (i + 1).ToString() + "();");
            }
            sw2.WriteLine("           Console.WriteLine(\"All Over\");");
            sw2.WriteLine("			time.Stop();");
            sw2.WriteLine("           Console.WriteLine(\"All time = {0}\",time.Elapsed);");
            sw2.WriteLine("           Console.WriteLine(\"CodeBeforeMutation.cs\");");
            sw2.WriteLine("           sw.Close();");
            sw2.WriteLine("		}");
            for (int i = 0; i < testCasePool.Count; i++)
            {
                sw2.WriteLine("		public static void TestMethod" + (i + 1).ToString() + "()");
                sw2.WriteLine("		{");
                sw2.WriteLine("			sw.WriteLine(\"" + testCasePool[i].name + "_result\");");
                sw2.WriteLine("			Console.WriteLine(\"" + testCasePool[i].name + "_result\");");
                sw2.WriteLine("			try");
                sw2.WriteLine("			{");
                List<string> codes = new List<string>();
                foreach (Object ob in testCasePool[i].Objects)
                {
                    codes = ObjectCreateCodeGenerate(ob, Language.Csharp);
                    foreach (string s in codes)
                    {
                        sw2.WriteLine("				" + s);
                    }
                }
                int returnParaNum = 0;
                List<bool> isStringFlag = new List<bool>();
                isStringFlag.Add(false);
                foreach (BehaviorMember bm in testCasePool[i].methodSequences)
                {
                    if (bm.returnType != "void")
                    {
                        returnParaNum++;
                        if (bm.returnType == "string")
                        {
                            isStringFlag.Add(true);
                        }
                        else
                        {
                            isStringFlag.Add(false);
                        }
                    }
                    codes = FunctionCreateCodeGenerate(bm, "result" + returnParaNum.ToString(), Language.Csharp);
                    foreach (string s in codes)
                    {
                        sw2.WriteLine("				" + s);
                    }
                }
                sw2.WriteLine("				sw.WriteLine(\"pass\");");
                for (int j = 1; j <= returnParaNum; j++)
                {
                    if (isStringFlag[j] == true)
                    {
                        sw2.WriteLine("				sw.WriteLine(result" + j + ");");
                    }
                    else
                    {
                        sw2.WriteLine("				sw.WriteLine(result" + j + ".ToString());");
                    }
                }
                sw2.WriteLine("			}");
                sw2.WriteLine("			catch");
                sw2.WriteLine("			{");
                sw2.WriteLine("				sw.WriteLine(\"fail\");");
                sw2.WriteLine("			}");
                sw2.WriteLine("			sw.Flush();");
                sw2.WriteLine("		}");
            }
            //sw2.WriteLine("	sw.Close();");
            sw2.WriteLine("	}");
            sw2.WriteLine("}");
            sw2.Close();
        }
      
        private void GenerateCodeAfterMutationCsharp2(string version)
        {
            string result = version + "CodeAfterMutation.cs";
            StreamWriter sw2 = new StreamWriter(result, false);
            sw2.WriteLine("using " + testProgramAfterNameSpace + ";");
            sw2.WriteLine("using System;");
            sw2.WriteLine("using System.IO;");
            sw2.WriteLine("using Microsoft.VisualStudio.TestTools.UnitTesting;");
            sw2.WriteLine("namespace ARTOO_Test1");
            sw2.WriteLine("{");
            sw2.WriteLine("	public class UnitTest1");
            sw2.WriteLine("	{");
            sw2.WriteLine("		private static StreamWriter sw=new StreamWriter(\"" + version + "CodeAfterMutation_result.txt\",false);");
            sw2.WriteLine("		public static void Main()");
            sw2.WriteLine("		{");
            for (int i = 0; i < testCasePool.Count; i++)
            {
                sw2.WriteLine("			TestMethod" + (i + 1).ToString() + "();");
            }
            sw2.WriteLine("		}");
            for (int i = 0; i < testCasePool.Count; i++)
            {
                sw2.WriteLine("		public static void TestMethod" + (i + 1).ToString() + "()");
                sw2.WriteLine("		{");
                sw2.WriteLine("			sw.WriteLine(\"" + testCasePool[i].name + "_result\");");                
                List<string> codes = new List<string>();
                foreach (Object ob in testCasePool[i].Objects)
                {
                    codes = ObjectCreateCodeGenerate(ob, Language.Csharp);
                    foreach (string s in codes)
                    {
                        sw2.WriteLine("			" + s);
                    }
                }
                int returnParaNum = 0;                
                foreach (BehaviorMember bm in testCasePool[i].methodSequences)
                {
                    if (bm.returnType != "void")
                    {
                        returnParaNum++;                        
                    }
                    codes = FunctionCreateCodeGenerate(bm, "result" + returnParaNum.ToString(), Language.Csharp);
                    sw2.WriteLine("			try");
                    sw2.WriteLine("			{");
                    foreach (string s in codes)
                    {
                        sw2.WriteLine("				" + s);
                    }
                    if (bm.returnType != "void")
                    {
                        if (bm.returnType == "char")
                        {
                            sw2.WriteLine("				Assert::AreEqual(result" + returnParaNum + ",(char)" + bm.returnOracal + ");");
                        }
                        else
                        {
                            sw2.WriteLine("				Assert.AreEqual(result" + returnParaNum + "," + bm.returnOracal + ");");
                        }
                    }
                    sw2.WriteLine("			}");
                    sw2.WriteLine("			catch");
                    sw2.WriteLine("			{");
                    sw2.WriteLine("				sw.WriteLine(\"fail\");");
                    sw2.WriteLine("				sw.WriteLine(\"" + bm.defineInWhichClass + "." + bm.name + "\");");
                    sw2.WriteLine("				sw.Flush();");
                    sw2.WriteLine("				return;");
                    sw2.WriteLine("			}");
                }
                sw2.WriteLine("			sw.WriteLine(\"pass\");");
                sw2.WriteLine("			sw.Flush();");
                sw2.WriteLine("		}");
            }
            sw2.WriteLine("	sw.Close();");
            sw2.WriteLine("	}");
            sw2.WriteLine("}");
            sw2.Close();
        }
        private void GenerateCodeAfterMutationCsharp(string version)
        {
            string result = version + "CodeAfterMutation.cs";
            StreamWriter sw2 = new StreamWriter(result, false);
            sw2.WriteLine("using " + testProgramAfterNameSpace + ";");
            sw2.WriteLine("using System;");
            sw2.WriteLine("using System.IO;");
            sw2.WriteLine("using System.Diagnostics;");
            sw2.WriteLine("using Microsoft.VisualStudio.TestTools.UnitTesting;");
            sw2.WriteLine("namespace ARTOO_Test1");
            sw2.WriteLine("{");
            sw2.WriteLine("	public class UnitTest1");
            sw2.WriteLine("	{");
            sw2.WriteLine("		private static StreamWriter sw=new StreamWriter(\"" + version + "CodeAfterMutation_result.txt\",false);");
            sw2.WriteLine("		public static void Main()");
            sw2.WriteLine("		{");
            sw2.WriteLine("			Stopwatch time = new Stopwatch();");
            sw2.WriteLine("			time.Start();");
            for (int i = 0; i < testCasePool.Count; i++)
            {
                sw2.WriteLine("			TestMethod" + (i + 1).ToString() + "();");
            }
            sw2.WriteLine("           Console.WriteLine(\"All Over\");");
            sw2.WriteLine("			time.Stop();");
            sw2.WriteLine("           Console.WriteLine(\"All time = {0}\",time.Elapsed);");
            sw2.WriteLine("           Console.WriteLine(\"CodeBeforeMutation.cs\");");
            sw2.WriteLine("           sw.Close();");
            sw2.WriteLine("		}");
            for (int i = 0; i < testCasePool.Count; i++)
            {
                sw2.WriteLine("		public static void TestMethod" + (i + 1).ToString() + "()");
                sw2.WriteLine("		{");
                sw2.WriteLine("			sw.WriteLine(\"" + testCasePool[i].name + "_result\");");
                sw2.WriteLine("			Console.WriteLine(\"" + testCasePool[i].name + "_resultAfter\");");
                List<string> codes = new List<string>();
                foreach (Object ob in testCasePool[i].Objects)
                {
                    codes = ObjectCreateCodeGenerate(ob, Language.Csharp);
                    foreach (string s in codes)
                    {
                        sw2.WriteLine("			" + s);
                    }
                }
                int returnParaNum = 0;
                foreach (BehaviorMember bm in testCasePool[i].methodSequences)
                {
                    if (bm.returnType != "void")
                    {
                        returnParaNum++;
                    }
                    codes = FunctionCreateCodeGenerate(bm, "result" + returnParaNum.ToString(), Language.Csharp);
                    sw2.WriteLine("			try");
                    sw2.WriteLine("			{");
                    foreach (string s in codes)
                    {
                        sw2.WriteLine("				" + s);
                    }
                    if (bm.returnType != "void")
                    {
                        if (bm.returnType == "char")
                        {
                            sw2.WriteLine("				Assert::AreEqual(result" + returnParaNum + ",(char)" + bm.returnOracal + ");");
                        }
                        else
                        {
                            sw2.WriteLine("				Assert.AreEqual(result" + returnParaNum + "," + bm.returnOracal + ");");
                        }
                    }
                    sw2.WriteLine("			}");
                    sw2.WriteLine("			catch");
                    sw2.WriteLine("			{");
                    sw2.WriteLine("				sw.WriteLine(\"fail\");");
                    sw2.WriteLine("				sw.WriteLine(\"" + bm.defineInWhichClass + "." + bm.name + "\");");
                    sw2.WriteLine("				sw.Flush();");
                    sw2.WriteLine("				return;");
                    sw2.WriteLine("			}");
                }
                sw2.WriteLine("			sw.WriteLine(\"pass\");");
                sw2.WriteLine("			sw.Flush();");
                sw2.WriteLine("		}");
            }
            sw2.WriteLine("	}");
            sw2.WriteLine("}");
            sw2.Close();
        }
       
        private void GenerateCodeBeforeMutationCplusplus2(string version)
        {
            string result = version + "CodeBeforeMutation.cpp";
            StreamWriter sw2 = new StreamWriter(result, false);
            sw2.WriteLine("#include \"" + testProgramBefore + "\"");
            sw2.WriteLine("using namespace System;");
            sw2.WriteLine("using namespace System::Text;");
            sw2.WriteLine("using namespace System::Collections::Generic;");
            sw2.WriteLine("using namespace	Microsoft::VisualStudio::TestTools::UnitTesting;");
            sw2.WriteLine("using namespace System::IO;");            
            sw2.WriteLine("	public ref class UnitTest1");
            sw2.WriteLine("	{");
            sw2.WriteLine("	public:");
            sw2.WriteLine("		static StreamWriter^ sw=gcnew StreamWriter(\"" + version + "CodeBeforeMutation_result.txt\",true);");
            for (int i = 0; i < testCasePool.Count; i++)
            {
                sw2.WriteLine("		static void TestMethod" + (i + 1).ToString() + "()");
                sw2.WriteLine("		{");
                sw2.WriteLine("			sw->WriteLine(\"" + testCasePool[i].name + "_result\");");
                sw2.WriteLine("			try");
                sw2.WriteLine("			{");
                List<string> codes = new List<string>();
                foreach (Object ob in testCasePool[i].Objects)
                {
                    codes = ObjectCreateCodeGenerate(ob,Language.Cplusplus);
                    foreach (string s in codes)
                    {
                        sw2.WriteLine("				" + s);
                    }
                }
                int returnParaNum = 0;
                List<bool> isStringFlag = new List<bool>(); 
                isStringFlag.Add(false);
                foreach (BehaviorMember bm in testCasePool[i].methodSequences)
                {
                    if (bm.returnType != "void")
                    {
                        returnParaNum++;
                        if (bm.returnType == "string")
                        {
                            isStringFlag.Add(true);
                        }
                        else
                        {
                            isStringFlag.Add(false);
                        }
                    }
                    codes = FunctionCreateCodeGenerate(bm, "result" + returnParaNum.ToString(), Language.Cplusplus);
                    foreach (string s in codes)
                    {
                        sw2.WriteLine("				" + s);
                    }
                }
                sw2.WriteLine("				sw->WriteLine(\"pass\");");
                for (int j = 1; j <= returnParaNum; j++)
                {
                    if (isStringFlag[j] == true)
                    {
                        sw2.WriteLine("				sw->WriteLine(gcnew String(result" + j + ".c_str()));");
                    }
                    else
                    {
                        sw2.WriteLine("				sw->WriteLine(result" + j + ".ToString());");
                    }
                }
                sw2.WriteLine("			}");
                sw2.WriteLine("			catch(...)");
                sw2.WriteLine("			{");
                sw2.WriteLine("				sw->WriteLine(\"fail\");");
                sw2.WriteLine("			}");
                sw2.WriteLine("			sw->Flush();");
                sw2.WriteLine("		};");
            }
            sw2.WriteLine("	};");
            sw2.WriteLine("		void main()");
            sw2.WriteLine("		{");
            for (int i = 0; i < testCasePool.Count; i++)
            {
                sw2.WriteLine("			UnitTest1::TestMethod" + (i + 1).ToString() + "();");
            }
            sw2.WriteLine("		}");
            sw2.Close();
        }
        private void GenerateCodeBeforeMutationCplusplus(string version)
        {
            string result = version + "CodeBeforeMutation.cpp";
            StreamWriter sw2 = new StreamWriter(result, false);

            //sw2.WriteLine("#include \"Calendar_Before.cpp\"");
            //sw2.WriteLine("#include \"SATM_Before.cpp\"");
            sw2.WriteLine("#include \"Net_before.cpp\"");
            //sw2.WriteLine("#include \"foudation_before.cpp\"");
            //sw2.WriteLine("#include \"" + testProgramBefore + "\"");
            sw2.WriteLine("#include<fstream>");
            sw2.WriteLine("using namespace std;");

            sw2.WriteLine("	class UnitTest1");
            sw2.WriteLine("	{");
            sw2.WriteLine("	public:");
            sw2.WriteLine("		 ofstream outfile;");
            sw2.WriteLine("		 UnitTest1(string s)");
            sw2.WriteLine("		 {");
            sw2.WriteLine("		     outfile.open(s.c_str(), ofstream::out);");
            sw2.WriteLine("		 }");
            sw2.WriteLine("		 void Close()");
            sw2.WriteLine("		 {");
            sw2.WriteLine("		     outfile.close();");
            sw2.WriteLine("		 }");
            for (int i = 0; i < testCasePool.Count; i++)
            {
                sw2.WriteLine("	    void TestMethod" + (i + 1).ToString() + "()");
                sw2.WriteLine("		{");
                sw2.WriteLine("			outfile<<\"" + testCasePool[i].name + "_result\"<<endl;");
                //sw2.WriteLine("			cout<<\"" + testCasePool[i].name + "_result\"<<endl;");
                sw2.WriteLine("			try");
                sw2.WriteLine("			{");
                List<string> codes = new List<string>();
                foreach (Object ob in testCasePool[i].Objects)
                {
                    codes = ObjectCreateCodeGenerate(ob, Language.Cplusplus);
                    foreach (string s in codes)
                    {
                        sw2.WriteLine("				" + s);
                    }
                }
                int returnParaNum = 0;
                List<bool> isStringFlag = new List<bool>(); 
                isStringFlag.Add(false);
                foreach (BehaviorMember bm in testCasePool[i].methodSequences)
                {
                    if (bm.returnType != "void")
                    {
                        returnParaNum++;
                        if (bm.returnType == "string")
                        {
                            isStringFlag.Add(true);
                        }
                        else
                        {
                            isStringFlag.Add(false);
                        }
                    }
                    codes = FunctionCreateCodeGenerate(bm, "result" + returnParaNum.ToString(), Language.Cplusplus);
                    foreach (string s in codes)
                    {
                        sw2.WriteLine("				" + s);
                    }
                }
                sw2.WriteLine("				outfile<<\"pass\"<<endl;");
                //sw2.WriteLine("				cout<<\"pass\"<<endl;");
                for (int j = 1; j <= returnParaNum; j++)
                {
                    if (isStringFlag[j] == true)
                    {
                        sw2.WriteLine("				outfile<<result" + j + ".c_str()<<endl;");
                    }
                    else
                    {
                        sw2.WriteLine("				outfile<<result" + j + "<<endl;");
                    }
                }
                sw2.WriteLine("			}");
                sw2.WriteLine("			catch(...)");
                sw2.WriteLine("			{");
                sw2.WriteLine("				outfile<<\"fail\"<<endl;");
                sw2.WriteLine("			}");
                // sw2.WriteLine("			sw->Flush();");
                sw2.WriteLine("		}");
            }
            sw2.WriteLine("	};");
            sw2.WriteLine("		void main()");
            sw2.WriteLine("		{");
            sw2.WriteLine("			string s = \"" + version + "CodeBeforeMutation_result.txt\";");
            sw2.WriteLine("			UnitTest1 t(s);");

            for (int i = 0; i < testCasePool.Count; i++)
            {
                sw2.WriteLine("			t.TestMethod" + (i + 1).ToString() + "();");
            }
            sw2.WriteLine("			t.Close();");
            sw2.WriteLine("		}");

            sw2.Close();
        }

        private void GenerateCodeAfterMutationCplusplus2(string version)
        {
            string result = version + "CodeAfterMutation.cpp";
            StreamWriter sw2 = new StreamWriter(result, false);
            sw2.WriteLine("#include \"" + testProgramAfter + "\"");
            sw2.WriteLine("using namespace System;");
            sw2.WriteLine("using namespace System::Text;");
            sw2.WriteLine("using namespace System::Collections::Generic;");
            sw2.WriteLine("using namespace	Microsoft::VisualStudio::TestTools::UnitTesting;");
            sw2.WriteLine("using namespace System::IO;");
            sw2.WriteLine("	public ref class UnitTest1");
            sw2.WriteLine("	{");
            sw2.WriteLine("	public:");
            sw2.WriteLine("		static StreamWriter^ sw=gcnew StreamWriter(\"" + version + "CodeAfterMutation_result.txt\",true);");            
            for (int i = 0; i < testCasePool.Count; i++)
            {
                sw2.WriteLine("		static void TestMethod" + (i + 1).ToString() + "()");
                sw2.WriteLine("		{");
                sw2.WriteLine("			sw->WriteLine(\"" + testCasePool[i].name + "_result\");");
               
                List<string> codes = new List<string>();
                foreach (Object ob in testCasePool[i].Objects)
                {
                    codes = ObjectCreateCodeGenerate(ob, Language.Cplusplus);
                    foreach (string s in codes)
                    {
                        sw2.WriteLine("			" + s);
                    }
                }
                int returnParaNum = 0;
                foreach (BehaviorMember bm in testCasePool[i].methodSequences)
                {
                    if (bm.returnType != "void")
                    {
                        returnParaNum++;
                    }
                    codes = FunctionCreateCodeGenerate(bm, "result" + returnParaNum.ToString(), Language.Cplusplus);
                    sw2.WriteLine("			try");
                    sw2.WriteLine("			{");
                    foreach (string s in codes)
                    {
                        sw2.WriteLine("				" + s);
                    }
                    if (bm.returnType != "void")
                    {
                        if (bm.returnType == "string")
                        {
                            sw2.WriteLine("				Assert::AreEqual(gcnew String(result" + returnParaNum + ".c_str())," + bm.returnOracal + ");");
                        }
                        else
                        {
                            if (bm.returnType == "char")
                            {
                                sw2.WriteLine("				Assert::AreEqual(result" + returnParaNum + ",(char)" + bm.returnOracal + ");");
                            }
                            else
                            {
                                sw2.WriteLine("				Assert::AreEqual(result" + returnParaNum + "," + bm.returnOracal + ");");
                            }
                        }
                    }
                    sw2.WriteLine("			}");
                    sw2.WriteLine("			catch(...)");
                    sw2.WriteLine("			{");
                    sw2.WriteLine("				sw->WriteLine(\"fail\");");
                    sw2.WriteLine("				sw->WriteLine(\""+bm.defineInWhichClass+"."+bm.name+"\");");
                    sw2.WriteLine("				sw->Flush();");
                    sw2.WriteLine("				return;");
                    sw2.WriteLine("			}");
                }
                sw2.WriteLine("			sw->WriteLine(\"pass\");");
                sw2.WriteLine("			sw->Flush();");
                sw2.WriteLine("		};");
            }
            sw2.WriteLine("	};");
            sw2.WriteLine("		void main()");
            sw2.WriteLine("		{");
            for (int i = 0; i < testCasePool.Count; i++)
            {
                sw2.WriteLine("			UnitTest1::TestMethod" + (i + 1).ToString() + "();");
            }
            sw2.WriteLine("		}");
            sw2.Close();
        }
        private void GenerateCodeAfterMutationCplusplus(string version)
        {
            string result = version + "CodeAfterMutation.cpp";
            StreamWriter sw2 = new StreamWriter(result, false);
     
            sw2.WriteLine("#include \"Net_after.cpp\"");
            sw2.WriteLine("#include<fstream>");
            sw2.WriteLine("#include<stdexcept>");
            sw2.WriteLine("using namespace std;");
            sw2.WriteLine("	 class UnitTest1");
            sw2.WriteLine("	{");
            sw2.WriteLine("	public:");
            sw2.WriteLine("		 ofstream outfile;");
            sw2.WriteLine("		 UnitTest1(string s)");
            sw2.WriteLine("		 {");
            sw2.WriteLine("		     outfile.open(s.c_str(), ofstream::out);");
            sw2.WriteLine("		 }");
            sw2.WriteLine("		 void Close()");
            sw2.WriteLine("		 {");
            sw2.WriteLine("		     outfile.close();");
            sw2.WriteLine("		 }");
            //Console.WriteLine("zhulili:{0}", testCasePool.Count);
            for (int i = 0; i < testCasePool.Count; i++)
            {
                sw2.WriteLine("		void TestMethod" + (i + 1).ToString() + "()");
                sw2.WriteLine("		{");
                sw2.WriteLine("			outfile<<\"" + testCasePool[i].name + "_result\"<<endl;");

                List<string> codes = new List<string>();
                foreach (Object ob in testCasePool[i].Objects)
                {
                    codes = ObjectCreateCodeGenerate(ob, Language.Cplusplus);
                    foreach (string s in codes)
                    {
                        sw2.WriteLine("			" + s);
                    }
                }
                int returnParaNum = 0;
                foreach (BehaviorMember bm in testCasePool[i].methodSequences)
                {
                    if (bm.returnType != "void")
                    {
                        returnParaNum++;
                    }
                    codes = FunctionCreateCodeGenerate(bm, "result" + returnParaNum.ToString(), Language.Cplusplus);
                    sw2.WriteLine("			try");
                    sw2.WriteLine("			{");
                    foreach (string s in codes)
                    {
                        sw2.WriteLine("				" + s);
                    }
                    if (bm.returnType != "void")
                    {
                        if (bm.returnType == "string")
                        {
                            sw2.WriteLine("				if(result" + returnParaNum + "!=" + bm.returnOracal + ")");
                            sw2.WriteLine("				      throw runtime_error(\"Two values are not equal!\");");
                        }
                        else
                        {
                            if (bm.returnType == "char")
                            {
                                sw2.WriteLine("				if(result" + returnParaNum + "!='" + bm.returnOracal + "')");
                                sw2.WriteLine("				      throw runtime_error(\"Two values are not equal!\");");
                            }
                            else
                            {
                                sw2.WriteLine("				if(result" + returnParaNum + "!=" + bm.returnOracal + ")");
                                sw2.WriteLine("				      throw runtime_error(\"Two values are not equal!\");");
                            }
                        }
                    }
                    sw2.WriteLine("			}");
                    sw2.WriteLine("			catch(...)");
                    sw2.WriteLine("			{");
                    sw2.WriteLine("				outfile<<\"fail\"<<endl;");
                    sw2.WriteLine("				outfile<<\"" + bm.defineInWhichClass + "." + bm.name + "\"<<endl;");

                    sw2.WriteLine("				return;");
                    sw2.WriteLine("			}");
                }
                sw2.WriteLine("			outfile<<\"pass\"<<endl;");
                sw2.WriteLine("		};");
            }
            sw2.WriteLine("	};");
            sw2.WriteLine("		void main()");
            sw2.WriteLine("		{");
            sw2.WriteLine("			string s = \"" + version + "CodeAfterMutation_result.txt\";");
            sw2.WriteLine("			UnitTest1 t(s);");

            for (int i = 0; i < testCasePool.Count; i++)
            {
                sw2.WriteLine("			t.TestMethod" + (i + 1).ToString() + "();");
            }
            sw2.WriteLine("			t.Close();");
            sw2.WriteLine("		}");

            sw2.Close();
        }
        private void GenerateCodeBeforeMutationAndCalculateTime(string version, Language language)
        {
            Stopwatch time = new Stopwatch();
            time.Start();
            if (language == Language.Cplusplus)
            {
                GenerateCodeBeforeMutationCplusplus(version);
            }
            else if (language == Language.Csharp)
            {
                GenerateCodeBeforeMutationCsharp(version);
            }
            else
            {
                throw new ArgumentException("Language error!");
            }
            time.Stop();
            Console.WriteLine("GenerateCodeBeforeMutation time used：{0}", time.Elapsed);
            consoleSw.WriteLine("GenerateCodeBeforeMutation time used：{0}", time.Elapsed);
            Console.WriteLine("Execute the file: \"" + version + "CodeBeforeMutation.cpp\"" + "and put the result file to current location,when completed,press \"Enter\" to continue");
            consoleSw.WriteLine("Execute the file: \"" + version + "CodeBeforeMutation.cpp\"" + "and put the result file to current location,when completed,press \"Enter\" to continue");
        }
        private void GenerateCodeAfterMutationAndCalculateTime(string version, Language language)
        {
            Stopwatch time = new Stopwatch();
            time.Start();
            if (language == Language.Cplusplus)
            {
                GenerateCodeAfterMutationCplusplus(version);
            }
            else if (language == Language.Csharp)
            {
                GenerateCodeAfterMutationCsharp(version);
            }
            else
            {
                throw new ArgumentException("Language error!");
            }
            time.Stop();
            Console.WriteLine("GenerateCodeAfterMutation time used：{0}", time.Elapsed);
            consoleSw.WriteLine("GenerateCodeAfterMutation time used：{0}", time.Elapsed);
            consoleResult.T2 = time.Elapsed.TotalSeconds;
            Console.WriteLine("Execute the file: \"" + version + "CodeAfterMutation.cpp\"" + "and put the result file to current location,when completed,press \"Enter\" to continue");
            consoleSw.WriteLine("Execute the file: \"" + version + "CodeAfterMutation.cpp\"" + "and put the result file to current location,when completed,press \"Enter\" to continue");            
        }
        private void WaitUserPressEnter()
        {
            ConsoleKeyInfo cki = Console.ReadKey();
            while (cki.Key != ConsoleKey.Enter)
            {
                cki = Console.ReadKey();
            }
        }
        private void ExcuteCodeCsharp( string setName,string reference,string fileName)
        {
            System.Diagnostics.Process pro = new System.Diagnostics.Process();
            pro.StartInfo.UseShellExecute = false;
            string arg = @"/out:" + setName + " /r:\""+CompilerPath.UnitTestFramework+"\" /r:" + reference + " " + fileName;
            //Console.WriteLine();

            //Console.WriteLine(arg);
            //Console.WriteLine();
            pro.StartInfo.FileName = CompilerPath.csc;
            pro.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            pro.StartInfo.Arguments = arg;
            pro.StartInfo.RedirectStandardOutput = true;
            pro.Start();
            pro.WaitForExit();
            StreamWriter sw3 = new StreamWriter("result2.txt", false);
            string s = pro.StandardOutput.ReadToEnd();
            sw3.WriteLine(s);
            //Console.WriteLine(s);
            sw3.Flush();
            sw3.Close();
            pro = new System.Diagnostics.Process();
            pro.StartInfo.FileName = setName;
           // pro.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            //Console.WriteLine(setName);
            //Console.WriteLine();
            pro.Start();
            //pro.WaitForExit();
            if (!pro.WaitForExit(40000))
                pro.Kill();
           
        }
        private void ExcuteCodeCplusplus( string fileName)
        {
            File.Copy(CompilerPath.vcvars32, "clpusclpus.bat",true);
            StreamWriter sw = new StreamWriter("clpusclpus.bat",true);
            sw.WriteLine("\"" + CompilerPath.cl + "\"" + " /bigobj /clr /FU \"" + CompilerPath.UnitTestFramework + "\" " + fileName);
            sw.Close();

            Process pro = new System.Diagnostics.Process();
            pro.StartInfo.FileName = "clpusclpus.bat";
            pro.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            pro.Start();
            pro.WaitForExit();
            //Console.WriteLine("File name is {0}", fileName);
            pro = new System.Diagnostics.Process();
            pro.StartInfo.FileName = fileName.Split('.')[0]+".exe";
            pro.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            pro.Start();
            pro.WaitForExit();
            File.Delete("clpusclpus.bat");
            File.Delete(fileName.Split('.')[0] + ".obj");
            File.Delete(fileName.Split('.')[0] + ".exe");
        }
        
        private void ReadParametersFromXml(string xmlFilePath)
        {
            XmlDocument myXmlDoc = new XmlDocument();
            myXmlDoc.Load(xmlFilePath);
            XmlNode rootNode = myXmlDoc.SelectSingleNode("Parameters");

            string ParameterValue = rootNode.SelectSingleNode("language").InnerText;
            if(ParameterValue=="c#")
            {
                language = Language.Csharp;
            }
            else if(ParameterValue=="c++")
            {
                language = Language.Cplusplus;
            }

            switch(rootNode.SelectSingleNode("algorithm").InnerText.Split(',')[currentAlgorithmIndex])
            {
                case "LOPART":
                    algorithm = Algorithm.LOPART;
                    break;
                case "UnRand":
                    algorithm = Algorithm.UnRand;
                    break;
                case "ARTooX_M":
                    algorithm = Algorithm.ARTooX_M;
                    break;
                case "ARTOO":
                    algorithm = Algorithm.ARTOO;
                    break;
                case "ARTGen":
                    algorithm = Algorithm.ARTGen;
                    break;
                case "UnRand_M":
                    algorithm = Algorithm.UnRand_M;
                    break;
                default :
                    throw new ArgumentException("algorithm invalid:" + rootNode.SelectSingleNode("algorithm").InnerText);                    
                    
            }

            forgetParameter = int.Parse(rootNode.SelectSingleNode("forgetParameter").InnerText);
            ARTGenDiverseMaxNum = int.Parse(rootNode.SelectSingleNode("ARTGenDiverseNum").InnerText);
            isTestcaseNumFixed = bool.Parse(rootNode.SelectSingleNode("isTestcaseNumFixed").InnerText);
            isRunFSCS_ART_FiexdTime = bool.Parse(rootNode.SelectSingleNode("isRunFSCS_ART_FiexdTime").InnerText);
            string[] result = rootNode.SelectSingleNode("FixedTestcaseNums").InnerText.Split(',');
            FixedTestcaseNums = new int[result.Length];
            for(int i=0;i<FixedTestcaseNums.Length;i++)
            {
                FixedTestcaseNums[i] = int.Parse(result[i]);
            }
            FunctionParamterCustomTyperangeUsed = bool.Parse(rootNode.SelectSingleNode("FunctionParamterCustomTyperangeUsed").InnerText);
            FunctionParamterCustomTyperangeMin = int.Parse(rootNode.SelectSingleNode("FunctionParamterCustomTyperangeMin").InnerText);
            FunctionParamterCustomTyperangeMax = int.Parse(rootNode.SelectSingleNode("FunctionParamterCustomTyperangeMax").InnerText);
            loopCreateNumMax = int.Parse(rootNode.SelectSingleNode("loopCreateNumMax").InnerText);
            testProgramBefore = rootNode.SelectSingleNode("testProgramBefore").InnerText;
            testProgramAfter = rootNode.SelectSingleNode("testProgramAfter").InnerText;
            classDiagramFile = rootNode.SelectSingleNode("classDiagramFile").InnerText;
            testProgramBeforeNameSpace = rootNode.SelectSingleNode("testProgramBeforeNameSpace").InnerText;
            testProgramAfterNameSpace = rootNode.SelectSingleNode("testProgramAfterNameSpace").InnerText;
        }

   
        private Distance NewCalutateTestcaseDistance(TestCase candidate, List<TestCase> executedTestcases)
        {         
            NewTestCaseDistanceCalculator tc = new NewTestCaseDistanceCalculator(candidate, objects);
            Distance newDist = tc.CalculateTestCaseDistance();
            return newDist;
        }
    }
    public class CompilerPath
    {
        public static string csc;
        public static string cl;
        public static string UnitTestFramework;
        public static string vcvars32;
        public static void LoadPath(string xmlFilePath)
        {
            XmlDocument myXmlDoc = new XmlDocument();
            myXmlDoc.Load(xmlFilePath);
            XmlNode rootNode = myXmlDoc.SelectSingleNode("CompilerPath");
            csc = rootNode.SelectSingleNode("csc").InnerText;
            cl = rootNode.SelectSingleNode("cl").InnerText;
            UnitTestFramework = rootNode.SelectSingleNode("UnitTestFramework").InnerText;
            vcvars32 = rootNode.SelectSingleNode("vcvars32").InnerText;

            if (!File.Exists(csc)) throw new ArgumentException("文件路径错误：" + csc);
            if (!File.Exists(cl)) throw new ArgumentException("文件路径错误：" + cl);
            if (!File.Exists(UnitTestFramework)) throw new ArgumentException("文件路径错误：" + UnitTestFramework);
            if (!File.Exists(vcvars32)) throw new ArgumentException("文件路径错误：" + vcvars32);
        }
    }
}