public class StockTimer: H3.SmartForm.Timer
{
    public StockTimer() { }
    protected override void OnWork(H3.IEngine engine)
    {
        DateTime now = DateTime.Now;
        DateTime sTime = DateTime.Parse(now.ToString("yyyy-MM-dd 01:00:00"));
        DateTime eTime = DateTime.Parse(now.ToString("yyyy-MM-dd 05:00:00"));
        if(sTime <= now && eTime >= now)
        {
            dos(engine);
        }
    }
    public void dos(H3.IEngine engine)
    {
        DateTime beforeDT = DateTime.Now;
        string systemUserId = H3.Organization.User.SystemUserId;

        string selectSQL = "select count(1) as num from i_D117502usedCarInventory where carStatus <> (select ObjectId from i_D117502CarStatus where carStatus = '已售出')";
        System.Data.DataTable Count = engine.Query.QueryTable(selectSQL, null);
        int num = Convert.ToInt32(Count.Rows[0]["num"]);
        string updateSQL = "update i_D117502usedCarInventory set LibraryAge = datediff(now(),date) where carStatus <> (select ObjectId from i_D117502CarStatus where carStatus = '已售出')";
        engine.Query.QueryTable(updateSQL, null);

        H3.DataModel.BizObjectSchema aSchema = engine.BizObjectManager.GetPublishedSchema("D117502carInterval");
        H3.DataModel.BizObject aBo = new H3.DataModel.BizObject(engine, aSchema, systemUserId);
        aBo.CreatedBy = H3.Organization.User.SystemUserId;
        aBo.OwnerId = H3.Organization.User.SystemUserId;
        aBo["ExecTime"] = beforeDT + string.Empty;
        aBo["ExecCount"] = num + string.Empty;
        aBo["err"] = "inerrancy";
        DateTime afterDT = System.DateTime.Now;
        TimeSpan dt = afterDT.Subtract(beforeDT);
        aBo["ExecutionTime"] = dt + string.Empty;
        aBo.Status = H3.DataModel.BizObjectStatus.Effective;
        aBo.Create();
    }
}