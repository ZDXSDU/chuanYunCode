using System;
using System.Collections.Generic;
using System.Text;
using H3;

public class D117502SuspensionInterval: H3.SmartForm.SmartFormController
{
    public D117502SuspensionInterval(H3.SmartForm.SmartFormRequest request): base(request)
    {
    }

    protected override void OnLoad(H3.SmartForm.LoadSmartFormResponse response)
    {
        base.OnLoad(response);
        response.Actions.Remove("Remove");
        response.Actions.Remove("Print");
        response.Actions.Remove("ViewQrCode");
    }
    protected override void OnSubmit(string actionName, H3.SmartForm.SmartFormPostValue postValue, H3.SmartForm.SubmitSmartFormResponse response)
    {
        base.OnSubmit(actionName, postValue, response);
        H3.IEngine engine = this.Request.Engine;
        // new FoodTimer().holidayBeginTomorrow(this.Engine);
        // new FoodTimer().holidayEndTomorrow(this.Engine);
    }
}
public class FoodTimer: H3.SmartForm.Timer
{
    public FoodTimer() { }
    protected override void OnWork(H3.IEngine engine)
    {
        DateTime now = DateTime.Now;
        DateTime sTime = DateTime.Parse(now.ToString("yyyy-MM-dd 19:00:00"));
        DateTime eTime = DateTime.Parse(now.ToString("yyyy-MM-dd 23:00:00"));
        if(sTime <= now && eTime >= now)
        {
            holidayBeginTomorrow(engine);
            holidayEndTomorrow(engine);
        }
    }
    public void dos(H3.IEngine engine) // dos方法废弃不用，但不要删除  —— ZDX SDU 
    {
        System.Data.DataTable TomorrowDT = engine.Query.QueryTable("SELECT DATE_SUB(CURDATE(),INTERVAL -1 DAY) AS Tomorrow", null);
        string Tomorrow = TomorrowDT.Rows[0]["Tomorrow"].ToString();
        string sql1 = "UPDATE i_D117502UserInformation SET isMoratorium = 1 WHERE i_D117502UserInformation.ObjectId in (SELECT JobNumber FROM i_D117502employeeSuspensionSet WHERE startDate = '" + Tomorrow + "')";
        string sql11 = "UPDATE i_D117502employeeSuspensionSet SET isEnd = 1 WHERE startDate = '" + Tomorrow + "'";
        string sql2 = "UPDATE i_D117502UserInformation SET isMoratorium = 0 WHERE i_D117502UserInformation.ObjectId in (SELECT JobNumber FROM i_D117502employeeSuspensionSet WHERE endDate = '" + Tomorrow + "')";
        string sql22 = "UPDATE i_D117502employeeSuspensionSet SET isFinish = 1 WHERE i_D117502employeeSuspensionSet.endDate = '" + Tomorrow + "'";
        engine.Query.QueryTable(sql1, null);
        engine.Query.QueryTable(sql11, null);
        engine.Query.QueryTable(sql2, null);
        engine.Query.QueryTable(sql22, null);
        System.Data.DataTable Countb = engine.Query.QueryTable("SELECT JobNumber FROM i_D117502employeeSuspensionSet WHERE startDate = '" + Tomorrow + "'", null);
        string CountBegin = Countb.Rows.Count.ToString();
        System.Data.DataTable Counte = engine.Query.QueryTable("SELECT JobNumber FROM i_D117502employeeSuspensionSet WHERE endDate = '" + Tomorrow + "'", null);
        string CountEnd = Counte.Rows.Count.ToString();
        DateTime now = DateTime.Now;
        string systemUserId = H3.Organization.User.SystemUserId;
        H3.DataModel.BizObjectSchema aSchema = engine.BizObjectManager.GetPublishedSchema("D117502SuspensionInterval");
        H3.DataModel.BizObject aBo = new H3.DataModel.BizObject(engine, aSchema, systemUserId);
        aBo.CreatedBy = H3.Organization.User.SystemUserId;
        aBo.OwnerId = H3.Organization.User.SystemUserId;
        aBo["executionDate"] = now + string.Empty;
        aBo["date"] = Tomorrow + string.Empty;
        aBo["begin"] = CountBegin + string.Empty;
        aBo["end"] = CountEnd + string.Empty;
        aBo.Status = H3.DataModel.BizObjectStatus.Effective;
        aBo.Create();

    }
    public void holidayBeginTomorrow(H3.IEngine engine)
    {
        System.Globalization.GregorianCalendar gc = new System.Globalization.GregorianCalendar();
        int weekOfYear = gc.GetWeekOfYear(DateTime.Now, System.Globalization.CalendarWeekRule.FirstDay, DayOfWeek.Monday);
        int weekDay = Convert.ToInt32(DateTime.Now.DayOfWeek);
        weekOfYear = weekDay == 0 ? weekOfYear - 1 : weekOfYear;
        DateTime Tommorow = DateTime.Today.AddDays(1);
        string systemUserId = H3.Organization.User.SystemUserId;
        H3.Data.Filter.Filter filter = new H3.Data.Filter.Filter();
        H3.Data.Filter.And andMatcher = new H3.Data.Filter.And();
        andMatcher.Add(new H3.Data.Filter.ItemMatcher("startDate", H3.Data.ComparisonOperatorType.Equal, Tommorow));
        filter.Matcher = andMatcher;
        H3.DataModel.BizObjectSchema schema = engine.BizObjectManager.GetPublishedSchema("D117502employeeSuspensionSet");
        H3.DataModel.BizObject[] boArray = H3.DataModel.BizObject.GetList(engine, systemUserId, schema, H3.DataModel.GetListScopeType.GlobalAll, filter);
        H3.DataModel.BizObjectSchema statisticsSchema = engine.BizObjectManager.GetPublishedSchema("D117502statistics");
        H3.DataModel.BizObject statistics = new H3.DataModel.BizObject(engine, statisticsSchema, systemUserId);
        if(boArray != null && boArray.Length > 0)
        {
            for(int i = 0;i < boArray.Length; i++)
            {
                string JobNumber = boArray[i]["JobNumber"].ToString();
                H3.DataModel.BizObject UserInformation = H3.DataModel.BizObject.Load(systemUserId, engine, "D117502UserInformation", JobNumber, false);
                UserInformation["isMoratorium"] = true;
                UserInformation.Status = H3.DataModel.BizObjectStatus.Effective;
                UserInformation.Update();
                System.Data.DataTable TomorrowDT = engine.Query.QueryTable("SELECT objectId FROM I_D117502employeeSuspensionSet WHERE I_D117502employeeSuspensionSet.JobNumber = '" + JobNumber + "' AND I_D117502employeeSuspensionSet.startDate ='" + Tommorow + "'", null);
                string employeeSuspensionSettObjectId = TomorrowDT.Rows[0]["objectId"].ToString();
                H3.DataModel.BizObject employeeSuspensionSet = H3.DataModel.BizObject.Load(systemUserId, engine, "D117502employeeSuspensionSet", employeeSuspensionSettObjectId, false);
                employeeSuspensionSet["isEnd"] = true;
                employeeSuspensionSet.Status = H3.DataModel.BizObjectStatus.Effective;
                employeeSuspensionSet.Update();
                statistics.CreatedBy = systemUserId;
                statistics.OwnerId = systemUserId;
                statistics["userObjectId"] = JobNumber;
                statistics["week"] = weekOfYear;
                statistics["date"] = Tommorow;
                statistics["times"] = 1;
                statistics["remark"] = "当日该员工请假，本周消耗一次补贴机会；";
                statistics.Status = H3.DataModel.BizObjectStatus.Effective;
                statistics.Create();
            }
            H3.DataModel.BizObjectSchema aSchema = engine.BizObjectManager.GetPublishedSchema("D117502SuspensionInterval");
            H3.DataModel.BizObject aBo = new H3.DataModel.BizObject(engine, aSchema, systemUserId);
            aBo.CreatedBy = systemUserId;
            aBo.OwnerId = systemUserId;
            aBo["executionDate"] = DateTime.Now + string.Empty;
            aBo["date"] = Tommorow + string.Empty;
            aBo["counts"] = boArray.Length + string.Empty;
            aBo["type"] = "请假开始，开始暂停";
            aBo.Status = H3.DataModel.BizObjectStatus.Effective;
            aBo.Create();
        }
        else
        {
            H3.DataModel.BizObjectSchema aSchema = engine.BizObjectManager.GetPublishedSchema("D117502SuspensionInterval");
            H3.DataModel.BizObject aBo = new H3.DataModel.BizObject(engine, aSchema, systemUserId);
            aBo.CreatedBy = systemUserId;
            aBo.OwnerId = systemUserId;
            aBo["executionDate"] = DateTime.Now + string.Empty;
            aBo["date"] = Tommorow + string.Empty;
            aBo["counts"] = 0 + string.Empty;
            aBo["type"] = "请假开始，开始暂停";
            aBo.Status = H3.DataModel.BizObjectStatus.Effective;
            aBo.Create();
        }
    }
    public void holidayEndTomorrow(H3.IEngine engine)
    {
        System.Globalization.GregorianCalendar gc = new System.Globalization.GregorianCalendar();
        int weekOfYear = gc.GetWeekOfYear(DateTime.Now, System.Globalization.CalendarWeekRule.FirstDay, DayOfWeek.Monday);
        int weekDay = Convert.ToInt32(DateTime.Now.DayOfWeek);
        weekOfYear = weekDay == 0 ? weekOfYear - 1 : weekOfYear;
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
                H3.DataModel.BizObject UserInformation = H3.DataModel.BizObject.Load(systemUserId, engine, "D117502UserInformation", JobNumber, false);
                UserInformation["isMoratorium"] = false;
                UserInformation.Status = H3.DataModel.BizObjectStatus.Effective;
                UserInformation.Update();
                System.Data.DataTable TomorrowDT = engine.Query.QueryTable("SELECT objectId FROM I_D117502employeeSuspensionSet WHERE I_D117502employeeSuspensionSet.JobNumber = '" + JobNumber + "' AND I_D117502employeeSuspensionSet.endDate ='" + today + "'", null);
                string employeeSuspensionSettObjectId = TomorrowDT.Rows[0]["objectId"].ToString();
                H3.DataModel.BizObject employeeSuspensionSet = H3.DataModel.BizObject.Load(systemUserId, engine, "D117502employeeSuspensionSet", employeeSuspensionSettObjectId, false);
                employeeSuspensionSet["isEnd"] = false;
                employeeSuspensionSet["isFinish"] = true;
                employeeSuspensionSet.Status = H3.DataModel.BizObjectStatus.Effective;
                employeeSuspensionSet.Update();
            }
            H3.DataModel.BizObjectSchema aSchema = engine.BizObjectManager.GetPublishedSchema("D117502SuspensionInterval");
            H3.DataModel.BizObject aBo = new H3.DataModel.BizObject(engine, aSchema, systemUserId);
            aBo.CreatedBy = systemUserId;
            aBo.OwnerId = systemUserId;
            aBo["executionDate"] = DateTime.Now + string.Empty;
            aBo["date"] = today + string.Empty;
            aBo["counts"] = boArray.Length + string.Empty;
            aBo["type"] = "请假结束，暂停结束";
            aBo.Status = H3.DataModel.BizObjectStatus.Effective;
            aBo.Create();
        }
        else
        {
            H3.DataModel.BizObjectSchema aSchema = engine.BizObjectManager.GetPublishedSchema("D117502SuspensionInterval");
            H3.DataModel.BizObject aBo = new H3.DataModel.BizObject(engine, aSchema, systemUserId);
            aBo.CreatedBy = systemUserId;
            aBo.OwnerId = systemUserId;
            aBo["executionDate"] = DateTime.Now + string.Empty;
            aBo["date"] = today + string.Empty;
            aBo["counts"] = 0 + string.Empty;
            aBo["type"] = "请假结束，暂停结束";
            aBo.Status = H3.DataModel.BizObjectStatus.Effective;
            aBo.Create();
        }
    }
}