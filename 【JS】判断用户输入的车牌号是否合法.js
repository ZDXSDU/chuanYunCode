function isLicenseNo( str ) {
            var regExp = /(^[京津沪渝冀豫云辽黑湘皖鲁新苏浙赣鄂桂甘晋蒙陕吉闽贵粤青藏川宁琼使领A-Z]{1}[A-Z]{1}[A-Z0-9]{4}[A-Z0-9挂学警港澳临]{1}$)/;
            if( !regExp.test( str ) ) {
                return 1;
            }
        }