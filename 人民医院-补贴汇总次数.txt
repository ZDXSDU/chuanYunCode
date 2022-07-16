
using System;
using System.Collections.Generic;
using System.Text;
using H3;

public class D117502totalSub_ListViewController: H3.SmartForm.ListViewController
{
    public D117502totalSub_ListViewController(H3.SmartForm.ListViewRequest request): base(request)
    {
    }

    protected override void OnLoad(H3.SmartForm.LoadListViewResponse response)
    {
        base.OnLoad(response);
    }

    protected override void OnSubmit(string actionName, H3.SmartForm.ListViewPostValue postValue, H3.SmartForm.SubmitListViewResponse response)
    {
        base.OnSubmit(actionName, postValue, response);
        if(actionName == "ZIZHU")
        {
            string sqlZZ = "SELECT ObjectId, userName, workerNumber, times.countZIZHU FROM I_D117502UserInformation LEFT JOIN (SELECT F0000002, COUNT(1) AS countZIZHU FROM I_D117502g6wevjqhkantcxn3scnc8omh5 WHERE I_D117502g6wevjqhkantcxn3scnc8omh5.F0000007 > 0 GROUP BY I_D117502g6wevjqhkantcxn3scnc8omh5.F0000002) AS times ON I_D117502UserInformation.workerNumber= times.F0000002 WHERE times.countZIZHU >= 1";
            string sqlTC = "SELECT ObjectId, userName, workerNumber, times.countTAOCAN FROM I_D117502UserInformation LEFT JOIN (SELECT F0000004, COUNT(1) AS countTAOCAN FROM I_D117502vfnk3tafznrv9lpjbbbyiv7d5 WHERE I_D117502vfnk3tafznrv9lpjbbbyiv7d5.F0000011 > 0 GROUP BY I_D117502vfnk3tafznrv9lpjbbbyiv7d5.F0000004) AS times ON I_D117502UserInformation.workerNumber= times.F0000004 WHERE times.countTAOCAN >= 1";
            string sqlLD = "SELECT ObjectId, userName, workerNumber, times.countLINGDIAN FROM I_D117502UserInformation LEFT JOIN (SELECT F0000003, COUNT(1) AS countLINGDIAN FROM I_D117502vva15fytdurizrzn4cgba7i03 WHERE I_D117502vva15fytdurizrzn4cgba7i03.F0000010 > 0 GROUP BY I_D117502vva15fytdurizrzn4cgba7i03.F0000003) AS times ON I_D117502UserInformation.workerNumber= times.F0000003 WHERE times.countLINGDIAN >= 1";
            System.Data.DataTable dtAccountZZ = this.Request.Engine.Query.QueryTable(sqlZZ, null);
            System.Data.DataTable dtAccountTC = this.Request.Engine.Query.QueryTable(sqlTC, null);
            System.Data.DataTable dtAccountLD = this.Request.Engine.Query.QueryTable(sqlLD, null);
            int dtAccountCountZZ = dtAccountZZ.Rows.Count;
            int dtAccountCountTC = dtAccountTC.Rows.Count;
            int dtAccountCountLD = dtAccountLD.Rows.Count;

            string systemUserId = H3.Organization.User.SystemUserId;
            H3.IEngine engine = this.Request.Engine;
            H3.DataModel.BulkCommit commitT = new H3.DataModel.BulkCommit();
            H3.DataModel.BizObjectSchema aSchema = engine.BizObjectManager.GetPublishedSchema("D117502totalSub");
            for(int i = 0;i < dtAccountCountZZ; i++)
            {
                H3.DataModel.BizObject aBo = new H3.DataModel.BizObject(engine, aSchema, systemUserId);
                aBo.CreatedBy = systemUserId;
                aBo.OwnerId = systemUserId;
                aBo["user"] = dtAccountZZ.Rows[i]["ObjectId"];
                aBo["useName"] = dtAccountZZ.Rows[i]["userName"];
                aBo["workNumber"] = dtAccountZZ.Rows[i]["workerNumber"];
                aBo["times"] = dtAccountZZ.Rows[i]["countZIZHU"];
                aBo["remarks"] = "自助餐，氚云合计值";
                aBo.Status = H3.DataModel.BizObjectStatus.Effective;
                aBo.Create(commitT);
            }
            for(int i = 0;i < dtAccountCountZZ; i++)
            {
                H3.DataModel.BizObject aBo = new H3.DataModel.BizObject(engine, aSchema, systemUserId);
                aBo.CreatedBy = systemUserId;
                aBo.OwnerId = systemUserId;
                aBo["user"] = dtAccountTC.Rows[i]["ObjectId"];
                aBo["useName"] = dtAccountTC.Rows[i]["userName"];
                aBo["workNumber"] = dtAccountTC.Rows[i]["workerNumber"];
                aBo["times"] = dtAccountTC.Rows[i]["countTAOCAN"];
                aBo["remarks"] = "套餐，氚云合计值";
                aBo.Status = H3.DataModel.BizObjectStatus.Effective;
                aBo.Create(commitT);
            }
            for(int i = 0;i < dtAccountCountZZ; i++)
            {
                H3.DataModel.BizObject aBo = new H3.DataModel.BizObject(engine, aSchema, systemUserId);
                aBo.CreatedBy = systemUserId;
                aBo.OwnerId = systemUserId;
                aBo["user"] = dtAccountLD.Rows[i]["ObjectId"];
                aBo["useName"] = dtAccountLD.Rows[i]["userName"];
                aBo["workNumber"] = dtAccountLD.Rows[i]["workerNumber"];
                aBo["times"] = dtAccountLD.Rows[i]["countLINGDIAN"];
                aBo["remarks"] = "零点，氚云合计值";
                aBo.Status = H3.DataModel.BizObjectStatus.Effective;
                aBo.Create(commitT);
            }
            string errorMsg = null;
            commitT.Commit(engine.BizObjectManager, out errorMsg);
            response.Message = "操作成功!自助餐：" + dtAccountCountZZ.ToString() + "；零点：" + dtAccountCountLD.ToString() + "；套餐：" + dtAccountCountTC.ToString() + "。";
        }
        if(actionName == "DELETE")
        {
            System.Data.DataTable dtAccount = this.Request.Engine.Query.QueryTable("DELETE FROM I_D117502totalSub", null);
            response.Message = "操作成功!";
        }
        if(actionName == "res")
        {
            string sqltc = "SELECT ObjectId, userName, workerNumber, times.countTAOCAN FROM I_D117502UserInformation LEFT JOIN (SELECT user, COUNT(1) AS countTAOCAN FROM I_D117502OrderPackages WHERE I_D117502OrderPackages.subsidyafterChange = 0 GROUP BY I_D117502OrderPackages.user) AS times ON I_D117502UserInformation.ObjectId= times.user WHERE times.countTAOCAN >= 1";
            string sql1ld = "SELECT ObjectId, userName, workerNumber, times.countLINGDIAN FROM I_D117502UserInformation LEFT JOIN (SELECT userObjectId, COUNT(1) AS countLINGDIAN FROM I_D117502OrdersCommodity WHERE I_D117502OrdersCommodity.subsidyafterChange = 0 GROUP BY I_D117502OrdersCommodity.userObjectId) AS times ON I_D117502UserInformation.ObjectId= times.userObjectId WHERE times.countLINGDIAN >= 1";
            string sql1zz = "SELECT ObjectId, userName, workerNumber, times.countZZ FROM I_D117502UserInformation LEFT JOIN (SELECT JobNumber, COUNT(1) AS countZZ FROM I_D117502OrdersBuffet WHERE I_D117502OrdersBuffet.subsidyPayment = 10 GROUP BY I_D117502OrdersBuffet.JobNumber) AS times ON I_D117502UserInformation.ObjectId= times.JobNumber WHERE times.countZZ >= 1";
            System.Data.DataTable dtAccountTC = this.Request.Engine.Query.QueryTable(sqltc, null);
            System.Data.DataTable dtAccountLD = this.Request.Engine.Query.QueryTable(sql1ld, null);
            System.Data.DataTable dtAccountZZ = this.Request.Engine.Query.QueryTable(sql1zz, null);
            int CountTC = dtAccountTC.Rows.Count;
            int CountLD = dtAccountLD.Rows.Count;
            int CountZZ = dtAccountZZ.Rows.Count;
            string systemUserId = H3.Organization.User.SystemUserId;
            H3.IEngine engine = this.Request.Engine;

            H3.DataModel.BulkCommit commitT = new H3.DataModel.BulkCommit();
            H3.DataModel.BizObjectSchema aSchema = engine.BizObjectManager.GetPublishedSchema("D117502totalSub");
            for(int i = 0;i < CountTC; i++)
            {
                H3.DataModel.BizObject aBo = new H3.DataModel.BizObject(engine, aSchema, systemUserId);
                aBo.CreatedBy = systemUserId;
                aBo.OwnerId = systemUserId;
                aBo["user"] = dtAccountTC.Rows[i]["ObjectId"];
                aBo["useName"] = dtAccountTC.Rows[i]["userName"];
                aBo["workNumber"] = dtAccountTC.Rows[i]["workerNumber"];
                aBo["times"] = dtAccountTC.Rows[i]["countTAOCAN"];
                aBo["remarks"] = "套餐，小程序合计值";
                aBo.Status = H3.DataModel.BizObjectStatus.Effective;
                aBo.Create(commitT);
            }
            for(int i = 0;i < CountLD; i++)
            {
                H3.DataModel.BizObject aBo = new H3.DataModel.BizObject(engine, aSchema, systemUserId);
                aBo.CreatedBy = systemUserId;
                aBo.OwnerId = systemUserId;
                aBo["user"] = dtAccountLD.Rows[i]["ObjectId"];
                aBo["useName"] = dtAccountLD.Rows[i]["userName"];
                aBo["workNumber"] = dtAccountLD.Rows[i]["workerNumber"];
                aBo["times"] = dtAccountLD.Rows[i]["countLINGDIAN"];
                aBo["remarks"] = "零点，小程序合计值";
                aBo.Status = H3.DataModel.BizObjectStatus.Effective;
                aBo.Create(commitT);
            }
            for(int i = 0;i < CountZZ; i++)
            {
                H3.DataModel.BizObject aBo = new H3.DataModel.BizObject(engine, aSchema, systemUserId);
                aBo.CreatedBy = systemUserId;
                aBo.OwnerId = systemUserId;
                aBo["user"] = dtAccountZZ.Rows[i]["ObjectId"];
                aBo["useName"] = dtAccountZZ.Rows[i]["userName"];
                aBo["workNumber"] = dtAccountZZ.Rows[i]["workerNumber"];
                aBo["times"] = dtAccountZZ.Rows[i]["countZZ"];
                aBo["remarks"] = "自助餐，小程序合计值";
                aBo.Status = H3.DataModel.BizObjectStatus.Effective;
                aBo.Create(commitT);
            }
            string errorMsg = null;
            commitT.Commit(engine.BizObjectManager, out errorMsg);
            response.Message = "操作成功!套餐：" + CountTC.ToString() + "；零点：" + CountLD.ToString() + "；自助：" + CountZZ.ToString() + "条数据";
        }
        // if(actionName == "five")
        // {
        //     string systemUserId = H3.Organization.User.SystemUserId;
        //     DateTime dt1 = new DateTime(2022, 7, 11);
        //     DateTime dt2 = new DateTime(2022, 7, 12);
        //     DateTime dt3 = new DateTime(2022, 7, 13);
        //     DateTime dt4 = new DateTime(2022, 7, 14);
        //     DateTime dt5 = new DateTime(2022, 7, 15);
        //     DateTime dt6 = new DateTime(2022, 7, 16);
        //     H3.DataModel.BizObjectSchema schema = this.Request.Engine.BizObjectManager.GetPublishedSchema("D117502employeeSuspensionSet");

        //     H3.Data.Filter.Filter filter2 = new H3.Data.Filter.Filter();
        //     H3.Data.Filter.And andMatcher2 = new H3.Data.Filter.And();
        //     andMatcher2.Add(new H3.Data.Filter.ItemMatcher("startDate", H3.Data.ComparisonOperatorType.Above, dt1));
        //     andMatcher2.Add(new H3.Data.Filter.ItemMatcher("endDate", H3.Data.ComparisonOperatorType.Below, dt5));
        //     filter2.Matcher = andMatcher2;
        //     H3.DataModel.BizObject[] boArray2 = H3.DataModel.BizObject.GetList(this.Request.Engine, systemUserId, schema, H3.DataModel.GetListScopeType.GlobalAll, filter2);

        //     H3.Data.Filter.Filter filter = new H3.Data.Filter.Filter();
        //     H3.Data.Filter.And andMatcher = new H3.Data.Filter.And();
        //     andMatcher.Add(new H3.Data.Filter.ItemMatcher("startDate", H3.Data.ComparisonOperatorType.NotAbove, dt1));
        //     andMatcher.Add(new H3.Data.Filter.ItemMatcher("endDate", H3.Data.ComparisonOperatorType.NotBelow, dt5));
        //     filter.Matcher = andMatcher;
        //     H3.DataModel.BizObject[] boArray = H3.DataModel.BizObject.GetList(this.Request.Engine, systemUserId, schema, H3.DataModel.GetListScopeType.GlobalAll, filter);

        //     H3.Data.Filter.Filter filter1 = new H3.Data.Filter.Filter();
        //     H3.Data.Filter.Or andMatcher1 = new H3.Data.Filter.Or();
        //     andMatcher1.Add(new H3.Data.Filter.ItemMatcher("startDate", H3.Data.ComparisonOperatorType.Equal, dt1));
        //     andMatcher1.Add(new H3.Data.Filter.ItemMatcher("startDate", H3.Data.ComparisonOperatorType.Equal, dt5));
        //     andMatcher1.Add(new H3.Data.Filter.ItemMatcher("endDate", H3.Data.ComparisonOperatorType.Equal, dt1));
        //     andMatcher1.Add(new H3.Data.Filter.ItemMatcher("endDate", H3.Data.ComparisonOperatorType.Equal, dt5));
        //     filter1.Matcher = andMatcher1;
        //     H3.DataModel.BizObject[] boArray1 = H3.DataModel.BizObject.GetList(this.Request.Engine, systemUserId, schema, H3.DataModel.GetListScopeType.GlobalAll, filter1);

        //     if(boArray2 != null && boArray2.Length > 0)
        //     {
        //         int timers = 1;
        //         H3.DataModel.BizObjectSchema aSchema = this.Request.Engine.BizObjectManager.GetPublishedSchema("D117502totalSub");
        //         for(int i = 0;i < boArray2.Length; i++)
        //         {
        //             if(dt1.CompareTo(boArray2[i]["startDate"]) >= 0 && dt1.CompareTo(boArray2[i]["endDate"]) == -1)
        //             {
        //                 timers += 1;
        //             }
        //             if(dt2.CompareTo(boArray2[i]["startDate"]) >= 0 && dt2.CompareTo(boArray2[i]["endDate"]) == -1)
        //             {
        //                 timers += 1;
        //             }
        //             if(dt3.CompareTo(boArray2[i]["startDate"]) >= 0 && dt3.CompareTo(boArray2[i]["endDate"]) == -1)
        //             {
        //                 timers += 1;
        //             }
        //             if(dt4.CompareTo(boArray2[i]["startDate"]) >= 0 && dt4.CompareTo(boArray2[i]["endDate"]) == -1)
        //             {
        //                 timers += 1;
        //             }
        //             if(dt5.CompareTo(boArray2[i]["startDate"]) >= 0 && dt5.CompareTo(boArray2[i]["endDate"]) == -1)
        //             {
        //                 timers += 1;
        //             }
        //             H3.DataModel.BizObject aBo = new H3.DataModel.BizObject(this.Request.Engine, aSchema, systemUserId);
        //             aBo.CreatedBy = systemUserId;
        //             aBo.OwnerId = systemUserId;
        //             aBo["user"] = boArray2[i]["JobNumber"];
        //             aBo["useName"] = boArray2[i]["staffName"];
        //             aBo["workNumber"] = boArray2[i]["Name"];
        //             aBo["times"] = timers;
        //             aBo["remarks"] = "根据CompareTo得出的本周请假天数";
        //             aBo.Status = H3.DataModel.BizObjectStatus.Effective;
        //             aBo.Create();
        //             timers = 1;
        //         }
        //     }
        //     if(boArray != null && boArray.Length > 0)
        //     {
        //         for(int j = 0;j < boArray.Length; j++)
        //         {
        //             H3.DataModel.BizObjectSchema aSchema = this.Request.Engine.BizObjectManager.GetPublishedSchema("D117502totalSub");
        //             H3.DataModel.BizObject aBo = new H3.DataModel.BizObject(this.Request.Engine, aSchema, systemUserId);
        //             aBo.CreatedBy = systemUserId;
        //             aBo.OwnerId = systemUserId;
        //             aBo["user"] = boArray[j]["JobNumber"];
        //             aBo["useName"] = boArray[j]["staffName"];
        //             aBo["workNumber"] = boArray[j]["Name"];
        //             aBo["times"] = 5;
        //             aBo["remarks"] = "从周一到周五都请假的职工";
        //             aBo.Status = H3.DataModel.BizObjectStatus.Effective;
        //             aBo.Create();
        //         }
        //     }
        //     if(boArray1 != null && boArray1.Length > 0)
        //     {
        //         for(int k = 0;k < boArray1.Length; k++)
        //         {
        //             H3.DataModel.BizObjectSchema aSchema = this.Request.Engine.BizObjectManager.GetPublishedSchema("D117502totalSub");
        //             H3.DataModel.BizObject aBo = new H3.DataModel.BizObject(this.Request.Engine, aSchema, systemUserId);
        //             aBo.CreatedBy = systemUserId;
        //             aBo.OwnerId = systemUserId;
        //             aBo["user"] = boArray1[k]["JobNumber"];
        //             aBo["useName"] = boArray1[k]["staffName"];
        //             aBo["workNumber"] = boArray1[k]["Name"];
        //             aBo["times"] = 1;
        //             aBo["remarks"] = "只有周一或者周五请假一天的职工";
        //             aBo.Status = H3.DataModel.BizObjectStatus.Effective;
        //             aBo.Create();
        //         }
        //     }
        //     response.Message = "操作成功! ";
        // }
        if(actionName == "five")
        {
            string systemUserId = H3.Organization.User.SystemUserId;
            DateTime dt1 = new DateTime(2022, 7, 11);
            DateTime dt2 = new DateTime(2022, 7, 12);
            DateTime dt3 = new DateTime(2022, 7, 13);
            DateTime dt4 = new DateTime(2022, 7, 14);
            DateTime dt5 = new DateTime(2022, 7, 15);
            H3.DataModel.BizObjectSchema schema = this.Request.Engine.BizObjectManager.GetPublishedSchema("D117502employeeSuspensionSet");

            H3.Data.Filter.Filter filter2 = new H3.Data.Filter.Filter();
            H3.Data.Filter.And andMatcher2 = new H3.Data.Filter.And();
            filter2.Matcher = andMatcher2;
            H3.DataModel.BizObject[] boArray2 = H3.DataModel.BizObject.GetList(this.Request.Engine, systemUserId, schema, H3.DataModel.GetListScopeType.GlobalAll, filter2);

            if(boArray2 != null && boArray2.Length > 0)
            {
                int timers = 1;
                H3.DataModel.BizObjectSchema aSchema = this.Request.Engine.BizObjectManager.GetPublishedSchema("D117502totalSub");
                for(int i = 0;i < boArray2.Length; i++)
                {
                    if(dt1.CompareTo(boArray2[i]["startDate"]) >= 0 && dt1.CompareTo(boArray2[i]["endDate"]) == -1)
                    {
                        timers += 1;
                    }
                    if(dt2.CompareTo(boArray2[i]["startDate"]) >= 0 && dt2.CompareTo(boArray2[i]["endDate"]) == -1)
                    {
                        timers += 1;
                    }
                    if(dt3.CompareTo(boArray2[i]["startDate"]) >= 0 && dt3.CompareTo(boArray2[i]["endDate"]) == -1)
                    {
                        timers += 1;
                    }
                    if(dt4.CompareTo(boArray2[i]["startDate"]) >= 0 && dt4.CompareTo(boArray2[i]["endDate"]) == -1)
                    {
                        timers += 1;
                    }
                    H3.DataModel.BizObject aBo = new H3.DataModel.BizObject(this.Request.Engine, aSchema, systemUserId);
                    aBo.CreatedBy = systemUserId;
                    aBo.OwnerId = systemUserId;
                    aBo["user"] = boArray2[i]["JobNumber"];
                    aBo["useName"] = boArray2[i]["staffName"];
                    aBo["workNumber"] = boArray2[i]["Name"];
                    aBo["times"] = timers;
                    aBo["remarks"] = "根据CompareTo得出的本周请假天数";
                    aBo.Status = H3.DataModel.BizObjectStatus.Effective;
                    aBo.Create();
                    timers = 1;
                }
            }
            response.Message = "操作成功! 共计：" + boArray2.Length.ToString() + "人次";
        }
    }
}