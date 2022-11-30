if(actionName == "Submit" && this.Request.IsCreateMode)
        {
            DateTime now = DateTime.Now;//获取当前时间
            string isPrint = this.Request.BizObject["immediately"].ToString().ToUpper();
            string SN = "";
            string sendType = this.Request.BizObject["SendType"].ToString(); // 获取配送方式 “送货”OR“自提”
            string Canteen = this.Request.BizObject["CanteenT"].ToString();
            H3.BizBus.BizStructureSchema paramSchema = new H3.BizBus.BizStructureSchema();
            paramSchema.Add(new H3.BizBus.ItemSchema("SN", "SN编码", H3.Data.BizDataType.ShortString, 200, null));
            paramSchema.Add(new H3.BizBus.ItemSchema("OrderInfo", "打印内容", H3.Data.BizDataType.ShortString, 200, null));
            string OrderInfo = "";
            OrderInfo = "<CB>" + Canteen + "消费凭证" + "</CB><BR>";//标题字体如需居中放大,就需要用标签套上
            H3.DataModel.BizObject accountBo = H3.DataModel.BizObject.Load(H3.Organization.User.SystemUserId, this.Request.Engine, "D154150Sq71gd7subnu2khkrko91ii477", this.Request.BizObjectId, false);
            if(accountBo != null)
            {
                OrderInfo += "<B><BOLD>" + accountBo["SeqNo"].ToString() + "</BOLD></B><BR>";
            }
            OrderInfo += "<B><BOLD>工号：" + this.Request.BizObject["JobNumberT"].ToString() + "</BOLD></B><BR>";
            OrderInfo += "姓名：" + this.Request.BizObject["employee"].ToString() + "<BR>";
            //OrderInfo += "下单时间：" + now.ToString() + "<BR>";
            OrderInfo += "下单时间：" + accountBo["CreatedTime"].ToString() + "<BR>";
            OrderInfo += "--------------------------------<BR>";//名称 菜品 主食 汤类 数量 合计 地址 配送时间
            OrderInfo += "消费：" + this.Request.BizObject["amount"].ToString() + "<BR>";
            //查询菜品订单
            H3.Data.Filter.Filter filter = new H3.Data.Filter.Filter();  //构建过滤器
            H3.Data.Filter.And andMatcher = new H3.Data.Filter.And();    //构造And匹配器
            andMatcher.Add(new H3.Data.Filter.ItemMatcher("ParentObjectId", H3.Data.ComparisonOperatorType.Equal, this.Request.BizObject["ObjectId"].ToString()));  //添加筛选条件
            filter.Matcher = andMatcher;
            H3.DataModel.BizObjectSchema schema = this.Engine.BizObjectManager.GetPublishedSchema("D154150Fm0dx6iwkifys4uq1x2puybia4");   //获取模块Schema
            H3.DataModel.BizObject[] boArray = H3.DataModel.BizObject.GetList(this.Engine, H3.Organization.User.SystemUserId,
                schema, H3.DataModel.GetListScopeType.GlobalAll, filter); //查询返回的结果对象
            string SetMeal = "";
            if(boArray != null && boArray.Length > 0)
            {
                for(int i = 0;i < boArray.Length; i++)
                {
                    H3.DataModel.BizObject bo = boArray[i];
                    OrderInfo += "<B><BOLD>套餐：" + bo["SetMeal"].ToString() + "</BOLD></B><BR>";
                    OrderInfo += "菜品：" + bo["Menu"].ToString() + "<BR>";
                    OrderInfo += "<B><BOLD>数量：" + bo["Quantity"].ToString() + "</BOLD></B><BR>";
                    OrderInfo += "合计：" + bo["Total"].ToString() + "<BR>";
                    OrderInfo += "--------------------------------<BR>";
                    //判断子表中是否存在下架菜品
                    string Select = bo["Select"].ToString();
                    H3.Data.Filter.Filter filters = new H3.Data.Filter.Filter();  //构建过滤器
                    H3.Data.Filter.And andMatchers = new H3.Data.Filter.And();    //构造And匹配器
                    andMatchers.Add(new H3.Data.Filter.ItemMatcher("ObjectId", H3.Data.ComparisonOperatorType.Equal, Select));  //添加筛选条件
                    andMatchers.Add(new H3.Data.Filter.ItemMatcher("onLine", H3.Data.ComparisonOperatorType.Equal, 1));  //添加筛选条件
                    filters.Matcher = andMatchers;
                    H3.DataModel.BizObjectSchema schemas = this.Engine.BizObjectManager.GetPublishedSchema("D117502foodNameUse");   //获取模块Schema
                    H3.DataModel.BizObject[] boArrays = H3.DataModel.BizObject.GetList(this.Engine, H3.Organization.User.SystemUserId,
                        schemas, H3.DataModel.GetListScopeType.GlobalAll, filters); //查询返回的结果对象
                    if(boArrays == null) 
                    {
                        response.Errors.Add("当前商品:" + bo["nameT"].ToString() + "已下架，请重新挑选");
                    }
                }
            }
            OrderInfo += "地址：<BR>";
            OrderInfo += "<B><BOLD>" + this.Request.BizObject["LocationT"].ToString() + "</BOLD></B><BR>";
            OrderInfo += "联系电话：" + this.Request.BizObject["CellPhone"].ToString() + "<BR>";
            if(isPrint == "FALSE")
            {
                OrderInfo += "<B><BOLD>配送时间：" + this.Request.BizObject["sendTimeTT"].ToString() + "</BOLD></B><BR>";
            }
            OrderInfo += "备注：" + this.Request.BizObject["remarks"].ToString() + "<BR>";
            OrderInfo += "--------------------------------<BR>";
            OrderInfo += "加链加(上海)人工智能科技有限公司";
            OrderInfo += "                                <BR>";
            OrderInfo += "<BR>";
            H3.BizBus.BizStructure paramData = new H3.BizBus.BizStructure(paramSchema);
            if(Canteen == "营养食堂")
            {
                SN = "921633990"; // 营养食堂  自提
            } else
            {
                if(sendType == "自提")
                {
                    SN = "922611124"; // 中裕职工餐厅  自提
                } else
                {
                    SN = "922611315"; // 中裕职工餐厅  送货
                }
            }
            paramData["SN"] = SN;
            paramData["OrderInfo"] = OrderInfo;
            H3.BizBus.InvokeResult InResult = this.Engine.BizBus.Invoke(H3.Organization.User.SystemUserId, H3.BizBus.AccessPointType.Legacy,
                this.Request.SchemaCode, "Prints", paramData);
        }