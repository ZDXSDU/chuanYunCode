1.--------------------------------------定时器每天执行，将上一天的补贴消费的员工都放入表中
if(actionName == "statistics")
        {
            string yesterdayDate = DateTime.Now.AddDays(-1).ToShortDateString();
            string sql1 = "SELECT userObjectId FROM i_D117502SubsidyRecords WHERE TO_DAYS(NOW()) - TO_DAYS(changeTime) = 1 AND i_D117502SubsidyRecords.changeAmount > 0 AND i_D117502SubsidyRecords.changeType = '消费'";
            System.Data.DataTable Countb = this.Request.Engine.Query.QueryTable(sql1, null);
            System.Globalization.GregorianCalendar gc = new System.Globalization.GregorianCalendar();
            int weekOfYear = gc.GetWeekOfYear(DateTime.Now, System.Globalization.CalendarWeekRule.FirstDay, DayOfWeek.Monday);
            int RowsCount = Countb.Rows.Count;
            for(int i = 0;i < RowsCount; i++)
            {
                H3.IEngine engine = this.Request.Engine;
                string systemUserId = H3.Organization.User.SystemUserId;
                string currentUserId = this.Request.UserContext.UserId;
                H3.DataModel.BizObjectSchema aSchema = engine.BizObjectManager.GetPublishedSchema("D117502statistics");
                H3.DataModel.BizObject aBo = new H3.DataModel.BizObject(engine, aSchema, systemUserId);
                aBo.CreatedBy = currentUserId;
                aBo.OwnerId = currentUserId;
                aBo["week"] = weekOfYear;
                aBo["date"] = yesterdayDate;
                aBo["times"] = 1;
                aBo["userObjectId"] = Countb.Rows[i]["userObjectId"];
                aBo.Status = H3.DataModel.BizObjectStatus.Effective;
                aBo.Create();
            }
        }