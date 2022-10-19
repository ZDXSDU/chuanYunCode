$.extend( $.JForm, {
    OnLoad: () => {
        this.customerLevel.SetValue( "H" );
        let that = this;
        this.userId.BindChange( "amao", () => {
            that.userForm.SetValue( that.userId.GetValue() );
        })
        this.businessConsultantID.BindChange( "666", ( data ) => {
            that.salesConsultant.SetValue( that.businessConsultantID.GetValue() );
            that.businessConsultantID.UnbindChange( "666" );
        })
        this.AGE.BindChange( "自动选择年龄段", () => {
            if( that.AGE.GetValue() > 0 ) {
                debugger;
                $.SmartForm.PostForm( "SelectYearOf", {
                    year: that.AGE.GetValue()
                }, ( data ) => {
                    if( data.Errors && data.Errors.length ) {
                        $.IShowError( "错误", JSON.stringify( data.Errors ) );
                    } else {
                        that.ageGroups.SetValue( data.ReturnData.OBJ );
                    }
                }, ( error ) => {
                    $.IShowError( "错误", JSON.stringify( error ) );
                }, false )
            }
        })
        if( $.SmartForm.ResponseContext.ActivityCode == "Saler" ) { // 只有在销售接待流程节点时才可以修改试乘试驾的开关，以及弹出窗口
            debugger;
            let CurrentBizObject = "";
            $.SmartForm.PostForm( "GetCurrentBizobjectID", {
                ActivityCode: "Saler"
            }, ( data ) => {
                if( data.Errors && data.Errors.length ) {
                    $.IShowError( "错误", JSON.stringify( data.Errors ) );
                } else {
                    CurrentBizObject = data.ReturnData.CurrentBizObject;
                }
            },
                ( error ) => {
                    $.IShowError( "错误", JSON.stringify( error ) );
                }, false );
            this.isTestdrive.BindChange( "是否试乘试驾", () => {
                if( that.isTestdrive.GetValue() === true ) {
                    /**          PROTIPS BY ZDXSDU
                     * 
                     * 1. 自然进店的客流什么都没有，只需要传递当前表的objectId即可
                     * 2. 邀约进店、网销客户、已有潜客 这三种除了由潜客以外，还有一大堆诸如姓名、手机号、年龄、年龄段、地址等信息，也需要一并传到弹窗的新表单内
                     */
                    let params = {
                        passengerFlow: CurrentBizObject
                    };//传递到表单JSON
                    let checkIsChange = false;
                    let showlist = true;
                    $.IShowForm( 'D117502Testdrive', '', params, checkIsChange, showlist, {
                        showInModal: true,
                        title: "新增试乘试驾信息",
                        height: 500,
                        width: 900,
                        OnShowCallback: ( data ) => {

                        }, onHiddenCallback: ( data ) => {
                            /**             PROTIPS BY ZDXSDU
                             *  将弹窗中所填写的字段值拿到并放到当前未关闭的表单内
                             */
                            let res = data.Data;
                            that.clientName.SetValue(res.clientName); // 客户姓名
                            that.sex.SetValue(res.sex); // 性别
                            that.Birthday.SetValue(res.Birthday); // 出生日期
                            that.cellphone.SetValue(res.cellphone); // 手机号
                            that.AGE.SetValue(res.AGE); // 年龄
                            that.ageGroups.SetValue(res.ageGroups); // 年龄段
                        }
                    });
                }
            })
        } else {
            this.isTestdrive.SetReadonly( true ); //设置"是/否试乘试驾"不可写
            this.isTestdrive.SetVisible( true );//设置"是/否试乘试驾"不可见
        }
    },
    OnLoadActions: function( actions ) {
    },
    OnValidate: function( actionControl ) {
        return true;
    }
});