$.extend( $.JForm, {
    OnLoad: function() {
        const THAT = this;
        this.userId.BindChange( "amao", () => {
            THAT.userFrom.SetValue( THAT.userId.GetValue() );
        })
        /** 工时统计 BEGIN **/
        const isAsync = false;
        if( !( $.SmartForm.ResponseContext.IsCreateMode ) ) {
            this.D148737e8fa914992a741dd9e676959235f4a52.BindChange( '增修工时', () => { AJAX( "IncreaseMaintenanceItems" ); });
            this.D1487373e21d1c221a44a6c83dba9ba9028ecc8.BindChange( '预估工时', () => { AJAX( "EstimateMaintenanceItems" ); });
        }
        function AJAX( sectionName ) {
            $.SmartForm.PostForm( sectionName, {}, ( data ) => {
                if( data.Errors && data.Errors.length ) {
                    $.IShowError( "错误", JSON.stringify( data.Errors ) );
                } else {
                    console.log( data.ReturnData );
                }
            }, ( error ) => {
                $.IShowError( "错误", JSON.stringify( error ) );
            }, isAsync );
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