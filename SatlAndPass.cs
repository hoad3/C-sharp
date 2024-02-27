using System;
using System.Collections;
using System.Security.Cryptography;
using System.Text;

public class SatlAndPass
{
    private const string PasswordServer = "123456";//Gỉa lập mật khẩu được lưu ở Server
    private const string PasswordClient = "123456";//Giả lập mật khẩu được nhập ở Client
    private static string _userSalt = "";
    private static string _userCombined = "";
    
    public static void Main(string[] args)
    {
        foreach (var _ in Server())
        {
            
        }
    }
//giả lập Server
    private static IEnumerable<int> Server()
    {
        using var user = User().GetEnumerator();
        var salt = GenerateSalt();
        _userSalt = salt;
        user.MoveNext();
        var pass = PasswordServer;// giả lập mật khẩu được lưu ở Server 
        var serverCombined = CPAS(pass, salt);//mật khẩu và muối được băm ở Server
        serverCombined = Hash(serverCombined);
        var userCombined = _userCombined;
        Console.WriteLine(userCombined == serverCombined);// so sánh chuỗi băm cảu mật khẩu và muối ở Server và Client
        
        yield break;
    }
//Giả lập User
    private static IEnumerable<int> User()
    {
        var pass = PasswordClient; //giả lập mật khẩu được nhập ở Client
        var salt = _userSalt;
        var combined = CPAS(pass, salt);////mật khẩu và muối được băm ở Client
        combined = Hash(combined);
        _userCombined = combined;
        yield return 1;
        
    } 
//Hàm tạo muối
    public static String GenerateSalt()
    {
        byte[] salt = new byte[32];

        using (var rng = new RNGCryptoServiceProvider())
        {
            rng.GetBytes(salt);
        }

        return Convert.ToBase64String(salt);
    }
//Hàm gộp Pass và muối
    public static String CPAS(String pass, String salt)
    {
        return pass + salt;
    }
//Hàm băm 
    public static String Hash(String input)
    {
        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] bytes = Encoding.UTF8.GetBytes(input);

            byte[] hash = sha256.ComputeHash(bytes);

            return Convert.ToBase64String(hash);
        }
    }
}