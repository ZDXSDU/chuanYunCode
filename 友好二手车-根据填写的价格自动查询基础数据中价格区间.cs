JS>>>>>>>>>>>>>

this.transactionprice.BindChange( "自动填充价格区间", () => { // transactionprice是填写的价格
            if( that.transactionprice.GetValue() > 0 ) { // transactionprice是填写的价格
                debugger;
                $.SmartForm.PostForm( "SelectPriceRange", {
                    price: that.transactionprice.GetValue() // transactionprice是填写的价格
                }, ( data ) => {
                    if( data.Errors && data.Errors.length ) {
                        $.IShowError( "错误", JSON.stringify( data.Errors ) );
                    } else {
                        that.prices.SetValue(data.ReturnData.OBJ); // prices是价格区间的关联表单
                    }
                }, ( error ) => {
                    $.IShowError( "错误", JSON.stringify( error ) );
                }, false )
            }
        })



C#>>>>>>>>>>>>
if(actionName == "SelectPriceRange")
        {
            response.ReturnData = new Dictionary<string, object>();
            string ReturnData_price = this.Request["price"];
            double price = double.Parse(ReturnData_price);
            string SELECT_SQL = "SELECT ObjectId, min, max FROM I_D117502prices WHERE isUse = 1";
            System.Data.DataTable SelectDT = this.Engine.Query.QueryTable(SELECT_SQL, null);
            int length = SelectDT.Rows.Count;
            for(int i = 0;i < length; i++)
            {
                double min = double.Parse(SelectDT.Rows[i]["min"].ToString());
                double max = double.Parse(SelectDT.Rows[i]["max"].ToString());
                if(price <= max && price > min)
                {
                    response.ReturnData.Add("OBJ", SelectDT.Rows[i]["ObjectId"].ToString());
                    break;
                }
            }
        }