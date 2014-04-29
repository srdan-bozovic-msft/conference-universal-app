using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conference.Contracts.Models.Helpers
{
    public static class TrackHelper
    {
        public static string GetTitleForTrack(string code)
        {
            switch (code)
            {
                case "ALM":
                    return "Application Lifecycle & Project Management";
                case "APM":
                    return "Agile i upravljanje projektima";
                case "CLD":
                    return "Cloud, Azure app Development";
                case "DBI":
                    return "Data Platform & Bussiness Intelligence";
                case "DEV":
                    return "Desktop Languages, Frameworks, Developer Tools";
                case "DVC":
                    return "Windows 8, Windows Phone & Mobile Services";
                case "DYN":
                    return "Dynamics";
                case "GAM":
                    return "Game Development";
                case "SES":
                    return "SharePoint, Office 365 & Enterprise Social";
                case "SRV":
                    return "Windows Server, Networks, Cloud Platform and Modern Datacenter";
                case "WEB":
                    return "Web Development";
                case "UX":
                    return "Korisničko iskustvo";
                case "MSC":
                    return "Other";
                default:
                    return String.Empty;
            }
        }

        public static string GetImageUrlForTrack(string code)
        {
            return String.Format("/Assets/Tracks/{0}.png", code);
        }
    }
}
