/*
 * **************************************************************************
 * ********************                                  ********************
 * ********************      佛祖镇楼       BUG辟易       ********************
 * ********************                                  ********************
 * **************************************************************************
 *                                                                          *
 *                                   _oo8oo_                                *
 *                                  o8888888o                               *
 *                                  88" . "88                               *
 *                                  (| -_- |)                               *
 *                                  0\  =  /0                               *
 *                                ___/'==='\___                             *
 *                              .' \\|     |// '.                           *
 *                             / \\|||  :  |||// \                          *
 *                            / _||||| -:- |||||_ \                         *
 *                           |   | \\\  -  /// |   |                        *
 *                           | \_|  ''\---/''  |_/ |                        *
 *                           \  .-\__  '-'  __/-.  /                        *
 *                         ___'. .'  /--.--\  '. .'___                      *
 *                      ."" '<  '.___\_<|>_/___.'  >' "".                   *
 *                     | | :  `- \`.:`\ _ /`:.`/ -`  : | |                  *
 *                     \  \ `-.   \_ __\ /__ _/   .-` /  /                  *
 *                 =====`-.____`.___ \_____/ ___.`____.-`=====              *
 *                                   `=---=`                                *
 * **************************************************************************
 * ********************      				             ********************
 * ********************      佛祖镇楼       BUG辟易        ********************
 * ********************                                  ********************
 * **************************************************************************
 */
using System;
using System.Collections.Generic;
using System.Text;
using H3;

public class D117502interval: H3.SmartForm.SmartFormController
{
    public D117502interval(H3.SmartForm.SmartFormRequest request): base(request)
    {
    }

    protected override void OnLoad(H3.SmartForm.LoadSmartFormResponse response)
    {
        base.OnLoad(response);
        response.Actions.Remove("Remove");
        response.Actions.Remove("Print");
        response.Actions.Remove("ViewQrCode");
        //补贴的同时调用外部接口 同步补贴金额
        // H3.BizBus.BizStructureSchema contextSchema = new H3.BizBus.BizStructureSchema();
        // H3.BizBus.BizStructureSchema msgSchema = new H3.BizBus.BizStructureSchema();
        // H3.BizBus.BizStructureSchema structureSchema = new H3.BizBus.BizStructureSchema();
        // Dictionary < string, string > querys=new Dictionary<string, string>();
        // string objectid = "62a0917d-d0a7-44e5-9b92-6eb8f29c4844";//主键
        // querys.Add("objectid", objectid);
        // querys.Add("Money", "10");
        // //调用Invoke接口，系统底层访问第三方WebService接口的Invoke方法
        // H3.BizBus.InvokeResult InResult = this.Engine.BizBus.InvokeApi(H3.Organization.User.SystemUserId, H3.BizBus.AccessPointType.ThirdConnection,
        //     "UpdateUsersSubsidy", "GET", "application/x-www-form-urlencoded;charset=UTF-8", null, querys, null, structureSchema);
        new Subsidies().StatisticsSubsidyOut(this.Engine);
    }

    protected override void OnSubmit(string actionName, H3.SmartForm.SmartFormPostValue postValue, H3.SmartForm.SubmitSmartFormResponse response)
    {
        base.OnSubmit(actionName, postValue, response);
    }
    public class Subsidies: H3.SmartForm.Timer
    {
        public Subsidies() { }
        protected override void OnWork(H3.IEngine engine)
        {
            DateTime now = DateTime.Now;
            DateTime sTime = DateTime.Parse(now.ToString("yyyy-MM-dd 00:00:00"));
            DateTime eTime = DateTime.Parse(now.ToString("yyyy-MM-dd 04:00:00"));
            DateTime sTime2 = DateTime.Parse(now.ToString("yyyy-MM-dd 01:00:00"));
            DateTime eTime2 = DateTime.Parse(now.ToString("yyyy-MM-dd 05:00:00"));
            int week = Convert.ToInt32(DateTime.Now.DayOfWeek);
            if(sTime <= now && eTime >= now && week != 6 && week != 0) 
            {
                dos(engine);
            }
            if(sTime2 <= now && eTime2 >= now && week != 6 && week != 0)
            {
                dos2(engine);
            }
            if(sTime <= now && eTime >= now)
            {
                Statistics(engine);
            }
            if(sTime <= now && eTime >= now && (week == 6 || week == 0))
            {
                StatisticsSubsidyOut(engine);
            }
            if(sTime2 <= now && eTime2 >= now && (week == 6 || week == 0))
            {
                StatisticsSubsidyIn(engine);
            }
        }
        public void dos(H3.IEngine engine)
        {
            string systemUserId = H3.Organization.User.SystemUserId;
            for(int j = 1;j <= 3;)
            {
                System.Globalization.GregorianCalendar gc = new System.Globalization.GregorianCalendar();
                int weekOfYear = gc.GetWeekOfYear(DateTime.Now, System.Globalization.CalendarWeekRule.FirstDay, DayOfWeek.Monday);
                H3.DataModel.BizObjectSchema logSchema = engine.BizObjectManager.GetPublishedSchema("D117502interval");
                H3.DataModel.BizObject log = new H3.DataModel.BizObject(engine, logSchema, systemUserId);
                log.CreatedBy = systemUserId;
                log.OwnerId = systemUserId;
                H3.DataModel.BulkCommit fcommit = new H3.DataModel.BulkCommit();//补贴金额归零
                DateTime now = DateTime.Now;
                H3.Data.Filter.Filter filter = new H3.Data.Filter.Filter(); //构建过滤器
                filter.FromRowNum = (j - 1) * 1000;
                filter.ToRowNum = j * 1000;
                H3.Data.Filter.And andMatcher = new H3.Data.Filter.And();    //构造And匹配器
                andMatcher.Add(new H3.Data.Filter.ItemMatcher("startUsing", H3.Data.ComparisonOperatorType.Equal, "True"));  //添加筛选条件
                filter.Matcher = andMatcher;
                H3.DataModel.BizObjectSchema schema = engine.BizObjectManager.GetPublishedSchema("D117502UserInformation");   //获取模块Schema
                H3.DataModel.BizObject[] boArray = H3.DataModel.BizObject.GetList(engine, H3.Organization.User.SystemUserId, schema, H3.DataModel.GetListScopeType.GlobalAll, filter); //查询返回的结果对象
                if(boArray != null && boArray.Length > 0)
                {
                    for(int i = 0;i < boArray.Length; i++)
                    {
                        H3.DataModel.BizObject bo = boArray[i];
                        //创建表单数据
                        H3.DataModel.BizObjectSchema aSchema = engine.BizObjectManager.GetPublishedSchema("D117502SubsidyRechargeLOG");
                        H3.DataModel.BizObject aBo = new H3.DataModel.BizObject(engine, aSchema, systemUserId);
                        //归零
                        aBo.CreatedBy = H3.Organization.User.SystemUserId;
                        aBo["EmployeeCode"] = bo["ObjectId"].ToString();// 工号
                        aBo["telphoneNumber"] = bo["telphoneNumber"].ToString();// 手机号
                        aBo["employeeName"] = bo["userName"].ToString();// 姓名
                        aBo["BeforeRecharge"] = bo["subsidyBalance"].ToString();// 
                        aBo["RechargeAmount"] = (0 - double.Parse(bo["subsidyBalance"].ToString())).ToString();
                        aBo["AfterRecharge"] = "0";
                        aBo["workerNumber"] = bo["workerNumber"].ToString(); // 工号T
                        aBo["RechargeTime"] = now + string.Empty;
                        aBo["rechargeType"] = "归零";
                        aBo["week"] = weekOfYear;
                        aBo.Status = H3.DataModel.BizObjectStatus.Effective;
                        aBo.Create(fcommit);
                        //补贴的同时调用外部接口 同步补贴金额
                        H3.BizBus.BizStructureSchema contextSchema = new H3.BizBus.BizStructureSchema();
                        H3.BizBus.BizStructureSchema msgSchema = new H3.BizBus.BizStructureSchema();
                        H3.BizBus.BizStructureSchema structureSchema = new H3.BizBus.BizStructureSchema();
                        Dictionary < string, string > querys=new Dictionary<string, string>();
                        string objectid = bo["ObjectId"].ToString();//主键
                        querys.Add("objectid", objectid);
                        querys.Add("Money", "0");
                        //调用Invoke接口，系统底层访问第三方WebService接口的Invoke方法
                        H3.BizBus.InvokeResult InResult = engine.BizBus.InvokeApi(H3.Organization.User.SystemUserId, H3.BizBus.AccessPointType.ThirdConnection,
                            "UpdateUsersSubsidy", "GET", "application/x-www-form-urlencoded;charset=UTF-8", null, querys, null, structureSchema);
                    }
                    string errorMsg = null;
                    fcommit.Commit(engine.BizObjectManager, out errorMsg);
                    log["CreatedTime"] = now.ToString();
                    log["Content"] = "工作日-归零；第" + ((j * 1000).ToString()) + "条";
                    log["log"] = errorMsg;
                    log.Status = H3.DataModel.BizObjectStatus.Effective;
                    log.Create();
                }
                j++;
            }
        }

        public void dos2(H3.IEngine engine)
        {
            string systemUserId = H3.Organization.User.SystemUserId;
            for(int j = 1;j <= 3;)
            {
                System.Globalization.GregorianCalendar gc = new System.Globalization.GregorianCalendar();
                int weekOfYear = gc.GetWeekOfYear(DateTime.Now, System.Globalization.CalendarWeekRule.FirstDay, DayOfWeek.Monday);
                H3.DataModel.BizObjectSchema logSchema = engine.BizObjectManager.GetPublishedSchema("D117502interval");
                H3.DataModel.BizObject log = new H3.DataModel.BizObject(engine, logSchema, systemUserId);
                log.CreatedBy = systemUserId;
                log.OwnerId = systemUserId;
                H3.DataModel.BulkCommit  commit = new H3.DataModel.BulkCommit();//补贴金额充值
                DateTime now = DateTime.Now;//获取当前时间
                H3.Data.Filter.Filter filter = new H3.Data.Filter.Filter(); //构建过滤器
                filter.FromRowNum = (j - 1) * 1000;
                filter.ToRowNum = j * 1000;
                H3.Data.Filter.And   andMatcher = new H3.Data.Filter.And();    //构造And匹配器
                andMatcher.Add(new H3.Data.Filter.ItemMatcher("startUsing", H3.Data.ComparisonOperatorType.Equal, "True"));  //添加筛选条件
                filter.Matcher = andMatcher;
                H3.DataModel.BizObjectSchema schema = engine.BizObjectManager.GetPublishedSchema("D117502UserInformation");   //获取模块Schema
                H3.DataModel.BizObject[] boArray = H3.DataModel.BizObject.GetList(engine, H3.Organization.User.SystemUserId, schema, H3.DataModel.GetListScopeType.GlobalAll, filter); //查询返回的结果对象
                if(boArray != null && boArray.Length > 0)
                {
                    for(int i = 0;i < boArray.Length; i++)
                    {
                        H3.DataModel.BizObject bo = boArray[i];
                        //创建表单数据
                        H3.DataModel.BizObjectSchema aSchema = engine.BizObjectManager.GetPublishedSchema("D117502SubsidyRechargeLOG");
                        H3.DataModel.BizObject Bo = new H3.DataModel.BizObject(engine, aSchema, systemUserId);
                        Bo.CreatedBy = H3.Organization.User.SystemUserId;
                        Bo["EmployeeCode"] = bo["ObjectId"].ToString();//工号 关联表单
                        Bo["telphoneNumber"] = bo["telphoneNumber"].ToString();//手机号
                        Bo["employeeName"] = bo["userName"].ToString();//姓名
                        Bo["workerNumber"] = bo["workerNumber"].ToString(); // 工号T
                        Bo["BeforeRecharge"] = bo["subsidyBalance"].ToString();
                        if(bo["isMoratorium"].ToString() == "True")
                        {
                            Bo["RechargeAmount"] = "0";
                            Bo["AfterRecharge"] = "0";
                        } else
                        {
                            Bo["RechargeAmount"] = "10";
                            Bo["AfterRecharge"] = "10";
                        }
                        Bo["RechargeTime"] = now + string.Empty;
                        Bo["rechargeType"] = "充值";
                        Bo["week"] = weekOfYear;
                        Bo.Status = H3.DataModel.BizObjectStatus.Effective;
                        Bo.Create(commit);
                        //补贴的同时调用外部接口 同步补贴金额
                        H3.BizBus.BizStructureSchema contextSchema = new H3.BizBus.BizStructureSchema();
                        H3.BizBus.BizStructureSchema msgSchema = new H3.BizBus.BizStructureSchema();
                        H3.BizBus.BizStructureSchema structureSchema = new H3.BizBus.BizStructureSchema();
                        Dictionary < string, string > querys=new Dictionary<string, string>();
                        string objectid = bo["ObjectId"].ToString();//主键
                        querys.Add("objectid", objectid);
                        querys.Add("Money", "10");
                        //调用Invoke接口，系统底层访问第三方WebService接口的Invoke方法
                        H3.BizBus.InvokeResult InResult = engine.BizBus.InvokeApi(H3.Organization.User.SystemUserId, H3.BizBus.AccessPointType.ThirdConnection,
                            "UpdateUsersSubsidy", "GET", "application/x-www-form-urlencoded;charset=UTF-8", null, querys, null, structureSchema);
                    }
                    string errorMsg = null;
                    commit.Commit(engine.BizObjectManager, out errorMsg);
                    log["CreatedTime"] = now.ToString();
                    log["Content"] = "工作日-充值；第" + ((j * 1000).ToString()) + "条";
                    log["log2"] = errorMsg;
                    log.Status = H3.DataModel.BizObjectStatus.Effective;
                    log.Create();
                }
                j++;
            }
        }
        public void Statistics(H3.IEngine engine)
        {
            System.Globalization.GregorianCalendar gc = new System.Globalization.GregorianCalendar();
            int weekOfYear = gc.GetWeekOfYear(DateTime.Now, System.Globalization.CalendarWeekRule.FirstDay, DayOfWeek.Monday);
            int weekDay = Convert.ToInt32(DateTime.Now.DayOfWeek);
            weekOfYear = weekDay == 0 ? weekOfYear - 1 : weekOfYear;
            string yesterdayDate = DateTime.Now.AddDays(-1).ToShortDateString();
            string sql1 = "SELECT userObjectId FROM i_D117502SubsidyRecords WHERE TO_DAYS(NOW()) - TO_DAYS(changeTime) = 1 AND i_D117502SubsidyRecords.changeAmount > 0 AND i_D117502SubsidyRecords.changeType = '消费'";
            System.Data.DataTable Countb = engine.Query.QueryTable(sql1, null);
            int RowsCount = Countb.Rows.Count;
            string systemUserId = H3.Organization.User.SystemUserId;
            H3.DataModel.BulkCommit commit = new H3.DataModel.BulkCommit();
            for(int i = 0;i < RowsCount; i++)
            {
                H3.DataModel.BizObjectSchema aSchema = engine.BizObjectManager.GetPublishedSchema("D117502statistics");
                H3.DataModel.BizObject statistics = new H3.DataModel.BizObject(engine, aSchema, systemUserId);
                statistics.CreatedBy = systemUserId;
                statistics.OwnerId = systemUserId;
                statistics["week"] = weekOfYear;
                statistics["date"] = yesterdayDate;
                statistics["times"] = 1;
                statistics["userObjectId"] = Countb.Rows[i]["userObjectId"];
                statistics.Status = H3.DataModel.BizObjectStatus.Effective;
                statistics.Create(commit);
            }
            string errorMsg = null;
            commit.Commit(engine.BizObjectManager, out errorMsg);
            DateTime now = DateTime.Now;
            H3.DataModel.BizObjectSchema Schema = engine.BizObjectManager.GetPublishedSchema("D117502interval");
            H3.DataModel.BizObject LOG = new H3.DataModel.BizObject(engine, Schema, systemUserId);
            LOG.CreatedBy = H3.Organization.User.SystemUserId;
            LOG.OwnerId = H3.Organization.User.SystemUserId;
            LOG["Content"] = "日常统计昨日的补贴消费次数；共计：" + RowsCount.ToString() + "条";
            LOG.Status = H3.DataModel.BizObjectStatus.Effective;
            LOG.Create();
        }
        public void StatisticsSubsidyOut(H3.IEngine engine)
        {
            System.Globalization.GregorianCalendar gc = new System.Globalization.GregorianCalendar();
            int weekOfYear = gc.GetWeekOfYear(DateTime.Now, System.Globalization.CalendarWeekRule.FirstDay, DayOfWeek.Monday);
            int weekDay = Convert.ToInt32(DateTime.Now.DayOfWeek);
            weekOfYear = weekDay == 0 ? weekOfYear - 1 : weekOfYear;
            string systemUserId = H3.Organization.User.SystemUserId;
            H3.DataModel.BizObjectSchema logSchema = engine.BizObjectManager.GetPublishedSchema("D117502interval");
            H3.DataModel.BizObject log = new H3.DataModel.BizObject(engine, logSchema, systemUserId);
            string sql1 = "SELECT ObjectId, userName, workerNumber, telphoneNumber, subsidyBalance, times.count FROM I_D117502UserInformation LEFT JOIN (SELECT userObjectId, COUNT(1) AS count FROM I_D117502statistics WHERE I_D117502statistics.week = " + weekOfYear + " GROUP BY I_D117502statistics.userObjectId) AS times ON I_D117502UserInformation.ObjectId = times.userObjectId WHERE times.count < 5";
            System.Data.DataTable CountA = engine.Query.QueryTable(sql1, null);
            int num = CountA.Rows.Count;
            H3.DataModel.BulkCommit commitT = new H3.DataModel.BulkCommit();
            H3.DataModel.BizObjectSchema aSchema = engine.BizObjectManager.GetPublishedSchema("D117502SubsidyRechargeLOG");
            for(int i = 0;i < num; i++)
            {
                H3.DataModel.BizObject SubsidyRecharg = new H3.DataModel.BizObject(engine, aSchema, systemUserId);
                SubsidyRecharg.CreatedBy = H3.Organization.User.SystemUserId;
                SubsidyRecharg["EmployeeCode"] = CountA.Rows[i]["ObjectId"].ToString();
                SubsidyRecharg["telphoneNumber"] = CountA.Rows[i]["telphoneNumber"].ToString();
                SubsidyRecharg["employeeName"] = CountA.Rows[i]["userName"].ToString();
                SubsidyRecharg["BeforeRecharge"] = CountA.Rows[i]["subsidyBalance"].ToString();
                SubsidyRecharg["RechargeAmount"] = (0 - double.Parse(CountA.Rows[i]["subsidyBalance"].ToString())).ToString();
                SubsidyRecharg["AfterRecharge"] = "0";
                SubsidyRecharg["workerNumber"] = CountA.Rows[i]["workerNumber"].ToString();
                SubsidyRecharg["RechargeTime"] = DateTime.Now + string.Empty;
                SubsidyRecharg["rechargeType"] = "归零";
                SubsidyRecharg["week"] = weekOfYear;
                SubsidyRecharg.Status = H3.DataModel.BizObjectStatus.Effective;
                SubsidyRecharg.Create(commitT);
                //补贴的同时调用外部接口 同步补贴金额
                H3.BizBus.BizStructureSchema contextSchema = new H3.BizBus.BizStructureSchema();
                H3.BizBus.BizStructureSchema msgSchema = new H3.BizBus.BizStructureSchema();
                H3.BizBus.BizStructureSchema structureSchema = new H3.BizBus.BizStructureSchema();
                Dictionary < string, string > querys=new Dictionary<string, string>();
                string objectid = CountA.Rows[i]["ObjectId"].ToString();//主键
                querys.Add("objectid", objectid);
                querys.Add("Money", "0");
                //调用Invoke接口，系统底层访问第三方WebService接口的Invoke方法
                H3.BizBus.InvokeResult InResult = engine.BizBus.InvokeApi(H3.Organization.User.SystemUserId, H3.BizBus.AccessPointType.ThirdConnection,
                    "UpdateUsersSubsidy", "GET", "application/x-www-form-urlencoded;charset=UTF-8", null, querys, null, structureSchema);
            }
            string errorMsg = null;
            commitT.Commit(engine.BizObjectManager, out errorMsg);
            log.CreatedBy = systemUserId;
            log.OwnerId = systemUserId;
            log["CreatedTime"] = DateTime.Now + string.Empty;
            log["Content"] = "周末特供版-归零，共计 :" + num + "条";
            log["log"] = errorMsg;
            log.Status = H3.DataModel.BizObjectStatus.Effective;
            log.Create();
        }
        public void StatisticsSubsidyIn(H3.IEngine engine)
        {
            System.Globalization.GregorianCalendar gc = new System.Globalization.GregorianCalendar();
            int weekOfYear = gc.GetWeekOfYear(DateTime.Now, System.Globalization.CalendarWeekRule.FirstDay, DayOfWeek.Monday);
            int weekDay = Convert.ToInt32(DateTime.Now.DayOfWeek);
            weekOfYear = weekDay == 0 ? weekOfYear - 1 : weekOfYear;
            string systemUserId = H3.Organization.User.SystemUserId;
            H3.DataModel.BizObjectSchema logSchema = engine.BizObjectManager.GetPublishedSchema("D117502interval");
            H3.DataModel.BizObject log = new H3.DataModel.BizObject(engine, logSchema, systemUserId);
            string sql1 = "SELECT ObjectId, userName, workerNumber, telphoneNumber, subsidyBalance, isMoratorium, times.count FROM I_D117502UserInformation LEFT JOIN (SELECT userObjectId, COUNT(1) AS count FROM I_D117502statistics WHERE I_D117502statistics.week = " + weekOfYear + " GROUP BY I_D117502statistics.userObjectId) AS times ON I_D117502UserInformation.ObjectId = times.userObjectId WHERE times.count < 5";
            System.Data.DataTable CountA = engine.Query.QueryTable(sql1, null);
            int num = CountA.Rows.Count;
            H3.DataModel.BulkCommit commitT = new H3.DataModel.BulkCommit();
            H3.DataModel.BizObjectSchema aSchema = engine.BizObjectManager.GetPublishedSchema("D117502SubsidyRechargeLOG");
            for(int i = 0;i < num; i++)
            {
                H3.DataModel.BizObject SubsidyRecharg = new H3.DataModel.BizObject(engine, aSchema, systemUserId);
                SubsidyRecharg.CreatedBy = H3.Organization.User.SystemUserId;
                SubsidyRecharg["EmployeeCode"] = CountA.Rows[i]["ObjectId"].ToString();
                SubsidyRecharg["telphoneNumber"] = CountA.Rows[i]["telphoneNumber"].ToString();
                SubsidyRecharg["employeeName"] = CountA.Rows[i]["userName"].ToString();
                SubsidyRecharg["BeforeRecharge"] = CountA.Rows[i]["subsidyBalance"].ToString();
                if(CountA.Rows[i]["isMoratorium"].ToString() == "1")
                {
                    SubsidyRecharg["RechargeAmount"] = "0";
                    SubsidyRecharg["AfterRecharge"] = "0";
                }
                else
                {
                    SubsidyRecharg["RechargeAmount"] = "10";
                    SubsidyRecharg["AfterRecharge"] = "10";
                }
                SubsidyRecharg["workerNumber"] = CountA.Rows[i]["workerNumber"].ToString();
                SubsidyRecharg["RechargeTime"] = DateTime.Now + string.Empty;
                SubsidyRecharg["rechargeType"] = "充值";
                SubsidyRecharg["week"] = weekOfYear;
                SubsidyRecharg.Status = H3.DataModel.BizObjectStatus.Effective;
                SubsidyRecharg.Create(commitT);
                //补贴的同时调用外部接口 同步补贴金额
                H3.BizBus.BizStructureSchema contextSchema = new H3.BizBus.BizStructureSchema();
                H3.BizBus.BizStructureSchema msgSchema = new H3.BizBus.BizStructureSchema();
                H3.BizBus.BizStructureSchema structureSchema = new H3.BizBus.BizStructureSchema();
                Dictionary < string, string > querys=new Dictionary<string, string>();
                string objectid = CountA.Rows[i]["ObjectId"].ToString();//主键
                querys.Add("objectid", objectid);
                querys.Add("Money", "10");
                //调用Invoke接口，系统底层访问第三方WebService接口的Invoke方法
                H3.BizBus.InvokeResult InResult = engine.BizBus.InvokeApi(H3.Organization.User.SystemUserId, H3.BizBus.AccessPointType.ThirdConnection,
                    "UpdateUsersSubsidy", "GET", "application/x-www-form-urlencoded;charset=UTF-8", null, querys, null, structureSchema);
            }
            string errorMsg = null;
            commitT.Commit(engine.BizObjectManager, out errorMsg);
            log.CreatedBy = systemUserId;
            log.OwnerId = systemUserId;
            log["CreatedTime"] = DateTime.Now + string.Empty;
            log["Content"] = "周末特供版-充值，共计 :" + num + "条";
            log["log"] = errorMsg;
            log.Status = H3.DataModel.BizObjectStatus.Effective;
            log.Create();
        }
    }
}