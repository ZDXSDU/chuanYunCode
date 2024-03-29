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

        H3.IEngine engine = this.Request.Engine;
        string systemUserId = H3.Organization.User.SystemUserId;
        // new Subsidies().StatisticsSubsidyIn(engine);


       
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
            if(sTime <= now && eTime >= now)
            {
                dos1(engine); // 新版补贴归零 + 补贴消费次数统计（每日运行）                
            }

            if(sTime2 <= now && eTime2 >= now && (week == 6 || week == 0))
            {
                StatisticsSubsidyIn(engine); // 周末特供版职工补贴充值（周末运行）
            }
            if(sTime2 <= now && eTime2 >= now && week != 6 && week != 0)
            {
                dos2(engine); // 补贴充值（工作日运行）
            }
        }
        public void dos1(H3.IEngine engine) // 新版补贴归零 + 补贴消费次数统计（每日运行）
        {
            string systemUserId = H3.Organization.User.SystemUserId;
            DateTime now = DateTime.Now;
            System.Globalization.GregorianCalendar gc = new System.Globalization.GregorianCalendar();
            int weekOfYear = gc.GetWeekOfYear(DateTime.Now, System.Globalization.CalendarWeekRule.FirstDay, DayOfWeek.Monday);
            int weekDay = Convert.ToInt32(DateTime.Now.DayOfWeek);
            if(weekDay == 1)
            {
                engine.Query.QueryTable("UPDATE I_D117502UserInformation SET week = " + weekOfYear + ", subsidiesThisWeekNumber = 0, subsidyBalance = 0", null);
                H3.DataModel.BizObjectSchema logSchema = engine.BizObjectManager.GetPublishedSchema("D117502interval");
                H3.DataModel.BizObject log = new H3.DataModel.BizObject(engine, logSchema, systemUserId);
                log.CreatedBy = systemUserId;
                log.OwnerId = systemUserId;
                log["CreatedTime"] = now.ToString();
                log["Content"] = "周一，不统计补贴次数，仅重置当前周和次数";
                log["log"] = "当前周：" + weekOfYear.ToString();
                log.Status = H3.DataModel.BizObjectStatus.Effective;
                log.Create();
            }
            else
            {
                for(int j = 1;j <= 3;)
                {
                    H3.DataModel.BizObjectSchema logSchema = engine.BizObjectManager.GetPublishedSchema("D117502interval");
                    H3.DataModel.BizObject log = new H3.DataModel.BizObject(engine, logSchema, systemUserId);
                    log.CreatedBy = systemUserId;
                    log.OwnerId = systemUserId;
                    H3.DataModel.BulkCommit fcommit = new H3.DataModel.BulkCommit();
                    H3.DataModel.BulkCommit updateCommit = new H3.DataModel.BulkCommit();
                    H3.Data.Filter.Filter filter = new H3.Data.Filter.Filter();
                    filter.FromRowNum = (j - 1) * 1000;
                    filter.ToRowNum = j * 1000;
                    H3.Data.Filter.And andMatcher = new H3.Data.Filter.And();
                    andMatcher.Add(new H3.Data.Filter.ItemMatcher("startUsing", H3.Data.ComparisonOperatorType.Equal, "True"));
                    // andMatcher.Add(new H3.Data.Filter.ItemMatcher("isMoratorium", H3.Data.ComparisonOperatorType.Equal, "False"));
                    filter.Matcher = andMatcher;
                    H3.DataModel.BizObjectSchema schema = engine.BizObjectManager.GetPublishedSchema("D117502UserInformation");
                    H3.DataModel.BizObject[] boArray = H3.DataModel.BizObject.GetList(engine, systemUserId, schema, H3.DataModel.GetListScopeType.GlobalAll, filter);
                    if(boArray != null && boArray.Length > 0)
                    {
                        for(int i = 0;i < boArray.Length; i++)
                        {
                            H3.DataModel.BizObject bo = boArray[i];
                            if((Convert.ToInt32(bo["subsidyBalance"]) < 10) && (bo["isMoratorium"].ToString() == "False"))
                            {
                                string obj = bo["ObjectId"].ToString();
                                H3.DataModel.BizObject accountBo = H3.DataModel.BizObject.Load(systemUserId, engine, "D117502UserInformation", obj, false);
                                accountBo["subsidiesThisWeekNumber"] = Convert.ToInt32(bo["subsidiesThisWeekNumber"]) + 1;
                                accountBo.Status = H3.DataModel.BizObjectStatus.Effective;
                                accountBo.Update(updateCommit);
                            }
                            H3.DataModel.BizObjectSchema aSchema = engine.BizObjectManager.GetPublishedSchema("D117502SubsidyRechargeLOG");
                            H3.DataModel.BizObject aBo = new H3.DataModel.BizObject(engine, aSchema, systemUserId);
                            aBo.CreatedBy = H3.Organization.User.SystemUserId;
                            aBo["EmployeeCode"] = bo["ObjectId"].ToString();
                            aBo["telphoneNumber"] = bo["telphoneNumber"].ToString();
                            aBo["employeeName"] = bo["userName"].ToString();
                            aBo["BeforeRecharge"] = bo["subsidyBalance"].ToString();
                            aBo["RechargeAmount"] = (0 - double.Parse(bo["subsidyBalance"].ToString())).ToString();
                            aBo["AfterRecharge"] = "0";
                            aBo["workerNumber"] = bo["workerNumber"].ToString();
                            aBo["RechargeTime"] = now + string.Empty;
                            aBo["rechargeType"] = "归零";
                            aBo["week"] = weekOfYear;
                            aBo.Status = H3.DataModel.BizObjectStatus.Effective;
                            aBo.Create(fcommit);
                        }
                        string errorMsg = null;
                        updateCommit.Commit(engine.BizObjectManager, out errorMsg);
                        fcommit.Commit(engine.BizObjectManager, out errorMsg);
                        log["CreatedTime"] = now.ToString();
                        log["Content"] = "Daily - 补贴消费统计 + 归零；第" + ((j * 1000).ToString()) + "条";
                        log["log"] = errorMsg;
                        log.Status = H3.DataModel.BizObjectStatus.Effective;
                        log.Create();
                    }
                    j++;
                }
            }
        }
        public void dos2(H3.IEngine engine) // 补贴充值（工作日运行）
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
                H3.DataModel.BulkCommit  commit = new H3.DataModel.BulkCommit();
                DateTime now = DateTime.Now;
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
                        // H3.BizBus.BizStructureSchema contextSchema = new H3.BizBus.BizStructureSchema();
                        // H3.BizBus.BizStructureSchema msgSchema = new H3.BizBus.BizStructureSchema();
                        // H3.BizBus.BizStructureSchema structureSchema = new H3.BizBus.BizStructureSchema();
                        // Dictionary < string, string > querys=new Dictionary<string, string>();
                        // string objectid = bo["ObjectId"].ToString();//主键
                        // querys.Add("objectid", objectid);
                        // querys.Add("Money", "10");
                        // //调用Invoke接口，系统底层访问第三方WebService接口的Invoke方法
                        // H3.BizBus.InvokeResult InResult = engine.BizBus.InvokeApi(H3.Organization.User.SystemUserId, H3.BizBus.AccessPointType.ThirdConnection,
                        //     "UpdateUsersSubsidy", "GET", "application/x-www-form-urlencoded;charset=UTF-8", null, querys, null, structureSchema);
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
        public void StatisticsSubsidyIn(H3.IEngine engine) // 周末特供版职工补贴充值，判断条件（当前周补贴次数，是否暂停，是否在职）（每周六、日运行）
        {
            /*
            *   1. 在运行前需要先用SQL确定待执行的数据条目共多少
            *   2. 根据获得的结果判定运行区间 0-1000；1001-2000
            *   3. 根据区间判断是否需要双重for循环（一千条以下不循环，一千条到两千条之间循环两次，两千条以上循环三次）
            *
            *                       -- ZDX SDU
            */
            string systemUserId = H3.Organization.User.SystemUserId;
            System.Globalization.GregorianCalendar gc = new System.Globalization.GregorianCalendar();
            int weekOfYear = gc.GetWeekOfYear(DateTime.Now, System.Globalization.CalendarWeekRule.FirstDay, DayOfWeek.Monday);
            int weekDay = Convert.ToInt32(DateTime.Now.DayOfWeek);
            H3.DataModel.BizObjectSchema logSchema = engine.BizObjectManager.GetPublishedSchema("D117502interval");
            H3.DataModel.BizObject log = new H3.DataModel.BizObject(engine, logSchema, systemUserId);
            string sql = "SELECT COUNT(1) AS Count FROM I_D117502UserInformation WHERE startUsing = 1 AND subsidiesThisWeekNumber < 5 AND isMoratorium = 0"; // 生产环境，次数小于5次
            // string sql = "SELECT COUNT(1) AS Count FROM I_D117502UserInformation WHERE startUsing = 1 AND subsidiesThisWeekNumber < 0 AND isMoratorium = 0"; // 测试环境，次数小于两次
            System.Data.DataTable Count = engine.Query.QueryTable(sql, null);
            int num = Convert.ToInt32(Count.Rows[0]["Count"]); // 这里是查出来的总次数
            if(num > 0)
            {
                if(num <= 1000)
                {
                    H3.Data.Filter.Filter filter = new H3.Data.Filter.Filter();
                    H3.Data.Filter.And andMatcher = new H3.Data.Filter.And();
                    // andMatcher.Add(new H3.Data.Filter.ItemMatcher("subsidiesThisWeekNumber", H3.Data.ComparisonOperatorType.Below, 0)); // 测试环境
                    andMatcher.Add(new H3.Data.Filter.ItemMatcher("subsidiesThisWeekNumber", H3.Data.ComparisonOperatorType.Below, 5)); // 生产环境
                    andMatcher.Add(new H3.Data.Filter.ItemMatcher("startUsing", H3.Data.ComparisonOperatorType.Equal, true));
                    andMatcher.Add(new H3.Data.Filter.ItemMatcher("isMoratorium", H3.Data.ComparisonOperatorType.Equal, false));
                    filter.Matcher = andMatcher;
                    H3.DataModel.BizObjectSchema schema = engine.BizObjectManager.GetPublishedSchema("D117502UserInformation");
                    H3.DataModel.BizObject[] boArray = H3.DataModel.BizObject.GetList(engine, H3.Organization.User.SystemUserId, schema, H3.DataModel.GetListScopeType.GlobalAll, filter);
                    H3.DataModel.BulkCommit commitT = new H3.DataModel.BulkCommit();
                    // H3.DataModel.BizObjectSchema aSchema = engine.BizObjectManager.GetPublishedSchema("D117502Sqsdbfxa6s9vl0w2z5a9x8p9a5"); // 测试的表单 没有业务规则
                    H3.DataModel.BizObjectSchema aSchema = engine.BizObjectManager.GetPublishedSchema("D117502SubsidyRechargeLOG"); // 真实的表单 生产环境中
                    if(boArray != null && boArray.Length > 0)
                    {
                        for(int i = 0;i < boArray.Length; i++)
                        {
                            H3.DataModel.BizObject bo = boArray[i];
                            H3.DataModel.BizObject SubsidyRecharg = new H3.DataModel.BizObject(engine, aSchema, systemUserId);
                            SubsidyRecharg.CreatedBy = H3.Organization.User.SystemUserId;
                            SubsidyRecharg["EmployeeCode"] = bo["ObjectId"].ToString();
                            SubsidyRecharg["telphoneNumber"] = bo["telphoneNumber"].ToString();
                            SubsidyRecharg["employeeName"] = bo["userName"].ToString();
                            SubsidyRecharg["BeforeRecharge"] = bo["subsidyBalance"].ToString();
                            SubsidyRecharg["RechargeAmount"] = "10";
                            SubsidyRecharg["AfterRecharge"] = "10";
                            SubsidyRecharg["workerNumber"] = bo["workerNumber"].ToString();
                            SubsidyRecharg["RechargeTime"] = DateTime.Now + string.Empty;
                            SubsidyRecharg["rechargeType"] = "充值";
                            SubsidyRecharg["week"] = weekOfYear;
                            SubsidyRecharg.Status = H3.DataModel.BizObjectStatus.Effective;
                            SubsidyRecharg.Create(commitT);
                        }
                    }
                    string errorMsg = null;
                    commitT.Commit(engine.BizObjectManager, out errorMsg);
                    log.CreatedBy = systemUserId;
                    log.OwnerId = systemUserId;
                    log["CreatedTime"] = DateTime.Now + string.Empty;
                    log["Content"] = "周末特供版-充值，共计 :" + num + "条；";
                    log["log"] = errorMsg;
                    // log["log2"] = "【测试ing】";
                    log.Status = H3.DataModel.BizObjectStatus.Effective;
                    log.Create();
                }
                if(num > 1000 && num <= 2000)
                {
                    H3.DataModel.BulkCommit commitT = new H3.DataModel.BulkCommit();
                    // H3.DataModel.BizObjectSchema aSchema = engine.BizObjectManager.GetPublishedSchema("D117502Sqsdbfxa6s9vl0w2z5a9x8p9a5"); // 测试的表单 没有业务规则
                    H3.DataModel.BizObjectSchema aSchema = engine.BizObjectManager.GetPublishedSchema("D117502SubsidyRechargeLOG"); // 真实的表单 生产环境中
                    int boArrayLength = 0;
                    for(int j = 0;j < 2; j++)
                    {
                        H3.Data.Filter.Filter filter = new H3.Data.Filter.Filter();
                        H3.Data.Filter.And andMatcher = new H3.Data.Filter.And();
                        filter.FromRowNum = j * 1000;
                        filter.ToRowNum = (j + 1) * 1000;
                        andMatcher.Add(new H3.Data.Filter.ItemMatcher("subsidiesThisWeekNumber", H3.Data.ComparisonOperatorType.Below, 5)); // 生产环境
                        // andMatcher.Add(new H3.Data.Filter.ItemMatcher("subsidiesThisWeekNumber", H3.Data.ComparisonOperatorType.Below, 0)); // 测试环境
                        andMatcher.Add(new H3.Data.Filter.ItemMatcher("startUsing", H3.Data.ComparisonOperatorType.Equal, true));
                        andMatcher.Add(new H3.Data.Filter.ItemMatcher("isMoratorium", H3.Data.ComparisonOperatorType.Equal, false));
                        filter.Matcher = andMatcher;
                        H3.DataModel.BizObjectSchema schema = engine.BizObjectManager.GetPublishedSchema("D117502UserInformation");
                        H3.DataModel.BizObject[] boArray = H3.DataModel.BizObject.GetList(engine, H3.Organization.User.SystemUserId, schema, H3.DataModel.GetListScopeType.GlobalAll, filter);
                        boArrayLength = boArray.Length;
                        if(boArray != null && boArray.Length > 0)
                        {
                            for(int i = 0;i < boArray.Length; i++)
                            {
                                H3.DataModel.BizObject bo = boArray[i];
                                H3.DataModel.BizObject SubsidyRecharg = new H3.DataModel.BizObject(engine, aSchema, systemUserId);
                                SubsidyRecharg.CreatedBy = H3.Organization.User.SystemUserId;
                                SubsidyRecharg["EmployeeCode"] = bo["ObjectId"].ToString();
                                SubsidyRecharg["telphoneNumber"] = bo["telphoneNumber"].ToString();
                                SubsidyRecharg["employeeName"] = bo["userName"].ToString();
                                SubsidyRecharg["BeforeRecharge"] = bo["subsidyBalance"].ToString();
                                SubsidyRecharg["RechargeAmount"] = "10";
                                SubsidyRecharg["AfterRecharge"] = "10";
                                SubsidyRecharg["workerNumber"] = bo["workerNumber"].ToString();
                                SubsidyRecharg["RechargeTime"] = DateTime.Now + string.Empty;
                                SubsidyRecharg["rechargeType"] = "充值";
                                SubsidyRecharg["week"] = weekOfYear;
                                SubsidyRecharg.Status = H3.DataModel.BizObjectStatus.Effective;
                                SubsidyRecharg.Create(commitT);
                            }
                        }
                    }
                    string errorMsg = null;
                    commitT.Commit(engine.BizObjectManager, out errorMsg);
                    log.CreatedBy = systemUserId;
                    log.OwnerId = systemUserId;
                    log["CreatedTime"] = DateTime.Now + string.Empty;
                    log["Content"] = "周末特供版-充值，共计 :" + num + "条";
                    log["log"] = errorMsg;
                    // log["log2"] = "【测试ing】";
                    log.Status = H3.DataModel.BizObjectStatus.Effective;
                    log.Create();
                }
                if(num > 2000)
                {
                    H3.DataModel.BulkCommit commitT = new H3.DataModel.BulkCommit();
                    // H3.DataModel.BizObjectSchema aSchema = engine.BizObjectManager.GetPublishedSchema("D117502Sqsdbfxa6s9vl0w2z5a9x8p9a5"); // 测试的表单 没有业务规则
                    H3.DataModel.BizObjectSchema aSchema = engine.BizObjectManager.GetPublishedSchema("D117502SubsidyRechargeLOG"); // 真实的表单 生产环境中
                    int boArrayLength = 0;
                    for(int j = 0;j < 3; j++)
                    {
                        H3.Data.Filter.Filter filter = new H3.Data.Filter.Filter();
                        H3.Data.Filter.And andMatcher = new H3.Data.Filter.And();
                        filter.FromRowNum = j * 1000;
                        filter.ToRowNum = (j + 1) * 1000;
                        // andMatcher.Add(new H3.Data.Filter.ItemMatcher("subsidiesThisWeekNumber", H3.Data.ComparisonOperatorType.Below, 0)); // 测试环境
                        andMatcher.Add(new H3.Data.Filter.ItemMatcher("subsidiesThisWeekNumber", H3.Data.ComparisonOperatorType.Below, 5)); // 生产环境
                        andMatcher.Add(new H3.Data.Filter.ItemMatcher("startUsing", H3.Data.ComparisonOperatorType.Equal, true));
                        andMatcher.Add(new H3.Data.Filter.ItemMatcher("isMoratorium", H3.Data.ComparisonOperatorType.Equal, false));
                        filter.Matcher = andMatcher;
                        H3.DataModel.BizObjectSchema schema = engine.BizObjectManager.GetPublishedSchema("D117502UserInformation");
                        H3.DataModel.BizObject[] boArray = H3.DataModel.BizObject.GetList(engine, H3.Organization.User.SystemUserId, schema, H3.DataModel.GetListScopeType.GlobalAll, filter);
                        boArrayLength = boArray.Length;
                        if(boArray != null && boArray.Length > 0)
                        {
                            for(int i = 0;i < boArray.Length; i++)
                            {
                                H3.DataModel.BizObject bo = boArray[i];
                                H3.DataModel.BizObject SubsidyRecharg = new H3.DataModel.BizObject(engine, aSchema, systemUserId);
                                SubsidyRecharg.CreatedBy = H3.Organization.User.SystemUserId;
                                SubsidyRecharg["EmployeeCode"] = bo["ObjectId"].ToString();
                                SubsidyRecharg["telphoneNumber"] = bo["telphoneNumber"].ToString();
                                SubsidyRecharg["employeeName"] = bo["userName"].ToString();
                                SubsidyRecharg["BeforeRecharge"] = bo["subsidyBalance"].ToString();
                                SubsidyRecharg["RechargeAmount"] = "10";
                                SubsidyRecharg["AfterRecharge"] = "10";
                                SubsidyRecharg["workerNumber"] = bo["workerNumber"].ToString();
                                SubsidyRecharg["RechargeTime"] = DateTime.Now + string.Empty;
                                SubsidyRecharg["rechargeType"] = "充值";
                                SubsidyRecharg["week"] = weekOfYear;
                                SubsidyRecharg.Status = H3.DataModel.BizObjectStatus.Effective;
                                SubsidyRecharg.Create(commitT);
                            }
                        }
                    }
                    string errorMsg = null;
                    commitT.Commit(engine.BizObjectManager, out errorMsg);
                    log.CreatedBy = systemUserId;
                    log.OwnerId = systemUserId;
                    log["CreatedTime"] = DateTime.Now + string.Empty;
                    log["Content"] = "周末特供版-充值，共计 :" + num + "条；";
                    log["log"] = errorMsg;
                    // log["log2"] = "【测试ing】";
                    log.Status = H3.DataModel.BizObjectStatus.Effective;
                    log.Create();
                }
            }
            else 
            {
                log.CreatedBy = systemUserId;
                log.OwnerId = systemUserId;
                log["CreatedTime"] = DateTime.Now + string.Empty;
                log["Content"] = "周末特供版-充值，共计 :" + num + "条；";
                log["log"] = "";
                // log["log2"] = "【测试ing】";
                log.Status = H3.DataModel.BizObjectStatus.Effective;
                log.Create();
            }
        }
        public void Statistics(H3.IEngine engine) // 【废弃不用】旧版统计昨日的补贴消费次数（通过中间表的方式进行）（每日运行）
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
            LOG["date"] = yesterdayDate;
            LOG.Status = H3.DataModel.BizObjectStatus.Effective;
            LOG.Create();
        }
        public void holiday(H3.IEngine engine) // 【废弃不用】根据职工的是否请假控件，向中间表插入占位数据（每日运行）
        {
            System.Globalization.GregorianCalendar gc = new System.Globalization.GregorianCalendar();
            int weekOfYear = gc.GetWeekOfYear(DateTime.Now, System.Globalization.CalendarWeekRule.FirstDay, DayOfWeek.Monday);
            int weekDay = Convert.ToInt32(DateTime.Now.DayOfWeek);
            weekOfYear = weekDay == 0 ? weekOfYear - 1 : weekOfYear;
            string todayDate = DateTime.Now.ToShortDateString();
            string systemUserId = H3.Organization.User.SystemUserId;
            H3.Data.Filter.Filter filter = new H3.Data.Filter.Filter();
            H3.Data.Filter.And andMatcher = new H3.Data.Filter.And();
            andMatcher.Add(new H3.Data.Filter.ItemMatcher("isMoratorium", H3.Data.ComparisonOperatorType.Equal, true));
            filter.Matcher = andMatcher;
            H3.DataModel.BizObjectSchema schema = engine.BizObjectManager.GetPublishedSchema("D117502UserInformation");
            H3.DataModel.BizObject[] boArray = H3.DataModel.BizObject.GetList(engine, systemUserId, schema, H3.DataModel.GetListScopeType.GlobalAll, filter);
            H3.DataModel.BulkCommit commit = new H3.DataModel.BulkCommit();
            if(boArray != null && boArray.Length > 0)
            {
                for(int i = 0;i < boArray.Length; i++)
                {
                    H3.DataModel.BizObjectSchema aSchema = engine.BizObjectManager.GetPublishedSchema("D117502statistics");
                    H3.DataModel.BizObject statistics = new H3.DataModel.BizObject(engine, aSchema, systemUserId);
                    statistics.CreatedBy = systemUserId;
                    statistics.OwnerId = systemUserId;
                    statistics["week"] = weekOfYear;
                    statistics["date"] = todayDate;
                    statistics["times"] = 1;
                    statistics["remark"] = "当日该职工请假";
                    statistics["userObjectId"] = boArray[i]["objectId"];
                    statistics.Status = H3.DataModel.BizObjectStatus.Effective;
                    statistics.Create(commit);
                }
            }
            string errorMsg = null;
            commit.Commit(engine.BizObjectManager, out errorMsg);
            DateTime now = DateTime.Now;
            H3.DataModel.BizObjectSchema Schema = engine.BizObjectManager.GetPublishedSchema("D117502interval");
            H3.DataModel.BizObject LOG = new H3.DataModel.BizObject(engine, Schema, systemUserId);
            LOG.CreatedBy = H3.Organization.User.SystemUserId;
            LOG.OwnerId = H3.Organization.User.SystemUserId;
            LOG["Content"] = "日常统计职工因为请假占用的补贴次数；共计：" + boArray.Length.ToString() + "条";
            LOG["date"] = todayDate;
            LOG.Status = H3.DataModel.BizObjectStatus.Effective;
            LOG.Create();
        }
        public void dos(H3.IEngine engine) // 【废弃不用】原来的补贴归零（每日运行）
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
                        // H3.BizBus.BizStructureSchema contextSchema = new H3.BizBus.BizStructureSchema();
                        // H3.BizBus.BizStructureSchema msgSchema = new H3.BizBus.BizStructureSchema();
                        // H3.BizBus.BizStructureSchema structureSchema = new H3.BizBus.BizStructureSchema();
                        // Dictionary < string, string > querys=new Dictionary<string, string>();
                        // string objectid = bo["ObjectId"].ToString();//主键
                        // querys.Add("objectid", objectid);
                        // querys.Add("Money", "0");
                        // //调用Invoke接口，系统底层访问第三方WebService接口的Invoke方法
                        // H3.BizBus.InvokeResult InResult = engine.BizBus.InvokeApi(H3.Organization.User.SystemUserId, H3.BizBus.AccessPointType.ThirdConnection, "UpdateUsersSubsidy", "GET", "application/x-www-form-urlencoded;charset=UTF-8", null, querys, null, structureSchema);
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
        public void StatisticsSubsidyOut(H3.IEngine engine) // 【废弃不用】周末特供版职工归零（归零可以使用上方dos1方法）
        {
            System.Globalization.GregorianCalendar gc = new System.Globalization.GregorianCalendar();
            int weekOfYear = gc.GetWeekOfYear(DateTime.Now, System.Globalization.CalendarWeekRule.FirstDay, DayOfWeek.Monday);
            int weekDay = Convert.ToInt32(DateTime.Now.DayOfWeek);
            weekOfYear = weekDay == 0 ? weekOfYear - 1 : weekOfYear;
            string systemUserId = H3.Organization.User.SystemUserId;
            H3.DataModel.BizObjectSchema logSchema = engine.BizObjectManager.GetPublishedSchema("D117502interval");
            H3.DataModel.BizObject log = new H3.DataModel.BizObject(engine, logSchema, systemUserId);
            // string sql1 = "SELECT ObjectId, userName, workerNumber, telphoneNumber, subsidyBalance, times.count FROM I_D117502UserInformation LEFT JOIN (SELECT userObjectId, COUNT(1) AS count FROM I_D117502statistics WHERE I_D117502statistics.week = " + weekOfYear + " GROUP BY I_D117502statistics.userObjectId) AS times ON I_D117502UserInformation.ObjectId = times.userObjectId WHERE times.count < 5";
            string sql1 = "SELECT ObjectId, userName, workerNumber, telphoneNumber, subsidyBalance, log.count FROM I_D117502UserInformation LEFT JOIN (SELECT workNumber, SUM(times) AS count FROM I_D117502totalSub GROUP BY I_D117502totalSub.workNumber) AS log ON I_D117502UserInformation.workerNumber = log.workNumber WHERE log.count < 5";
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
        public void StatisticsSubsidy(H3.IEngine engine) // 【07.24 临时使用】周末特供版职工补贴充值，判断条件（当前周，补贴次数，是否暂停，是否在职）（每周六、日运行）
        {
            System.Globalization.GregorianCalendar gc = new System.Globalization.GregorianCalendar();
            int weekOfYear = gc.GetWeekOfYear(DateTime.Now, System.Globalization.CalendarWeekRule.FirstDay, DayOfWeek.Monday);
            int weekDay = Convert.ToInt32(DateTime.Now.DayOfWeek);
            string systemUserId = H3.Organization.User.SystemUserId;
            // for(int j = 1;j <= 2; j++)
            // {
            H3.Data.Filter.Filter filter = new H3.Data.Filter.Filter();
            // filter.FromRowNum = (j - 1) * 1000;
            // filter.ToRowNum = j * 1000;
            H3.Data.Filter.And andMatcher = new H3.Data.Filter.And();
            andMatcher.Add(new H3.Data.Filter.ItemMatcher("is", H3.Data.ComparisonOperatorType.Equal, 1));
            filter.Matcher = andMatcher;
            H3.DataModel.BizObjectSchema schema = engine.BizObjectManager.GetPublishedSchema("D117502ats8338cou8d73f6eauot3ah6");
            H3.DataModel.BizObject[] boArray = H3.DataModel.BizObject.GetList(engine, H3.Organization.User.SystemUserId, schema, H3.DataModel.GetListScopeType.GlobalAll, filter);
            H3.DataModel.BizObjectSchema logSchema = engine.BizObjectManager.GetPublishedSchema("D117502interval");
            H3.DataModel.BizObject log = new H3.DataModel.BizObject(engine, logSchema, systemUserId);
            H3.DataModel.BulkCommit commitT = new H3.DataModel.BulkCommit();
            H3.DataModel.BizObjectSchema aSchema = engine.BizObjectManager.GetPublishedSchema("D117502SubsidyRechargeLOG");
            if(boArray != null && boArray.Length > 0)
            {
                for(int i = 0;i < boArray.Length; i++)
                {
                    H3.DataModel.BizObject bo = boArray[i];
                    H3.DataModel.BizObject SubsidyRecharg = new H3.DataModel.BizObject(engine, aSchema, systemUserId);
                    SubsidyRecharg.CreatedBy = H3.Organization.User.SystemUserId;
                    SubsidyRecharg["EmployeeCode"] = bo["userObjectId"].ToString();
                    SubsidyRecharg["telphoneNumber"] = bo["telphoneNumber"].ToString();
                    SubsidyRecharg["employeeName"] = bo["employeeName"].ToString();
                    SubsidyRecharg["BeforeRecharge"] = bo["BeforeRecharge"].ToString();
                    if(bo["isMoratorium"].ToString() == "1")
                    {
                        SubsidyRecharg["RechargeAmount"] = "0";
                        SubsidyRecharg["AfterRecharge"] = "0";
                    }
                    else
                    {
                        SubsidyRecharg["RechargeAmount"] = "10";
                        SubsidyRecharg["AfterRecharge"] = "10";
                    }
                    SubsidyRecharg["workerNumber"] = bo["F0000001"].ToString();
                    SubsidyRecharg["RechargeTime"] = DateTime.Now + string.Empty;
                    SubsidyRecharg["rechargeType"] = "充值";
                    SubsidyRecharg["week"] = weekOfYear;
                    SubsidyRecharg.Status = H3.DataModel.BizObjectStatus.Effective;
                    SubsidyRecharg.Create(commitT);
                }
            }
            string errorMsg = null;
            commitT.Commit(engine.BizObjectManager, out errorMsg);
            log.CreatedBy = systemUserId;
            log.OwnerId = systemUserId;
            log["CreatedTime"] = DateTime.Now + string.Empty;
            log["Content"] = "周末特供版-充值，共计 :" + boArray.Length.ToString() + "条";
            log["log"] = errorMsg;
            log.Status = H3.DataModel.BizObjectStatus.Effective;
            log.Create();
            // }
        }
    }
}