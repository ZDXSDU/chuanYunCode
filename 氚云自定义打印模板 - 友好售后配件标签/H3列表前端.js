$.ListView.ActionPreDo = function( actionCode ) {
    if( actionCode == 'smallLabel' ) { // 打印小标签

    }
    if( actionCode == 'BigLabel' ) {
        let objects = $.ListView.GetSelected();
        let OBJs = '';
        if( objects == null || objects == undefined || objects == "" ) {
            $.IShowWarn( '你没有选中任何数据,请勾选！' );
            return;
        }
        else {
            for( let i = 0;i < objects.length;i++ ) {
                OBJs += ( objects[ i ].ObjectId + ',' );
            }
            setTimeout(() => {
                $.ListView.Post( "BigLabel", {
                    OBJs: OBJs.substr( 0, OBJs.length - 1 )
                }, ( data ) => {
                    if( data.ReturnData.res.length > 0 ) {
                        let alize = `<div class='a'><style>html {font-family: sans-serif;line-height: 1.15;-ms-text-size-adjust: 100%;-webkit-text-size-adjust: 100%;} body {margin: 0;}article,aside,
                        footer,header,nav,section {display: block;}h1 {font-size: 2em;margin: 0.67em 0;}figcaption,figure,main {display: block;} figure {margin: 1em 40px;}hr {box-sizing: content-box;height: 0;
                        overflow: visible;}pre {font-family: monospace, monospace;font-size: 1em;} a {background-color: transparent;-webkit-text-decoration-skip: objects;}a:active,a:hover {outline-width: 0;} 
                        abbr[title] {border-bottom: none; text-decoration: underline;text-decoration: underline dotted;}b,strong {font-weight: inherit;}b,strong {font-weight: bolder;}code,kbd,samp {font-family: 
                        monospace, monospace;font-size: 1em;}dfn {font-style: italic;}mark {background-color: #ff0;color: #000;}small {font-size: 80%;}sub,sup {font-size: 75%;line-height: 0;position: relative;
                        vertical-align: baseline;}sub {bottom: -0.25em;}sup {top: -0.5em;}audio,video {display: inline-block;}audio:not([controls]) {display: none;height: 0;}img {border-style: none;}svg:not(:root) {
                        overflow: hidden;}button,input,optgroup,select,textarea {font-family: sans-serif;font-size: 100%;line-height: 1.15;margin: 0;}button,input {overflow: visible;}button,select {
                        text-transform: none;}button,html [type="button"],[type="reset"],[type="submit"] {-webkit-appearance: button;}button::-moz-focus-inner,[type="button"]::-moz-focus-inner,
                        [type="reset"]::-moz-focus-inner,[type="submit"]::-moz-focus-inner {border-style: none;padding: 0;}button:-moz-focusring,[type="button"]:-moz-focusring,[type="reset"]:-moz-focusring,
                        [type="submit"]:-moz-focusring {outline: 1px dotted ButtonText;}fieldset {border: 1px solid #c0c0c0;margin: 0 2px;padding: 0.35em 0.625em 0.75em;}legend {box-sizing: border-box;
                        color: inherit;display: table;max-width: 100%;padding: 0;white-space: normal;}progress {display: inline-block;vertical-align: baseline;}textarea {overflow: auto;}[type="checkbox"],
                        [type="radio"] {box-sizing: border-box;padding: 0;}[type="number"]::-webkit-inner-spin-button,[type="number"]::-webkit-outer-spin-button {height: auto;}[type="search"] 
                        {-webkit-appearance: textfield;outline-offset: -2px;}[type="search"]::-webkit-search-cancel-button,[type="search"]::-webkit-search-decoration {-webkit-appearance: none;}
                        ::-webkit-file-upload-button {-webkit-appearance: button;font: inherit;}details,menu {display: block;}summary {display: list-item;}canvas {display: inline-block;}template {display: none;}
                        [hidden] {display: none;}</style><style>.a {width: 210mm;height: 340mm;font-size: 14pt;font-family: SimSun, 宋体, serif;color: black;line-height: 1.4;text-align: justify;margin: 0;padding: 0;}
                        .b {width: 210mm;height: 340mm;padding: 0 24mm 0 24mm;background-color: #fff;}table {width: 45%;float: left;margin: 3.0% 2.5%;}td {margin: 0;padding-left: 6px;}.TDl {width: 45%;}.TDll {width: 20%;
                        }.bTop {border-top: 1px solid #555;}.bRight {border-right: 1px solid #555;}.bBottom {border-bottom: 1px solid #555;}.bLeft {border-left: 1px solid #555;}.line {width: 100%;
                        border-bottom: 1px dashed #555;*zoom: 1;}.line::after {visibility: hidden;clear: both;display: block;content: ".";height: 0}</style><div class='b'>`;
                        for( let i = 0;i < data.ReturnData.res.length;i++ ) {
                            alize += `<table cellspacing="0" cellpadding="0"><tr><td class="bTop bRight bLeft" style="text-align: center;font-weight: 600;" colspan="4">` + data.ReturnData.res[ i ].BU + "事业部" + data.ReturnData.res[ i ].BR + "店</td></tr>"; // TH
                            alize += `<tr><td colspan="2" class="bTop bRight bBottom bLeft TDl">配件号</td><td colspan="2" class="bTop bRight bBottom">` + data.ReturnData.res[ i ].PartNo + "</td></tr>"; // 配件号
                            alize += `<tr><td colspan="2" class="bRight bBottom bLeft TDl">配件名称</td><td colspan="2" class="bRight bBottom">` + data.ReturnData.res[ i ].PartName + "</td></tr>"; // 配件名称
                            alize += `<tr><td colspan="2" class="bRight bBottom bLeft TDl">货位号</td><td colspan="2" class="bRight bBottom">` + data.ReturnData.res[ i ].Cargospace + "</td></tr>"; // 货位号
                            alize += `<tr><td colspan="2" class="bRight bBottom bLeft TDl">适用车型</td><td colspan="2" class="bRight bBottom">` + data.ReturnData.res[ i ].Model + "</td></tr>"; // 适用车型
                            alize += `<tr><td class="bRight bBottom bLeft TDll">出库价</td><td class="bRight bBottom">` + data.ReturnData.res[ i ].OutPrice + `</td><td class="bRight bBottom TDll">周转率</td><td class="bRight bBottom">22.20%</td></tr>`; // 出库价 周转率
                            alize += `<tr><td class="bRight bBottom bLeft TDll">备注</td><td colspan="3" class="bRight bBottom">没有备注</td></tr></table>`; // 备注
                            if( ( i + 1 ) % 2 === 0 && ( i + 1 ) > 1 ) { alize += `<div class="line"></div>`; }
                        }
                        alize += "</div></div>";
                        console.log( alize )
                        window.document.body.innerHTML = alize;
                        window.print();
                    }
                    else {
                        if( data.ReturnData != null && data.ReturnData.Message ) {
                            $.IShowError( data.ReturnData.Message );
                        } else {
                            $.IShowError( '操作失败！请联系管理员处理' );
                        }
                    }
                }, ( data ) => {
                     $.IShowError( '操作失败！请联系管理员处理' );
                }, false );
            }, 1000 )
        }
    }
};