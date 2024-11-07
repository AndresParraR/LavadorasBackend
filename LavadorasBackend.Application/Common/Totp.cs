using OtpNet;

namespace Lavadoras.Application.Common;

public class Totp
{
    private const int _size = 6;
    private const int _step = 600;
    private const OtpHashMode _hashMode = OtpHashMode.Sha256;

    public static string SecretKey() => Base32Encoding.ToString(KeyGeneration.GenerateRandomKey(_hashMode));

    public static string GenerateCode(string key) => new OtpNet.Totp(Base32Encoding.ToBytes(key), _step, _hashMode, _size).ComputeTotp();

    public static bool VerifyCode(string key, string code) => new OtpNet.Totp(Base32Encoding.ToBytes(key), _step, _hashMode, _size).VerifyTotp(code, out _, new(1, 1));
}
