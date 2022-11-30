$.extend( $.JForm, {
    // 加载事件
    OnLoad: function() {
        const that = this;
        this.tempUserOBJID.BindChange( "chuanCheng", () => {
            that.user.SetValue( that.tempUserOBJID.GetValue() )
        })
        if( $.SmartForm.ResponseContext.ActivityCode == "Activity2" ) {
            this.statusNow.SetValue( "待确认" );
        }
        if( $.SmartForm.ResponseContext.ActivityCode == "Activity3" ) {
            this.statusNow.SetValue( "已确认" );
        }
        if( $.SmartForm.ResponseContext.ActivityCode == "Activity8" ) {
            debugger;
            this.statusNow.SetValue( "已完工" );
            function timediff( begin_time, end_time ) {
                let beginTime = ( new Date( begin_time ).getTime() ) / 1000;
                let endTime = ( new Date( end_time ).getTime() ) / 1000;
                var starttime = ''
                var endtime = ''
                if( beginTime < endTime ) {
                    starttime = beginTime;
                    endtime = endTime;
                } else {
                    starttime = endTime;
                    endtime = beginTime;
                }
                var timediff = endtime - starttime;
                var days = parseInt( timediff / 86400 );
                var remain = timediff % 86400;
                var hours = parseInt( timediff / 3600 );
                var remain = remain % 3600;
                var mins = parseInt( remain / 60 );
                if( days < 1 ) {
                    var res = hours + '小时' + mins + '分';
                } else {
                    var res = days + '天' + hours + '小时' + mins + '分';
                }
                return res;
            }

            var parent = this;
            this.D117502details1.BindChange( "Set",
                function( data ) {
                    var responseData = data[ 0 ]; // 当前行
                    if( responseData != null && responseData.DataField == "D117502details1.endTime" ) {
                        var currentRowId = responseData.ObjectId;   //获取行ID 
                        var end_time = this.GetCellManager( currentRowId, "D117502details1.endTime" ).GetValue();
                        var begin_time = this.GetCellManager( currentRowId, "D117502details1.startingTime" ).GetValue();
                        if( end_time < begin_time ) {
                            $.IShowWarn( "结束时间不能小于开始时间！" );
                        } else {
                            var total = timediff( begin_time, end_time );
                            parent.D117502details1.UpdateRow( currentRowId, {
                                "D117502details1.F0000005": total
                            });
                        }
                    }
                })
        }
        if( $.SmartForm.ResponseContext.ActivityCode == "Activity11" ) {
            this.statusNow.SetValue( "结束" );
        }
    },

    // 按钮事件
    OnLoadActions: function( actions ) {
    },

    // 提交校验
    OnValidate: function( actionControl ) {
        if( $.SmartForm.ResponseContext.ActivityCode == "Activity2" ) {
            var details = this.D117502details;// 用车 子表业务对象
            var details1 = this.D117502details1;// 派车 子表业务对象
            var now = this.now.GetValue();
            details1.ClearRows(); // 清除子表中的所有行
            var rowsCount = this.D117502details.GetRowsCount();
            for( var i = 0;i < rowsCount;i++ ) {
                var countInOneRow = details.GetValue()[ i ].numberOfUnits;

                if( details.GetValue()[ i ].carTime < now ) {
                    $.IShowWarn( "用车时间不能小于当前时间！" );
                    return false;
                } else {
                    var temp = details.GetValue()[ i ];
                    if( countInOneRow > 1 ) {
                        for( var j = 0;j < temp.numberOfUnits;j++ ) {
                            var subObjectId = $.IGuid();  // 创建行ID
                            details1.AddRow( subObjectId, {
                                "D117502details1.car": temp.tempObj,
                                "D117502details1.startingTime": temp.carTime,
                                "D117502details1.address": temp.F0000003
                            })
                        }
                    } else if( countInOneRow == 1 ) {
                        var subObjectId = $.IGuid();  //创建行ID
                        details1.AddRow( subObjectId, {
                            "D117502details1.car": temp.tempObj,
                            "D117502details1.startingTime": temp.carTime,
                            "D117502details1.address": temp.F0000003
                        })
                    }
                }
            }
        }
        if( $.SmartForm.ResponseContext.ActivityCode == "Activity8" ) {
            var details1 = this.D117502details1;// 派车 子表业务对象
            var now = this.now.GetValue();
            var rowsCount = details1.GetRowsCount();
            for( var i = 0;i < rowsCount;i++ ) {
                if( details1.GetValue()[ i ].endTime < details1.GetValue()[ i ].startingTime ) {
                    $.IShowWarn( "结束时间不能小于开始时间！" );
                    return false;
                }
            }
        }
        return true;
    },

    // 提交前事件
    BeforeSubmit: function( action, postValue ) {
    },

    // 提交后事件
    AfterSubmit: function( action, responseValue ) {
    }
});