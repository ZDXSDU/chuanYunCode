// 👇 判断当前是新增数据提交还是编辑旧数据提交，如果是编辑旧数据提交，那就什么也不管直接提交
        if(this.Request.IsCreateMode)
        {
            //  👇 在当前表单中分别拿到分店、客户名称、手机号的值，放在字符串变量中对于object类型的值需要转成string类型
            string branchStore = this.Request.BizObject["branchStore"].ToString(); // 👈 分店
            string clientName = this.Request.BizObject["clientName"].ToString(); // 👈 客户名称
            string cellphone = this.Request.BizObject["cellphone"].ToString(); // 👈 手机号
            //  👇 写一个SQL语句，看看目标表单（潜客档案）中有没有符合条件(分店、客户名称、手机号匹配)的数据
            string selectSql = "SELECT COUNT(1) AS COUNT FROM I_D117502potentialCustomers WHERE I_D117502potentialCustomers.branchStore = '" + branchStore + "' AND I_D117502potentialCustomers.clientName = '" + clientName + "' AND I_D117502potentialCustomers.telphoneNumber = '" + cellphone + "'";
            System.Data.DataTable Count = this.Engine.Query.QueryTable(selectSql, null); // 👈 执行这个SQL
            int num = Convert.ToInt32(Count.Rows[0]["COUNT"]); // 👈 拿到查询的结果 如果查出来的符合要求的数据条数==0，那证明潜客档案里面没有类似的数据，需要使用业务对象创建一条
            if(num == 0)
            {
                string systemUserId = H3.Organization.User.SystemUserId; // 👈 获取系统虚拟用户的人员Id
                H3.DataModel.BizObjectSchema aSchema = this.Request.Engine.BizObjectManager.GetPublishedSchema("D117502potentialCustomers"); // 👈 获取潜客档案表单的表单结构对象
                H3.DataModel.BizObject aBo = new H3.DataModel.BizObject(this.Request.Engine, aSchema, systemUserId); // 👈 new一个潜客档案表单的业务对象aBo，并赋值
                aBo.CreatedBy = this.Request.UserContext.UserId;; // 👈 设置业务对象的创建人为系统虚拟用户
                aBo.OwnerId = this.Request.BizObject["businessConsultant"].ToString(); // 销售顾问
                aBo["businessUnit"] = this.Request.BizObject["businessUnit"].ToString(); // 事业部
                aBo["stores"] = this.Request.BizObject["stores"].ToString(); // 门店
                aBo["branchStore"] = this.Request.BizObject["branchStore"].ToString(); // 分店
                aBo["OwnerDeptId"] = this.Request.BizObject["OwnerDeptId"].ToString(); // 分店所属部门
                aBo["intentionalModel"] = this.Request.BizObject["Model"].ToString(); // 意向车型
                aBo["clientName"] = this.Request.BizObject["clientName"].ToString(); // 客户姓名
                aBo["sex"] = this.Request.BizObject["sex"].ToString(); // 性别
                aBo["ageGroups"] = this.Request.BizObject["ageGroups"].ToString(); // 年龄段
                aBo["telphoneNumber"] = this.Request.BizObject["cellphone"].ToString(); // 手机号
                aBo["customerLevel"] = this.Request.BizObject["customerLevel"].ToString(); // 客户级别
                aBo["levelRequirements"] = this.Request.BizObject["levelRequirements"].ToString(); // 级别要求
                aBo["nextCommunication"] = this.Request.BizObject["nextCommunication"].ToString(); // 下次沟通时间
                aBo["entryTime"] = this.Request.BizObject["entryTime"].ToString(); // 建档时间
                aBo["userId"] = this.Request.BizObject["userId"].ToString(); // userId
                aBo["userFrom"] = this.Request.BizObject["userFrom"].ToString(); // userFrom               
                aBo.Status = H3.DataModel.BizObjectStatus.Effective; // 👈 设置业务对象数据为生效状态
                aBo.Create();  // 👈 将业务对象创建到数据库中
            }
        }