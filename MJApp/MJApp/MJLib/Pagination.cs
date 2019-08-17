using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MJApp.MJLib
{
    public class Pagination
    {
        
        public static string paginate(int numrows, int starting, int recpage, string link, string div, string str)
        {
            int next = starting + recpage;
            double v = ((numrows / recpage) - 1) * recpage;
            int var = Convert.ToInt32(v);
            double v2 = (starting / recpage) + 1;
            int page_showing = Convert.ToInt32(v2);
            double v3 = (numrows / recpage);
            int total_page = Convert.ToInt32(Math.Floor(v3));
            
            int last = 0;
            double b1 = 0;
            if(numrows % recpage !=0)
            {
                b1 = (numrows / recpage) * recpage;
                last = Convert.ToInt32(b1);
            }
            else
            {
                b1 = ((numrows / recpage) - 1) * recpage;
                last = Convert.ToInt32(b1);
            }
            int previous = starting - recpage;
            string anc = "<ul class='pagination'>";
            if (previous < 0)
            {
                anc += "<li><a href=\"javascript:void(0)\">First</li>";
                anc += "<li><a href=\"javascript:void(0)\">Previus</li>";
            }
            else
            {
                anc += "<li class='next'><a href=\"javascript:void(0)\" onclick=\"javascript:MJPaging('" + link + "','" + div + "',0,'" + str + "');\">First</a></li>";
                anc += "<li class='next'><a href=\"javascript:void(0)\" onclick=\"javascript:MJPaging('" + link + "','" + div + "','"+previous+"','" + str + "');\">Previous</a></li>";
            }
            int norepeat = 4;
            int j = 1;
            string anch = "";
            for(int i = page_showing; i>1; i--)
            {
                int fpreviousPage = i - 1;
                double c1 = (fpreviousPage * recpage) - recpage;
                int page =Convert.ToInt32( Math.Floor(c1));
                anch = "<li><a href=\"javascript:void(0)\" onclick=\"javascript:MJPaging('" + link + "','" + div + "','" + page + "','" + str + "');\">" + fpreviousPage + "</a></li>" + anch;
                if (j == norepeat) break;
                j++;
            }

            anc += anch;
            anc += "<li class='active'><a href=\"javascript:void(0)\">" + page_showing + "</li>";
            j = 1;
            for(int i =page_showing; i<total_page; i++)
            {
                int fnextPage = i + 1;
                double c2 = (fnextPage * recpage) - recpage;
                int page = Convert.ToInt32(Math.Floor(c2));
                anc += "<li><a href=\"javascript:void(0)\" onclick=\"javascript:MJPaging('" + link + "','" + div + "','" + page + "','" + str + "');\">" + fnextPage + "</a></li>";
                if (j == norepeat) break;
                j++;
            }
            //-------
            if(next >=numrows)
            {
                anc += "<li><a href=\"javascript:void(0)\">Next</li>";
                anc += "<li><a href=\"javascript:void(0)\">Last</li>";
                
            }
            else
            {
                anc += "<li class='next'><a href=\"javascript:void(0)\" onclick=\"javascript:MJPaging('" + link + "','" + div + "','" + next + "','" + str + "');\">Next</a></li>";
                anc += "<li class='next'><a href=\"javascript:void(0)\" onclick=\"javascript:MJPaging('" + link + "','" + div + "','" + last + "','" + str + "');\">Last</a></li>";
            }
            anc += "</ul>";
   
            return anc;

        }
    }
}