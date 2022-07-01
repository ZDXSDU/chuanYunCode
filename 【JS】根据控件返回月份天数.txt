function getCountDays( date ) {
            let curDate = new Date( date );
            let curMonth = curDate.getMonth();
            curDate.setMonth( curMonth + 1 );
            curDate.setDate( 0 );
            return curDate.getDate();
        }
        const temp = this;
        this.belongDate.BindChange( 'ZDX SDU', function() {
            let this_date = temp.belongDate.GetValue();
            temp.monthDays.SetValue( getCountDays( this_date ));
        })