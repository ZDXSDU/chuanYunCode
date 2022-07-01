public class FoodTimer: H3.SmartForm.Timer
{
    public FoodTimer() { }
    protected override void OnWork(H3.IEngine engine)
    {
        DateTime now = DateTime.Now;
        DateTime sTime = DateTime.Parse(now.ToString("yyyy-MM-dd 08:00:00"));
        DateTime eTime = DateTime.Parse(now.ToString("yyyy-MM-dd 12:00:00"));
        int week = Convert.ToInt32(DateTime.Now.DayOfWeek);
        if(sTime <= now && eTime >= now && week == 1)
        {
            do2(engine);
        }
    }
    public void do2(H3.IEngine engine)
    {
        System.Globalization.GregorianCalendar gc = new System.Globalization.GregorianCalendar();
        int weekOfYear = gc.GetWeekOfYear(DateTime.Now, System.Globalization.CalendarWeekRule.FirstDay, DayOfWeek.Monday);
        int lastWeekNum = weekOfYear - 1;
        int nowYear = DateTime.Now.Year;
        string nowYearWeek = nowYear.ToString() + weekOfYear.ToString();
        DateTime now = DateTime.Now;
        string systemUserId = H3.Organization.User.SystemUserId;
        H3.DataModel.BizObjectSchema logSchema = engine.BizObjectManager.GetPublishedSchema("D117502Szke3xsvrnq5dbvh3w0vjlt5r6");
        H3.DataModel.BizObject log = new H3.DataModel.BizObject(engine, logSchema, systemUserId);
        log.CreatedBy = systemUserId;
        log.OwnerId = systemUserId;
        log["executionDate"] = now.ToString();
        H3.DataModel.BulkCommit commit = new H3.DataModel.BulkCommit();
        H3.Data.Filter.Filter filter = new H3.Data.Filter.Filter();
        H3.Data.Filter.And andMatcher = new H3.Data.Filter.And();
        andMatcher.Add(new H3.Data.Filter.ItemMatcher("WeekNum", H3.Data.ComparisonOperatorType.Equal, lastWeekNum));  // 筛选条件:数据申报时间为上一周的
        filter.Matcher = andMatcher;
        H3.DataModel.BizObjectSchema schema = engine.BizObjectManager.GetPublishedSchema("D117502missionProgress");
        H3.DataModel.BizObject[] boArray = H3.DataModel.BizObject.GetList(engine, H3.Organization.User.SystemUserId, schema, H3.DataModel.GetListScopeType.GlobalAll, filter); // 查询返回的结果
        if(boArray != null && boArray.Length > 0)
        {
            for(int i = 0;i < boArray.Length; i++)
            {
                H3.DataModel.BizObject bo = boArray[i];
                H3.DataModel.BizObjectSchema aSchema = engine.BizObjectManager.GetPublishedSchema("D117502missionProgress");
                H3.DataModel.BizObject Bo = new H3.DataModel.BizObject(engine, aSchema, systemUserId);
                Bo.CreatedBy = H3.Organization.User.SystemUserId;
                Bo.OwnerId = bo["OwnerId"].ToString();
                Bo["affiliatedUnit"] = bo["affiliatedUnit"].ToString(); // 所属单位，关联表单
                Bo["declarationDate"] = DateTime.Now.ToString(); // 申报日期
                Bo["lastweekData"] = bo["ObjectId"].ToString(); // 上周数据，关联表单
                Bo["pressConference"] = bo["pressConference"].ToString(); // 发布会
                Bo["eventScreenings"] = bo["eventScreenings"].ToString(); // 活动场次
                Bo["typicalCase"] = bo["typicalCase"].ToString(); // 发布典型案例 
                Bo["typicalPropaganda"] = bo["typicalPropaganda"].ToString(); // 先进典型宣传 
                Bo["paperBrochure"] = bo["paperBrochure"].ToString(); // 纸质宣传册
                Bo["electronicPromotional"] = bo["electronicPromotional"].ToString(); // 电子宣传片
                Bo["otherPromotional"] = bo["otherPromotional"].ToString(); // 其他宣传品
                Bo["traditionalMedia"] = bo["traditionalMedia"].ToString(); // 传统媒体报道稿件数量
                Bo["postWorks"] = bo["postWorks"].ToString(); // 发布作品数量
                Bo["clicksReads"] = bo["clicksReads"].ToString(); // 点击量、阅读量、曝光量、转发量
                Bo["interactivePromotion"] = bo["interactivePromotion"].ToString(); // 互动宣传覆盖人次 
                Bo["sendMessage"] = bo["sendMessage"].ToString(); // 发送短信
                Bo["centralMedia"] = bo["centralMedia"].ToString(); // 中央媒体刊载量 
                Bo["otherMedia"] = bo["otherMedia"].ToString(); // 其他媒体刊载量 
                Bo["coverCrowd"] = bo["coverCrowd"].ToString(); // 覆盖人群
                Bo["occurNegative"] = bo["occurNegative"].ToString(); // 发生负面
                Bo["successfullyHandled"] = bo["successfullyHandled"].ToString(); // 已顺利处置
                Bo["pressConferenceL"] = bo["pressConference"].ToString(); // 发布会---上周
                Bo["eventScreeningsL"] = bo["eventScreenings"].ToString(); // 活动场次---上周
                Bo["typicalCaseL"] = bo["typicalCase"].ToString(); // 发布典型案例 ---上周
                Bo["typicalPropagandaL"] = bo["typicalPropaganda"].ToString(); // 先进典型宣传---上周 
                Bo["paperBrochureL"] = bo["paperBrochure"].ToString(); // 纸质宣传册---上周
                Bo["electronicPromotionalL"] = bo["electronicPromotional"].ToString(); // 电子宣传片---上周
                Bo["otherPromotionalL"] = bo["otherPromotional"].ToString(); // 其他宣传品---上周
                Bo["traditionalMediaL"] = bo["traditionalMedia"].ToString(); // 传统媒体报道稿件数量---上周
                Bo["postWorksL"] = bo["postWorks"].ToString(); // 发布作品数量---上周
                Bo["clicksReadsL"] = bo["clicksReads"].ToString(); // 点击量、阅读量、曝光量、转发量---上周
                Bo["interactivePromotionL"] = bo["interactivePromotion"].ToString(); // 互动宣传覆盖人次---上周 
                Bo["sendMessageL"] = bo["sendMessage"].ToString(); // 发送短信---上周
                Bo["centralMediaL"] = bo["centralMedia"].ToString(); // 中央媒体刊载量 ---上周
                Bo["otherMediaL"] = bo["otherMedia"].ToString(); // 其他媒体刊载量 ---上周
                Bo["coverCrowdL"] = bo["coverCrowd"].ToString(); // 覆盖人群---上周
                Bo["occurNegativeL"] = bo["occurNegative"].ToString(); // 发生负面---上周
                Bo["F0000001"] = bo["successfullyHandled"].ToString(); // 已顺利处置---上周
                Bo["WeekNum"] = weekOfYear; // 周；就是当前时间是一年中的第几周
                Bo["LastWeekNum"] = lastWeekNum; // 上一周是一年中的第几周，用与过滤器使用
                Bo["WeekNumT"] = nowYearWeek + bo["F0000002"].ToString(); // 年周；用于提交的时候防止重复提交
                Bo["userName"] = bo["userName"].ToString(); // 用户；关联表单
                Bo["userID"] = bo["userID"].ToString(); // 用户ID
                Bo["OwnerDeptId"] = bo["OwnerDeptId"].ToString(); // 所属部门，用于控制权限
                Bo["F0000002"] = bo["F0000002"].ToString(); // 街道办事处的中文名字，用于防止重复提交的时候使用
                Bo.Status = H3.DataModel.BizObjectStatus.Running;
                Bo.Create(commit);
            }
            string errorMsg = null;
            commit.Commit(engine.BizObjectManager, out errorMsg);
            log["log"] = "执行成功！共计" + boArray.Length.ToString() + "条";
            log.Status = H3.DataModel.BizObjectStatus.Effective;
            log.Create();
        }
        else
        {
            log["log"] = "执行失败！未查询到上周的数据";
            log.Status = H3.DataModel.BizObjectStatus.Effective;
            log.Create();
        }
    }
}