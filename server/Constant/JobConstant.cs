namespace server.Constant
{
    public class JobConstant
    {
        public abstract class Status
        {
            public static readonly string ACTIVE = "ACTIVE";
            public static readonly string INACTIVE = "INACTIVE";
        }

        public abstract class Method
        {
            public static readonly string GET = "GET";
            public static readonly string POST = "POST";
            public static readonly string PUT = "PUT";
            public static readonly string DELETE = "DELETE";
        }
    }
}
