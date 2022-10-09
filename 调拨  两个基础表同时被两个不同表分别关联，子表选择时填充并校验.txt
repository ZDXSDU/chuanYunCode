友好礼品管理，跨门店调拨

JS >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>


$.extend( $.JForm, {
    OnLoad: () => {
        let that = this;
        that.userId.BindChange( "amao", () => {
            that.userFrom.SetValue( that.userId.GetValue() );
        })
        //【01】拿到用户的门店 - 提交之前校验子表中选择的礼品的库存数量与调拨数量
        let STU = '';
        that.F0000005.BindChange( "GetSTU", () => {
            STU = that.F0000005.GetValue();
        })
        //【02】为子表绑定事件触发器，拿到新选择的礼品 - 提交之前校验子表中选择的礼品的库存数量与调拨数量
        let currentFather = this.D148737Fbcttylokxsvjsk2ht7np7lic2;
        let currentGiftID = "D148737Fbcttylokxsvjsk2ht7np7lic2.F0000012";
        currentFather.BindChange( "Set", ( data ) => {
            let responseData = data[ 0 ];
            if( responseData != null && responseData.DataField == currentGiftID ) {
                let giftId = currentFather.GetCellManager( responseData.ObjectId, currentGiftID ).GetValue();
                //【03】根据前两步拿到的门店OBJ和礼品OBJ 发配至后端查询数据库 - 提交之前校验子表中选择的礼品的库存数量与调拨数量
                if( !( giftId == "" || STU == "" ) ) {
                    $.SmartForm.PostForm( "SelectQuantity", {
                        STU: STU,
                        gift: giftId
                    }, ( data ) => {
                        if( data.Errors && data.Errors.length ) {
                            $.IShowError( "錯誤", JSON.stringify( data.Errors ) );
                        } else {
                            //【04】拿到后端传回的数据  并判断,如果查无此人，那就逐出家门，如果人丁兴旺那就子表赋值 - 提交之前校验子表中选择的礼品的库存数量与调拨数量
                            let res = JSON.stringify( data.ReturnData.Quantity );
                            let res2 = data.ReturnData.Quantity;
                            if( res == -999 ) {
                                $.IConfirm( "溫馨提示", "您的門店禮品庫存內沒有該件禮品的記錄，請檢查是否選擇錯誤或先為其辦理入庫", ( data ) => {
                                    if( data || !data ) {
                                        currentFather.UpdateRow( responseData.ObjectId, {
                                            "D148737Fbcttylokxsvjsk2ht7np7lic2.F0000012": "",
                                        });
                                    }
                                });
                            } else {
                                currentFather.UpdateRow( responseData.ObjectId, {
                                    "D148737Fbcttylokxsvjsk2ht7np7lic2.F0000036": parseInt( res2 ),
                                });
                            }
                        }
                    }, ( error ) => {
                        $.IShowError( "错误", JSON.stringify( error ) );
                    }, false )
                } else if( STU == "" ) {
                    $.IShowWarn( "警告", "請先選擇門店" );
                }
            }
        })
    },
    OnValidate: ( actionControl ) => {
        //【05】提交检验，只能是调拨数量小于库存数量而且不能是0 - 提交之前校验子表中选择的礼品的库存数量与调拨数量
        let parent = this;
        if( actionControl.Action == "Submit" && $.SmartForm.ResponseContext.ActivityCode == "Activity2" ) {
            let childTableData = parent.D148737Fbcttylokxsvjsk2ht7np7lic2.GetValue();
            for( let i = 0;i < childTableData.length;i++ ) {
                let itemData = childTableData[ i ];
                if( itemData[ "F0000016" ] > itemData[ "F0000036" ] ) {
                    $.IConfirm( "警告", "禮品的調撥數量不能大於禮品的庫存數量,請檢查並更正", ( data ) => { });
                    return false;
                    break;
                }
                if( itemData[ "F0000016" ] <= 0 ) {
                    $.IConfirm( "警告", "禮品的調撥數量不能小於等於0,請檢查並更正", ( data ) => { });
                    return false;
                    break;
                }
            }
        }
        return true;
    }
});


C# >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>


protected override void OnSubmit(string actionName, H3.SmartForm.SmartFormPostValue postValue, H3.SmartForm.SubmitSmartFormResponse response)
    {
        if(actionName == "SelectQuantity")
        {
            response.ReturnData = new Dictionary<string, object>();
            string STU = this.Request["STU"];
            string gift = this.Request["gift"];
            if(gift != "" || STU != "") // 这里需要增加判空，子表新增一行数据时，也会冒泡到这里
            {
                // 根据传回的门店和礼品，去库存表内查找相应的库存数量
                string SELECT_SQL = "SELECT F0000023 AS Quantity FROM I_D148737Scghzt2zd048gdc5dppafq6545 WHERE F0000005 = '" + STU + "' AND F0000024 = '" + gift + "'";
                /**
                 * PROTIPS:
                 * 如果当前门店的当前礼品库存数可以查到，扔回去res即可
                 * 如果当前门店的当前礼品库存数查不到，那说明这个门店的库存表中根本没有这件礼品的那条数据，扔回去Err
                 */
                System.Data.DataTable SelectDT = this.Engine.Query.QueryTable(SELECT_SQL, null);
                if(SelectDT.Rows.Count == 0) // 查无此人
                {
                    int error = -999;
                    response.ReturnData.Add("Quantity", error);
                }
                else 
                {
                    response.ReturnData.Add("Quantity", SelectDT.Rows[0]["Quantity"].ToString());
                }
            }
        }
        base.OnSubmit(actionName, postValue, response);
    }