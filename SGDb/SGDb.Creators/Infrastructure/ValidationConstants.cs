namespace SGDb.Creators.Infrastructure
{
    public static class ValidationConstants
    {
        public const string AllowedImgExtensions = "jpeg,jpg,png,gif";

        public static string[] AllowedTagsInGameAboutSection = new string[] { "h1", "h2", "h3", "h4", "h5", "h6", "div", "p", "ol", "ul", "li", "td", "th", "tr", "thead", "tbody", "tfooter", "span", "img" };
    }
}