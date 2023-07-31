namespace FastFoodShop.Web.Utility;

public class SD
{
    public static string CouponAPIBase { get; set; }
    public static string ProductAPIBase { get; set; }
    public static string AuthAPIBase { get; set; }
    
    public const string RoleAdmin = "admin";
    public const string RoleClient = "client";

    public const string TokenCookie = "JWTToken";
}