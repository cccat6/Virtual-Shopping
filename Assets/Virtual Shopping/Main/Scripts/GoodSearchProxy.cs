using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System;
using UnityEngine;
using System.Text.RegularExpressions;
using System.Collections;

public class GoodSearchProxy : MonoBehaviour
{
    public enum queryOrderBy
    {
        price,
        sales,
        power,
        lastedittime
    }
    public static string apiKey = "0F18C88DE06BE3E22F103CFB0887EAF7";
    public static string url = "https://holoworld.search.windows.net/indexes/temp/docs?api-version=2016-09-01";

    /// <summary>
    /// 总的查询外部调用函数，可以根据需要增加筛选条件
    /// </summary>
    /// <param name="searchKeyWord">搜索关键词</param>
    /// <param name="orderOrNotOrder">是否排序，true为排序</param>
    /// <param name="queryorderby">排序类型，枚举型</param>
    /// <param name="ascOrDesc">增序还是降序，true为增序</param>
    /// <param name="filterPrice">价格筛选，布尔值</param>
    /// <param name="priceUpper">价格上限，double值</param>
    /// <param name="priceLower">价格下限，double值</param>
    /// <param name="filterSales">销量筛选，布尔值</param>
    /// <param name="salesUpper">销量上限，int值</param>
    /// <param name="salesLower">销量下限，int值</param>
    /// <returns></returns>
    public static List<ControlCenter.GoodInfo> reresult = null;
    public static IEnumerator GetSearchResult(string searchKeyWord, bool orderOrNotOrder, queryOrderBy queryorderby, bool ascOrDesc, bool filterPrice, double priceUpper, double priceLower, bool filterSales, int salesUpper, int salesLower)
    {
        string searchQueryExpression = GetSearchQueryExpression(searchKeyWord, orderOrNotOrder, queryorderby, ascOrDesc).Replace(" ", "%20");
        
        WWW www = new WWW(searchQueryExpression);
        while (!www.isDone)
            yield return www;
        string requestResult = www.text;

        List<ControlCenter.GoodInfo> g = GoodInfoEntiry(requestResult);
        List<ControlCenter.GoodInfo> result = new List<ControlCenter.GoodInfo>();
        if (filterPrice)
        {
            result = GetGoodSearchResultFilter(g, ascOrDesc, priceUpper, priceLower);
        }
        else if (filterSales)
        {
            result = GetGoodSearchResultFilter(g, ascOrDesc, salesUpper, salesLower);
        }
        else
        {
            result = g;
        }
        reresult = result;
    }

    /// <summary>
    /// 构造Get请求的查询表达式
    /// </summary>
    /// <param name="searchKeyWord">查询关键词</param>
    /// <param name="OrderOrNotOrder">指示排序或者不排序</param>
    /// <param name="queryorderby">指示排序类型，参数为枚举类型</param>
    /// <param name="ascOrDesc">指示递增排序还是递减排序，默认递增</param>
    /// <returns></returns>
    public static string GetSearchQueryExpression(string searchKeyWord, bool orderOrNotOrder, queryOrderBy queryorderby, bool ascOrDesc)
    {
        string orderQueryExpression = string.Empty;
        if (orderOrNotOrder) //指示是否排序
        {
            orderQueryExpression = "&$orderby=" + queryorderby + " asc";
            if (!ascOrDesc) //指示递增排序还是递减排序，默认递增
            {
                orderQueryExpression = "&$orderby=" + queryorderby + " desc";
            }
        }
        string queryExpression = url + "&api-key=" + apiKey + "&search=" + searchKeyWord + orderQueryExpression;

        return queryExpression;
    }

    /// <summary>
    /// Get请求
    /// </summary>
    /// <param name="url">查询表达式</param>
    /// <returns></returns>
    /*public static IEnumerator Get(string url)
    {
        WWW www = new WWW(url);
        while (!www.isDone)
            yield return www;
        requestResult = www.text;
    }*/

    /// <summary>
    /// 将返回的Json数据转为强类型的Good类型
    /// </summary>
    /// <param name="json">Get返回的文本</param>
    /// <returns>一个Good实例，包含ControlCenter.GoodInfo</returns>
    public static List<ControlCenter.GoodInfo> GoodInfoEntiry(string json)
    {
        List<ControlCenter.GoodInfo> g = new List<ControlCenter.GoodInfo>();
        if (json != "")
        {
            string value = json.Substring(json.IndexOf('[') + 1, json.IndexOf(']') - json.IndexOf('['));
            string[] valueArray = value.Split('"', '"');
            int pt = 0; //指示当前的ControlCenter.GoodInfo
            for (int i = 0; i < valueArray.Length; i++)
            {
                /*foreach (string now in valueArray)
                    Debug.Log(now);*/
                if (valueArray[i] == "id")
                {
                    g.Add(new ControlCenter.GoodInfo());
                    g[pt].id = valueArray[i + 2];
                }
                if (valueArray[i] == "name")
                {
                    g[pt].name = valueArray[i + 2];
                }
                if (valueArray[i] == "price")
                {
                    g[pt].price = valueArray[i + 2];
                }
                if (valueArray[i] == "sales")
                {
                    string sale = "";
                    MatchCollection matchSet = Regex.Matches(valueArray[i + 1], "[0-9]");
                    foreach (Match aMatch in matchSet)
                    {
                        sale += aMatch;
                    }
                    g[pt].sales = Convert.ToInt32(sale);
                }
                if (valueArray[i] == "isScene")
                {
                    g[pt].isScene = valueArray[i + 1].Contains("true") ? "1" : "0";
                    pt++;
                }
            }
            return g;
        }
        else
        {
            ControlCenter.ShowMessage(Language.lang.responseerror);
            throw new Exception("The String is null");
        }

    }

    /// <summary>
    /// 对返回结果价格排序、筛选
    /// </summary>
    /// <param name="g"></param>
    /// <param name="ascOrDesc">增序还是降序，true为增序</param>
    /// <param name="priceUpper">价格上限</param>
    /// <param name="priceLower">价格下限</param>
    /// <returns></returns>
    public static List<ControlCenter.GoodInfo> GetGoodSearchResultFilter(List<ControlCenter.GoodInfo> g, bool ascOrDesc, double priceUpper, double priceLower)
    {
        if (ascOrDesc)
        {
            Comparison<ControlCenter.GoodInfo> comparison = new Comparison<ControlCenter.GoodInfo>
             ((ControlCenter.GoodInfo x, ControlCenter.GoodInfo y) =>
             {
                 if (Convert.ToDouble(x.price) < Convert.ToDouble(y.price))
                     return -1;
                 else if (Convert.ToDouble(x.price) == Convert.ToDouble(y.price))
                     return 0;
                 else
                     return 1;
             });
            g.Sort(comparison);
            List<ControlCenter.GoodInfo> result = new List<ControlCenter.GoodInfo>();
            foreach (ControlCenter.GoodInfo item in g)
            {
                if (Convert.ToDouble(item.price) >= priceLower && Convert.ToDouble(item.price) <= priceUpper)
                {
                    result.Add(item);
                }
            }
            return result;
        }
        else
        {
            Comparison<ControlCenter.GoodInfo> comparison = new Comparison<ControlCenter.GoodInfo>
             ((ControlCenter.GoodInfo x, ControlCenter.GoodInfo y) =>
             {
                 if (Convert.ToDouble(x.price) < Convert.ToDouble(y.price))
                     return 1;
                 else if (Convert.ToDouble(x.price) == Convert.ToDouble(y.price))
                     return 0;
                 else
                     return -1;
             });
            g.Sort(comparison);
            List<ControlCenter.GoodInfo> result = new List<ControlCenter.GoodInfo>();
            foreach (ControlCenter.GoodInfo item in g)
            {
                if (Convert.ToDouble(item.price) >= priceLower && Convert.ToDouble(item.price) <= priceUpper)
                {
                    result.Add(item);
                }
            }
            return result;
        }

    }

    /// <summary>
    /// 对返回结果销量筛选
    /// </summary>
    /// <param name="g"></param>
    /// <param name="ascOrDesc">增序还是降序，true为增序</param>
    /// <param name="salesUpper">价格上限</param>
    /// <param name="salesLower">价格下限</param>
    /// <returns></returns>
    public static List<ControlCenter.GoodInfo> GetGoodSearchResultFilter(List<ControlCenter.GoodInfo> g, bool ascOrDesc, int salesUpper, double salesLower)
    {
        if (ascOrDesc)
        {
            Comparison<ControlCenter.GoodInfo> comparison = new Comparison<ControlCenter.GoodInfo>
             ((ControlCenter.GoodInfo x, ControlCenter.GoodInfo y) =>
             {
                 if (x.sales < y.sales)
                     return -1;
                 else if (x.sales == y.sales)
                     return 0;
                 else
                     return 1;
             });
            g.Sort(comparison);
            List<ControlCenter.GoodInfo> result = new List<ControlCenter.GoodInfo>();
            foreach (ControlCenter.GoodInfo item in g)
            {
                if (item.sales >= salesLower && item.sales <= salesUpper)
                {
                    result.Add(item);
                }
            }
            return result;
        }
        else
        {
            Comparison<ControlCenter.GoodInfo> comparison = new Comparison<ControlCenter.GoodInfo>
             ((ControlCenter.GoodInfo x, ControlCenter.GoodInfo y) =>
             {
                 if (x.sales < y.sales)
                     return 1;
                 else if (x.sales == y.sales)
                     return 0;
                 else
                     return -1;
             });
            g.Sort(comparison);
            List<ControlCenter.GoodInfo> result = new List<ControlCenter.GoodInfo>();
            foreach (ControlCenter.GoodInfo item in g)
            {
                if (item.sales >= salesLower && item.sales <= salesUpper)
                {
                    result.Add(item);
                }
            }
            return result;
        }

    }
}
