public class MyTest_Timer: H3.SmartForm.Timer
{
    public MyTest_Timer() { }
    protected override void OnWork(H3.IEngine engine)
    {
        DateTime now = DateTime.Now;
        DateTime sTime = DateTime.Parse(now.ToString("yyyy-MM-dd 00:00:00"));
        DateTime eTime = DateTime.Parse(now.ToString("yyyy-MM-dd 04:00:00"));
        if(sTime <= now && eTime >= now)
        {
            Execute_1(engine);
        }
    }
    public void Execute_1(H3.IEngine engine)
    {
        string yesterdayDate = DateTime.Now.AddDays(-1).ToShortDateString();
        string sql1 = "SELECT userObjectId FROM i_D117502SubsidyRecords WHERE TO_DAYS(NOW()) - TO_DAYS(changeTime) = 1 AND i_D117502SubsidyRecords.changeAmount > 0 AND i_D117502SubsidyRecords.changeType = '消费'";
        System.Data.DataTable Countb = engine.Query.QueryTable(sql1, null);
        System.Globalization.GregorianCalendar gc = new System.Globalization.GregorianCalendar();
        int weekOfYear = gc.GetWeekOfYear(DateTime.Now, System.Globalization.CalendarWeekRule.FirstDay, DayOfWeek.Monday);
        int RowsCount = Countb.Rows.Count;
        string systemUserId = H3.Organization.User.SystemUserId;
        H3.DataModel.BulkCommit commit = new H3.DataModel.BulkCommit();
        for(int i = 0;i < RowsCount; i++)
        {
            H3.DataModel.BizObjectSchema aSchema = engine.BizObjectManager.GetPublishedSchema("D117502statistics");
            H3.DataModel.BizObject aBo = new H3.DataModel.BizObject(engine, aSchema, systemUserId);
            aBo.CreatedBy = systemUserId;
            aBo.OwnerId = systemUserId;
            aBo["week"] = weekOfYear;
            aBo["date"] = yesterdayDate;
            aBo["times"] = 1;
            aBo["userObjectId"] = Countb.Rows[i]["userObjectId"];
            aBo.Status = H3.DataModel.BizObjectStatus.Effective;
            aBo.Create(commit);
        }
        DateTime now = DateTime.Now;
        H3.DataModel.BizObjectSchema Schema = engine.BizObjectManager.GetPublishedSchema("D117502statisticsInterver");
        H3.DataModel.BizObject LOG = new H3.DataModel.BizObject(engine, Schema, systemUserId);
        LOG.CreatedBy = H3.Organization.User.SystemUserId;
        LOG.OwnerId = H3.Organization.User.SystemUserId;
        LOG["executionDate"] = now + string.Empty;
        LOG["count"] = RowsCount;
        LOG.Status = H3.DataModel.BizObjectStatus.Effective;
        LOG.Create();
        string errorMsg = null;
        commit.Commit(engine.BizObjectManager, out errorMsg);
    }
}

