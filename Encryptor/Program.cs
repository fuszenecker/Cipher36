using System.CommandLine;

var passwordArgument = new Argument<string>("password", () => "", "The password.");
var textArgument = new Argument<string>("text", () => "", "The plain text or encrypted text.");
var verboseOption = new Option<bool>(new string[] { "-v", "--verbose" }, () => false, "Verbose: print key and text.");
var decryptOption = new Option<bool>(new string[] { "-d", "--decrypt" }, () => false, "Decrypt the text, only makes sense if autokey = true.");
var autokeyOption = new Option<bool>(new string[] { "-a", "--autokey" }, () => true, "Use autokey algorithm.");

var cmd = new RootCommand { passwordArgument, textArgument, autokeyOption, decryptOption, verboseOption };
cmd.Description = "Text encryptor application.";

System.CommandLine.Handler.SetHandler(cmd, new Action<string, string, bool, bool, bool>(
    (password, text, autokey, decrypt, verbose) => 
        Console.WriteLine(Encryptor.Encrypt(password, text, autokey, decrypt, verbose))),
    passwordArgument, textArgument, autokeyOption, decryptOption, verboseOption);

return cmd.Invoke(args);
