﻿namespace Camino.IdentityManager.Contracts
{
    public interface ITextEncryption
    {
        string Encrypt(string plainText, string saltKey);
        string Decrypt(string encryptedText, string saltKey);
    }
}
