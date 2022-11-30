JS>>>>>>>>>>>>>>>>>>>>>>>>>

// 提交校验
    OnValidate: function( actionControl ) {
        debugger;
        var that = this;
        var counttt = "0";
        $.SmartForm.PostForm( "beforSubmitData", {
            vin: that.vin.GetValue()
        }, ( data ) => {
            if( data.Errors && data.Errors.length ) {
                $.IShowError( "错误", JSON.stringify( data.Errors ) );
            } else {
                if( data.ReturnData.count != "0" ) {
                    counttt = data.ReturnData.count;
                    let Price = data.ReturnData.Price;
                    let SubShop = data.ReturnData.SubShop;
                    let count = data.ReturnData.count;
                    $.IShowError( "错误", "该车辆曾在 " + count + "家店面评估过价格，其中“" + SubShop + "”的评估价格为：" + Price + "元" );
                }
            }
        }, ( error ) => {
            $.IShowError( "错误", JSON.stringify( error ) );
        }, false )
        if( counttt == "0" ) {
            return true;
        } else {
            return false;
        }
    },






C#>>>>>>>>>>>>>>>>>>>>>>>>>>>>

if(actionName == "beforSubmitData")
        {
            response.ReturnData = new Dictionary<string, object>();
            string thisVIN = this.Request["vin"] + string.Empty;
            // string thisVIN = this.Request.BizObject["vin"].ToString();
            string selectSQL = "SELECT stores, appraisalPrice FROM I_D117502AppraisalAssessment WHERE vin = '" + thisVIN + "'";
            System.Data.DataTable selectDT = this.Engine.Query.QueryTable(selectSQL, null);
            int selectDTLenth = selectDT.Rows.Count;
            if(selectDTLenth != 0)
            {
                string selectUnitSQL = "SELECT SubShop FROM I_D117502SubShop WHERE ObjectId = '" + selectDT.Rows[0]["stores"] + "'";
                System.Data.DataTable selectSubShopDT = this.Engine.Query.QueryTable(selectUnitSQL, null);
                response.ReturnData.Add("count", selectDTLenth.ToString());
                response.ReturnData.Add("SubShop", selectSubShopDT.Rows[0]["SubShop"].ToString());
                response.ReturnData.Add("Price", selectDT.Rows[0]["appraisalPrice"].ToString());
            }
            else
            {
                response.ReturnData.Add("count", "0");
            }
        }