using System.Security.Cryptography;

namespace Core.Helpers.Cryptography
{
    // using ---
    // var (publicKey, privateKey) = GenerateKeys(2048);
    // var data = new byte[] { 1, 2, 3 };
    // var signedData = Sign(data, privateKey);
    // var isValid = VerifySignature(data, signedData, publicKey);

    public static class DigitalSignatureHelper
    {
        public static (RSAParameters publicKey, RSAParameters privateKey) GenerateKeys(int keyLength)
        {
            using var rsa = RSA.Create();
            rsa.KeySize = keyLength;
            return (
                publicKey: rsa.ExportParameters(includePrivateParameters: false),
                privateKey: rsa.ExportParameters(includePrivateParameters: true));
        }

        public static byte[] Sign(byte[] data, RSAParameters privateKey)
        {
            using var rsa = RSA.Create();
            rsa.ImportParameters(privateKey);

            // the hash to sign
            byte[] hash;
            using (var sha256 = SHA256.Create())
            {
                hash = sha256.ComputeHash(data);
            }

            var rsaFormatter = new RSAPKCS1SignatureFormatter(rsa);
            rsaFormatter.SetHashAlgorithm("SHA256");
            return rsaFormatter.CreateSignature(hash);
        }

        public static bool VerifySignature(byte[] data, byte[] signature, RSAParameters publicKey)
        {
            using var rsa = RSA.Create();
            rsa.ImportParameters(publicKey);

            // the hash to sign
            byte[] hash;
            using (var sha256 = SHA256.Create())
            {
                hash = sha256.ComputeHash(data);
            }

            var rsaFormatter = new RSAPKCS1SignatureDeformatter(rsa);
            rsaFormatter.SetHashAlgorithm("SHA256");
            return rsaFormatter.VerifySignature(hash, signature);
        }
    }
}
