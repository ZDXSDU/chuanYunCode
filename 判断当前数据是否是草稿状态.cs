H3.DataModel.BizObjectStatus dd =  this.Request.BizObject.Status;

        if(dd.ToString() == "Draft")
        {
            response.Actions.Remove("Print");
            response.Actions.Remove("ViewQrCode");
        }
        else
        {
            response.Actions.Remove("Remove");
            response.Actions.Remove("Print");
            response.Actions.Remove("ViewQrCode");
        }