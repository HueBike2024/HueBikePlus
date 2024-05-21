//------------------------------------------------
// Author:                      Nhan Phan
//
// Copyright 2021
//------------------------------------------------

using Core.Models.Datatable;
using System;

namespace Core.Helpers
{
    public static class DatatableHelper
    {
        public static string ConvertToQueryParamString(params QueryParamObject[] pars)
        {
            string query = String.Empty;
            foreach (var par in pars)
            {
                if (par.Value != null)
                    query += string.Format("&{0}={1}", par.Name, par.Value);
            }

            return string.IsNullOrWhiteSpace(query) ? query : query.Substring(1);
        }
    }
}
