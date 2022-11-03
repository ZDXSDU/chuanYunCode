// H3.IEngine engine = this.Request.Engine;
        // H3.Data.Filter.Filter filter = new H3.Data.Filter.Filter();
        // H3.Data.Filter.And andMatcher = new H3.Data.Filter.And();
        // andMatcher.Add(new H3.Data.Filter.ItemMatcher("DfileID", H3.Data.ComparisonOperatorType.NotEqual, "")); //附件ID不为空
        // andMatcher.Add(new H3.Data.Filter.ItemMatcher("DThumbnailUrl", H3.Data.ComparisonOperatorType.Equal, "")); //图片缩略图为空
        // filter.Matcher = andMatcher;
        // H3.DataModel.BizObjectSchema schema = engine.BizObjectManager.GetPublishedSchema("D11750242f8dcf539074e93abb57a584208d56a");
        // H3.DataModel.BizObject[] boArray = H3.DataModel.BizObject.GetList(engine, H3.Organization.User.SystemUserId, schema, H3.DataModel.GetListScopeType.GlobalAll, filter);
        // if(boArray != null && boArray.Length > 0)
        // {
        //     for(int i = 0;i < boArray.Length; i++)
        //     {
        //         H3.DataModel.BizObject accountBo = H3.DataModel.BizObject.Load(H3.Organization.User.SystemUserId, this.Request.Engine, "D11750242f8dcf539074e93abb57a584208d56a", boArray[i].ObjectId, false);
        //         accountBo["DThumbnailUrl"] = this.Request.UserContext.GetThumbnailUrl(boArray[i]["DfileID"].ToString()).ToString();
        //         accountBo.Status = H3.DataModel.BizObjectStatus.Effective;
        //         accountBo.Update();
                
        //     }
        // }