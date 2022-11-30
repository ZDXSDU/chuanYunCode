    protected override void OnSubmit(string actionName, H3.SmartForm.SmartFormPostValue postValue, H3.SmartForm.SubmitSmartFormResponse response)
    {
        base.OnSubmit(actionName, postValue, response);
        if(this.Request.BizObject["clientType"].ToString() == "卖车")
        {
            this.Request.Engine.Query.QueryTable("UPDATE I_D117502UsedCarCustomers SET Name = '" + this.Request.BizObject["carNum"].ToString() + "' WHERE ObjectId = '" + this.Request.BizObjectId.ToString() + "'", null);
        }
        else 
        {
            if(this.Request.BizObject["clientTypePC"].ToString() == "个人")
            {
                string SQL = "UPDATE I_D117502UsedCarCustomers SET Name = '" + this.Request.BizObject["clientName"].ToString() + "' WHERE ObjectId = '" + this.Request.BizObjectId.ToString() + "'";
                this.Request.Engine.Query.QueryTable(SQL, null);
            }
            else 
            {
                this.Request.Engine.Query.QueryTable("UPDATE I_D117502UsedCarCustomers SET Name = '" + this.Request.BizObject["companyName"].ToString() + "' WHERE ObjectId = '" + this.Request.BizObjectId.ToString() + "'", null);
            }
        }
    }