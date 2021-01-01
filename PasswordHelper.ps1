﻿Add-Type -TypeDefinition @'
namespace FsInfoCat
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Security.Cryptography;
    using System.Text;

    [StructLayout(LayoutKind.Explicit)]
    public struct PwHash : IEquatable<PwHash>, IEquatable<string>
    {
        private const int FIELD_OFFSET_1 = 8;
        private const int FIELD_OFFSET_2 = 16;
        private const int FIELD_OFFSET_3 = 24;
        private const int FIELD_OFFSET_4 = 32;
        private const int FIELD_OFFSET_5 = 40;
        private const int FIELD_OFFSET_6 = 48;
        private const int FIELD_OFFSET_7 = 56;
        public const int HASH_BYTES_LENGTH = 64;
        public const int SALT_BYTES_LENGTH = 8;
        public const int TOTAL_LENGTH = HASH_BYTES_LENGTH + SALT_BYTES_LENGTH;

        [FieldOffset(0)]
        private readonly ulong _hashBits0;

        [FieldOffset(FIELD_OFFSET_1)]
        private readonly ulong _hashBits1;

        [FieldOffset(FIELD_OFFSET_2)]
        private readonly ulong _hashBits2;

        [FieldOffset(FIELD_OFFSET_3)]
        private readonly ulong _hashBits3;

        [FieldOffset(FIELD_OFFSET_4)]
        private readonly ulong _hashBits4;

        [FieldOffset(FIELD_OFFSET_5)]
        private readonly ulong _hashBits5;

        [FieldOffset(FIELD_OFFSET_6)]
        private readonly ulong _hashBits6;

        [FieldOffset(FIELD_OFFSET_7)]
        private readonly ulong _hashBits7;

        [FieldOffset(HASH_BYTES_LENGTH)]
        private readonly ulong _saltBits;

        private PwHash(byte[] sha512Hash, ulong salt)
        {
            _hashBits0 = BitConverter.ToUInt64(sha512Hash, 0);
            _hashBits1 = BitConverter.ToUInt64(sha512Hash, FIELD_OFFSET_1);
            _hashBits2 = BitConverter.ToUInt64(sha512Hash, FIELD_OFFSET_2);
            _hashBits3 = BitConverter.ToUInt64(sha512Hash, FIELD_OFFSET_3);
            _hashBits4 = BitConverter.ToUInt64(sha512Hash, FIELD_OFFSET_4);
            _hashBits5 = BitConverter.ToUInt64(sha512Hash, FIELD_OFFSET_5);
            _hashBits6 = BitConverter.ToUInt64(sha512Hash, FIELD_OFFSET_6);
            _hashBits7 = BitConverter.ToUInt64(sha512Hash, FIELD_OFFSET_7);
            _saltBits = salt;
        }

        private static byte[] ComputeHash(string rawPw, byte[] salt)
        {
            using (SHA512 sha = SHA512.Create())
            {
                sha.ComputeHash(Encoding.ASCII.GetBytes(rawPw).Concat(salt).ToArray());
                return sha.Hash;
            }   
        }

        public static PwHash? Import(string base64EncodedHash)
        {
            if (string.IsNullOrWhiteSpace(base64EncodedHash))
                return null;
            byte[] data;
            try { data = Convert.FromBase64String(base64EncodedHash); }
            catch (Exception exc) { throw new ArgumentException("Invalid base-64 string", "base64EncodedHash", exc); }
            if (data.Length != TOTAL_LENGTH)
                throw new ArgumentException("Invalid data length", "base64EncodedHash");
            return new PwHash(data, BitConverter.ToUInt64(data, HASH_BYTES_LENGTH));
        }

        public static PwHash? Create(string rawPw)
        {
            if (string.IsNullOrEmpty(rawPw))
                return null;
            byte[] salt = new byte[SALT_BYTES_LENGTH];
            using (RNGCryptoServiceProvider cryptoServiceProvider = new RNGCryptoServiceProvider())
                cryptoServiceProvider.GetBytes(salt);
            return new PwHash(ComputeHash(rawPw, salt), BitConverter.ToUInt64(salt, 0));
        }

        public static PwHash? Create(string rawPw, ulong salt)
        {
            if (string.IsNullOrEmpty(rawPw))
                return null;
            return new PwHash(ComputeHash(rawPw, BitConverter.GetBytes(salt)), salt);
        }

        public static bool Test(PwHash? hash, string rawPw)
        {
            return (hash.HasValue) ? hash.Value.Test(rawPw) : string.IsNullOrEmpty(rawPw);
        }

        public bool Test(string rawPw)
        {
            if (string.IsNullOrEmpty(rawPw))
                return false;
            byte[] hash = ComputeHash(rawPw, BitConverter.GetBytes(_saltBits));
            return BitConverter.ToUInt64(hash, 0) == _hashBits0 &&
                BitConverter.ToUInt64(hash, FIELD_OFFSET_1) == _hashBits1 && BitConverter.ToUInt64(hash, FIELD_OFFSET_2) == _hashBits2 &&
                BitConverter.ToUInt64(hash, FIELD_OFFSET_3) == _hashBits3 && BitConverter.ToUInt64(hash, FIELD_OFFSET_4) == _hashBits4 &&
                BitConverter.ToUInt64(hash, FIELD_OFFSET_5) == _hashBits5 && BitConverter.ToUInt64(hash, FIELD_OFFSET_6) == _hashBits6 &&
                BitConverter.ToUInt64(hash, FIELD_OFFSET_7) == _hashBits7;
        }

        public override string ToString()
        {
            return Convert.ToBase64String(BitConverter.GetBytes(_hashBits0).Concat(BitConverter.GetBytes(_hashBits1)).Concat(BitConverter.GetBytes(_hashBits2))
                .Concat(BitConverter.GetBytes(_hashBits3)).Concat(BitConverter.GetBytes(_hashBits4)).Concat(BitConverter.GetBytes(_hashBits5))
                .Concat(BitConverter.GetBytes(_hashBits6)).Concat(BitConverter.GetBytes(_hashBits7)).Concat(BitConverter.GetBytes(_saltBits))
                .ToArray());
        }

        public bool Equals(PwHash other)
        {
            return _hashBits0 == other._hashBits0 && _hashBits1 == other._hashBits1 && _hashBits2 == other._hashBits2 &&
                _hashBits3 == other._hashBits3 && _hashBits4 == other._hashBits4 && _hashBits5 == other._hashBits5 &&
                _hashBits6 == other._hashBits6 && _hashBits7 == other._hashBits7 && _saltBits == other._saltBits;
        }

        public bool Equals(string other)
        {
            if (string.IsNullOrWhiteSpace(other))
                return false;
            byte[] data;
            try { data = Convert.FromBase64String(other); }
            catch { return false; }
            return data.Length == TOTAL_LENGTH && BitConverter.ToUInt64(data, 0) == _hashBits0 &&
                BitConverter.ToUInt64(data, FIELD_OFFSET_1) == _hashBits1 && BitConverter.ToUInt64(data, FIELD_OFFSET_2) == _hashBits2 &&
                BitConverter.ToUInt64(data, FIELD_OFFSET_3) == _hashBits3 && BitConverter.ToUInt64(data, FIELD_OFFSET_4) == _hashBits4 &&
                BitConverter.ToUInt64(data, FIELD_OFFSET_5) == _hashBits5 && BitConverter.ToUInt64(data, FIELD_OFFSET_6) == _hashBits6 &&
                BitConverter.ToUInt64(data, FIELD_OFFSET_7) == _hashBits7 && BitConverter.ToUInt64(data, HASH_BYTES_LENGTH) == _saltBits;
        }

        public override bool Equals(object obj)
        {
            if (null == obj)
                return false;
            if (obj is PwHash)
                return Equals((PwHash)obj);
            return obj is string && Equals((string)obj);
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }
    }
}
'@ -ReferencedAssemblies 'System.Runtime';

Function Read-FunctionChoice {
    Param(
        [AllowEmptyString()]
        [string]$Message = ""
    )
    $ChoiceCollection = [System.Collections.ObjectModel.Collection[System.Management.Automation.Host.ChoiceDescription]]::new();
    $ChoiceCollection.Add([System.Management.Automation.Host.ChoiceDescription]::new('Enter Password', 'Enter password to hash'));
    $HashFromClipboard = -1;
    $EnterHash = 1;
    $TestFromClipboard = -1;
    $Quit = -1;
    if ([System.Windows.Clipboard]::ContainsText([System.Windows.TextDataFormat]::Text)) {
        $Text = [System.Windows.Clipboard]::GetText([System.Windows.TextDataFormat]::Text);
        if ($Text.Length -gt 0) {
            $ChoiceCollection.Add([System.Management.Automation.Host.ChoiceDescription]::new('Hash From clipboard', 'Convert clipboard text to password hash'));
            $HashFromClipboard = 1;
            $ChoiceCollection.Add([System.Management.Automation.Host.ChoiceDescription]::new('Test PW and Hash', 'Enter password and base4-encoded hash to compare'));
            $EnterHash = 2;
            if ($null -eq $Script:GetFunctionChoices_Regex) {
                $Script:GetFunctionChoices_Regex = [System.Text.RegularExpressions.Regex]::new('^\s*([A-Za-z\d+/]{96})\s*$', [System.Text.RegularExpressions.RegexOptions]::Compiled);
            }
            $m = $Script:GetFunctionChoices_Regex.Match($Text);
            if ($m.Success) {
                $ChoiceCollection.Add([System.Management.Automation.Host.ChoiceDescription]::new('Test from Clipboard', 'Test base64-encoded hash in clipboard with entered password'));
                $TestFromClipboard = 3;
            }
        } else {
            $ChoiceCollection.Add([System.Management.Automation.Host.ChoiceDescription]::new('Test PW and Hash', 'Enter password and base4-encoded hash to compare'));
        }
    } else {
        $ChoiceCollection.Add([System.Management.Automation.Host.ChoiceDescription]::new('Test PW and Hash', 'Enter password and base4-encoded hash to compare'));
    }
    $ChoiceCollection.Add([System.Management.Automation.Host.ChoiceDescription]::new('Quit'));
    $msg = 'Select desired function';
    if ($Message.Length -gt 0) { $msg = "$Message`n`n$msg" }
    $i = $Host.UI.PromptForChoice('Function', $msg, $ChoiceCollection, 0);
    if ($null -ne $i -and $i -ge 0 -and $i -lt $ChoiceCollection.Count - 1) {
        switch ($i) {
            { $_ -eq $HashFromClipboard } {
                'HashFromClipboard' | Write-Output;
                break;
            }
            { $_ -eq $EnterHash } {
                'EnterHash' | Write-Output;
                break;
            }
            { $_ -eq $TestFromClipboard } {
                'TestFromClipboard' | Write-Output;
                break;
            }
            default {
                'EnterPassword' | Write-Output;
                break;
            }
        }
    } else {
        'Quit' | Write-Output;
    }
}

$OldWarningPreference = $WarningPreference;
$OldInformationPreference = $InformationPreference;
$WarningPreference = [System.Management.Automation.ActionPreference]::Continue;
$InformationPreference = [System.Management.Automation.ActionPreference]::Continue;

try {
    $FunctionChoice = Read-FunctionChoice;
    while ($FunctionChoice -ne 'Quit') {
        $LastMessage = '';
        switch ($FunctionChoice) {
            'HashFromClipboard' {
                $PwHash = [FsInfoCat.PwHash]::Create(([System.Windows.Clipboard]::GetText([System.Windows.TextDataFormat]::Text)));
                $Base64Hash = $PwHash.ToString();
                [System.Windows.Clipboard]::SetText($Base64Hash);
                $LastMessage = "Base64-Encoded hash (copied to clipboard):`n`t$Base64Hash";
                break;
            }
            'EnterHash' {
                $Password = Read-Host -Prompt 'Enter password';
                if ([string]::IsNullOrEmpty($Password)) {
                    $LastMessage = 'Warning: No password to test';
                } else {
                    do {
                        $ExpectedHash = Read-Host -Prompt 'Enter base64-encoded hash string';
                        $PwHash = [FsInfoCat.PwHash]::Import($ExpectedHash);
                        if ($null -eq $PwHash) {
                            $LastMessage = 'Warning: No hash to test';
                            break;
                        }
                        if ($PwHash.Test($Password)) {
                            $LastMessage = "Success!`n`tBase64-encoded hash string matches the hashed password";
                        } else {
                            $LastMessage = "Failed!`n`tBase64-encoded hash string does not match the hashed password";
                        }
                    } while (1);
                }
                break;
            }
            'TestFromClipboard' {
                $Password = Read-Host -Prompt 'Enter password';
                if ([string]::IsNullOrEmpty($Password)) {
                    'No password to test' | Write-Warning;
                } else {
                    $PwHash = [FsInfoCat.PwHash]::Import(([System.Windows.Clipboard]::GetText([System.Windows.TextDataFormat]::Text)));
                    if ($PwHash.Test($Password)) {
                        $LastMessage = "Success!`n`tBase64-encoded hash string matches the hashed password";
                    } else {
                        $LastMessage = "Failed!`n`tBase64-encoded hash string does not match the hashed password";
                    }
                }
                break;
            }
            default {
                $Password = Read-Host -Prompt 'Enter password';
                $PwHash = [FsInfoCat.PwHash]::Create($Password);
                if ($null -eq $PwHash) {
                    $LastMessage = 'Warning: No password to hash';
                } else {
                    $Base64Hash = $PwHash.ToString();
                    [System.Windows.Clipboard]::SetText($Base64Hash);
                    $LastMessage = "Base64-Encoded hash (copied to clipboard):`n`t$Base64Hash";
                }
                break;
            }
        }
        $FunctionChoice = Read-FunctionChoice -Message $LastMessage;
    }
} finally {
    $WarningPreference = $OldWarningPreference;
    $InformationPreference = $OldInformationPreference;
}
