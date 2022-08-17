
using System;
using System.Collections.Generic;
using System.Text;
using H3;

public class D117502974f1fad35864bb887fe751861c29e69: H3.SmartForm.SmartFormController
{
    public D117502974f1fad35864bb887fe751861c29e69(H3.SmartForm.SmartFormRequest request): base(request)
    {
    }

    protected override void OnLoad(H3.SmartForm.LoadSmartFormResponse response)
    {
        base.OnLoad(response);
        response.Actions.Remove("Remove");
        response.Actions.Remove("Print");
        response.Actions.Remove("ViewQrCode");
        // new MyTest_Timer().updateNameOfbuffet(this.Engine);
    }

    protected override void OnSubmit(string actionName, H3.SmartForm.SmartFormPostValue postValue, H3.SmartForm.SubmitSmartFormResponse response)
    {
        base.OnSubmit(actionName, postValue, response);
    }
}
public class MyTest_Timer: H3.SmartForm.Timer
{
    public MyTest_Timer() { }
    protected override void OnWork(H3.IEngine engine)
    {
        DateTime now = DateTime.Now;
        int week = Convert.ToInt32(DateTime.Now.DayOfWeek);
        DateTime sTime_holiday = DateTime.Parse(now.ToString("yyyy-MM-dd 12:00:00"));
        DateTime eTime_holiday = DateTime.Parse(now.ToString("yyyy-MM-dd 17:00:00"));
        DateTime sTime = DateTime.Parse(now.ToString("yyyy-MM-dd 00:00:00"));
        DateTime eTime = DateTime.Parse(now.ToString("yyyy-MM-dd 04:00:00"));
        DateTime sTime2 = DateTime.Parse(now.ToString("yyyy-MM-dd 01:00:00"));
        DateTime eTime2 = DateTime.Parse(now.ToString("yyyy-MM-dd 05:00:00"));
        // 【每天 12:00 ~ 17:00】 运行第二天的请假暂停；如果第二天请假（明天是请假的第一天），那么提前（提前是指：在补贴开始之前）暂停；如果今天请假结束（今天是请假的最后一天），那么提前关闭暂停；
        if(sTime_holiday <= now && eTime_holiday >= now)
        {
            holidayBeginTomorrow(engine);
            holidayEndToday(engine);
        }
        // 【每天 00:00 ~ 04:00】 运行补贴支付消费统计和归零；如果这个职工补贴余额小于10，那么就是他今天消费或者请假了，占用一次补贴次数
        if(sTime <= now && eTime >= now)
        {
            statisticsAndClearZero(engine);
        }
        // 【周末 01:00 ~ 05:00】 运行周末特供版职工充值补贴，只有运行当天没有请假暂停并且在职并且本周补贴消费次数小于5次的小可爱才有机会得到补贴
        if(sTime2 <= now && eTime2 >= now && (week == 6 || week == 0))
        {
            StatisticsSubsidyIn(engine);
        }
        // 【工作日 01:00~05:00】 运行职工补贴充值；没有中间商赚差价，直接SQL
        if(sTime2 <= now && eTime2 >= now && week != 6 && week != 0)
        {
            recharge(engine);
        }
        // 每隔4小时运行一次 将自助餐表单中空余的职工姓名字段补充完整
        updateNameOfbuffet(engine);
    }
    public void holidayBeginTomorrow(H3.IEngine engine)
    {
        DateTime beforeDT = System.DateTime.Now;
        DateTime Tommorow = DateTime.Today.AddDays(1);
        string systemUserId = H3.Organization.User.SystemUserId;
        H3.Data.Filter.Filter filter = new H3.Data.Filter.Filter();
        H3.Data.Filter.And andMatcher = new H3.Data.Filter.And();
        andMatcher.Add(new H3.Data.Filter.ItemMatcher("startDate", H3.Data.ComparisonOperatorType.Equal, Tommorow));
        filter.Matcher = andMatcher;
        H3.DataModel.BizObjectSchema schema = engine.BizObjectManager.GetPublishedSchema("D117502employeeSuspensionSet");
        H3.DataModel.BizObject[] boArray = H3.DataModel.BizObject.GetList(engine, systemUserId, schema, H3.DataModel.GetListScopeType.GlobalAll, filter);
        if(boArray != null && boArray.Length > 0)
        {
            for(int i = 0;i < boArray.Length; i++)
            {
                string JobNumber = boArray[i]["JobNumber"].ToString();
                string employeeSuspensionSettObjectId = boArray[i]["ObjectId"].ToString();
                H3.DataModel.BizObject UserInformation = H3.DataModel.BizObject.Load(systemUserId, engine, "D117502UserInformation", JobNumber, false);
                UserInformation["isMoratorium"] = true;
                UserInformation.Status = H3.DataModel.BizObjectStatus.Effective;
                UserInformation.Update();
                H3.DataModel.BizObject employeeSuspensionSet = H3.DataModel.BizObject.Load(systemUserId, engine, "D117502employeeSuspensionSet", employeeSuspensionSettObjectId, false);
                employeeSuspensionSet["isEnd"] = true;
                employeeSuspensionSet.Status = H3.DataModel.BizObjectStatus.Effective;
                employeeSuspensionSet.Update();
            }
            H3.DataModel.BizObjectSchema aSchema = engine.BizObjectManager.GetPublishedSchema("D117502974f1fad35864bb887fe751861c29e69");
            H3.DataModel.BizObject aBo = new H3.DataModel.BizObject(engine, aSchema, systemUserId);
            aBo.CreatedBy = systemUserId;
            aBo.OwnerId = systemUserId;
            aBo["Runtime"] = DateTime.Now + string.Empty;
            aBo["description"] = "职工明天请假，开始暂停补贴";
            aBo["count"] = boArray.Length;
            DateTime afterDT = System.DateTime.Now;
            TimeSpan dt = afterDT.Subtract(beforeDT);
            aBo["ExecutionTime"] = dt.ToString();
            aBo["basedate"] = Tommorow + string.Empty;
            aBo["Debugmarkup"] = "生产环境 auto";
            aBo["Errorlogs"] = "inerrancy";
            aBo.Status = H3.DataModel.BizObjectStatus.Effective;
            aBo.Create();
        }
        else
        {
            H3.DataModel.BizObjectSchema aSchema = engine.BizObjectManager.GetPublishedSchema("D117502974f1fad35864bb887fe751861c29e69");
            H3.DataModel.BizObject aBo = new H3.DataModel.BizObject(engine, aSchema, systemUserId);
            aBo.CreatedBy = systemUserId;
            aBo.OwnerId = systemUserId;
            aBo["Runtime"] = DateTime.Now + string.Empty;
            aBo["description"] = "职工明天请假，开始暂停补贴";
            aBo["count"] = 0;
            DateTime afterDT = System.DateTime.Now;
            TimeSpan dt = afterDT.Subtract(beforeDT);
            aBo["ExecutionTime"] = dt.ToString();
            aBo["basedate"] = Tommorow + string.Empty;
            aBo["Debugmarkup"] = "生产环境";
            aBo["Errorlogs"] = "inerrancy";
            aBo.Status = H3.DataModel.BizObjectStatus.Effective;
            aBo.Create();
        }
    }
    public void holidayEndToday(H3.IEngine engine)
    {
        DateTime beforeDT = System.DateTime.Now;
        DateTime today = DateTime.Today;
        string systemUserId = H3.Organization.User.SystemUserId;
        H3.Data.Filter.Filter filter = new H3.Data.Filter.Filter();
        H3.Data.Filter.And andMatcher = new H3.Data.Filter.And();
        andMatcher.Add(new H3.Data.Filter.ItemMatcher("endDate", H3.Data.ComparisonOperatorType.Equal, today));
        filter.Matcher = andMatcher;
        H3.DataModel.BizObjectSchema schema = engine.BizObjectManager.GetPublishedSchema("D117502employeeSuspensionSet");
        H3.DataModel.BizObject[] boArray = H3.DataModel.BizObject.GetList(engine, systemUserId, schema, H3.DataModel.GetListScopeType.GlobalAll, filter);
        if(boArray != null && boArray.Length > 0)
        {
            for(int i = 0;i < boArray.Length; i++)
            {
                string JobNumber = boArray[i]["JobNumber"].ToString();
                string employeeSuspensionSettObjectId = boArray[i]["ObjectId"].ToString();
                H3.DataModel.BizObject UserInformation = H3.DataModel.BizObject.Load(systemUserId, engine, "D117502UserInformation", JobNumber, false);
                UserInformation["isMoratorium"] = false;
                UserInformation.Status = H3.DataModel.BizObjectStatus.Effective;
                UserInformation.Update();
                H3.DataModel.BizObject employeeSuspensionSet = H3.DataModel.BizObject.Load(systemUserId, engine, "D117502employeeSuspensionSet", employeeSuspensionSettObjectId, false);
                employeeSuspensionSet["isEnd"] = false;
                employeeSuspensionSet["isFinish"] = true;
                employeeSuspensionSet.Status = H3.DataModel.BizObjectStatus.Effective;
                employeeSuspensionSet.Update();
            }
            H3.DataModel.BizObjectSchema aSchema = engine.BizObjectManager.GetPublishedSchema("D117502974f1fad35864bb887fe751861c29e69");
            H3.DataModel.BizObject aBo = new H3.DataModel.BizObject(engine, aSchema, systemUserId);
            aBo.CreatedBy = systemUserId;
            aBo.OwnerId = systemUserId;
            aBo["Runtime"] = DateTime.Now + string.Empty;
            aBo["description"] = "职工今天请假结束，从明天开始正常补贴";
            aBo["count"] = boArray.Length;
            DateTime afterDT = System.DateTime.Now;
            TimeSpan dt = afterDT.Subtract(beforeDT);
            aBo["ExecutionTime"] = dt.ToString();
            aBo["basedate"] = today + string.Empty;
            aBo["Debugmarkup"] = "生产环境 auto";
            aBo["Errorlogs"] = "inerrancy";
            aBo.Status = H3.DataModel.BizObjectStatus.Effective;
            aBo.Create();
        }
        else
        {
            H3.DataModel.BizObjectSchema aSchema = engine.BizObjectManager.GetPublishedSchema("D117502974f1fad35864bb887fe751861c29e69");
            H3.DataModel.BizObject aBo = new H3.DataModel.BizObject(engine, aSchema, systemUserId);
            aBo.CreatedBy = systemUserId;
            aBo.OwnerId = systemUserId;
            aBo["Runtime"] = DateTime.Now + string.Empty;
            aBo["description"] = "职工今天请假结束，从明天开始正常补贴";
            aBo["count"] = 0;
            DateTime afterDT = System.DateTime.Now;
            TimeSpan dt = afterDT.Subtract(beforeDT); // 执行耗时（时间差）
            aBo["ExecutionTime"] = dt.ToString();
            aBo["basedate"] = today + string.Empty;
            aBo["Debugmarkup"] = "生产环境";
            aBo["Errorlogs"] = "inerrancy";
            aBo.Status = H3.DataModel.BizObjectStatus.Effective;
            aBo.Create();
        }
    }
    public void statisticsAndClearZero(H3.IEngine engine)
    {
        DateTime beforeDT = System.DateTime.Now;
        string systemUserId = H3.Organization.User.SystemUserId;
        DateTime now = DateTime.Now;
        System.Globalization.GregorianCalendar gc = new System.Globalization.GregorianCalendar();
        int weekOfYear = gc.GetWeekOfYear(DateTime.Now, System.Globalization.CalendarWeekRule.FirstDay, DayOfWeek.Monday);
        int weekDay = Convert.ToInt32(DateTime.Now.DayOfWeek);
        if(weekDay == 1)
        {
            string updateSQL = "UPDATE I_D117502UserInformation SET week = " + weekOfYear + ", subsidiesThisWeekNumber = 0, subsidyBalance = 0"; // 生产环境
            // string updateSQL = "UPDATE I_D117502Spem1xof74brn0h19b6nxpyxs3 SET week = " + weekOfYear + ", subsidiesThisWeekNumber = 0, subsidyBalance = 0"; // 测试环境
            engine.Query.QueryTable(updateSQL, null);
            H3.DataModel.BizObjectSchema aSchema = engine.BizObjectManager.GetPublishedSchema("D117502974f1fad35864bb887fe751861c29e69");
            H3.DataModel.BizObject aBo = new H3.DataModel.BizObject(engine, aSchema, systemUserId);
            aBo.CreatedBy = systemUserId;
            aBo.OwnerId = systemUserId;
            aBo["Runtime"] = DateTime.Now + string.Empty;
            aBo["description"] = "【周一】重置当前周、本周补贴次数同时所有人补贴清零";
            aBo["count"] = 2405;
            DateTime afterDT = System.DateTime.Now;
            TimeSpan dt = afterDT.Subtract(beforeDT);
            aBo["ExecutionTime"] = dt.ToString();
            aBo["basedate"] = "第 " + weekOfYear.ToString() + " 周";
            aBo["Debugmarkup"] = "生产环境 auto";
            aBo["Errorlogs"] = "inerrancy";
            aBo.Status = H3.DataModel.BizObjectStatus.Effective;
            aBo.Create();
        }
        else
        {
            string selectSQL = "SELECT COUNT(1) AS num FROM I_D117502UserInformation WHERE subsidyBalance < 10 AND startUsing = 1"; // 生产环境
            // string selectSQL = "SELECT COUNT(1) AS num FROM I_D117502Spem1xof74brn0h19b6nxpyxs3 WHERE subsidyBalance < 10 AND startUsing = 1"; // 测试环境
            System.Data.DataTable Count = engine.Query.QueryTable(selectSQL, null);
            int num = Convert.ToInt32(Count.Rows[0]["num"]);
            string statisticsSQL = "UPDATE I_D117502UserInformation SET subsidiesThisWeekNumber = subsidiesThisWeekNumber + 1 WHERE subsidyBalance < 10 AND startUsing = 1"; // 生产环境
            // string statisticsSQL = "UPDATE I_D117502Spem1xof74brn0h19b6nxpyxs3 SET subsidiesThisWeekNumber = subsidiesThisWeekNumber + 1 WHERE subsidyBalance < 10 AND startUsing = 1"; // 测试环境
            engine.Query.QueryTable(statisticsSQL, null);
            string clearZeroSQL = "UPDATE I_D117502UserInformation SET subsidyBalance = 0"; // 生产环境
            // string clearZeroSQL = "UPDATE I_D117502Spem1xof74brn0h19b6nxpyxs3 SET subsidyBalance = 0"; // 测试环境
            engine.Query.QueryTable(clearZeroSQL, null);
            H3.DataModel.BizObjectSchema aSchema = engine.BizObjectManager.GetPublishedSchema("D117502974f1fad35864bb887fe751861c29e69");
            H3.DataModel.BizObject aBo = new H3.DataModel.BizObject(engine, aSchema, systemUserId);
            aBo.CreatedBy = systemUserId;
            aBo.OwnerId = systemUserId;
            aBo["Runtime"] = DateTime.Now + string.Empty;
            aBo["description"] = "统计昨天的补贴次数 + 全员归零";
            aBo["count"] = num;
            DateTime afterDT = System.DateTime.Now;
            TimeSpan dt = afterDT.Subtract(beforeDT);
            aBo["ExecutionTime"] = dt.ToString();
            aBo["basedate"] = DateTime.Today.AddDays(-1).ToString();
            aBo["Debugmarkup"] = "生产环境 auto";
            aBo["Errorlogs"] = "inerrancy；上述“count”为补贴余额<10的人数";
            aBo.Status = H3.DataModel.BizObjectStatus.Effective;
            aBo.Create();
        }
    }
    public void recharge(H3.IEngine engine)
    {
        DateTime beforeDT = System.DateTime.Now;
        string systemUserId = H3.Organization.User.SystemUserId;
        string selectSQL = "SELECT COUNT(1) AS num FROM I_D117502UserInformation WHERE startUsing = 1 AND isMoratorium = 0"; // 生产环境
        // string selectSQL = "SELECT COUNT(1) AS num FROM I_D117502Spem1xof74brn0h19b6nxpyxs3 WHERE startUsing = 1 AND isMoratorium = 0"; // 测试环境
        System.Data.DataTable Count = engine.Query.QueryTable(selectSQL, null);
        int num = Convert.ToInt32(Count.Rows[0]["num"]);
        string rechargeSQL = "UPDATE I_D117502UserInformation SET subsidyBalance = 10 WHERE startUsing = 1 AND isMoratorium = 0"; // 生产环境
        // string rechargeSQL = "UPDATE I_D117502Spem1xof74brn0h19b6nxpyxs3 SET subsidyBalance = 10 WHERE startUsing = 1 AND isMoratorium = 0"; // 测试环境
        engine.Query.QueryTable(rechargeSQL, null);
        H3.DataModel.BizObjectSchema aSchema = engine.BizObjectManager.GetPublishedSchema("D117502974f1fad35864bb887fe751861c29e69");
        H3.DataModel.BizObject aBo = new H3.DataModel.BizObject(engine, aSchema, systemUserId);
        aBo.CreatedBy = systemUserId;
        aBo.OwnerId = systemUserId;
        aBo["Runtime"] = DateTime.Now + string.Empty;
        aBo["description"] = "【工作日】补贴充值";
        aBo["count"] = num;
        DateTime afterDT = System.DateTime.Now;
        TimeSpan dt = afterDT.Subtract(beforeDT);
        aBo["ExecutionTime"] = dt.ToString();
        aBo["basedate"] = "“undefined”";
        aBo["Debugmarkup"] = "生产环境 auto";
        aBo["Errorlogs"] = "inerrancy";
        aBo.Status = H3.DataModel.BizObjectStatus.Effective;
        aBo.Create();
    }
    public void StatisticsSubsidyIn(H3.IEngine engine)
    {
        DateTime beforeDT = System.DateTime.Now;
        string systemUserId = H3.Organization.User.SystemUserId;
        string selectSQL = "SELECT COUNT(1) AS num FROM I_D117502UserInformation WHERE subsidiesThisWeekNumber < 5 AND startUsing = 1 AND isMoratorium = 0"; // 生产环境
        // string selectSQL = "SELECT COUNT(1) AS num FROM I_D117502Spem1xof74brn0h19b6nxpyxs3 WHERE subsidiesThisWeekNumber < 5 AND startUsing = 1 AND isMoratorium = 0"; // 测试环境
        System.Data.DataTable Count = engine.Query.QueryTable(selectSQL, null);
        int num = Convert.ToInt32(Count.Rows[0]["num"]);
        string rechargeSQL = "UPDATE I_D117502UserInformation SET subsidyBalance = 10 WHERE subsidiesThisWeekNumber < 5 AND startUsing = 1 AND isMoratorium = 0"; // 生产环境
        // string rechargeSQL = "UPDATE I_D117502Spem1xof74brn0h19b6nxpyxs3 SET subsidyBalance = 100 WHERE subsidiesThisWeekNumber < 5 AND startUsing = 1 AND isMoratorium = 0"; // 测试环境
        engine.Query.QueryTable(rechargeSQL, null);
        H3.DataModel.BizObjectSchema aSchema = engine.BizObjectManager.GetPublishedSchema("D117502974f1fad35864bb887fe751861c29e69");
        H3.DataModel.BizObject aBo = new H3.DataModel.BizObject(engine, aSchema, systemUserId);
        aBo.CreatedBy = systemUserId;
        aBo.OwnerId = systemUserId;
        aBo["Runtime"] = DateTime.Now + string.Empty;
        aBo["description"] = "【周末】补贴充值";
        aBo["count"] = num;
        DateTime afterDT = System.DateTime.Now;
        TimeSpan dt = afterDT.Subtract(beforeDT);
        aBo["ExecutionTime"] = dt.ToString();
        aBo["basedate"] = "“undefined”";
        aBo["Debugmarkup"] = "生产环境 auto";
        aBo["Errorlogs"] = "inerrancy";
        aBo.Status = H3.DataModel.BizObjectStatus.Effective;
        aBo.Create();
    }
    public void updateNameOfbuffet(H3.IEngine engine)
    {
        DateTime beforeDT = System.DateTime.Now;
        string systemUserId = H3.Organization.User.SystemUserId;
        string selectSQL = "SELECT COUNT(1) AS num FROM I_D117502OrdersBuffet WHERE userName = ''";
        System.Data.DataTable Count = engine.Query.QueryTable(selectSQL, null);
        int num = Convert.ToInt32(Count.Rows[0]["num"]);
        if(num > 0) 
        {
            string rechargeSQL = "UPDATE I_D117502OrdersBuffet SET I_D117502OrdersBuffet.userName = (SELECT userName FROM I_D117502UserInformation WHERE I_D117502UserInformation.objectId = I_D117502OrdersBuffet.JobNumber) WHERE userName = ''";
            engine.Query.QueryTable(rechargeSQL, null);
            H3.DataModel.BizObjectSchema aSchema = engine.BizObjectManager.GetPublishedSchema("D117502974f1fad35864bb887fe751861c29e69");
            H3.DataModel.BizObject aBo = new H3.DataModel.BizObject(engine, aSchema, systemUserId);
            aBo.CreatedBy = systemUserId;
            aBo.OwnerId = systemUserId;
            aBo["Runtime"] = DateTime.Now + string.Empty;
            aBo["description"] = "【循环】自助餐更新姓名";
            aBo["count"] = num;
            DateTime afterDT = System.DateTime.Now;
            TimeSpan dt = afterDT.Subtract(beforeDT);
            aBo["ExecutionTime"] = dt.ToString();
            aBo["basedate"] = "“undefined”";
            aBo["Debugmarkup"] = "4H 循环";
            aBo["Errorlogs"] = "inerrancy";
            aBo.Status = H3.DataModel.BizObjectStatus.Effective;
            aBo.Create();
        }
        else
        {
            H3.DataModel.BizObjectSchema aSchema = engine.BizObjectManager.GetPublishedSchema("D117502974f1fad35864bb887fe751861c29e69");
            H3.DataModel.BizObject aBo = new H3.DataModel.BizObject(engine, aSchema, systemUserId);
            aBo.CreatedBy = systemUserId;
            aBo.OwnerId = systemUserId;
            aBo["Runtime"] = DateTime.Now + string.Empty;
            aBo["description"] = "【循环】自助餐更新姓名";
            aBo["count"] = 0;
            DateTime afterDT = System.DateTime.Now;
            TimeSpan dt = afterDT.Subtract(beforeDT);
            aBo["ExecutionTime"] = dt.ToString();
            aBo["basedate"] = "“undefined”";
            aBo["Debugmarkup"] = "4H 循环";
            aBo["Errorlogs"] = "未找到可执行的数据";
            aBo.Status = H3.DataModel.BizObjectStatus.Effective;
            aBo.Create();
        }
    }
}