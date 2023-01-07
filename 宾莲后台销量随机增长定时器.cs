// 表单》》》》》》》》》》》》》》》》》》》》》》》》》》

using System;
using System.Collections.Generic;
using System.Text;
using H3;

public class D11750239bINTERVAL: H3.SmartForm.SmartFormController
{
    public D11750239bINTERVAL(H3.SmartForm.SmartFormRequest request): base(request)
    {
    }

    protected override void OnLoad(H3.SmartForm.LoadSmartFormResponse response)
    {
        base.OnLoad(response);
        response.Actions.Remove("Remove");
        response.Actions.Remove("ViewQrCode");
        response.Actions.Remove("Save");
        response.Actions.Remove("Submit");
        response.Actions.Remove("RetrieveInstance");
        response.Actions.Remove("Reject");
        response.Actions.Remove("CancelInstance");
        response.Actions.Remove("Print");
        response.Actions.Remove("Edit");
        response.Actions.Remove("Forward");
        response.Actions.Remove("Read");
        H3.IEngine engine = this.Request.Engine;
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
        DateTime sTime = DateTime.Parse(now.ToString("yyyy-MM-dd 06:00:00"));
        DateTime eTime = DateTime.Parse(now.ToString("yyyy-MM-dd 21:00:00"));
        if(sTime <= now && eTime >= now)
        {
            Execute_2(engine);
        }
    }

    public void Execute_2(H3.IEngine engine)
    {
        DateTime beforeDT = DateTime.Now;
        string systemUserId = H3.Organization.User.SystemUserId;
        /** 查找符合条件的数据并遍历、判断、赋值、批处理提交 BEGIN **/        
        string selectSQL = "SELECT objectid AS OBJ, sales, MerchandiseName AS GOODSNAME FROM I_D117502MerchandiseManagement WHERE isEnabled = 1";
        System.Data.DataTable OBJS = engine.Query.QueryTable(selectSQL, null);
        H3.DataModel.BulkCommit commit = new H3.DataModel.BulkCommit();
        Random rd = new Random();
        for(int i = 0;i < OBJS.Rows.Count; i++)
        {
            if(OBJS.Rows[i]["GOODSNAME"].ToString() == "贡乳-特色酸奶" || OBJS.Rows[i]["GOODSNAME"].ToString() == "巴氏鲜牛奶")
            {
                H3.DataModel.BizObject accountBo = H3.DataModel.BizObject.Load(systemUserId, engine, "D117502MerchandiseManagement", OBJS.Rows[i]["OBJ"].ToString(), false);
                accountBo["sales"] = Convert.ToInt32(OBJS.Rows[i]["sales"].ToString()) + rd.Next(300, 501);
                accountBo.Status = H3.DataModel.BizObjectStatus.Effective;
                accountBo.Update(commit);
            }
            else
            {
                H3.DataModel.BizObject accountBo = H3.DataModel.BizObject.Load(systemUserId, engine, "D117502MerchandiseManagement", OBJS.Rows[i]["OBJ"].ToString(), false);
                accountBo["sales"] = Convert.ToInt32(OBJS.Rows[i]["sales"].ToString()) + rd.Next(5, 10);
                accountBo.Status = H3.DataModel.BizObjectStatus.Effective;
                accountBo.Update(commit);
            }
        }
        string errorMsg = null;
        commit.Commit(engine.BizObjectManager, out errorMsg);
        /** 查找符合条件的数据并遍历、判断、赋值、批处理提交 END     创建执行日志 BEGIN **/
        H3.DataModel.BizObjectSchema aSchema = engine.BizObjectManager.GetPublishedSchema("D11750239bINTERVAL");
        H3.DataModel.BizObject aBo = new H3.DataModel.BizObject(engine, aSchema, systemUserId);
        aBo.CreatedBy = systemUserId;
        aBo.OwnerId = systemUserId;
        aBo["ERROR"] = errorMsg == null ? "inerrancy" : errorMsg;
        DateTime afterDT = System.DateTime.Now;
        TimeSpan dt = afterDT.Subtract(beforeDT);
        aBo["ExecutionTime"] = dt + string.Empty;
        aBo.Status = H3.DataModel.BizObjectStatus.Effective;
        aBo.Create();
        /** 创建执行日志 END **/        
    }
}
// 列表》》》》》》》》》》》》》》》》》》》》》》》》》》》》》》》》》

using System;
using System.Collections.Generic;
using System.Text;
using H3;

public class D11750239bINTERVAL_ListViewController: H3.SmartForm.ListViewController
{
    public D11750239bINTERVAL_ListViewController(H3.SmartForm.ListViewRequest request): base(request)
    {
    }

    protected override void OnLoad(H3.SmartForm.LoadListViewResponse response)
    {
        base.OnLoad(response);
        foreach(Dictionary < string, object > data in response.ReturnData)
        {
            string timesSPAN = data["ExecutionTime"].ToString();
            TimeSpan dt = TimeSpan.Parse(timesSPAN);
            long sss = Convert.ToInt64(dt.TotalMilliseconds);
            if(sss < 700)
            {
                data["ExecutionTime"] = new H3.SmartForm.ListViewCustomCell(data["ExecutionTime"], H3.SmartForm.Color.Green);
            }
            if(sss >= 700 && sss < 1000)
            {
                data["ExecutionTime"] = new H3.SmartForm.ListViewCustomCell(data["ExecutionTime"], H3.SmartForm.Color.Yellow);
            }
            if(sss >= 1000)
            {
                data["ExecutionTime"] = new H3.SmartForm.ListViewCustomCell(data["ExecutionTime"], H3.SmartForm.Color.Red);
            }
        }
    }

    protected override void OnSubmit(string actionName, H3.SmartForm.ListViewPostValue postValue, H3.SmartForm.SubmitListViewResponse response)
    {
        base.OnSubmit(actionName, postValue, response);
    }
}