$.extend( $.JForm, {
    OnLoad: function() {
        const THAT = this;
        this.userId.BindChange( "amao", () => {
            THAT.userFrom.SetValue( THAT.userId.GetValue() );
        })
        /** 工时统计 BEGIN **/
        const isAsync = false;
        if( !( $.SmartForm.ResponseContext.IsCreateMode ) ) {
            this.D148737e8fa914992a741dd9e676959235f4a52.BindChange( '增修工时', () => { GetLists(); });
            this.D1487373e21d1c221a44a6c83dba9ba9028ecc8.BindChange( '预估工时', () => { GetLists(); });
        }
        const GetLists = () => {
            this.D117502manhour.ClearRows();
            let AF_detail = this.D1487373e21d1c221a44a6c83dba9ba9028ecc8.GetValue();
            let CFromControlManager = this.D117502manhour;
            for( let i = 0;i < AF_detail.length;i++ ) {
                CFromControlManager.AddRow( $.IGuid(), {
                    "D117502manhour.maintenanceMeasures": AF_detail[i].maintenanceMeasures, // 维修项目
                    "D117502manhour.F0000005": AF_detail[i].F0000005, // 修理工
                    "D117502manhour.chargingMethods": AF_detail[i].chargingMethods // 收费方式
                })
            }
            let BF_detail = this.D148737e8fa914992a741dd9e676959235f4a52.GetValue();            
            for( let i = 0;i < BF_detail.length;i++ ) {
                console.log(BF_detail[i].hourlyFees);
                console.log(BF_detail[i]);
                CFromControlManager.AddRow( $.IGuid(), {
                    "D117502manhour.maintenanceMeasures": BF_detail[i].maintenanceMeasures, // 维修项目
                    "D117502manhour.F0000005": BF_detail[i].F0000005, // 修理工
                    "D117502manhour.chargingMethods": BF_detail[i].chargingMethods // 收费方式
                })
            }
        }
        /** 工时统计 END **/
    },
    OnLoadActions: function( actions ) {
    },
    OnValidate: function( actionControl ) {
        return true;
    },
    BeforeSubmit: function( action, postValue ) {
    },
    AfterSubmit: function( action, responseValue ) {
    }
});